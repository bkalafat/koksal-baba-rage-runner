# UI Systems Specification

## ADDED Requirements

### Requirement: HUD (Heads-Up Display)
The system SHALL display score, rage bar, and pause button during gameplay.

#### Scenario: HUD elements visibility
- **WHEN** game is in Playing state
- **THEN** HUD SHALL display current score (top-left), rage bar (bottom-center), and pause button (top-right)

#### Scenario: Score display updates
- **WHEN** ScoreService updates current score
- **THEN** HUD SHALL update score text within 1 frame with formatted integer (e.g., "1,234")

#### Scenario: Rage bar fill display
- **WHEN** RageMeter value changes
- **THEN** HUD SHALL update rage bar fill amount (0-100% horizontal bar) with smooth lerp (0.1s transition)

#### Scenario: Rage ready prompt
- **WHEN** RageMeter reaches 1.0
- **THEN** HUD SHALL display "Tap to Rage!" text (localized) above rage bar with pulsing animation

#### Scenario: Pause button tap
- **WHEN** player taps pause button
- **THEN** GameManager SHALL transition to Paused state and display pause overlay

### Requirement: Pause Menu
The system SHALL provide pause overlay with resume and quit options.

#### Scenario: Pause overlay display
- **WHEN** game transitions to Paused state
- **THEN** UI SHALL display semi-transparent overlay (alpha 0.8), "Paused" title (localized), "Resume" button, and "Quit to Menu" button

#### Scenario: Resume button tap
- **WHEN** player taps "Resume" button in pause overlay
- **THEN** GameManager SHALL transition back to Playing state and hide pause overlay

#### Scenario: Quit to menu button tap
- **WHEN** player taps "Quit to Menu" button in pause overlay
- **THEN** GameManager SHALL save session progress, transition to MainMenu state, and unload Game scene

### Requirement: Results Screen
The system SHALL display final score, best score, and action buttons after game over.

#### Scenario: Results screen display
- **WHEN** game transitions to Results state
- **THEN** UI SHALL display final score, best score comparison, "+[coins] coins" earned, and action buttons

#### Scenario: New best score indicator
- **WHEN** final score > best score
- **THEN** Results screen SHALL display "New Best!" banner with gold star icon and play celebration SFX

#### Scenario: Replay button
- **WHEN** player taps "Replay" button on Results screen
- **THEN** GameManager SHALL reset game state, transition to Playing state, and reload Game scene

#### Scenario: Revive button (eligible)
- **WHEN** Results screen loads and player has not used revive this run
- **THEN** UI SHALL display "Revive (Watch Ad)" button as enabled

#### Scenario: Revive button (ineligible)
- **WHEN** player has already used revive this run
- **THEN** UI SHALL hide "Revive (Watch Ad)" button

#### Scenario: Home button
- **WHEN** player taps "Home" button on Results screen
- **THEN** GameManager SHALL save session progress, transition to MainMenu state, and unload Game scene

### Requirement: Main Menu
The system SHALL provide main menu with Play, Shop, and Settings navigation.

#### Scenario: Main menu display
- **WHEN** game is in MainMenu state
- **THEN** UI SHALL display "Köksal Baba: Rage Runner" title, "Play" button, "Shop" button, "Settings" button, and best score text ("Best: [score]")

#### Scenario: Play button tap
- **WHEN** player taps "Play" button
- **THEN** GameManager SHALL transition to Playing state, load Game scene additively, and start new run

#### Scenario: Shop button tap
- **WHEN** player taps "Shop" button
- **THEN** GameManager SHALL display Shop overlay (modal) over MainMenu scene

#### Scenario: Settings button tap
- **WHEN** player taps "Settings" button
- **THEN** GameManager SHALL display Settings overlay (modal) over MainMenu scene

### Requirement: Shop UI
The system SHALL provide shop interface for purchasing IAPs and unlocking cosmetics with coins.

#### Scenario: Shop overlay display
- **WHEN** Shop overlay is opened
- **THEN** UI SHALL display total coins ("Total: [coins] coins"), "Remove Ads" button (unless owned), "Starter Pack" button, and cosmetics grid (hats, outfits)

#### Scenario: Cosmetics unlock with coins
- **WHEN** player taps locked cosmetic item in shop
- **THEN** UI SHALL display confirmation dialog "Unlock [item] for [cost] coins?" with Confirm/Cancel buttons

#### Scenario: Insufficient coins for unlock
- **WHEN** player attempts to unlock cosmetic with cost > total coins
- **THEN** UI SHALL display error message "Not enough coins" and shake cosmetic item

#### Scenario: Successful cosmetic unlock
- **WHEN** player confirms unlock and has sufficient coins
- **THEN** system SHALL deduct coins, mark cosmetic as unlocked (PlayerPrefs), and update shop grid to show "Owned" state

#### Scenario: IAP product display
- **WHEN** Shop overlay is opened
- **THEN** UI SHALL display "Remove Ads" ($2.99), "Starter Pack" ($4.99) with prices from IAPService

#### Scenario: Close shop button
- **WHEN** player taps "X" button in shop overlay
- **THEN** UI SHALL hide shop overlay and return to MainMenu

### Requirement: Settings UI
The system SHALL provide settings interface for sound, haptics, language, and privacy toggles.

#### Scenario: Settings overlay display
- **WHEN** Settings overlay is opened
- **THEN** UI SHALL display toggles for "Sound" (on/off), "Haptics" (on/off), "Language" (tr-TR/en-US dropdown), and "Privacy Consent" (on/off)

#### Scenario: Sound toggle
- **WHEN** player toggles "Sound" off
- **THEN** system SHALL mute all SFX and music, save preference to PlayerPrefs "SoundEnabled"

#### Scenario: Haptics toggle
- **WHEN** player toggles "Haptics" off
- **THEN** system SHALL disable all haptic feedback, save preference to PlayerPrefs "HapticsEnabled"

#### Scenario: Language selection
- **WHEN** player selects language from dropdown (tr-TR or en-US)
- **THEN** system SHALL save preference to PlayerPrefs "Language" and display "Restart required" message

#### Scenario: Privacy consent toggle
- **WHEN** player toggles "Privacy Consent" off
- **THEN** system SHALL disable analytics tracking, save preference to PlayerPrefs "AnalyticsConsent"

#### Scenario: Close settings button
- **WHEN** player taps "X" button in settings overlay
- **THEN** UI SHALL hide settings overlay and return to MainMenu

### Requirement: Localized String Tables
The system SHALL load all UI strings from localized string tables (CSV format).

#### Scenario: String table loading
- **WHEN** Bootstrap scene initializes
- **THEN** system SHALL load Localization.csv, parse tr-TR and en-US columns, and populate LocalizationService dictionary

#### Scenario: String lookup
- **WHEN** UI element requests localized string (e.g., "HUD_RAGE_READY")
- **THEN** LocalizationService SHALL return string for current language (PlayerPrefs "Language" or device language fallback to en-US)

#### Scenario: Missing translation fallback
- **WHEN** requested key does not exist in current language
- **THEN** LocalizationService SHALL return en-US translation with warning log

### Requirement: Responsive UI Layout
The system SHALL support portrait orientation on iPhone screens (5.5" to 6.7" aspect ratios).

#### Scenario: Safe area handling
- **WHEN** UI is rendered on device with notch (iPhone X+)
- **THEN** Canvas SHALL respect safe area insets and position HUD elements within safe bounds

#### Scenario: Resolution-independent layout
- **WHEN** game is launched on different iPhone screen sizes (5.5", 6.1", 6.7")
- **THEN** UI SHALL scale proportionally using CanvasScaler with "Scale with Screen Size" mode (reference resolution 1080x1920)

### Requirement: UI Animation and Juice
The system SHALL provide smooth transitions and feedback for UI interactions.

#### Scenario: Button press feedback
- **WHEN** player taps any button
- **THEN** button SHALL scale down to 0.9x for 0.1 seconds, play tap SFX, and trigger haptic feedback

#### Scenario: Screen transition fade
- **WHEN** scene transitions occur (MainMenu → Game, Results → MainMenu)
- **THEN** UI SHALL fade to black (0.3s), load scene, and fade from black (0.3s)

#### Scenario: Score count-up animation
- **WHEN** Results screen displays final score
- **THEN** score SHALL count up from 0 to final value over 1.0 second with easing (EaseOutQuad)

#### Scenario: Rage bar pulse animation
- **WHEN** RageMeter reaches 1.0
- **THEN** rage bar SHALL pulse scale (1.0x to 1.1x, 0.5s loop) until dash is activated
