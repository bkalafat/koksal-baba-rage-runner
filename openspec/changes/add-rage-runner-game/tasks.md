# Implementation Tasks

## Epic 1: Core Loop

### 1.1 Bootstrap and Service Infrastructure
- [ ] 1.1.1 Create Bootstrap scene with ServiceLocator singleton
- [ ] 1.1.2 Implement IInputService interface and TouchInputService concrete class
- [ ] 1.1.3 Implement ITimeService interface and TimeService concrete class
- [ ] 1.1.4 Create GameManager singleton with state machine (Bootstrap, MainMenu, Playing, Paused, GameOver, Results)
- [ ] 1.1.5 Configure quality settings (disable shadows, AA, set targetFrameRate 60)
- [ ] 1.1.6 Set up DontDestroyOnLoad for persistent GameManager
- [ ] 1.1.7 Test: Bootstrap initializes services and transitions to MainMenu

### 1.2 Player Mechanics
- [ ] 1.2.1 Create PlayerController MonoBehaviour with Rigidbody2D (gravityScale 2.5)
- [ ] 1.2.2 Implement tap input detection and velocity impulse (forwardSpeed 5.0, jumpSpeed 8.0)
- [ ] 1.2.3 Add Collider2D (trigger) for collision detection with obstacles and pickups
- [ ] 1.2.4 Implement OnTriggerEnter2D for obstacle collision (invoke GameOver if not dashing)
- [ ] 1.2.5 Implement OnTriggerEnter2D for pickup collision (invoke OnPickup callback)
- [ ] 1.2.6 Add Y position clamping (upper +5.0, lower -3.0 triggers GameOver)
- [ ] 1.2.7 Test: Player hops on tap, dies on obstacle collision, survives when dashing

### 1.3 Rage System
- [ ] 1.3.1 Create RageMeter class with float value [0, 1], gainPerTaunt (0.25), passiveDecay (0.1/sec)
- [ ] 1.3.2 Implement RageMeter.Update() for passive decay (only in Playing state)
- [ ] 1.3.3 Implement RageMeter.AddRage(amount) for pickup gain
- [ ] 1.3.4 Implement RageMeter.ActivateDash() to trigger 0.8s invulnerability + 30% speed boost
- [ ] 1.3.5 Add dash timer and expiration logic to restore normal state
- [ ] 1.3.6 Integrate dash state with PlayerController collision detection
- [ ] 1.3.7 Test: Rage meter fills on pickups, decays over time, dash grants invulnerability

### 1.4 Obstacle Spawning
- [ ] 1.4.1 Create ObjectPool generic class with pre-instantiation and reuse logic
- [ ] 1.4.2 Create obstacle prefabs: StaticPole, MovingBarrier, BreakableCrate (64x64 placeholder sprites)
- [ ] 1.4.3 Create Spawner MonoBehaviour with spawn timer and weighted random selection
- [ ] 1.4.4 Implement obstacle scrolling (leftward velocity, current difficulty speed)
- [ ] 1.4.5 Implement obstacle off-screen detection (X < -5.0) and return to pool
- [ ] 1.4.6 Create DifficultyCurve ScriptableObject with AnimationCurve for spawnPeriod, obstacleSpeed, minGap
- [ ] 1.4.7 Integrate DifficultyCurve lookup in Spawner based on elapsed run time
- [ ] 1.4.8 Test: Obstacles spawn at intervals, scroll leftward, return to pool, difficulty increases over time

### 1.5 Pickups
- [ ] 1.5.1 Create TauntToken and CoinBundle prefabs (32x32 placeholder sprites)
- [ ] 1.5.2 Implement pickup spawning in Spawner (Taunt every 4.0s, Coin every 8.0s)
- [ ] 1.5.3 Implement pickup collision in PlayerController (award rage/coins, play SFX, return to pool)
- [ ] 1.5.4 Test: Pickups spawn, collide with player, award rage/coins, return to pool

### 1.6 Scoring
- [ ] 1.6.1 Create ScoreService with distance tracking (1 pt per 0.5 units)
- [ ] 1.6.2 Implement pickup bonus scoring (Taunt +5, Coin +10, Crate +10)
- [ ] 1.6.3 Implement chain multiplier (consecutive pickups within 3s window, +2 per chain)
- [ ] 1.6.4 Implement best score persistence (PlayerPrefs "BestScore")
- [ ] 1.6.5 Implement coin currency persistence (PlayerPrefs "TotalCoins")
- [ ] 1.6.6 Test: Score accumulates from distance and pickups, chain multiplier works, best score persists

### 1.7 Game Loop
- [ ] 1.7.1 Create Game scene with player at (2.0, 0.0), ground plane, spawner
- [ ] 1.7.2 Implement game start flow: MainMenu Play button → load Game scene → Playing state
- [ ] 1.7.3 Implement game over flow: obstacle collision → GameOver state → Results screen
- [ ] 1.7.4 Implement pause flow: Pause button → Paused state → Resume button → Playing state
- [ ] 1.7.5 Test: Full run from MainMenu → Playing → GameOver → Results without exceptions

## Epic 2: Systems (Ads, IAP, Analytics)

### 2.1 Ad Service
- [ ] 2.1.1 Create IAdService interface (ShowInterstitial, ShowRewarded, IsRewardedLoaded)
- [ ] 2.1.2 Create MockAdService implementation for testing (immediate callbacks)
- [ ] 2.1.3 Create LevelPlayAdapter stub (register with ServiceLocator when USE_LEVELPLAY defined)
- [ ] 2.1.4 Implement interstitial frequency capping (max 1 per 3 minutes, PlayerPrefs "LastInterstitialTime")
- [ ] 2.1.5 Implement "Remove Ads" IAP check before showing interstitials
- [ ] 2.1.6 Integrate interstitial on GameOver transition
- [ ] 2.1.7 Integrate rewarded video for revive and coin bonus
- [ ] 2.1.8 Test: Interstitial shows on GameOver (mocked), rewarded video triggers revive callback

### 2.2 IAP Service
- [ ] 2.2.1 Create IIAPService interface (InitializePurchasing, BuyProduct, RestorePurchases)
- [ ] 2.2.2 Create MockIAPService implementation for testing (immediate success callbacks)
- [ ] 2.2.3 Create UnityIAPAdapter stub (register with ServiceLocator)
- [ ] 2.2.4 Define product catalog: "remove_ads" (non-consumable, $2.99), "starter_pack" (consumable, $4.99)
- [ ] 2.2.5 Implement "Remove Ads" purchase flow and PlayerPrefs "RemoveAdsPurchased" flag
- [ ] 2.2.6 Implement "Starter Pack" purchase flow (award 500 coins + 3 cosmetics)
- [ ] 2.2.7 Implement restore purchases on app start (iOS receipt validation stub)
- [ ] 2.2.8 Test: IAP purchases complete (mocked), "Remove Ads" flag persists, Starter Pack awards items

### 2.3 Analytics Service
- [ ] 2.3.1 Create IAnalyticsService interface (LogEvent, SetUserProperty)
- [ ] 2.3.2 Create MockAnalyticsService implementation for testing (stores events in memory list)
- [ ] 2.3.3 Create FirebaseAnalyticsAdapter stub (register with ServiceLocator when USE_FIREBASE defined)
- [ ] 2.3.4 Implement consent check in LogEvent (PlayerPrefs "AnalyticsConsent")
- [ ] 2.3.5 Integrate core events: AppStart, StartRun, Die, Score, BestScore
- [ ] 2.3.6 Integrate gameplay events: RageActivated, TauntPickup
- [ ] 2.3.7 Integrate monetization events: ShowAd, IAP, RewardedRevive
- [ ] 2.3.8 Test: Events logged to MockAnalyticsService, consent toggle disables logging

## Epic 3: UI

### 3.1 HUD
- [ ] 3.1.1 Create HUD Canvas (Screen Space - Overlay, 1080x1920 reference resolution)
- [ ] 3.1.2 Add score text (top-left), rage bar (bottom-center), pause button (top-right)
- [ ] 3.1.3 Implement HUD.UpdateScore(int score) with formatted string (e.g., "1,234")
- [ ] 3.1.4 Implement HUD.UpdateRageBar(float fill) with smooth lerp (0.1s transition)
- [ ] 3.1.5 Implement "Tap to Rage!" prompt when rage = 1.0 (pulse animation)
- [ ] 3.1.6 Test: HUD updates in real-time, rage bar animates, pause button works

### 3.2 Main Menu
- [ ] 3.2.1 Create MainMenu scene with title, Play button, Shop button, Settings button, best score text
- [ ] 3.2.2 Implement Play button → transition to Playing state, load Game scene
- [ ] 3.2.3 Implement Shop button → show Shop overlay (modal)
- [ ] 3.2.4 Implement Settings button → show Settings overlay (modal)
- [ ] 3.2.5 Test: Main menu navigation works, best score displays correctly

### 3.3 Results Screen
- [ ] 3.3.1 Create Results scene with final score, best score, coins earned, action buttons
- [ ] 3.3.2 Implement "New Best!" banner with particle effect (only if new best)
- [ ] 3.3.3 Implement Replay button → reset game, transition to Playing state
- [ ] 3.3.4 Implement Revive button (Watch Ad) → show rewarded video, respawn on success
- [ ] 3.3.5 Implement Home button → transition to MainMenu state
- [ ] 3.3.6 Implement score count-up animation (0 to final over 1.0s, EaseOutQuad)
- [ ] 3.3.7 Test: Results screen displays correctly, Replay/Revive/Home buttons work

### 3.4 Shop
- [ ] 3.4.1 Create Shop overlay with total coins display, IAP products, cosmetics grid
- [ ] 3.4.2 Implement "Remove Ads" button → trigger IAPService.BuyProduct("remove_ads")
- [ ] 3.4.3 Implement "Starter Pack" button → trigger IAPService.BuyProduct("starter_pack")
- [ ] 3.4.4 Implement cosmetic unlock with coins (confirmation dialog, deduct coins, save to PlayerPrefs)
- [ ] 3.4.5 Implement "Not enough coins" error message with shake animation
- [ ] 3.4.6 Test: Shop displays products, cosmetic unlock works, IAP purchase triggers callbacks

### 3.5 Settings
- [ ] 3.5.1 Create Settings overlay with toggles: Sound, Haptics, Language, Privacy Consent
- [ ] 3.5.2 Implement Sound toggle → save PlayerPrefs "SoundEnabled", mute AudioService
- [ ] 3.5.3 Implement Haptics toggle → save PlayerPrefs "HapticsEnabled", disable HapticService
- [ ] 3.5.4 Implement Language dropdown (tr-TR, en-US) → save PlayerPrefs "Language", show "Restart required"
- [ ] 3.5.5 Implement Privacy Consent toggle → save PlayerPrefs "AnalyticsConsent"
- [ ] 3.5.6 Test: Settings toggles save preferences, restart message displays on language change

### 3.6 Localization
- [ ] 3.6.1 Create Localization.csv with columns: Key, tr-TR, en-US (UTF-8 BOM)
- [ ] 3.6.2 Define all string keys (HUD, MainMenu, Results, Shop, Settings)
- [ ] 3.6.3 Create LocalizationService with CSV parsing and GetString(key) lookup
- [ ] 3.6.4 Implement device language detection (Application.systemLanguage → "tr-TR" or "en-US")
- [ ] 3.6.5 Integrate localized strings in all UI elements
- [ ] 3.6.6 Test: Language switches correctly, missing keys return placeholder, fallback to en-US works

### 3.7 UI Polish
- [ ] 3.7.1 Implement button press feedback (scale down to 0.9x, 0.1s duration)
- [ ] 3.7.2 Implement scene transition fade (black screen, 0.3s fade out/in)
- [ ] 3.7.3 Implement rage bar pulse animation when full (1.0x to 1.1x scale, 0.5s loop)
- [ ] 3.7.4 Implement safe area handling for notch devices (iPhone X+)
- [ ] 3.7.5 Test: UI animations smooth, safe area respected on iPhone 14 Pro

## Epic 4: Content

### 4.1 Art Assets
- [ ] 4.1.1 Create placeholder sprites: player (64x128), obstacles (64x64), pickups (32x32)
- [ ] 4.1.2 Create Colors.json palette: {"primary": "#FF5733", "secondary": "#33C3FF", "background": "#F0F0F0", "obstacle": "#808080", "pickup": "#FFD700", "player": "#FF6347"}
- [ ] 4.1.3 Apply colors to placeholder sprites via SpriteRenderer.color
- [ ] 4.1.4 Create Street biome background (repeating tiles, gray palette)
- [ ] 4.1.5 Create Boardwalk biome background (ocean, pier tiles, blue/yellow palette)
- [ ] 4.1.6 Implement background parallax scrolling (3 layers: 0.5x, 0.75x, 1.0x)
- [ ] 4.1.7 Test: Backgrounds scroll with parallax effect, colors match palette

### 4.2 Sprite Atlases
- [ ] 4.2.1 Create ObstaclesAtlas (pack obstacle sprites, max 2048x2048)
- [ ] 4.2.2 Create UIAtlas (pack UI sprites, max 2048x2048)
- [ ] 4.2.3 Create CharacterAtlas (pack player and cosmetic sprites, max 2048x2048)
- [ ] 4.2.4 Configure sprite import presets: ASTC 6x6 (iOS), bilinear filter, max size 2048
- [ ] 4.2.5 Test: Sprite atlases batch correctly, draw calls < 20 per frame

### 4.3 Cosmetics
- [ ] 4.3.1 Create cosmetic sprites: Hat A (red cap), Hat B (sunglasses), Hat C (top hat)
- [ ] 4.3.2 Create cosmetic sprites: Outfit A (tracksuit), Outfit B (suit), Outfit C (beach attire)
- [ ] 4.3.3 Implement cosmetic unlock costs (Hat A: 50, Hat B: 100, Hat C: 150, Outfit A: 75, Outfit B: 125, Outfit C: 200)
- [ ] 4.3.4 Implement cosmetic equip system (save to PlayerPrefs "EquippedHat", "EquippedOutfit")
- [ ] 4.3.5 Implement cosmetic sprite overlay rendering on player
- [ ] 4.3.6 Test: Cosmetics unlock with coins, equip persists across sessions, sprites display correctly

### 4.4 Animations
- [ ] 4.4.1 Create Animator for player: Idle (2-frame loop, 0.5s), Hop (single frame)
- [ ] 4.4.2 Implement animation state transition: grounded (velocity.y ≈ 0) → Idle, airborne → Hop
- [ ] 4.4.3 Create Animator for MovingBarrier: Oscillate (2-frame loop, 0.5s)
- [ ] 4.4.4 Test: Player animation transitions correctly, MovingBarrier oscillates

### 4.5 Particle Effects
- [ ] 4.5.1 Create crate break particle effect (8 wood shards, 0.5s lifetime, random velocity)
- [ ] 4.5.2 Create rage dash trail particle effect (red circles, 10/sec spawn rate, 0.3s lifetime)
- [ ] 4.5.3 Create pickup collection particle effect (4 star sprites, outward velocity, 0.4s lifetime)
- [ ] 4.5.4 Create new best score confetti particle effect (50 particles, 1.0s duration)
- [ ] 4.5.5 Test: Particle effects trigger correctly, no performance impact

## Epic 5: Audio & Haptics

### 5.1 Audio Service
- [ ] 5.1.1 Create IAudioService interface (PlaySFX, PlayMusic, StopMusic)
- [ ] 5.1.2 Create AudioService with AudioSource pool (10 pre-instantiated sources)
- [ ] 5.1.3 Implement PlaySFX(clipName) with pool retrieval and PlayOneShot
- [ ] 5.1.4 Implement AudioSource return to pool after clip finishes
- [ ] 5.1.5 Implement PlayMusic(clipName) with looping and volume control (0.7 default)
- [ ] 5.1.6 Implement mute/unmute based on PlayerPrefs "SoundEnabled"
- [ ] 5.1.7 Test: SFX play from pool, music loops correctly, mute toggle works

### 5.2 Sound Effects
- [ ] 5.2.1 Create placeholder SFX: hop.wav (0.2s), death.wav (0.5s), taunt.wav (0.3s), coin.wav (0.2s)
- [ ] 5.2.2 Create placeholder SFX: rage_start.wav (0.4s), rage_dash.wav (0.8s loop), rage_end.wav (0.3s)
- [ ] 5.2.3 Create placeholder SFX: crate_break.wav (0.3s), button_tap.wav (0.1s), celebration.wav (1.0s)
- [ ] 5.2.4 Configure SFX import: Vorbis quality 50, mono, Decompress on Load
- [ ] 5.2.5 Preload all SFX on Bootstrap (AudioClip.LoadAudioData)
- [ ] 5.2.6 Test: All SFX play correctly, no runtime I/O delays

### 5.3 Background Music
- [ ] 5.3.1 Create placeholder music: street_theme.mp3 (2-min loop, 120 BPM)
- [ ] 5.3.2 Create placeholder music: boardwalk_theme.mp3 (2-min loop, 130 BPM)
- [ ] 5.3.3 Configure music import: Vorbis quality 70, stereo, Streaming load type
- [ ] 5.3.4 Integrate music playback on game start (biome-specific)
- [ ] 5.3.5 Test: Music loops seamlessly, streaming load works, no memory spikes

### 5.4 Haptic Service
- [ ] 5.4.1 Create IHapticService interface (TriggerLight, TriggerMedium, TriggerError)
- [ ] 5.4.2 Create HapticService with iOS UIImpactFeedbackGenerator integration
- [ ] 5.4.3 Implement TriggerLight for tap and pickup collisions
- [ ] 5.4.4 Implement TriggerMedium for rage dash activation
- [ ] 5.4.5 Implement TriggerError for obstacle collision death
- [ ] 5.4.6 Implement enable/disable based on PlayerPrefs "HapticsEnabled"
- [ ] 5.4.7 Implement platform check (no-op on Android or unsupported devices)
- [ ] 5.4.8 Test: Haptics trigger correctly on iOS, no-op gracefully on unsupported platforms

## Epic 6: Compliance & Privacy

### 6.1 Privacy Manifest
- [ ] 6.1.1 Create PrivacyInfo.xcprivacy template with no PII collection declaration
- [ ] 6.1.2 Add NSPrivacyTracking = YES if ATT required (ad network dependency)
- [ ] 6.1.3 Add NSPrivacyTrackingDomains for ad network domains
- [ ] 6.1.4 Add NSPrivacyCollectedDataTypes = ["Analytics"] if consent granted
- [ ] 6.1.5 Test: PrivacyInfo.xcprivacy included in Xcode project post-Unity build

### 6.2 ATT (App Tracking Transparency)
- [ ] 6.2.1 Implement ATT permission prompt with custom message (if ad network requires IDFA)
- [ ] 6.2.2 Implement ATT acceptance → save PlayerPrefs "ATTGranted", enable IDFA tracking
- [ ] 6.2.3 Implement ATT denial → disable IDFA, continue with non-personalized ads
- [ ] 6.2.4 Test: ATT prompt displays on first launch (if required), preference persists

### 6.3 Compliance Documentation
- [ ] 6.3.1 Create Compliance/IP/NameRights.pdf placeholder (include in submission package)
- [ ] 6.3.2 Create Compliance/PrivacyPolicy.md with plain-language privacy policy
- [ ] 6.3.3 Create Compliance/ThirdPartyNotices.txt with SDK licenses and data usage
- [ ] 6.3.4 Create Compliance/AppStoreChecklist.md with submission checklist
- [ ] 6.3.5 Test: All compliance documents present and accessible

### 6.4 Age Rating
- [ ] 6.4.1 Review all in-game content (text, sprites, SFX) for family-friendly compliance
- [ ] 6.4.2 Ensure no realistic violence (comedic death animations only)
- [ ] 6.4.3 Ensure no offensive language (localization review)
- [ ] 6.4.4 Ensure no gambling mechanics (no loot boxes, direct IAP only)
- [ ] 6.4.5 Test: Content review completed, age rating PEGI 7+ / ESRB Everyone target met

## Epic 7: Performance & Testing

### 7.1 Performance Optimization
- [ ] 7.1.1 Implement object pooling for obstacles, pickups, audio sources (zero runtime Instantiate/Destroy)
- [ ] 7.1.2 Configure Physics2D: fixed timestep 0.02, disable unnecessary layer collisions
- [ ] 7.1.3 Configure QualitySettings: disable shadows, AA, realtime reflections
- [ ] 7.1.4 Optimize string formatting in UI (use StringBuilder or cached strings)
- [ ] 7.1.5 Verify sprite atlases batch correctly (draw calls < 20 per frame)
- [ ] 7.1.6 Test: 3-minute profiler session shows 60 FPS avg, zero GC spikes > 2ms

### 7.2 Build Size Optimization
- [ ] 7.2.1 Enable IL2CPP with Medium/High code stripping for iOS
- [ ] 7.2.2 Configure ASTC 6x6 texture compression for all sprites/atlases
- [ ] 7.2.3 Configure Vorbis audio compression (SFX quality 50, music quality 70)
- [ ] 7.2.4 Exclude unused assets from build (editor-only assets in Editor/ folders)
- [ ] 7.2.5 Test: Build Report shows IPA size < 150 MB (target 80-120 MB)

### 7.3 Memory Optimization
- [ ] 7.3.1 Verify texture memory < 100 MB (profiler validation)
- [ ] 7.3.2 Verify audio memory < 10 MB (SFX preloaded, music streaming)
- [ ] 7.3.3 Verify managed heap < 40 MB (minimize allocations)
- [ ] 7.3.4 Test: Xcode Instruments Memory Profiler shows total memory < 300 MB

### 7.4 Cold Start Optimization
- [ ] 7.4.1 Minimize Bootstrap scene initialization (defer non-critical services)
- [ ] 7.4.2 Use async loading for MainMenu scene (LoadSceneAsync)
- [ ] 7.4.3 Preload critical assets only (SFX, UI atlases), defer others
- [ ] 7.4.4 Test: Xcode Instruments Time Profiler shows cold start < 2.5s on iPhone 12

### 7.5 Automated Testing
- [ ] 7.5.1 Write Play Mode test: player jump impulse applies correctly
- [ ] 7.5.2 Write Play Mode test: rage dash grants invulnerability
- [ ] 7.5.3 Write Play Mode test: revive resets state correctly
- [ ] 7.5.4 Write Edit Mode test: DifficultyCurve monotonic decrease
- [ ] 7.5.5 Write Edit Mode test: ObjectPool returns instances
- [ ] 7.5.6 Write Edit Mode test: Analytics wrapper no-throw
- [ ] 7.5.7 Create Build/CI/RunTests.ps1 script for local CI (batch mode test execution)
- [ ] 7.5.8 Test: All tests pass, code coverage >= 70% for core assemblies

### 7.6 Device Testing
- [ ] 7.6.1 Test on iPhone XR (baseline performance target)
- [ ] 7.6.2 Test on iPhone 12 (primary target device)
- [ ] 7.6.3 Test on iPhone 14 Pro (high-end, verify safe area handling)
- [ ] 7.6.4 Verify 60 FPS steady, no frame drops, cold start < 2.5s on all devices
- [ ] 7.6.5 Test: Acceptance criteria met (60 FPS, zero GC spikes, full run without exceptions)

## Epic 8: Polish & Submission

### 8.1 Visual Polish
- [ ] 8.1.1 Add rage dash visual effect: red sprite tint (alpha 0.5), particle trail
- [ ] 8.1.2 Add pickup collection visual effect: star particles
- [ ] 8.1.3 Add new best score visual effect: confetti particles, gold banner
- [ ] 8.1.4 Add button press visual feedback: scale animation, tap SFX
- [ ] 8.1.5 Test: Visual effects enhance juice without performance impact

### 8.2 Audio Polish
- [ ] 8.2.1 Verify all gameplay actions have corresponding SFX (tap, collision, pickup, dash)
- [ ] 8.2.2 Verify background music loops seamlessly per biome
- [ ] 8.2.3 Verify audio volume levels are balanced (SFX 0.8, music 0.7)
- [ ] 8.2.4 Test: Audio enhances experience without being intrusive

### 8.3 ScriptableObject Configuration
- [ ] 8.3.1 Create DifficultyCurve asset with keyframes: 0s (1.25s spawn), 30s (1.1s), 60s (0.95s), 120s (0.75s)
- [ ] 8.3.2 Create SpawnConfig asset with weights: staticPole (50), movingBarrier (30), breakableCrate (20)
- [ ] 8.3.3 Create RageConfig asset: gainPerTaunt (0.25), passiveDecay (0.1), dashDuration (0.8), speedMultiplier (1.3)
- [ ] 8.3.4 Create AudioConfig asset: sfxVolume (0.8), musicVolume (0.7)
- [ ] 8.3.5 Create Theme assets: Street (gray palette, street_theme.mp3), Boardwalk (blue/yellow palette, boardwalk_theme.mp3)
- [ ] 8.3.6 Test: Designer can modify config assets without code changes, values apply in Play Mode

### 8.4 iOS Build Setup
- [ ] 8.4.1 Configure Unity iOS build settings: IL2CPP, minimum deployment target iOS 14.0, target SDK device (iPhone)
- [ ] 8.4.2 Generate Xcode project from Unity (File → Build Settings → iOS → Build)
- [ ] 8.4.3 Add PrivacyInfo.xcprivacy to Xcode project
- [ ] 8.4.4 Configure signing & capabilities (select development team, enable Push Notifications if needed for ads)
- [ ] 8.4.5 Build IPA for TestFlight submission
- [ ] 8.4.6 Test: IPA builds successfully, installs on device via TestFlight

### 8.5 App Store Assets
- [ ] 8.5.1 Create app icon (1024x1024 PNG, no transparency)
- [ ] 8.5.2 Capture 6.7" screenshots (iPhone 14 Pro Max resolution: 1290x2796)
- [ ] 8.5.3 Capture 5.5" screenshots (iPhone 8 Plus resolution: 1242x2208)
- [ ] 8.5.4 Write 15-30s App Preview script (gameplay trailer)
- [ ] 8.5.5 Write App Store description (tr-TR and en-US)
- [ ] 8.5.6 Test: All assets meet App Store guidelines

### 8.6 Final Acceptance Testing
- [ ] 8.6.1 Run full game session: MainMenu → Playing → GameOver → Results → Rewarded Revive → Results → Replay
- [ ] 8.6.2 Verify no exceptions logged in Console during full session
- [ ] 8.6.3 Verify 60 FPS steady during 3-minute profiler session (zero GC spikes > 2ms)
- [ ] 8.6.4 Verify build size < 150 MB
- [ ] 8.6.5 Verify cold start < 2.5s on iPhone 12
- [ ] 8.6.6 Verify App Store Privacy details completed (no unauthorized tracking)
- [ ] 8.6.7 Test: All acceptance criteria passed

## Dependencies & Parallel Work

**Can be parallelized:**
- Epic 1 (Core Loop) and Epic 2 (Systems) have minimal dependencies; services can be mocked early
- Epic 4 (Content) can proceed independently with placeholder art
- Epic 5 (Audio & Haptics) can proceed independently with placeholder audio

**Sequential dependencies:**
- Epic 3 (UI) depends on Epic 1 (GameManager state machine)
- Epic 7 (Performance & Testing) depends on all gameplay features (Epics 1-5)
- Epic 8 (Polish & Submission) depends on all prior epics

**Critical path:**
1. Epic 1.1-1.7 (Core Loop) → enables gameplay testing
2. Epic 3.1-3.7 (UI) → enables user interaction
3. Epic 7.1-7.6 (Performance & Testing) → validates acceptance criteria
4. Epic 8.6 (Final Acceptance Testing) → submission readiness
