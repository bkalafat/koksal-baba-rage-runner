# Rage System Specification

## ADDED Requirements

### Requirement: Rage Meter Management
The system SHALL maintain a rage meter as a float value [0.0, 1.0] that increases with pickups and decays over time.

#### Scenario: Initial rage meter value
- **WHEN** game starts a new run
- **THEN** RageMeter SHALL initialize to 0.0

#### Scenario: Taunt pickup increases rage
- **WHEN** player collects a Taunt token pickup
- **THEN** RageMeter SHALL increase by 0.25 (clamped to max 1.0)

#### Scenario: Passive decay during gameplay
- **WHEN** game is in Playing state and rage meter > 0
- **THEN** RageMeter SHALL decrease by 0.1 per second (0.1 * deltaTime)

#### Scenario: No decay during pause
- **WHEN** game is in Paused state
- **THEN** RageMeter SHALL not apply passive decay

#### Scenario: Rage meter reaches full
- **WHEN** RageMeter value equals 1.0
- **THEN** system SHALL enable Rage Dash activation and display "Tap to Rage!" prompt on HUD

### Requirement: Rage Dash Activation
The system SHALL allow player to activate Rage Dash when meter is full (1.0).

#### Scenario: Activate rage dash at full meter
- **WHEN** player taps screen while RageMeter is 1.0
- **THEN** RageMeter SHALL immediately drop to 0.0, activate Rage Dash state for 0.8 seconds, and play rage activation SFX

#### Scenario: Cannot activate rage dash below full
- **WHEN** player taps screen while RageMeter is < 1.0
- **THEN** system SHALL apply normal hop (rage dash does not activate)

#### Scenario: Rage dash duration
- **WHEN** Rage Dash is activated
- **THEN** system SHALL maintain dash state for exactly 0.8 seconds before returning to normal state

### Requirement: Rage Dash Effects
The system SHALL grant invulnerability and speed boost during Rage Dash.

#### Scenario: Invulnerability during dash
- **WHEN** player is in Rage Dash state
- **THEN** PlayerController SHALL ignore all obstacle collisions (OnTriggerEnter2D with obstacles has no effect)

#### Scenario: Speed boost during dash
- **WHEN** player is in Rage Dash state
- **THEN** PlayerController SHALL increase forward velocity by 30% (forwardSpeed * 1.3)

#### Scenario: Visual feedback during dash
- **WHEN** player is in Rage Dash state
- **THEN** system SHALL display red sprite tint (Color.red with alpha 0.5), enable particle trail effect, and play looping rage SFX

#### Scenario: Dash expiration
- **WHEN** Rage Dash duration expires (0.8 seconds elapsed)
- **THEN** system SHALL restore normal forward speed, disable particle trail, remove sprite tint, and stop rage SFX

### Requirement: Rage Meter UI Display
The system SHALL display rage meter fill state on HUD.

#### Scenario: Rage bar visual representation
- **WHEN** game is in Playing state
- **THEN** HUD SHALL display a horizontal bar (0-100% fill) representing RageMeter value

#### Scenario: Rage bar color coding
- **WHEN** RageMeter value < 1.0
- **THEN** rage bar SHALL display orange color
- **WHEN** RageMeter value = 1.0
- **THEN** rage bar SHALL display red color and pulse animation

#### Scenario: Rage ready prompt
- **WHEN** RageMeter value = 1.0
- **THEN** HUD SHALL display "Tap to Rage!" text above rage bar (localized)

### Requirement: Rage Meter Persistence Across Revive
The system SHALL reset rage meter to 0.0 when player uses revive.

#### Scenario: Revive resets rage meter
- **WHEN** player uses rewarded revive from Results screen
- **THEN** RageMeter SHALL reset to 0.0 and clear any active dash state

### Requirement: Rage Analytics Event
The system SHALL log analytics event when Rage Dash is activated.

#### Scenario: Rage activation tracking
- **WHEN** player activates Rage Dash
- **THEN** system SHALL call AnalyticsService.LogEvent("RageActivated", { "meterValue": 1.0, "runTime": currentRunTime })

### Requirement: Rage Meter Configuration
The system SHALL load rage meter parameters from ScriptableObject configuration.

#### Scenario: Configurable rage parameters
- **WHEN** RageMeter initializes
- **THEN** system SHALL load gainPerTaunt (default 0.25), passiveDecayPerSecond (default 0.1), dashDurationSec (default 0.8), and speedMultiplier (default 1.3) from RageConfig ScriptableObject

#### Scenario: Designer tuning
- **WHEN** designer modifies RageConfig asset values
- **THEN** changes SHALL take effect immediately on next Play Mode session without code changes
