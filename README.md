# Köksal Baba: Rage Runner

A hyper-casual 2D endless runner for iOS featuring the beloved Turkish personality Köksal Baba. Built with Unity 2022/2023 LTS.

## Overview

**Genre**: Hyper-Casual Mobile Runner  
**Platform**: iOS (14.0+), iPhone Portrait  
**Target Audience**: Casual mobile gamers seeking 60-120 second sessions  
**Monetization**: Ad-supported with optional IAP (Remove Ads, Starter Pack)

## Core Mechanics

- **One-Touch Controls**: Tap anywhere to hop forward
- **Rage System**: Collect "Taunt" tokens to fill rage meter, activate dash for invulnerability
- **Dynamic Difficulty**: Spawn rate and obstacle speed increase over 2-3 minute runs
- **Scoring**: Distance-based (1 pt per 0.5 units) + pickup bonuses + chain multipliers
- **Cosmetics**: Unlock hats and outfits with earned coins

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/           # Services, GameManager, ServiceLocator
│   ├── Gameplay/       # Player, obstacles, pickups, spawning, scoring
│   ├── UI/             # HUD, menus, shop, settings
│   └── Data/           # ScriptableObjects (DifficultyCurve, SpawnConfig, Theme)
├── Art/                # Sprites, atlases, placeholder art
├── SFX/                # Audio clips (placeholder)
├── Localization/       # CSV string tables (tr-TR, en-US)
├── Settings/           # Quality settings, Privacy Manifest
└── Scenes/             # Bootstrap, MainMenu, Game, Results

Compliance/
├── IP/                 # NameRights.md (IP authorization)
├── PrivacyPolicy.md    # GDPR/CCPA privacy policy
├── ThirdPartyNotices.txt
└── AppStoreChecklist.md
```

## Quick Start

### Development Setup

1. **Install Unity**: 2022 LTS or 2023 LTS recommended
2. **Install Visual Studio**: 2022 Community or 2026 Insider
3. **Open Project**: Open this folder in Unity Hub
4. **Configure iOS**: File → Build Settings → iOS → Switch Platform

### Run in Editor

1. Open scene: `Assets/Scenes/Bootstrap.unity`
2. Press Play (Unity Editor supports mouse input as tap substitute)
3. Navigate: MainMenu → Play → Game

### Build for iOS

See [BUILD_INSTRUCTIONS.md](BUILD_INSTRUCTIONS.md) for detailed iOS build steps.

## Architecture

### Service-Oriented Design

Core systems are implemented as services accessed via `ServiceLocator`:

- **IInputService**: Touch/mouse input abstraction
- **ITimeService**: Time queries (Time.time, deltaTime, timeScale)
- **IAudioService**: SFX and music playback with object pooling
- **IHapticService**: iOS Taptic Engine feedback
- **IAdService**: Ad network abstraction (LevelPlay/AdMob adapters)
- **IIAPService**: In-app purchase abstraction (Unity IAP adapter)
- **IAnalyticsService**: Analytics abstraction (Firebase/Unity Analytics adapters)
- **LocalizationService**: CSV-based string table management

### State Machine

`GameManager` manages game flow via state machine:

```
Bootstrap → MainMenu → Playing ⇄ Paused → GameOver → Results
                          ↓
                      (death/revive loop)
```

### Performance Strategy

- **Object Pooling**: Zero-GC gameplay (obstacles, pickups, AudioSources)
- **Sprite Atlases**: Batching for < 20 draw calls per frame
- **Texture Compression**: ASTC 6x6 (iOS), ETC2 (Android fallback)
- **Target**: 60 FPS on iPhone XR+, < 150 MB build, < 2.5s cold start

## Compliance & Privacy

- **No PII Collection**: Anonymous analytics only, user consent required
- **ATT/IDFA**: Optional, only if required by ad network
- **Privacy Manifest**: `Assets/Settings/PrivacyInfo.xcprivacy` included in Xcode build
- **IP Authorization**: `Compliance/IP/NameRights.md` must be signed and submitted to App Store

## Localization

Supported languages at launch:
- **en-US**: English (United States)
- **tr-TR**: Turkish (Turkey)

String table: `Assets/Localization/Strings.csv`

## Implementation Status

This project follows the [OpenSpec](openspec/project.md) workflow. Implementation tasks are tracked in [tasks.md](openspec/changes/add-rage-runner-game/tasks.md).

### Current Status (October 31, 2025)

**✅ Completed**:
- Core architecture (ServiceLocator, GameManager, state machine)
- Service interfaces (Input, Time, Audio, Haptics, Ads, IAP, Analytics)
- Player mechanics (PlayerController, RageMeter)
- Obstacle system (Obstacle base class, BreakableCrate, MovingBarrier)
- Pickup system (Taunt tokens, Coins)
- Spawning system (ObjectPool, Spawner, DifficultyCurve)
- Scoring system (ScoreService with distance, pickups, chains)
- UI framework (HUD, MainMenu, Results, Pause, Settings, Shop)
- Localization service (CSV string tables)
- Compliance artifacts (Privacy Policy, Third-Party Notices, App Store Checklist)
- Project structure and build configuration

**🚧 In Progress**:
- Scene setup (Bootstrap, MainMenu, Game, Results)
- Prefab creation (player, obstacles, pickups)
- Placeholder art and audio assets
- Ad/IAP/Analytics adapter implementations

**📋 Remaining** (see tasks.md for details):
- Epic 4: Content (art assets, sprite atlases, animations, particles)
- Epic 5: Audio & Haptics (SFX files, music, haptic integration)
- Epic 6: Compliance & Privacy (ATT integration, final review)
- Epic 7: Performance & Testing (optimization, automated tests, profiling)
- Epic 8: Polish & Submission (visual polish, ScriptableObject configs, App Store assets)

## Testing

### Play Mode Tests
- Player jump impulse applies correctly
- Rage dash grants invulnerability
- Revive resets state correctly

### Edit Mode Tests
- DifficultyCurve monotonic decrease
- ObjectPool returns instances
- Analytics wrapper no-throw

Run tests: Window → General → Test Runner

## Metrics & Targets

- **Build Size**: < 150 MB (target 80-120 MB)
- **Cold Start**: < 2.5s on iPhone 12
- **FPS**: 60 steady during gameplay
- **GC Spikes**: < 2ms during gameplay
- **Memory**: < 300 MB total

## References

- [OpenSpec Proposal](openspec/changes/add-rage-runner-game/proposal.md)
- [Design Document](openspec/changes/add-rage-runner-game/design.md)
- [Implementation Tasks](openspec/changes/add-rage-runner-game/tasks.md)
- [Privacy Policy](Compliance/PrivacyPolicy.md)
- [App Store Checklist](Compliance/AppStoreChecklist.md)

## License

All rights reserved. This project uses the authorized likeness of Köksal Baba and Riçıt. See `Compliance/IP/NameRights.md` for details.

---

**Developer**: [Your Name]  
**Unity Version**: 2022.3 LTS / 2023.3 LTS  
**Target SDK**: iOS 14.0+  
**Last Updated**: October 31, 2025
