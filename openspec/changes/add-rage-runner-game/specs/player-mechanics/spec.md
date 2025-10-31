# Player Mechanics Specification

## ADDED Requirements

### Requirement: Tap Input Control
The system SHALL apply a fixed impulse to the player's Rigidbody2D when the screen is tapped.

#### Scenario: Single tap triggers hop
- **WHEN** player taps screen during Playing state
- **THEN** PlayerController SHALL set Rigidbody2D.velocity to (forwardSpeed, jumpSpeed) where forwardSpeed = 5.0 units/sec and jumpSpeed = 8.0 units/sec

#### Scenario: Ignore taps during non-playing states
- **WHEN** player taps screen during Paused, GameOver, or Results state
- **THEN** PlayerController SHALL ignore the input and not apply impulse

#### Scenario: Multiple rapid taps
- **WHEN** player taps screen multiple times within 0.1 second
- **THEN** PlayerController SHALL apply impulse for each tap (no cooldown at v1)

### Requirement: Physics-Based Movement
The system SHALL use Unity's Rigidbody2D component with gravity to control player movement.

#### Scenario: Gravity pulls player down
- **WHEN** player is in mid-air after a hop
- **THEN** Physics2D SHALL apply gravity scale of 2.5 to create snappy, arcade-style fall

#### Scenario: Ground collision detection
- **WHEN** player's Rigidbody2D collides with ground collider
- **THEN** player SHALL land and be eligible for next hop input

#### Scenario: Continuous forward movement
- **WHEN** game is in Playing state
- **THEN** player SHALL maintain constant forward velocity component (no deceleration)

### Requirement: Collision Detection
The system SHALL detect collisions with obstacles and trigger game over unless player is dashing.

#### Scenario: Obstacle collision while not dashing
- **WHEN** player's Collider2D enters obstacle trigger during normal state
- **THEN** PlayerController SHALL invoke GameOver() callback and transition to GameOver state

#### Scenario: Obstacle collision while dashing
- **WHEN** player's Collider2D enters obstacle trigger during Rage Dash
- **THEN** PlayerController SHALL ignore collision (invulnerable) and pass through obstacle

#### Scenario: Pickup collision
- **WHEN** player's Collider2D enters pickup trigger (Taunt token or coin)
- **THEN** PlayerController SHALL invoke OnPickup() callback, destroy pickup GameObject, and award score/rage

#### Scenario: Breakable crate collision while dashing
- **WHEN** player's Collider2D enters breakable crate trigger during Rage Dash
- **THEN** PlayerController SHALL destroy crate, play break SFX, and award bonus points (+10)

#### Scenario: Breakable crate collision while not dashing
- **WHEN** player's Collider2D enters breakable crate trigger during normal state
- **THEN** PlayerController SHALL treat crate as solid obstacle and invoke GameOver()

### Requirement: Player Sprite and Animation
The system SHALL display player sprite (Köksal Baba) with idle and hop animations.

#### Scenario: Idle animation on ground
- **WHEN** player is grounded (Rigidbody2D.velocity.y ≈ 0)
- **THEN** Animator SHALL play "Idle" animation clip (2-frame loop, 0.5s duration)

#### Scenario: Hop animation in air
- **WHEN** player is in mid-air (Rigidbody2D.velocity.y > 0.1 or < -0.1)
- **THEN** Animator SHALL play "Hop" animation clip (single frame, stretched pose)

#### Scenario: Rage dash visual effect
- **WHEN** player is in Rage Dash state
- **THEN** PlayerController SHALL enable temporary sprite tint (red overlay, alpha 0.5) and particle trail

### Requirement: Player Boundary Constraints
The system SHALL constrain player Y position to prevent out-of-bounds.

#### Scenario: Upper boundary
- **WHEN** player's Y position exceeds +5.0 world units
- **THEN** PlayerController SHALL clamp Y velocity to 0 and prevent further upward movement

#### Scenario: Lower boundary (death plane)
- **WHEN** player's Y position falls below -3.0 world units
- **THEN** PlayerController SHALL invoke GameOver() (fell off world)

#### Scenario: X position locked
- **WHEN** game is in Playing state
- **THEN** player's X position SHALL remain fixed at 2.0 world units (side-scroller perspective, obstacles scroll toward player)

### Requirement: Player Respawn After Revive
The system SHALL reset player state when rewarded revive is used.

#### Scenario: Revive respawn position
- **WHEN** player uses rewarded revive from Results screen
- **THEN** PlayerController SHALL reset position to (2.0, 0.0), reset velocity to (forwardSpeed, 0), and grant 1.0 second invulnerability

#### Scenario: Invulnerability after revive
- **WHEN** player respawns with revive invulnerability
- **THEN** PlayerController SHALL ignore all obstacle collisions for 1.0 second and display flashing sprite effect

#### Scenario: Invulnerability expiration
- **WHEN** revive invulnerability timer reaches 0
- **THEN** PlayerController SHALL restore normal collision detection and remove flashing effect

### Requirement: Player Input Responsiveness
The system SHALL apply hop impulse within 1 frame (16ms at 60 FPS) of tap detection.

#### Scenario: Low-latency tap response
- **WHEN** player taps screen
- **THEN** Input detection and Rigidbody2D impulse application SHALL occur in the same FixedUpdate() frame

#### Scenario: No input buffering
- **WHEN** player taps screen while in mid-air
- **THEN** PlayerController SHALL apply impulse immediately (allows double-hop at v1, intentional)
