# Performance Specification

## ADDED Requirements

### Requirement: Target Frame Rate
The system SHALL maintain 60 FPS on iPhone XR class devices and above.

#### Scenario: Frame rate configuration
- **WHEN** Bootstrap scene initializes
- **THEN** system SHALL set Application.targetFrameRate to 60 and enable VSync (QualitySettings.vSyncCount = 1)

#### Scenario: Steady frame rate during gameplay
- **WHEN** game is in Playing state for 3-minute run
- **THEN** profiler SHALL show average frame time < 16.67ms (60 FPS) with 95th percentile < 20ms

#### Scenario: Frame drops detection
- **WHEN** frame time exceeds 33.33ms (< 30 FPS)
- **THEN** profiler SHALL show spike cause (GC, rendering, physics) for optimization

### Requirement: Zero-GC Gameplay
The system SHALL achieve zero GC allocations > 1 KB per frame during gameplay.

#### Scenario: Object pooling for obstacles
- **WHEN** obstacles are spawned/despawned during gameplay
- **THEN** system SHALL use ObjectPool with pre-instantiated instances (no runtime Instantiate/Destroy)

#### Scenario: Object pooling for pickups
- **WHEN** pickups are spawned/despawned during gameplay
- **THEN** system SHALL use ObjectPool with pre-instantiated instances

#### Scenario: Object pooling for audio sources
- **WHEN** SFX are played during gameplay
- **THEN** system SHALL use AudioSource pool with pre-warmed instances (no AddComponent calls)

#### Scenario: String concatenation avoidance
- **WHEN** UI updates score text during gameplay
- **THEN** system SHALL use StringBuilder or cached formatted strings (no "Score: " + score string concatenation)

#### Scenario: Profiler GC validation
- **WHEN** 3-minute gameplay session is profiled
- **THEN** Profiler SHALL show zero GC.Alloc spikes > 2ms during Playing state

### Requirement: Sprite Atlas Batching
The system SHALL reduce draw calls via sprite atlases.

#### Scenario: Obstacles atlas batching
- **WHEN** multiple obstacles are visible on screen
- **THEN** Unity SHALL batch all obstacle sprites into single draw call (SpriteAtlas enabled)

#### Scenario: UI atlas batching
- **WHEN** HUD is visible
- **THEN** Unity SHALL batch all UI sprites into single draw call (Canvas batching + SpriteAtlas)

#### Scenario: Draw call target
- **WHEN** gameplay is profiled
- **THEN** total draw calls SHALL be < 20 per frame (target: 10-15 for typical gameplay)

### Requirement: Texture Memory Optimization
The system SHALL use ASTC compression for iOS to minimize memory usage.

#### Scenario: ASTC 6x6 compression for sprites
- **WHEN** sprite textures are imported
- **THEN** Unity SHALL apply ASTC 6x6 block compression (iOS platform override) for 4:1 compression ratio

#### Scenario: Max texture size enforcement
- **WHEN** sprite atlases are built
- **THEN** Unity SHALL enforce max atlas size 2048x2048 (prevents oversized textures)

#### Scenario: Texture memory profiler validation
- **WHEN** game is profiled on device
- **THEN** total texture memory SHALL be < 100 MB (target: 60-80 MB)

### Requirement: Physics Optimization
The system SHALL optimize Physics2D settings for mobile performance.

#### Scenario: Fixed timestep configuration
- **WHEN** Bootstrap scene initializes
- **THEN** system SHALL set Time.fixedDeltaTime to 0.02 (50 Hz physics, sufficient for 2D runner)

#### Scenario: Collision matrix optimization
- **WHEN** Physics2D settings are configured
- **THEN** system SHALL disable unnecessary layer collision checks (e.g., Obstacle-Obstacle, Pickup-Pickup)

#### Scenario: Physics profiler validation
- **WHEN** gameplay is profiled
- **THEN** Physics2D.Simulate() time SHALL be < 2ms per frame

### Requirement: Audio Preloading
The system SHALL preload all SFX clips to avoid runtime I/O.

#### Scenario: SFX preload on Bootstrap
- **WHEN** Bootstrap scene initializes AudioService
- **THEN** system SHALL call AudioClip.LoadAudioData() for all SFX clips (20-30 clips, ~5 MB total)

#### Scenario: Music streaming
- **WHEN** background music is played
- **THEN** system SHALL use streaming load type (AudioClip.loadType = Streaming) to avoid large memory allocation

#### Scenario: Audio memory profiler validation
- **WHEN** game is profiled
- **THEN** total audio memory SHALL be < 10 MB (SFX preloaded, music streaming)

### Requirement: Cold Start Performance
The system SHALL achieve cold start (app launch to MainMenu) < 2.5 seconds on iPhone 12 class.

#### Scenario: Bootstrap scene loading
- **WHEN** app launches
- **THEN** Bootstrap scene SHALL initialize services (ServiceLocator, GameManager, Ads, IAP, Analytics) in < 1.5 seconds

#### Scenario: MainMenu scene loading
- **WHEN** Bootstrap completes
- **THEN** MainMenu scene SHALL load additively and display in < 1.0 second

#### Scenario: Cold start profiler validation
- **WHEN** cold start is profiled with Xcode Instruments
- **THEN** Time to Interactive (TTI) SHALL be < 2.5 seconds

### Requirement: Build Size Optimization
The system SHALL achieve build size < 150 MB for iOS IPA.

#### Scenario: Asset compression
- **WHEN** Unity builds iOS project
- **THEN** sprites SHALL use ASTC 6x6, audio SHALL use Vorbis compression, and unneeded assets SHALL be excluded from build

#### Scenario: Code stripping
- **WHEN** Unity builds iOS project (IL2CPP)
- **THEN** Unity SHALL enable Medium or High stripping level to remove unused code

#### Scenario: Build Report validation
- **WHEN** Unity build completes
- **THEN** Build Report SHALL show IPA size < 150 MB (target: 80-120 MB)

### Requirement: Memory Budget
The system SHALL operate within 300 MB memory budget on target devices.

#### Scenario: Texture memory budget
- **WHEN** game is running
- **THEN** texture memory SHALL be < 100 MB

#### Scenario: Audio memory budget
- **WHEN** game is running
- **THEN** audio memory SHALL be < 10 MB

#### Scenario: Code and assets memory budget
- **WHEN** game is running
- **THEN** code, ScriptableObjects, and Unity engine memory SHALL be < 150 MB

#### Scenario: Managed heap budget
- **WHEN** game is running
- **THEN** managed heap (C# objects) SHALL be < 40 MB

#### Scenario: Memory profiler validation
- **WHEN** game is profiled on iPhone 12
- **THEN** total memory usage SHALL be < 300 MB (Xcode Instruments Memory Profiler)

### Requirement: Quality Settings
The system SHALL configure quality settings for mobile performance.

#### Scenario: Disable shadows
- **WHEN** QualitySettings are configured
- **THEN** shadows SHALL be disabled (shadowResolution = Disable, shadowCascades = 0)

#### Scenario: Disable anti-aliasing
- **WHEN** QualitySettings are configured
- **THEN** MSAA SHALL be disabled (antiAliasing = 0) to save GPU bandwidth

#### Scenario: Disable realtime reflections
- **WHEN** QualitySettings are configured
- **THEN** realtime reflections SHALL be disabled (realtimeReflectionProbes = false)

### Requirement: Profiling Validation
The system SHALL pass performance validation with Unity Profiler and Xcode Instruments.

#### Scenario: Unity Profiler session
- **WHEN** 3-minute gameplay is profiled in Unity Profiler (device connected)
- **THEN** session SHALL show 60 FPS avg, zero GC spikes > 2ms, draw calls < 20, Physics2D < 2ms

#### Scenario: Xcode Instruments session
- **WHEN** game is profiled with Xcode Instruments (Time Profiler + Allocations)
- **THEN** session SHALL show CPU usage < 50%, memory < 300 MB, zero memory leaks

#### Scenario: Acceptance criteria validation
- **WHEN** performance validation is complete
- **THEN** game SHALL pass: 60 FPS steady, zero GC spikes > 2ms, cold start < 2.5s, build size < 150 MB

### Requirement: Device Compatibility
The system SHALL support iPhone XR and newer (iOS 14+).

#### Scenario: Minimum iOS version
- **WHEN** Unity builds iOS project
- **THEN** Xcode project SHALL set Minimum Deployment Target to iOS 14.0

#### Scenario: Device testing matrix
- **WHEN** performance validation is conducted
- **THEN** testing SHALL include iPhone XR (baseline), iPhone 12 (target), iPhone 14 Pro (high-end)

#### Scenario: Older device handling
- **WHEN** game runs on iPhone 8 or older (< iPhone XR)
- **THEN** system MAY display warning "Performance may vary on older devices" but SHALL remain playable
