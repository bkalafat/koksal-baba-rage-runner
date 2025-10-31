# Scoring Specification

## ADDED Requirements

### Requirement: Distance-Based Scoring
The system SHALL award points based on distance traveled by the player.

#### Scenario: Distance tracking
- **WHEN** game is in Playing state
- **THEN** ScoreService SHALL accumulate distance at rate of 1 point per 0.5 world units traveled

#### Scenario: Distance score calculation
- **WHEN** player has traveled 25.0 world units
- **THEN** ScoreService SHALL have awarded 50 distance points (25.0 / 0.5 = 50)

#### Scenario: Distance continues after revive
- **WHEN** player uses rewarded revive
- **THEN** ScoreService SHALL continue accumulating distance from current position (cumulative, not reset)

### Requirement: Pickup Bonus Scoring
The system SHALL award bonus points for collecting pickups.

#### Scenario: Taunt token bonus
- **WHEN** player collects a Taunt token
- **THEN** ScoreService SHALL immediately add +5 points to total score

#### Scenario: Coin bundle bonus
- **WHEN** player collects a coin bundle
- **THEN** ScoreService SHALL add +10 points to total score and award 5 coins (currency)

#### Scenario: Breakable crate bonus during dash
- **WHEN** player destroys a breakable crate during Rage Dash
- **THEN** ScoreService SHALL add +10 points to total score

### Requirement: Chain Multiplier System
The system SHALL award bonus points for consecutive pickup collections within a time window.

#### Scenario: Start pickup chain
- **WHEN** player collects first Taunt token
- **THEN** ScoreService SHALL start chain timer (3.0 second window) and set chain counter to 1

#### Scenario: Continue chain within window
- **WHEN** player collects second Taunt token within 3.0 seconds of first
- **THEN** ScoreService SHALL increment chain counter to 2, award base +5 points plus chain bonus (+2 points), reset chain timer to 3.0 seconds

#### Scenario: Chain multiplier calculation
- **WHEN** player has chain counter of N
- **THEN** ScoreService SHALL award basePickupBonus + (N * 2) points for next pickup in chain

#### Scenario: Break chain after timeout
- **WHEN** 3.0 seconds elapse without collecting another pickup
- **THEN** ScoreService SHALL reset chain counter to 0 and display "Chain Broken!" text on HUD

#### Scenario: Maximum chain display
- **WHEN** player achieves chain counter >= 5
- **THEN** ScoreService SHALL trigger "Mega Chain!" visual effect on HUD

### Requirement: Score Display
The system SHALL display current score on HUD during gameplay.

#### Scenario: Real-time score updates
- **WHEN** score increases during Playing state
- **THEN** HUD SHALL update score text within 1 frame (no delay)

#### Scenario: Score formatting
- **WHEN** score is displayed on HUD or Results screen
- **THEN** system SHALL format score as integer with comma separators (e.g., "1,234")

### Requirement: High Score Persistence
The system SHALL persist best score across game sessions using PlayerPrefs.

#### Scenario: Initial best score
- **WHEN** player launches game for first time
- **THEN** GameManager SHALL initialize best score to 0

#### Scenario: Best score comparison
- **WHEN** game ends and final score > best score
- **THEN** GameManager SHALL update best score to final score and save to PlayerPrefs key "BestScore"

#### Scenario: Best score display on results
- **WHEN** Results screen loads
- **THEN** UI SHALL display "Best: [bestScore]" below current run score

#### Scenario: New best score celebration
- **WHEN** final score > best score
- **THEN** Results screen SHALL display "New Best!" banner with particle effect and play celebration SFX

### Requirement: Score Analytics Events
The system SHALL log analytics events for scoring milestones.

#### Scenario: Final score tracking
- **WHEN** game ends (GameOver state)
- **THEN** system SHALL call AnalyticsService.LogEvent("Score", { "score": finalScore, "runTime": currentRunTime })

#### Scenario: Best score tracking
- **WHEN** player achieves new best score
- **THEN** system SHALL call AnalyticsService.LogEvent("BestScore", { "score": newBestScore })

### Requirement: Coin Currency System
The system SHALL track coins as persistent currency for cosmetic unlocks.

#### Scenario: Coin collection during run
- **WHEN** player collects coin bundle
- **THEN** ScoreService SHALL increment session coins counter by 5

#### Scenario: Coin persistence on game over
- **WHEN** game ends
- **THEN** GameManager SHALL add session coins to total coins and save to PlayerPrefs key "TotalCoins"

#### Scenario: Coin display on results
- **WHEN** Results screen loads
- **THEN** UI SHALL display "+[sessionCoins] coins earned this run"

#### Scenario: Total coins display in shop
- **WHEN** Shop screen is active
- **THEN** UI SHALL display "Total: [totalCoins] coins" at top of screen

### Requirement: Score Reset on New Run
The system SHALL reset session score and coin counters when starting a new run.

#### Scenario: Score reset on play button
- **WHEN** player taps Play button from MainMenu
- **THEN** ScoreService SHALL reset currentScore to 0, sessionCoins to 0, and chain counter to 0

#### Scenario: Score reset on replay button
- **WHEN** player taps Replay button from Results screen
- **THEN** ScoreService SHALL reset currentScore to 0, sessionCoins to 0, and chain counter to 0 (best score and total coins persist)
