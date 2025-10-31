# Analytics Specification

## ADDED Requirements

### Requirement: Analytics Service Abstraction
The system SHALL provide IAnalyticsService interface with adapters for Firebase Analytics and Unity Analytics.

#### Scenario: Analytics service initialization
- **WHEN** Bootstrap scene initializes services
- **THEN** AnalyticsService SHALL initialize selected provider (Firebase or Unity Analytics based on #define USE_FIREBASE or USE_UA) and register event tracking

#### Scenario: Analytics provider selection via define
- **WHEN** project is compiled with USE_FIREBASE define
- **THEN** ServiceLocator SHALL register FirebaseAnalyticsAdapter as IAnalyticsService implementation
- **WHEN** project is compiled with USE_UA define
- **THEN** ServiceLocator SHALL register UnityAnalyticsAdapter as IAnalyticsService implementation

#### Scenario: Analytics initialization failure handling
- **WHEN** analytics provider fails to initialize (network error, SDK missing)
- **THEN** AnalyticsService SHALL log warning and no-op all event calls (graceful degradation)

### Requirement: Core Analytics Events
The system SHALL log key funnel and gameplay events.

#### Scenario: AppStart event
- **WHEN** app launches and Bootstrap scene initializes
- **THEN** AnalyticsService SHALL log event "AppStart" with parameters: { "platform": "iOS", "version": Application.version, "sessionId": GUID }

#### Scenario: StartRun event
- **WHEN** player starts new run (Play button or Replay button)
- **THEN** AnalyticsService SHALL log event "StartRun" with parameters: { "biome": "Street|Boardwalk" }

#### Scenario: Die event
- **WHEN** player collides with obstacle and triggers game over
- **THEN** AnalyticsService SHALL log event "Die" with parameters: { "obstacleType": "staticPole|movingBarrier|breakableCrate", "score": currentScore, "runTime": elapsedSeconds }

#### Scenario: Score event
- **WHEN** game ends (GameOver state)
- **THEN** AnalyticsService SHALL log event "Score" with parameters: { "score": finalScore, "runTime": elapsedSeconds, "tauntsCollected": tauntCount, "coinsCollected": sessionCoins }

#### Scenario: BestScore event
- **WHEN** player achieves new best score
- **THEN** AnalyticsService SHALL log event "BestScore" with parameters: { "score": newBestScore, "previousBest": oldBestScore }

### Requirement: Gameplay Mechanic Events
The system SHALL log events for unique mechanics (Rage Dash, pickups).

#### Scenario: RageActivated event
- **WHEN** player activates Rage Dash
- **THEN** AnalyticsService SHALL log event "RageActivated" with parameters: { "meterValue": 1.0, "runTime": elapsedSeconds }

#### Scenario: TauntPickup event
- **WHEN** player collects Taunt token
- **THEN** AnalyticsService SHALL log event "TauntPickup" with parameters: { "chainCount": currentChain, "meterValue": rageMetерCurrentValue }

### Requirement: Monetization Events
The system SHALL log events for ad impressions and IAP purchases.

#### Scenario: ShowAd event (interstitial)
- **WHEN** interstitial ad is displayed
- **THEN** AnalyticsService SHALL log event "ShowAd" with parameters: { "type": "interstitial", "placement": "gameOver", "adNetwork": "LevelPlay|AdMob" }

#### Scenario: ShowAd event (rewarded)
- **WHEN** rewarded video ad is displayed
- **THEN** AnalyticsService SHALL log event "ShowAd" with parameters: { "type": "rewarded", "placement": "revive|coinBonus", "adNetwork": "LevelPlay|AdMob" }

#### Scenario: RewardedRevive event
- **WHEN** player completes rewarded video and respawns
- **THEN** AnalyticsService SHALL log event "RewardedRevive" with parameters: { "score": scoreAtDeath, "respawnPosition": playerPosition }

#### Scenario: IAP event
- **WHEN** IAP purchase completes successfully
- **THEN** AnalyticsService SHALL log event "IAP" with parameters: { "productId": "remove_ads|starter_pack", "price": priceUSD, "currency": "USD" }

### Requirement: User Consent Enforcement
The system SHALL respect user consent toggle for analytics tracking.

#### Scenario: Consent check before event logging
- **WHEN** AnalyticsService.LogEvent() is called
- **THEN** service SHALL check PlayerPrefs "AnalyticsConsent" flag
- **IF** consent is false, service SHALL no-op (do not send event)
- **IF** consent is true, service SHALL send event to backend

#### Scenario: Consent default (opt-out model)
- **WHEN** app launches for first time
- **THEN** "AnalyticsConsent" SHALL default to true (opt-out, GDPR-compliant with privacy policy disclosure)

#### Scenario: Consent toggle change
- **WHEN** player disables "Privacy Consent" toggle in Settings
- **THEN** AnalyticsService SHALL stop logging new events immediately

### Requirement: Event Parameter Constraints
The system SHALL enforce parameter size and type constraints per analytics provider.

#### Scenario: String parameter length limit
- **WHEN** event parameter string exceeds 100 characters
- **THEN** AnalyticsService SHALL truncate string to 100 characters and log warning

#### Scenario: Numeric parameter range
- **WHEN** event parameter is numeric (int, float)
- **THEN** AnalyticsService SHALL ensure value is within valid range for provider (e.g., Firebase: -2^63 to 2^63-1 for int)

#### Scenario: Parameter key naming convention
- **WHEN** event is logged
- **THEN** parameter keys SHALL use camelCase (e.g., "obstacleType", "runTime", "sessionId")

### Requirement: Analytics Error Handling
The system SHALL handle analytics errors gracefully without crashing gameplay.

#### Scenario: Event logging exception
- **WHEN** AnalyticsService.LogEvent() throws exception (network timeout, SDK error)
- **THEN** service SHALL catch exception, log error to Console, and continue gameplay

#### Scenario: No-throw guarantee
- **WHEN** any AnalyticsService method is called
- **THEN** method SHALL NEVER throw uncaught exception to caller

### Requirement: Session Tracking
The system SHALL track session duration and engagement.

#### Scenario: Session start time
- **WHEN** app launches
- **THEN** AnalyticsService SHALL record session start timestamp (Time.realtimeSinceStartup)

#### Scenario: Session duration on app close
- **WHEN** app is closed or backgrounded
- **THEN** AnalyticsService SHALL calculate session duration and log event "AppClose" with parameters: { "sessionDuration": durationSeconds }

### Requirement: Debug Mode for Testing
The system SHALL provide debug logging for analytics events in editor.

#### Scenario: Editor debug logging
- **WHEN** game runs in Unity Editor and ANALYTICS_DEBUG define is set
- **THEN** AnalyticsService SHALL log all events to Console with full parameter details

#### Scenario: Production builds
- **WHEN** game is built for iOS (release configuration)
- **THEN** AnalyticsService SHALL NOT log events to device console (silent mode)

### Requirement: Analytics Configuration
The system SHALL load analytics parameters from AnalyticsConfig ScriptableObject.

#### Scenario: Configurable event throttling
- **WHEN** AnalyticsService initializes
- **THEN** system SHALL load event throttle limits from AnalyticsConfig (e.g., max 100 events per session to avoid quota exhaustion)

#### Scenario: Configurable provider endpoint
- **WHEN** AnalyticsService initializes
- **THEN** system SHALL load provider endpoint URL from AnalyticsConfig (default: Firebase or Unity Analytics cloud endpoints)

### Requirement: Analytics Data Retention
The system SHALL document data retention policies per provider.

#### Scenario: Firebase Analytics retention
- **WHEN** Firebase Analytics is used
- **THEN** data SHALL be retained per Firebase default policy (90 days for raw events, 14 months for aggregated reports)

#### Scenario: Unity Analytics retention
- **WHEN** Unity Analytics is used
- **THEN** data SHALL be retained per Unity default policy (90 days for raw events)

#### Scenario: Data deletion request
- **WHEN** user requests data deletion via support email
- **THEN** developer SHALL manually delete user analytics data from Firebase/Unity Analytics console (no automated API at v1)
