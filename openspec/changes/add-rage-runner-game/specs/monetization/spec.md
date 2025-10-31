# Monetization Specification

## ADDED Requirements

### Requirement: Ad Service Abstraction
The system SHALL provide IAdService interface with adapters for Unity LevelPlay and AdMob.

#### Scenario: Ad service initialization
- **WHEN** Bootstrap scene initializes services
- **THEN** AdService SHALL initialize selected ad network (LevelPlay or AdMob based on #define or config) and register callbacks

#### Scenario: Ad network selection via define
- **WHEN** project is compiled with USE_LEVELPLAY define
- **THEN** ServiceLocator SHALL register LevelPlayAdapter as IAdService implementation
- **WHEN** project is compiled with USE_ADMOB define
- **THEN** ServiceLocator SHALL register AdMobAdapter as IAdService implementation

#### Scenario: Ad initialization failure handling
- **WHEN** ad network fails to initialize (network error, SDK missing)
- **THEN** AdService SHALL log error, disable ad features gracefully, and allow gameplay to continue

### Requirement: Interstitial Ads
The system SHALL display interstitial ads on game over with frequency capping.

#### Scenario: Show interstitial on game over
- **WHEN** game transitions to GameOver state
- **THEN** AdService SHALL check frequency cap and display interstitial ad if eligible (max 1 per 3 minutes)

#### Scenario: Frequency cap enforcement
- **WHEN** last interstitial was shown less than 3 minutes ago
- **THEN** AdService SHALL skip interstitial display and proceed to Results screen

#### Scenario: Interstitial load failure
- **WHEN** interstitial ad fails to load (no fill, network error)
- **THEN** AdService SHALL log event "AdLoadFailed" and proceed to Results screen without delay

#### Scenario: Interstitial closed callback
- **WHEN** player closes interstitial ad
- **THEN** AdService SHALL invoke onAdClosed callback and GameManager SHALL transition to Results state

#### Scenario: Respect "Remove Ads" IAP
- **WHEN** player has purchased "Remove Ads" IAP
- **THEN** AdService SHALL skip all interstitial displays and proceed directly to Results

### Requirement: Rewarded Video Ads
The system SHALL display rewarded video ads for one-time revive per run and optional coin bonuses.

#### Scenario: Revive button availability
- **WHEN** Results screen loads and player has not used revive this run
- **THEN** UI SHALL display "Revive (Watch Ad)" button as enabled

#### Scenario: Revive button unavailable after use
- **WHEN** player has already used revive this run
- **THEN** UI SHALL hide or disable "Revive (Watch Ad)" button

#### Scenario: Watch rewarded video for revive
- **WHEN** player taps "Revive (Watch Ad)" button
- **THEN** AdService SHALL load and show rewarded video ad

#### Scenario: Revive granted on ad completion
- **WHEN** rewarded video ad completes successfully
- **THEN** AdService SHALL invoke onRewardEarned callback, GameManager SHALL respawn player with 1.0s invulnerability, and mark revive as used for this run

#### Scenario: Revive denied on ad skip
- **WHEN** player closes rewarded video ad before completion
- **THEN** AdService SHALL invoke onAdClosed callback without reward, and Results screen SHALL remain active (no revive granted)

#### Scenario: Coin bonus rewarded video (optional)
- **WHEN** Results screen displays "+50 Coins (Watch Ad)" button and player taps it
- **THEN** AdService SHALL show rewarded video, and on completion, award +50 coins to player's total

#### Scenario: Rewarded video load failure
- **WHEN** rewarded video ad fails to load
- **THEN** AdService SHALL display error message "Ad not available, try again later" and disable revive button

### Requirement: In-App Purchase (IAP) Service
The system SHALL provide IIAPService interface for purchasing "Remove Ads" and "Starter Pack".

#### Scenario: IAP service initialization
- **WHEN** Bootstrap scene initializes services
- **THEN** IAPService SHALL initialize Unity IAP, fetch product catalog, and register purchase callbacks

#### Scenario: IAP catalog definition
- **WHEN** IAPService fetches product catalog
- **THEN** system SHALL define two products: "remove_ads" (non-consumable, $2.99 USD), "starter_pack" (consumable, $4.99 USD, includes 3 cosmetics + 500 coins)

#### Scenario: Purchase "Remove Ads"
- **WHEN** player taps "Remove Ads" button in Shop and completes purchase
- **THEN** IAPService SHALL mark purchase as owned, save flag to PlayerPrefs "RemoveAdsPurchased", and hide all interstitial ads

#### Scenario: Restore "Remove Ads" purchase on reinstall
- **WHEN** player reinstalls app and IAPService initializes
- **THEN** IAPService SHALL restore non-consumable purchases and set "RemoveAdsPurchased" flag if owned

#### Scenario: Purchase "Starter Pack"
- **WHEN** player taps "Starter Pack" button in Shop and completes purchase
- **THEN** IAPService SHALL award 500 coins to total, unlock 3 specific cosmetics (Hat A, Outfit A, Trail A), and consume purchase

#### Scenario: IAP purchase failure
- **WHEN** IAP purchase fails (payment declined, user cancels)
- **THEN** IAPService SHALL display error message "Purchase failed" and log event "IAPFailed" with reason

#### Scenario: IAP receipt validation
- **WHEN** IAP purchase completes
- **THEN** IAPService SHALL perform stub receipt validation (local validation only at v1, no server verification)

### Requirement: Monetization Analytics
The system SHALL log analytics events for all ad and IAP interactions.

#### Scenario: Ad impression tracking
- **WHEN** interstitial or rewarded video ad is displayed
- **THEN** system SHALL call AnalyticsService.LogEvent("ShowAd", { "type": "interstitial|rewarded", "placement": "gameOver|revive|coinBonus" })

#### Scenario: IAP purchase tracking
- **WHEN** IAP purchase completes successfully
- **THEN** system SHALL call AnalyticsService.LogEvent("IAP", { "productId": "remove_ads|starter_pack", "price": priceUSD })

#### Scenario: Revive usage tracking
- **WHEN** player successfully uses rewarded revive
- **THEN** system SHALL call AnalyticsService.LogEvent("RewardedRevive", { "score": scoreAtDeath })

### Requirement: Monetization UI Integration
The system SHALL display monetization options in Shop and Results screens.

#### Scenario: Shop screen product listing
- **WHEN** Shop screen is active
- **THEN** UI SHALL display "Remove Ads" button (unless owned), "Starter Pack" button, and prices from IAPService

#### Scenario: Remove Ads button state
- **WHEN** player has purchased "Remove Ads"
- **THEN** Shop SHALL display "Remove Ads" as "Owned" (disabled button)

#### Scenario: Results screen monetization placement
- **WHEN** Results screen loads
- **THEN** UI SHALL display "Replay" (free), "Revive (Watch Ad)" (if eligible), "Home" buttons, with optional "+50 Coins (Watch Ad)" banner

### Requirement: Ad/IAP Error Handling
The system SHALL gracefully handle ad and IAP errors without blocking gameplay.

#### Scenario: Ad network unavailable
- **WHEN** device is offline or ad network is unreachable
- **THEN** AdService SHALL skip ad displays and proceed with gameplay flow

#### Scenario: IAP service unavailable
- **WHEN** device is offline or IAP service is unreachable during Shop access
- **THEN** IAPService SHALL display "IAP unavailable, check connection" message and disable purchase buttons

#### Scenario: No-throw guarantee
- **WHEN** AdService or IAPService encounters exception
- **THEN** system SHALL catch exception, log error, and continue gameplay without crash
