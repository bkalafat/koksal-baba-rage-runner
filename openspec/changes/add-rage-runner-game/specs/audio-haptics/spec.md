# Audio and Haptics Specification

## ADDED Requirements

### Requirement: Sound Effects Management
The system SHALL provide comedic sound effects for gameplay actions.

#### Scenario: Tap/hop SFX
- **WHEN** player taps screen and applies jump impulse
- **THEN** AudioService SHALL play "hop.wav" SFX (0.2s duration, comedic boing sound)

#### Scenario: Collision/death SFX
- **WHEN** player collides with obstacle and triggers game over
- **THEN** AudioService SHALL play "death.wav" SFX (0.5s duration, comedic crash sound)

#### Scenario: Pickup SFX
- **WHEN** player collects Taunt token
- **THEN** AudioService SHALL play "taunt.wav" SFX (0.3s duration, comedic laugh sound)
- **WHEN** player collects coin bundle
- **THEN** AudioService SHALL play "coin.wav" SFX (0.2s duration, coin jingle)

#### Scenario: Rage activation SFX
- **WHEN** player activates Rage Dash
- **THEN** AudioService SHALL play "rage_start.wav" SFX (0.4s duration, power-up sound) followed by looping "rage_dash.wav" (0.8s loop duration)

#### Scenario: Rage end SFX
- **WHEN** Rage Dash expires
- **THEN** AudioService SHALL stop looping "rage_dash.wav" and play "rage_end.wav" SFX (0.3s duration)

#### Scenario: Crate break SFX
- **WHEN** player breaks crate during Rage Dash
- **THEN** AudioService SHALL play "crate_break.wav" SFX (0.3s duration, wood splinter sound)

#### Scenario: UI button tap SFX
- **WHEN** player taps any UI button
- **THEN** AudioService SHALL play "button_tap.wav" SFX (0.1s duration, subtle click)

#### Scenario: New best score SFX
- **WHEN** Results screen displays "New Best!" banner
- **THEN** AudioService SHALL play "celebration.wav" SFX (1.0s duration, fanfare)

### Requirement: Background Music
The system SHALL provide optional background music per biome with looping.

#### Scenario: Street biome music
- **WHEN** game starts run with Street biome selected
- **THEN** AudioService SHALL play "street_theme.mp3" music (looping, 120 BPM, 2-minute loop)

#### Scenario: Boardwalk biome music
- **WHEN** game starts run with Boardwalk biome selected
- **THEN** AudioService SHALL play "boardwalk_theme.mp3" music (looping, 130 BPM, 2-minute loop)

#### Scenario: Music volume control
- **WHEN** game is in Playing state
- **THEN** music SHALL play at 70% volume (configurable in AudioConfig ScriptableObject)

#### Scenario: Music mute on pause
- **WHEN** game transitions to Paused state
- **THEN** AudioService SHALL pause music playback
- **WHEN** game resumes to Playing state
- **THEN** AudioService SHALL resume music from paused position

#### Scenario: Music disabled by settings
- **WHEN** player disables "Sound" toggle in Settings
- **THEN** AudioService SHALL stop all music and SFX playback

### Requirement: Audio Object Pooling
The system SHALL pre-warm AudioSource pool to avoid runtime instantiation.

#### Scenario: AudioSource pool initialization
- **WHEN** Bootstrap scene initializes AudioService
- **THEN** system SHALL create 10 AudioSource components (attached to pooled GameObjects), deactivate them, and add to pool

#### Scenario: Play SFX from pool
- **WHEN** AudioService.PlaySFX("hop") is called
- **THEN** AudioService SHALL retrieve inactive AudioSource from pool, set clip, set volume, activate GameObject, and play AudioSource.PlayOneShot()

#### Scenario: Return AudioSource to pool
- **WHEN** AudioSource finishes playing clip
- **THEN** AudioSource SHALL deactivate GameObject and return to pool for reuse

#### Scenario: Pool exhaustion
- **WHEN** all 10 AudioSources are active and PlaySFX() is called
- **THEN** AudioService SHALL interrupt oldest playing AudioSource (FIFO) and reuse it for new SFX

### Requirement: Haptic Feedback
The system SHALL provide light haptic feedback for key gameplay actions on iOS.

#### Scenario: Tap haptic
- **WHEN** player taps screen and applies jump impulse
- **THEN** HapticService SHALL trigger UIImpactFeedbackGenerator with light impact style

#### Scenario: Collision haptic
- **WHEN** player collides with obstacle and triggers game over
- **THEN** HapticService SHALL trigger UINotificationFeedbackGenerator with error notification style

#### Scenario: Pickup haptic
- **WHEN** player collects Taunt token or coin bundle
- **THEN** HapticService SHALL trigger UIImpactFeedbackGenerator with light impact style

#### Scenario: Rage activation haptic
- **WHEN** player activates Rage Dash
- **THEN** HapticService SHALL trigger UIImpactFeedbackGenerator with medium impact style

#### Scenario: Haptic disabled by settings
- **WHEN** player disables "Haptics" toggle in Settings
- **THEN** HapticService SHALL skip all haptic calls (no-op)

#### Scenario: Haptic unsupported platform
- **WHEN** game runs on Android or device without Taptic Engine
- **THEN** HapticService SHALL gracefully no-op (no crashes)

### Requirement: Audio Configuration
The system SHALL load audio parameters from AudioConfig ScriptableObject.

#### Scenario: Configurable SFX volume
- **WHEN** AudioService plays SFX
- **THEN** system SHALL apply SFX volume multiplier from AudioConfig (default 0.8)

#### Scenario: Configurable music volume
- **WHEN** AudioService plays background music
- **THEN** system SHALL apply music volume multiplier from AudioConfig (default 0.7)

#### Scenario: Designer tuning
- **WHEN** designer modifies AudioConfig asset values
- **THEN** changes SHALL take effect immediately on next Play Mode session without code changes

### Requirement: Audio Compression and Formats
The system SHALL use appropriate audio formats and compression for mobile.

#### Scenario: SFX format
- **WHEN** SFX audio clips are imported
- **THEN** Unity SHALL use Vorbis compression (quality 50) for iOS, load type "Decompress on Load", mono channel

#### Scenario: Music format
- **WHEN** music audio clips are imported
- **THEN** Unity SHALL use Vorbis compression (quality 70) for iOS, load type "Streaming", stereo channel

#### Scenario: Audio clip preloading
- **WHEN** Bootstrap scene initializes
- **THEN** AudioService SHALL preload all SFX clips into memory (AudioClip.LoadAudioData()) to avoid runtime I/O

### Requirement: Audio Analytics
The system SHALL respect analytics consent for audio playback tracking (no PII).

#### Scenario: Audio playback tracking (optional)
- **WHEN** player completes run
- **THEN** system MAY log AnalyticsService.LogEvent("AudioStats", { "sfxPlayed": sfxCount, "musicEnabled": isMusicOn }) if consent is granted

#### Scenario: No audio tracking without consent
- **WHEN** player disables "Privacy Consent" toggle
- **THEN** system SHALL NOT log any audio-related analytics events
