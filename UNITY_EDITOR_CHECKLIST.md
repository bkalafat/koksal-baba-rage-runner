# Unity Editor Integration Checklist

This document tracks what has been implemented via code and what requires manual Unity Editor work.

## ✅ Completed via Code

### Core Systems
- [x] GameManager with full service initialization
- [x] ServiceLocator registrations for all services
- [x] Stub implementations for IAnalyticsService, IAdService, IIAPService
- [x] Scene transition logic in GameManager
- [x] PlayerPrefs persistence (best score, coins, purchases)

### Services
- [x] AudioService (SFX pooling, music playback)
- [x] HapticService (iOS haptic feedback)
- [x] TimeService
- [x] TouchInputService
- [x] LocalizationService
- [x] ScoreService (distance tracking, pickups, chains)

### Configuration Files
- [x] BootstrapDifficultyCurve.json (5 difficulty points over 120s)
- [x] DefaultSpawnConfig.json (obstacle weights)
- [x] DefaultTheme.json (color palette)

### Scenes (Scaffolds Created)
- [x] Bootstrap.unity scene file with GameManager GameObject

## 🔧 Requires Unity Editor Work

### Scenes Setup
- [ ] Open Bootstrap.unity and verify GameManager component
- [ ] Create MainMenu.unity scene
  - [ ] Add Canvas with MainMenuController
  - [ ] Add UI elements (title, buttons, best score text)
- [ ] Create Game.unity scene
  - [ ] Add Camera (orthographic, size 5)
  - [ ] Add Ground plane
  - [ ] Add Spawner GameObject with Spawner component
  - [ ] Add Player spawn point
  - [ ] Add HUD Canvas with HUDController
- [ ] Create Results.unity scene
  - [ ] Add Canvas with ResultsController
  - [ ] Add UI elements (score displays, buttons)

### Prefabs Creation
- [ ] Player prefab (64x128 sprite placeholder)
  - [ ] Add PlayerController component
  - [ ] Add RageMeter component
  - [ ] Add Rigidbody2D + BoxCollider2D
- [ ] StaticPole prefab (64x64 sprite)
  - [ ] Add Obstacle component
- [ ] MovingBarrier prefab (64x64 sprite)
  - [ ] Add MovingBarrier component
- [ ] BreakableCrate prefab (64x64 sprite)
  - [ ] Add BreakableCrate component
- [ ] TauntToken prefab (32x32 sprite)
  - [ ] Add Pickup component
- [ ] CoinBundle prefab (32x32 sprite)
  - [ ] Add Pickup component

### ScriptableObjects
- [ ] Create DifficultyCurve asset from BootstrapDifficultyCurve.json
- [ ] Create SpawnConfig asset from DefaultSpawnConfig.json
- [ ] Create Theme asset from DefaultTheme.json
- [ ] Assign ScriptableObjects to Spawner in Game scene

### UI Wiring
- [ ] Wire MainMenuController button references
- [ ] Wire HUDController text/image/button references
- [ ] Wire ResultsController text/button references
- [ ] Wire PauseMenuController (create overlay)
- [ ] Wire SettingsController (create overlay)
- [ ] Wire ShopController (create overlay)

### Project Settings
- [ ] Set up Build Settings with scene order (Bootstrap → MainMenu → Game → Results)
- [ ] Configure Player Settings for iOS (min version 14.0)
- [ ] Configure Input System (enable new Input System)
- [ ] Set up Physics2D collision matrix
- [ ] Configure Quality settings (target 60 FPS)

### Assets (Placeholders)
- [ ] Create placeholder sprites for player, obstacles, pickups
- [ ] Create placeholder SFX audio clips
- [ ] Create placeholder music track
- [ ] Import TextMeshPro essentials

## 📝 Next Steps

1. **Open Unity Editor** and load the project
2. **Verify Bootstrap scene** - Play it and check Console for service initialization logs
3. **Follow START_HERE_AFTER_SHOPPING.md** for detailed Unity Editor steps
4. **Create remaining scenes** (MainMenu, Game, Results)
5. **Create prefabs** with placeholder art
6. **Wire UI components** in each scene
7. **Test gameplay loop** end-to-end

## Estimated Time Remaining: 30-44 hours

- Phase 1: Bootstrap verification (1h)
- Phase 2: Scene creation (6-8h)
- Phase 3: Prefab creation (4-6h)
- Phase 4: UI wiring (6-8h)
- Phase 5: ScriptableObject creation (2-3h)
- Phase 6: Project settings (1-2h)
- Phase 7: Placeholder assets (4-6h)
- Phase 8: Testing and polish (6-10h)
