# Core Gameplay Specification

## ADDED Requirements

### Requirement: Game State Machine
The system SHALL manage game states (Bootstrap, MainMenu, Playing, Paused, GameOver, Results) with explicit transitions.

#### Scenario: Cold start initialization
- **WHEN** the app launches for the first time
- **THEN** Bootstrap scene SHALL load, initialize all services, and transition to MainMenu state

#### Scenario: Start run from main menu
- **WHEN** player taps "Play" button in MainMenu state
- **THEN** system SHALL transition to Playing state, load Game scene, and start spawning obstacles

#### Scenario: Pause during gameplay
- **WHEN** player taps pause button in Playing state
- **THEN** system SHALL transition to Paused state, freeze time scale, and display pause menu

#### Scenario: Resume from pause
- **WHEN** player taps resume button in Paused state
- **THEN** system SHALL transition back to Playing state and restore time scale to 1.0

#### Scenario: Die on obstacle collision
- **WHEN** player collides with obstacle while not dashing in Playing state
- **THEN** system SHALL transition to GameOver state, stop spawning, and prepare Results screen

#### Scenario: Navigate to results
- **WHEN** game is in GameOver state
- **THEN** system SHALL transition to Results state, display final score, and show monetization options (replay, revive, home)

### Requirement: Scene Management
The system SHALL provide four scenes: Bootstrap, MainMenu, Game, Results.

#### Scenario: Bootstrap scene initialization
- **WHEN** Bootstrap scene loads
- **THEN** system SHALL initialize GameManager, ServiceLocator, quality settings, target frame rate (60 FPS), and load MainMenu scene additively

#### Scenario: Main menu navigation
- **WHEN** MainMenu scene is active
- **THEN** system SHALL display Play button, Shop button, Settings button, and handle navigation to Game scene or Settings overlay

#### Scenario: Game scene lifecycle
- **WHEN** Game scene loads
- **THEN** system SHALL spawn player at starting position, initialize HUD, start spawner, and begin game loop

#### Scenario: Results scene display
- **WHEN** Results scene loads
- **THEN** system SHALL display final score, best score comparison, and action buttons (Replay, Revive [if eligible], Home)

### Requirement: Service Initialization
The system SHALL initialize all core services (Input, Time, Ads, IAP, Analytics) in Bootstrap scene before gameplay begins.

#### Scenario: Service locator setup
- **WHEN** Bootstrap scene Awake() is called
- **THEN** system SHALL register all service interfaces (IInputService, ITimeService, IAdService, IIAPService, IAnalyticsService) with concrete implementations

#### Scenario: Service dependency resolution
- **WHEN** gameplay code requests a service via ServiceLocator.Get<T>()
- **THEN** system SHALL return the registered singleton instance without null reference

#### Scenario: Service initialization order
- **WHEN** services are initialized
- **THEN** system SHALL initialize in order: Time → Input → Analytics → Ads → IAP to respect dependencies

### Requirement: Frame Rate and Quality Settings
The system SHALL target 60 FPS on iPhone XR class devices and above.

#### Scenario: Frame rate configuration on iOS
- **WHEN** Bootstrap scene sets up rendering
- **THEN** system SHALL set Application.targetFrameRate to 60 and enable VSync

#### Scenario: Quality settings for performance
- **WHEN** Bootstrap scene configures quality
- **THEN** system SHALL disable unnecessary quality features (shadows, anti-aliasing) and set texture quality to High on target devices

### Requirement: Persistent Game Manager
The system SHALL maintain a persistent GameManager singleton across scene transitions.

#### Scenario: GameManager persistence
- **WHEN** scene transitions occur (MainMenu → Game → Results)
- **THEN** GameManager SHALL persist via DontDestroyOnLoad and maintain session state (best score, coins, settings)

#### Scenario: Session data access
- **WHEN** gameplay systems need session data (best score, remove ads flag, language)
- **THEN** GameManager SHALL provide read/write access to PlayerPrefs-backed persistent data

### Requirement: Input Service Abstraction
The system SHALL provide an IInputService interface for tap input detection.

#### Scenario: Tap detection during gameplay
- **WHEN** player taps screen in Playing state
- **THEN** IInputService SHALL report tap event to registered listeners (PlayerController)

#### Scenario: Input blocking during pause
- **WHEN** game is in Paused or GameOver state
- **THEN** IInputService SHALL ignore gameplay input but allow UI navigation

### Requirement: Time Service Abstraction
The system SHALL provide an ITimeService interface for time scale management.

#### Scenario: Normal gameplay time
- **WHEN** game is in Playing state
- **THEN** ITimeService SHALL provide deltaTime at scale 1.0

#### Scenario: Paused time
- **WHEN** game is in Paused state
- **THEN** ITimeService SHALL set Time.timeScale to 0.0 and return 0 for deltaTime

#### Scenario: Slow-motion during rage dash
- **WHEN** player activates rage dash
- **THEN** ITimeService MAY apply time scale of 0.95 for juice effect (optional)
