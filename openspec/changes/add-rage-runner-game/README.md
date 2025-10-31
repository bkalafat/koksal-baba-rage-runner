# Köksal Baba: Rage Runner - OpenSpec Proposal

## Overview

This is a complete OpenSpec proposal for **Köksal Baba: Rage Runner**, a hyper-casual mobile runner game for iOS featuring one-touch controls, a unique Rage Meter mechanic, and family-friendly slapstick humor.

**Change ID:** `add-rage-runner-game`

**Status:** ✅ Validated with `openspec validate add-rage-runner-game --strict`

---

## Project Vision

Hyper-casual, one-touch runner starring "Köksal Baba" with "Riçıt" as the prankster foil. Family-friendly slapstick. 60–120 second sessions. Simple art, high juice, ad-supported with optional ad removal.

**Target Platforms:**
- Primary: iOS (App Store) — iPhone-only portrait
- Secondary (stretch): Android (Google Play)

**Tech Stack:** Unity 2D (Unity 2022/2023 LTS), IL2CPP, .NET 8 compatibility, Visual Studio 2022/2026 Insider

---

## Proposal Structure

### Core Documents

| File | Description |
|------|-------------|
| `proposal.md` | High-level overview: Why, What Changes, Impact |
| `design.md` | Technical architecture, design decisions, trade-offs |
| `tasks.md` | Implementation checklist organized by epics (CoreLoop, Systems, UI, Content, Compliance, Polish) |

### Specifications (14 Capabilities)

All spec deltas are located in `specs/[capability]/spec.md` with `## ADDED Requirements` sections:

1. **core-gameplay** - Game state machine, scene management, service initialization
2. **player-mechanics** - Tap input, physics movement, collision detection
3. **rage-system** - Rage meter management, dash activation, passive decay
4. **spawning-obstacles** - Dynamic difficulty curve, weighted spawning, object pooling
5. **scoring** - Distance tracking, pickup bonuses, chain multipliers
6. **monetization** - Ad service abstraction, IAP, frequency capping
7. **ui-systems** - HUD, MainMenu, Results, Shop, Settings, localization
8. **audio-haptics** - SFX management, music, haptic feedback
9. **content-assets** - Biomes, sprites, cosmetics, sprite atlases
10. **compliance-privacy** - Privacy Manifest, ATT, IP authorization, GDPR/CCPA
11. **localization** - CSV string tables, tr-TR and en-US support
12. **analytics** - Event wrapper, Firebase/Unity Analytics adapters, consent
13. **performance** - 60 FPS target, zero-GC gameplay, object pooling, build size optimization
14. **testing** - Play Mode tests, Edit Mode tests, integration tests, mocks

### Scaffolds (Unity C# Code)

All starter code is located in `scaffolds/`:

| File | Description |
|------|-------------|
| `GameManager.cs` | Persistent singleton managing game state and scene transitions |
| `PlayerController.cs` | Tap input, Rigidbody2D movement, collision detection |
| `RageMeter.cs` | Rage meter fill, decay, dash activation |
| `ObjectPool.cs` | Generic object pool for reusing GameObjects |
| `Spawner.cs` | Obstacle and pickup spawning with difficulty curve |
| `ScoreService.cs` | Distance tracking, pickup bonuses, chain multipliers |
| `IAdService.cs` | Ad service interface + MockAdService |
| `IIAPService.cs` | IAP service interface + MockIAPService |
| `IAnalyticsService.cs` | Analytics service interface + MockAnalyticsService |
| `HUDController.cs` | HUD management (score, rage bar, pause button) |
| `DifficultyCurve.cs` | ScriptableObject for difficulty progression |
| `SpawnConfig.cs` | ScriptableObject for spawn weights and gaps |
| `Theme.cs` | ScriptableObject for biome themes (background, palette, music) |

### Artifacts

All supporting documents are located in `artifacts/`:

| File | Description |
|------|-------------|
| `Colors.json` | Color palette for placeholder art and biomes |
| `PrivacyPolicy.md` | Privacy policy (no PII collection, analytics consent) |
| `NameRights_PLACEHOLDER.md` | Placeholder for IP authorization document |
| `ThirdPartyNotices.txt` | Third-party SDK licenses and data usage |
| `AppStoreChecklist.md` | Pre-submission checklist for App Store compliance |

---

## Quick Start

### 1. Review the Proposal

```bash
# View proposal summary
cat openspec/changes/add-rage-runner-game/proposal.md

# View design decisions
cat openspec/changes/add-rage-runner-game/design.md

# View implementation tasks
cat openspec/changes/add-rage-runner-game/tasks.md
```

### 2. Explore Specifications

```bash
# List all capabilities
openspec show add-rage-runner-game --json --deltas-only

# Read specific capability
cat openspec/changes/add-rage-runner-game/specs/core-gameplay/spec.md
```

### 3. Use Scaffolds in Unity

1. Create new Unity 2022/2023 LTS project
2. Copy all `.cs` files from `scaffolds/` to `Assets/Scripts/`
3. Organize by namespace:
   - `RageRunner.Core` → `Assets/Scripts/Core/`
   - `RageRunner.Gameplay` → `Assets/Scripts/Gameplay/`
   - `RageRunner.Services` → `Assets/Scripts/Services/`
   - `RageRunner.Data` → `Assets/Scripts/Data/`
   - `RageRunner.UI` → `Assets/Scripts/UI/`
4. Create ScriptableObject assets from templates (DifficultyCurve, SpawnConfig, Theme)
5. Follow `tasks.md` checklist sequentially

### 4. Apply Artifacts

1. Import `Colors.json` palette for placeholder sprites
2. Include `PrivacyPolicy.md` in app submission materials
3. Obtain actual signed `NameRights.pdf` (replace placeholder)
4. Include `ThirdPartyNotices.txt` in app bundle or support page
5. Use `AppStoreChecklist.md` before submission

---

## Implementation Plan

Follow the 8 epics in `tasks.md`:

1. **Epic 1: Core Loop** (Bootstrap, Player, Rage, Spawning, Scoring)
2. **Epic 2: Systems** (Ads, IAP, Analytics)
3. **Epic 3: UI** (HUD, MainMenu, Results, Shop, Settings, Localization)
4. **Epic 4: Content** (Art, Sprites, Cosmetics, Animations, Particles)
5. **Epic 5: Audio & Haptics** (SFX, Music, Haptic Feedback)
6. **Epic 6: Compliance & Privacy** (Privacy Manifest, ATT, Documentation)
7. **Epic 7: Performance & Testing** (Optimization, Tests, Device Testing)
8. **Epic 8: Polish & Submission** (Visual/Audio Polish, Build, App Store Assets)

**Critical Path:** Epic 1 → Epic 3 → Epic 7 → Epic 8

**Parallelizable:** Epics 2, 4, 5 can proceed independently with mocks

---

## Acceptance Criteria

The proposal SHALL pass the following criteria before v1 release:

- ✅ Build size < 150 MB
- ✅ Cold start < 2.5s on iPhone 12 class
- ✅ 60 FPS steady during 3-minute run (zero GC spikes > 2ms)
- ✅ Full run from menu → game → results → rewarded revive → results without exceptions
- ✅ App Store Privacy details completed (no unauthorized tracking)

---

## Validation

```bash
# Validate proposal structure and requirements
openspec validate add-rage-runner-game --strict

# Expected output: "Change 'add-rage-runner-game' is valid"
```

---

## Key Design Decisions

### Architecture
- **Service-Oriented Singletons** with ScriptableObject configuration
- **Object Pooling** for zero-GC gameplay (obstacles, pickups, audio sources)
- **Adapter Pattern** for ads/IAP/analytics (swap providers via #define)

### Gameplay
- **Fixed Tap Impulse** (no variable jump height) for simplicity
- **Pickup-Driven Rage** (not time or kill-based) for strategic risk/reward
- **Time-Based Difficulty** (not distance-based) for consistent pacing

### Monetization
- **Interstitial on GameOver** (frequency capped, respects "Remove Ads" IAP)
- **One-Time Rewarded Revive** per run (balances monetization with fairness)

### Performance
- **60 FPS Target** on iPhone XR+ with ASTC compression, sprite atlases
- **Zero-GC Gameplay** via object pooling and preloaded audio

---

## Out of Scope (v1)

- Leaderboards, social login, cloud save, server backend
- Android builds (stretch goal, not primary target)
- Real-person voice lines (SFX only)
- Complex progression systems (cosmetics unlock with coins, no RPG mechanics)

---

## Next Steps

### Before Implementation
1. ✅ Review proposal with team/stakeholders
2. ✅ Obtain actual signed `NameRights.pdf` from IP holders
3. ✅ Decide on ad network (LevelPlay vs AdMob) based on fill rates in target regions

### During Implementation
1. Follow `tasks.md` checklist sequentially
2. Mark tasks complete as you progress
3. Run `openspec validate add-rage-runner-game --strict` periodically to ensure spec alignment
4. Commit scaffolds to Unity project and begin Epic 1 (Core Loop)

### After Implementation
1. Complete Epic 7 (Performance & Testing) acceptance criteria validation
2. Use `AppStoreChecklist.md` to prepare App Store submission
3. Archive proposal: `openspec archive add-rage-runner-game --yes` (after deployment)

---

## License

Proprietary. Third-party SDK licenses listed in `artifacts/ThirdPartyNotices.txt`.

---

## Contact

For questions about this proposal:
- **Project Lead:** [Your Name]
- **Email:** [contact email]
- **Repository:** [GitHub/Bitbucket URL]

---

**OpenSpec Version:** 1.0  
**Created:** October 31, 2025  
**Last Updated:** October 31, 2025  
**Status:** ✅ Validated and Ready for Implementation
