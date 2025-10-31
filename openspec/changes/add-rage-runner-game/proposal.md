# Proposal: Köksal Baba: Rage Runner

## Why

Köksal Baba is a beloved public figure with comedic appeal that translates perfectly to a hyper-casual mobile runner. We have written permission to use the IP and a clear opportunity to deliver a family-friendly, slapstick game with 60-120 second sessions that appeal to casual mobile gamers. The simplicity of the one-touch mechanic combined with the unique "Rage Meter" system provides differentiation in an oversaturated genre. Ad-supported monetization with optional IAP removal keeps the barrier to entry low while enabling revenue generation.

## What Changes

- **Core Gameplay Loop**: One-touch runner with tap-to-hop mechanics, obstacle avoidance, and a unique Rage Meter system that grants temporary invulnerability and speed boosts
- **Rage Mechanic**: Pickup-driven meter that fills from "Taunt" tokens left by Riçıt, enabling strategic risk-reward gameplay through timed Rage Dashes
- **Spawning & Difficulty**: Dynamic obstacle spawner with 3-minute difficulty curve (spawn rate, speed, gap size) to maintain engagement without overwhelming new players
- **Scoring System**: Distance-based scoring with taunt pickup bonuses and chain multipliers (consecutive pickups within 3s window)
- **Monetization**: Interstitial ads on game over (frequency capped), rewarded video for one-time revive per run and optional coin bonuses, IAP for "Remove Ads" and starter cosmetic pack
- **Content**: 2 biomes (Street, Boardwalk), 3 obstacle types (static poles, moving barriers, breakable crates), 6 cosmetic items (3 hats, 3 outfits)
- **UI Systems**: Minimal HUD (score, rage bar, pause), results screen with replay/revive/home, settings for sound/haptics/language/privacy
- **Audio & Haptics**: Comedic SFX for actions (no voice lines), light haptic feedback for taps and collisions, optional music per biome
- **Compliance & Privacy**: No PII collection, ATT/IDFA only if required by ad network, Privacy Manifest, authorized IP usage via NameRights.pdf
- **Localization**: tr-TR and en-US at launch, string table management via CSV
- **Analytics**: Lightweight event tracking (AppStart, StartRun, Die, Score, BestScore, RageActivated, TauntPickup, ShowAd, IAP) with Firebase or Unity Analytics (switchable via define symbols)
- **Performance**: 60 FPS target on iPhone XR+, zero-GC gameplay through object pooling, prewarmed audio, sprite atlases
- **Testing**: Play Mode tests for core mechanics (jump impulse, rage dash invulnerability, revive state reset), Edit Mode tests for data validation (DifficultyCurve monotonic decrease, ObjectPool instance return, Analytics wrapper no-throw)
- **Unity Project Structure**: Organized Assets/ folder (Scripts/Core, Scripts/Gameplay, Scripts/UI, Scripts/Data, Art, SFX, Localization, Settings), ScriptableObject-driven configuration (DifficultyCurve, SpawnConfig, Theme), 4-scene architecture (Bootstrap, MainMenu, Game, Results)
- **Build & Platform**: Unity 2022/2023 LTS, IL2CPP, .NET 8 compatibility, iOS primary (iPhone portrait), Android secondary (stretch), build size < 150 MB, cold start < 2.5s on iPhone 12 class
- **Acceptance Criteria**: Full run from menu → game → results → rewarded revive → results without exceptions, 60 FPS steady during 3-minute profiled run with zero GC spikes > 2ms, App Store Privacy details completed, no unauthorized tracking

## Impact

- **Affected Specs**: This is a new project, so all capabilities are new additions
- **Affected Code**: Complete Unity 2D game project will be created from scratch
- **Capabilities Added**:
  - `core-gameplay` - Main game loop, state machine, scene flow
  - `player-mechanics` - Tap input, Rigidbody2D movement, collision detection
  - `rage-system` - Meter management, passive decay, dash activation, invulnerability
  - `spawning-obstacles` - Dynamic difficulty curve, weighted spawning, object pooling
  - `scoring` - Distance tracking, pickup bonuses, chain multipliers, high score persistence
  - `monetization` - Ad service abstraction (interstitial, rewarded), IAP service (remove ads, starter pack), receipt validation
  - `ui-systems` - HUD, pause, results, shop, settings, localized string tables
  - `audio-haptics` - SFX management, haptic feedback, music playback, volume controls
  - `content-assets` - Biomes, obstacles, pickups, cosmetics, sprite atlases, import presets
  - `compliance-privacy` - ATT/IDFA handling, Privacy Manifest, IP authorization, age rating
  - `localization` - String table management, tr-TR and en-US support, CSV import/export
  - `analytics` - Event wrapper, Firebase/Unity Analytics adapters, consent management
  - `performance` - Object pooling, zero-GC gameplay, sprite atlases, ASTC/ETC2 compression, 60 FPS target
  - `testing` - Play Mode tests for mechanics, Edit Mode tests for data validation

- **Dependencies**: Unity 2022/2023 LTS, Visual Studio 2022/2026 Insider, Unity LevelPlay or AdMob SDK, Firebase Analytics or Unity Analytics SDK
- **Risks**:
  - IP sensitivity requires careful content review and NameRights.pdf inclusion
  - Ad fill rates may vary; adapter abstraction mitigates network switching
  - Performance on older devices (< iPhone XR) may require quality settings
- **Out of Scope v1**: Leaderboards, social login, cloud save, server backend, Android builds (stretch goal)
