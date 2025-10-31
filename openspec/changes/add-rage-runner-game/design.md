# Design Document: Köksal Baba: Rage Runner

## Context

This is a hyper-casual mobile runner built in Unity 2D targeting iOS (primary) and Android (stretch). The game features a unique IP (Köksal Baba and Riçıt) with authorized usage rights. Target audience is casual mobile gamers seeking 60-120 second sessions with simple one-touch controls. The project is built on Windows 11 using Unity 2022/2023 LTS with Visual Studio 2022 Community or 2026 Insider.

**Constraints:**
- Build size < 150 MB
- Cold start < 2.5s on iPhone 12 class
- 60 FPS steady gameplay with zero GC spikes > 2ms
- No PII collection, ATT/IDFA only if ad network requires
- Family-friendly content, low age rating

**Stakeholders:**
- IP holders (Köksal Baba, Riçıt) - authorized usage via NameRights.pdf
- Players - casual mobile gamers seeking short, comedic experiences
- Monetization - ad revenue and optional IAP

## Goals / Non-Goals

### Goals
- Deliver a polished, 60 FPS hyper-casual runner with unique Rage Meter mechanic
- Implement ad-supported monetization with optional IAP (Remove Ads, Starter Pack)
- Ship on iOS App Store with proper compliance (Privacy Manifest, ATT/IDFA, IP authorization)
- Achieve zero-GC gameplay through object pooling and pre-warmed systems
- Support tr-TR and en-US localization at launch
- Enable lightweight analytics (Firebase or Unity Analytics) with user consent

### Non-Goals (v1)
- Leaderboards, social login, cloud save, server backend
- Android builds (stretch goal, not primary target)
- Real-person voice lines (SFX only)
- Complex progression systems (cosmetics unlock with coins, no RPG mechanics)

## Decisions

### Architecture: Service-Oriented Singletons with ScriptableObject Configuration

**What:** Core systems (Input, Ads, IAP, Analytics, Time) are implemented as service singletons accessed via a ServiceLocator or DI container. Gameplay tuning (difficulty curve, spawn config, theme) is data-driven via ScriptableObjects. Scene flow uses a Bootstrap → MainMenu → Game → Results architecture with persistent GameManager.

**Why:**
- Singletons provide global access without coupling (testable via interfaces)
- ScriptableObjects enable designer-friendly tuning without code changes
- Bootstrap scene ensures services initialize before gameplay
- State machine in GameManager keeps scene transitions explicit

**Alternatives Considered:**
- MonoBehaviour singletons: Harder to test, tightly coupled to Unity lifecycle
- Static classes: No interface support, difficult to mock for tests
- ECS (DOTS): Overkill for 2D runner, steep learning curve, immature tooling

**Trade-offs:**
- Service locator adds slight indirection but enables testability
- ScriptableObjects require asset management but decouple data from code

### Input: Tap-to-Hop with Rigidbody2D Impulse

**What:** Single tap anywhere on screen applies a fixed `(forwardVelocity, jumpVelocity)` impulse to the player's Rigidbody2D. Gravity pulls player down; no coyote time or jump buffering at v1.

**Why:**
- Simplest possible input for hyper-casual audience
- Rigidbody2D provides physics consistency
- Fixed impulse ensures predictable hops (no variable jump height)

**Alternatives Considered:**
- CharacterController2D: More control but requires custom collision/slope handling
- Transform.Translate: No physics, requires manual gravity and collision
- Variable jump height (hold to jump higher): Adds complexity for minimal benefit

**Trade-offs:**
- Fixed impulse limits advanced play but maintains accessibility
- No coyote time means slightly less forgiving, but acceptable for target audience

### Rage System: Pickup-Driven Meter with Passive Decay

**What:** RageMeter is a float [0, 1] that increases by 0.25 per "Taunt" pickup and decays passively at 0.1/sec. At 1.0, player can activate Rage Dash (0.8s duration, invulnerability, +30% speed). Meter depletes fully on dash activation.

**Why:**
- Pickup-driven gain creates strategic risk/reward (chase taunts vs play safe)
- Passive decay incentivizes aggressive play
- Short dash duration (0.8s) provides power without trivializing difficulty

**Alternatives Considered:**
- Time-based rage (fill over time): Less interactive, removes pickup incentive
- Kill-based rage (destroy obstacles): Conflicts with "avoid or dash through" core loop
- Infinite dash when full: Too powerful, removes challenge

**Trade-offs:**
- Passive decay can feel punishing if no pickups spawn; mitigated by tuned spawn rates
- 0.8s dash is short; feels great but requires precise timing

### Spawning: Weighted Random with Difficulty Curve

**What:** Spawner selects obstacle prefabs via weighted random (e.g., 50% poles, 30% barriers, 20% crates) and instantiates at configurable intervals. Difficulty curve (ScriptableObject) decreases spawn period from 1.25s → 0.75s over 120s, increases obstacle speed, and tightens gaps.

**Why:**
- Weighted spawn ensures variety without pure randomness
- Time-based curve (not distance) ensures consistent pacing regardless of player skill
- ScriptableObject enables designer tuning without code changes

**Alternatives Considered:**
- Pure random spawn: Can create unfair clusters or droughts
- Distance-based curve: Punishes slow players, rewards fast players (imbalanced)
- Fixed difficulty: Boring after first minute

**Trade-offs:**
- Time-based curve means skilled players don't accelerate difficulty faster (intentional for hyper-casual)
- Weighted spawn requires manual tuning to feel fair

### Scoring: Distance + Pickup Bonuses + Chain Multipliers

**What:** Score = (distance traveled × 2) + (taunt pickups × 5) + (consecutive pickup chains × 2). Chain window is 3 seconds; breaking chain resets multiplier.

**Why:**
- Distance ensures progress always scores
- Pickup bonuses reward aggressive play
- Chain multiplier adds skill expression without complexity

**Alternatives Considered:**
- Distance-only: Boring, no incentive to take risks
- Kill-based scoring: Conflicts with "avoid" loop
- Combo multipliers on multiple actions: Too complex for hyper-casual

**Trade-offs:**
- 3s chain window is tight; feels rewarding but not frustrating

### Monetization: Adapter Pattern for Ad Networks

**What:** IAdService interface with concrete adapters (LevelPlayAdapter, AdMobAdapter) selected via #define or ScriptableObject. Interstitial shows on game over (frequency capped), rewarded video for one-time revive or coin bonus. IAPService handles "Remove Ads" non-consumable and "Starter Pack" consumable (cosmetics).

**Why:**
- Adapter pattern decouples game logic from ad network specifics
- Frequency cap prevents ad fatigue
- One-time revive per run balances monetization with fairness
- "Remove Ads" is most common IAP for hyper-casual; starter pack adds variety

**Alternatives Considered:**
- Hardcoded ad network: Vendor lock-in, hard to switch
- Multiple rewarded revives: Trivializes difficulty, reduces ad value
- Subscription: Too heavy for hyper-casual audience

**Trade-offs:**
- Adapter adds slight complexity but pays off when switching networks
- One-time revive limits monetization but maintains game integrity

### UI: Minimal HUD + Results Screen

**What:** HUD shows only score, rage bar, and pause button. Results screen shows score, best score, and buttons (Replay, Revive [if eligible], Home). Settings screen has toggles for sound, haptics, language, privacy consent.

**Why:**
- Minimal HUD keeps focus on gameplay
- Results screen is the monetization point (interstitial + rewarded video)
- Settings are one-time setup, not in-game

**Alternatives Considered:**
- In-game coin counter: Distracting, coins unlock cosmetics (not urgent)
- Mid-run pause menu with shop: Interrupts flow

**Trade-offs:**
- Minimal HUD sacrifices information density for clarity (intentional for hyper-casual)

### Audio & Haptics: SFX + Haptics, No Voice Lines

**What:** Comedic SFX for tap, collision, dash, pickup. Light haptic feedback for tap and collision (iOS Taptic Engine). Optional background music per biome. No real-person voice lines.

**Why:**
- SFX + haptics provide juice without licensing risks
- No voice lines avoids IP sensitivity and localization complexity
- Music is optional to keep asset size low

**Alternatives Considered:**
- Real-person voice lines: IP risk, localization overhead, may not be comedic in all languages
- No haptics: Reduces juice, haptics are expected on modern iOS

**Trade-offs:**
- SFX-only limits personality but simplifies production

### Localization: CSV String Tables for tr-TR and en-US

**What:** All UI strings stored in CSV (key, tr-TR, en-US). Loaded at startup, selected by device language (fallback to en-US). No runtime language switching at v1.

**Why:**
- CSV is designer-friendly, easy to export/import
- Device language selection removes UI complexity
- tr-TR and en-US cover primary audiences

**Alternatives Considered:**
- Unity Localization package: Heavier, overkill for 2 languages
- JSON/XML: Less designer-friendly than CSV
- Runtime language switching: Adds UI complexity for v1

**Trade-offs:**
- CSV requires restart to switch languages (acceptable for v1)

### Analytics: Event Wrapper with Firebase/Unity Analytics Adapters

**What:** IAnalyticsService interface with FirebaseAdapter and UnityAnalyticsAdapter, selected via #define. Events: AppStart, StartRun, Die(obstacleType), Score, BestScore, RageActivated, TauntPickup, ShowAd(type), IAP(RemoveAds). Respects user consent toggle.

**Why:**
- Wrapper decouples game logic from analytics provider
- Lightweight events capture key funnel + monetization data
- Consent toggle ensures GDPR/CCPA compliance

**Alternatives Considered:**
- Hardcoded Firebase: Vendor lock-in
- Heavy event schema (position, velocity, etc.): Overkill, GDPR risk

**Trade-offs:**
- Adapter adds slight complexity but enables provider switching

### Performance: Object Pooling + Zero-GC Gameplay

**What:** All obstacles, pickups, and SFX use object pools (pre-instantiated, reused). No runtime Instantiate/Destroy during gameplay. Sprite atlases for batching. ASTC/ETC2 texture compression. Target 60 FPS on iPhone XR+.

**Why:**
- Object pooling eliminates GC spikes (zero-GC is critical for stable 60 FPS)
- Sprite atlases reduce draw calls
- Texture compression reduces memory and load times

**Alternatives Considered:**
- Runtime Instantiate/Destroy: Causes GC spikes, frame drops
- No pooling: Simpler but unacceptable for 60 FPS target

**Trade-offs:**
- Pooling adds upfront complexity but is mandatory for performance target

### Testing: Play Mode + Edit Mode Tests

**What:** Play Mode tests verify gameplay mechanics (jump impulse applies, rage dash grants invulnerability, revive resets state). Edit Mode tests verify data integrity (DifficultyCurve monotonic decrease, ObjectPool returns instances, Analytics wrapper no-throw).

**Why:**
- Play Mode tests catch gameplay regressions
- Edit Mode tests catch data/config errors before runtime

**Alternatives Considered:**
- Manual testing only: Slower, error-prone
- Full integration tests: Overkill for hyper-casual scope

**Trade-offs:**
- Tests add maintenance overhead but catch critical bugs early

### Project Structure: Assets/ Organized by System

**What:**
```
Assets/
├── Scripts/
│   ├── Core/ (GameManager, StateMachine, Services)
│   ├── Gameplay/ (PlayerController, RageMeter, Spawner, ObjectPool)
│   ├── UI/ (HUD, Pause, Results, Shop)
│   └── Data/ (ScriptableObjects)
├── Art/
├── SFX/
├── Localization/
├── Settings/
└── Scenes/
Compliance/
└── IP/
    └── NameRights.pdf
```

**Why:**
- System-based folders improve discoverability
- Separation of code, art, data follows Unity best practices

**Alternatives Considered:**
- Feature-based folders: Harder to navigate for small team
- Flat structure: Unmaintainable at scale

**Trade-offs:**
- System-based folders require discipline to maintain

## Risks / Trade-offs

### Risk: IP Sensitivity
**Mitigation:** Include NameRights.pdf in submission, avoid defamatory content, review all text/SFX with IP holders before release.

### Risk: Ad Fill Rates
**Mitigation:** Adapter pattern allows quick network switching. Monitor fill rates in analytics and pivot to higher-performing network.

### Risk: Performance on Older Devices (< iPhone XR)
**Mitigation:** Quality settings (lower resolution, disable effects). If needed, add device detection and auto-adjust.

### Risk: Analytics Provider Changes
**Mitigation:** IAnalyticsService abstraction allows swapping Firebase → Unity Analytics → custom backend with minimal code changes.

### Trade-off: One-Time Revive Limits Monetization
**Rationale:** Maintains game integrity and prevents pay-to-win perception. If metrics show demand, add "continue with ad" for 2nd revive in post-launch update.

### Trade-off: No Leaderboards at v1
**Rationale:** Reduces complexity and avoids server backend. If retention metrics are strong, add leaderboards in v1.1 via backend service.

## Migration Plan

N/A (new project, no migration required)

## Open Questions

1. **Ad Network Selection:** LevelPlay vs AdMob? Depends on fill rates and eCPM in target regions (Turkey, US).
   - **Decision:** Start with LevelPlay (better mediation), add AdMob adapter if needed.

2. **Android Stretch Goal:** If iOS launch is successful, prioritize Android or new content?
   - **Decision:** Defer until post-iOS-launch metrics are available.

3. **Cosmetics Unlock Mechanism:** Should cosmetics be unlockable via gameplay (coins) or IAP-only?
   - **Decision:** Coins from gameplay + optional IAP "Starter Pack" for early unlocks.

4. **Biome Unlock:** Should Boardwalk biome be unlocked via distance milestone, or available from start?
   - **Decision:** Available from start (reduces friction, both biomes are cosmetic).

5. **Difficulty Curve Tuning:** 120s ramp may be too fast or too slow. Requires playtesting.
   - **Decision:** Implement as ScriptableObject for easy iteration, gather metrics from alpha testers.
