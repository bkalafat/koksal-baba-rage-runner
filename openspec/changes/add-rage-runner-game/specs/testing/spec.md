# Testing Specification

## ADDED Requirements

### Requirement: Play Mode Tests for Gameplay Mechanics
The system SHALL provide Play Mode tests to verify core gameplay behaviors.

#### Scenario: Player jump impulse test
- **WHEN** Play Mode test simulates tap input
- **THEN** test SHALL assert PlayerController applies Rigidbody2D.velocity = (forwardSpeed, jumpSpeed) within tolerance ±0.1

#### Scenario: Rage dash invulnerability test
- **WHEN** Play Mode test activates Rage Dash and collides player with obstacle
- **THEN** test SHALL assert player remains alive (GameOver not triggered)

#### Scenario: Rage dash expiration test
- **WHEN** Play Mode test activates Rage Dash and waits 0.8 seconds
- **THEN** test SHALL assert player returns to normal state (invulnerability disabled, speed restored)

#### Scenario: Revive state reset test
- **WHEN** Play Mode test triggers GameOver and simulates rewarded revive
- **THEN** test SHALL assert player respawns at start position, RageMeter resets to 0.0, and 1.0s invulnerability is active

#### Scenario: Pickup collision test
- **WHEN** Play Mode test collides player with Taunt token
- **THEN** test SHALL assert RageMeter increases by 0.25, pickup GameObject is deactivated (returned to pool), and score increases by +5

#### Scenario: Score accumulation test
- **WHEN** Play Mode test simulates 10 world units of travel
- **THEN** test SHALL assert ScoreService.currentScore equals 20 (10 units / 0.5 = 20 points)

### Requirement: Edit Mode Tests for Data Validation
The system SHALL provide Edit Mode tests to verify ScriptableObject configurations.

#### Scenario: DifficultyCurve monotonic decrease test
- **WHEN** Edit Mode test loads DifficultyCurve asset
- **THEN** test SHALL assert spawnPeriod curve is monotonically decreasing (each keyframe time T has spawnPeriod(T) >= spawnPeriod(T+1))

#### Scenario: SpawnConfig weights sum test
- **WHEN** Edit Mode test loads SpawnConfig asset
- **THEN** test SHALL assert sum of obstacle weights (staticPole + movingBarrier + breakableCrate) > 0

#### Scenario: ObjectPool pre-instantiation test
- **WHEN** Edit Mode test initializes ObjectPool with 20 prefab instances
- **THEN** test SHALL assert pool.Count == 20 and all instances are inactive

#### Scenario: ObjectPool return instance test
- **WHEN** Edit Mode test requests instance from pool and returns it
- **THEN** test SHALL assert returned instance is inactive and pool.Count increases by 1

#### Scenario: Analytics wrapper no-throw test
- **WHEN** Edit Mode test calls AnalyticsService.LogEvent() with null parameters
- **THEN** test SHALL assert no exception is thrown (graceful null handling)

### Requirement: Integration Test for Full Run
The system SHALL provide integration test for menu → game → results flow.

#### Scenario: Full run integration test
- **WHEN** integration test starts from MainMenu scene
- **THEN** test SHALL:
  1. Simulate Play button tap → assert Game scene loads and Playing state activates
  2. Simulate 5 seconds of gameplay → assert score > 0 and obstacles spawn
  3. Simulate obstacle collision → assert GameOver state activates
  4. Assert Results scene displays with correct final score
  5. Simulate Replay button tap → assert game resets to Playing state with score = 0

#### Scenario: Rewarded revive integration test
- **WHEN** integration test simulates full run with revive
- **THEN** test SHALL:
  1. Simulate gameplay until GameOver
  2. Simulate rewarded video completion → assert player respawns with 1.0s invulnerability
  3. Simulate 5 more seconds of gameplay → assert score continues from pre-death value
  4. Simulate second GameOver → assert Revive button is disabled (one-time use)

### Requirement: Performance Test Automation
The system SHALL provide automated performance tests for GC and frame rate validation.

#### Scenario: GC allocation test
- **WHEN** performance test runs 60 frames of gameplay (1 second at 60 FPS)
- **THEN** test SHALL assert total GC.Alloc < 1 KB (Profiler.GetTotalAllocatedMemoryLong() delta)

#### Scenario: Frame time test
- **WHEN** performance test runs 180 frames of gameplay (3 seconds at 60 FPS)
- **THEN** test SHALL assert average frame time < 16.67ms and 95th percentile < 20ms

#### Scenario: Object pool churn test
- **WHEN** performance test spawns/despawns 100 obstacles
- **THEN** test SHALL assert zero Instantiate/Destroy calls (verified via Unity Test Framework custom profiler module)

### Requirement: UI Test Automation
The system SHALL provide automated UI tests for button interactions.

#### Scenario: Play button navigation test
- **WHEN** UI test simulates Play button tap in MainMenu
- **THEN** test SHALL assert GameManager.currentState == GameState.Playing and Game scene is loaded

#### Scenario: Pause button navigation test
- **WHEN** UI test simulates Pause button tap during Playing state
- **THEN** test SHALL assert GameManager.currentState == GameState.Paused and Time.timeScale == 0.0

#### Scenario: Shop button navigation test
- **WHEN** UI test simulates Shop button tap in MainMenu
- **THEN** test SHALL assert Shop overlay is active and visible

#### Scenario: Cosmetic unlock test
- **WHEN** UI test simulates cosmetic unlock with sufficient coins
- **THEN** test SHALL assert totalCoins decreases by unlock cost and cosmetic is marked as unlocked in PlayerPrefs

### Requirement: Ad Service Mock for Testing
The system SHALL provide mock IAdService implementation for testing without real ad networks.

#### Scenario: Mock interstitial ad test
- **WHEN** test registers MockAdService and triggers GameOver
- **THEN** MockAdService SHALL immediately invoke onAdClosed callback without delay

#### Scenario: Mock rewarded ad test
- **WHEN** test registers MockAdService and simulates rewarded video
- **THEN** MockAdService SHALL immediately invoke onRewardEarned callback with success = true

#### Scenario: Mock ad failure test
- **WHEN** test configures MockAdService to simulate ad load failure
- **THEN** AdService SHALL gracefully proceed without blocking gameplay

### Requirement: IAP Service Mock for Testing
The system SHALL provide mock IIAPService implementation for testing without real purchases.

#### Scenario: Mock IAP purchase test
- **WHEN** test registers MockIAPService and simulates "Remove Ads" purchase
- **THEN** MockIAPService SHALL immediately invoke onPurchaseComplete callback and set "RemoveAdsPurchased" flag

#### Scenario: Mock IAP failure test
- **WHEN** test configures MockIAPService to simulate purchase failure
- **THEN** IAPService SHALL display error message and not modify player data

### Requirement: Analytics Service Mock for Testing
The system SHALL provide mock IAnalyticsService implementation for testing without real analytics backend.

#### Scenario: Mock analytics event logging test
- **WHEN** test registers MockAnalyticsService and logs events
- **THEN** MockAnalyticsService SHALL store events in memory list for assertion (e.g., assert events.Contains("StartRun"))

#### Scenario: Analytics consent test
- **WHEN** test disables analytics consent and logs event
- **THEN** MockAnalyticsService SHALL no-op and event list remains empty

### Requirement: Localization Testing
The system SHALL provide tests for localization string lookup.

#### Scenario: Valid key lookup test
- **WHEN** test calls LocalizationService.GetString("HUD_SCORE") with language = en-US
- **THEN** test SHALL assert returned string equals "Score"

#### Scenario: Missing key fallback test
- **WHEN** test calls LocalizationService.GetString("INVALID_KEY")
- **THEN** test SHALL assert returned string equals "[INVALID_KEY]"

#### Scenario: Missing translation fallback test
- **WHEN** test calls LocalizationService.GetString("HUD_SCORE") with language = tr-TR and tr-TR column is empty
- **THEN** test SHALL assert returned string equals en-US value ("Score")

### Requirement: Collision Detection Testing
The system SHALL provide tests for collision layer filtering.

#### Scenario: Player-obstacle collision test
- **WHEN** test collides player with obstacle (not dashing)
- **THEN** test SHALL assert OnTriggerEnter2D is invoked and GameOver callback is called

#### Scenario: Player-pickup collision test
- **WHEN** test collides player with Taunt token
- **THEN** test SHALL assert OnTriggerEnter2D is invoked and OnPickup callback is called

#### Scenario: Obstacle-obstacle collision ignored test
- **WHEN** test places two obstacles overlapping
- **THEN** test SHALL assert no collision is detected (Physics2D layer matrix configured correctly)

### Requirement: Test Coverage Target
The system SHALL achieve minimum 70% code coverage for core gameplay systems.

#### Scenario: Coverage report generation
- **WHEN** all tests are executed with code coverage enabled
- **THEN** Unity Test Runner SHALL generate coverage report showing % coverage per assembly

#### Scenario: Coverage acceptance criteria
- **WHEN** coverage report is reviewed
- **THEN** core assemblies (Gameplay, Core, Data) SHALL have >= 70% line coverage

### Requirement: Continuous Testing (Manual CI)
The system SHALL support local CI script for automated test runs.

#### Scenario: Local CI script execution
- **WHEN** developer runs Build/CI/RunTests.ps1 (PowerShell script)
- **THEN** script SHALL:
  1. Launch Unity in batch mode
  2. Run all Edit Mode and Play Mode tests
  3. Generate test results XML and coverage report
  4. Exit with code 0 if all tests pass, code 1 if any test fails

#### Scenario: CI test failure notification
- **WHEN** CI script detects test failure
- **THEN** script SHALL output failed test names and stack traces to console
