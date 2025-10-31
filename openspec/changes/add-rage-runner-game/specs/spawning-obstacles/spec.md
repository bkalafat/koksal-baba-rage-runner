# Spawning and Obstacles Specification

## ADDED Requirements

### Requirement: Obstacle Spawning System
The system SHALL spawn obstacles at regular intervals with weighted random selection.

#### Scenario: Initial spawn period
- **WHEN** game starts a new run
- **THEN** Spawner SHALL spawn obstacles every 1.25 seconds

#### Scenario: Weighted random obstacle selection
- **WHEN** Spawner selects next obstacle to spawn
- **THEN** system SHALL choose from obstacle prefabs using configured weights (e.g., 50% static poles, 30% moving barriers, 20% breakable crates)

#### Scenario: Spawn position randomization
- **WHEN** Spawner instantiates an obstacle
- **THEN** system SHALL place it at X = 15.0 (off-screen right) and random Y within range [-1.6, +1.6] world units

#### Scenario: Obstacle scrolling
- **WHEN** obstacle is spawned
- **THEN** obstacle GameObject SHALL have constant leftward velocity (moves toward player) matching current difficulty speed

### Requirement: Dynamic Difficulty Curve
The system SHALL adjust spawn rate, obstacle speed, and gap size over time using DifficultyCurve ScriptableObject.

#### Scenario: Difficulty progression over time
- **WHEN** run time reaches 30 seconds
- **THEN** spawn period SHALL decrease to 1.1 seconds (from 1.25s)
- **WHEN** run time reaches 60 seconds
- **THEN** spawn period SHALL decrease to 0.95 seconds
- **WHEN** run time reaches 120 seconds
- **THEN** spawn period SHALL decrease to minimum 0.75 seconds

#### Scenario: Obstacle speed increase
- **WHEN** run time reaches 30 seconds
- **THEN** obstacle scrolling speed SHALL increase by 10% (from base 6.0 to 6.6 units/sec)
- **WHEN** run time reaches 120 seconds
- **THEN** obstacle scrolling speed SHALL reach maximum 8.4 units/sec (+40%)

#### Scenario: Gap size tightening
- **WHEN** run time reaches 60 seconds
- **THEN** minimum gap between consecutive obstacles SHALL decrease from 3.5 to 3.0 world units
- **WHEN** run time reaches 120 seconds
- **THEN** minimum gap SHALL reach minimum 2.5 world units

#### Scenario: Difficulty curve lookup
- **WHEN** Spawner queries DifficultyCurve at specific timestamp
- **THEN** DifficultyCurve SHALL interpolate between keyframes (AnimationCurve) to return smooth spawnPeriod, obstacleSpeed, and minGap values

### Requirement: Obstacle Types
The system SHALL support three obstacle types: static poles, moving barriers, and breakable crates.

#### Scenario: Static pole obstacle
- **WHEN** static pole is spawned
- **THEN** obstacle SHALL remain at fixed Y position, scroll leftward at current difficulty speed, and trigger game over on collision (unless dashing)

#### Scenario: Moving barrier obstacle
- **WHEN** moving barrier is spawned
- **THEN** obstacle SHALL oscillate vertically within ±1.0 unit range (sine wave, 2.0s period) while scrolling leftward, and trigger game over on collision (unless dashing)

#### Scenario: Breakable crate obstacle
- **WHEN** breakable crate is spawned
- **THEN** obstacle SHALL scroll leftward and behave as solid obstacle (game over on collision) unless player is dashing, in which case it SHALL break and award +10 score

#### Scenario: Crate break animation
- **WHEN** player in Rage Dash collides with breakable crate
- **THEN** system SHALL play break particle effect, play break SFX, destroy crate GameObject, and award score

### Requirement: Object Pooling
The system SHALL use object pooling to reuse obstacle GameObjects and avoid runtime Instantiate/Destroy.

#### Scenario: Pool initialization
- **WHEN** game starts
- **THEN** ObjectPool SHALL pre-instantiate 20 instances of each obstacle prefab, deactivate them, and add to pool

#### Scenario: Spawn from pool
- **WHEN** Spawner needs to spawn an obstacle
- **THEN** ObjectPool SHALL return an inactive instance from pool, reset position/velocity, and activate GameObject

#### Scenario: Return to pool
- **WHEN** obstacle scrolls off-screen left (X < -5.0)
- **THEN** obstacle SHALL deactivate itself and return to ObjectPool for reuse

#### Scenario: Pool exhaustion
- **WHEN** ObjectPool has no available instances of requested type
- **THEN** ObjectPool SHALL instantiate a new instance at runtime (graceful degradation) and log warning

### Requirement: Pickup Spawning
The system SHALL spawn Taunt token and coin pickups at intervals independent of obstacles.

#### Scenario: Taunt token spawn rate
- **WHEN** game is in Playing state
- **THEN** Spawner SHALL spawn Taunt tokens every 4.0 seconds at random Y within [-1.6, +1.6]

#### Scenario: Coin bundle spawn rate
- **WHEN** game is in Playing state
- **THEN** Spawner SHALL spawn coin bundles every 8.0 seconds at random Y within [-1.6, +1.6]

#### Scenario: Pickup collision detection
- **WHEN** player's Collider2D enters pickup trigger
- **THEN** pickup SHALL invoke PlayerController.OnPickup(), award rage/coins, play pickup SFX, and return to pool

#### Scenario: Pickup scrolling
- **WHEN** pickup is spawned
- **THEN** pickup GameObject SHALL scroll leftward at current obstacle speed (same as obstacles)

### Requirement: Spawn Configuration
The system SHALL load spawn parameters from SpawnConfig ScriptableObject.

#### Scenario: Configurable spawn weights
- **WHEN** Spawner initializes
- **THEN** system SHALL load obstacle prefab references and weights from SpawnConfig asset (e.g., {staticPole: 50, movingBarrier: 30, breakableCrate: 20})

#### Scenario: Configurable gap constraints
- **WHEN** Spawner places next obstacle
- **THEN** system SHALL enforce minGap (default 3.5) and maxGap (default 6.0) from SpawnConfig, adjusted by difficulty curve

#### Scenario: Designer tuning
- **WHEN** designer modifies SpawnConfig asset values
- **THEN** changes SHALL take effect immediately on next Play Mode session without code changes

### Requirement: Spawner Lifecycle
The system SHALL start/stop spawning based on game state.

#### Scenario: Start spawning on game start
- **WHEN** game transitions to Playing state
- **THEN** Spawner SHALL begin spawning obstacles and pickups using configured periods

#### Scenario: Stop spawning on game over
- **WHEN** game transitions to GameOver state
- **THEN** Spawner SHALL immediately stop spawning new obstacles/pickups (existing obstacles continue scrolling)

#### Scenario: Clear obstacles on restart
- **WHEN** player restarts from Results screen
- **THEN** Spawner SHALL return all active obstacles/pickups to pool and reset spawn timers
