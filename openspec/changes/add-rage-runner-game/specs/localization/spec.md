# Localization Specification

## ADDED Requirements

### Requirement: String Table Management
The system SHALL load all UI strings from CSV string table with tr-TR and en-US columns.

#### Scenario: String table file format
- **WHEN** Localization.csv is created
- **THEN** file SHALL have columns: Key, tr-TR, en-US (UTF-8 encoding with BOM)

#### Scenario: String table loading on startup
- **WHEN** Bootstrap scene initializes
- **THEN** LocalizationService SHALL load Localization.csv from Resources folder, parse rows, and populate dictionary {Key: {tr-TR: "...", en-US: "..."}}

#### Scenario: String table parse error handling
- **WHEN** Localization.csv has malformed rows (missing columns, invalid UTF-8)
- **THEN** LocalizationService SHALL log error for problematic row and skip it (graceful degradation)

### Requirement: Language Selection
The system SHALL determine UI language from device language with fallback to en-US.

#### Scenario: Device language detection on first launch
- **WHEN** app launches for first time
- **THEN** LocalizationService SHALL read Application.systemLanguage, map to "tr-TR" if Turkish, otherwise default to "en-US", and save to PlayerPrefs "Language"

#### Scenario: Manual language override
- **WHEN** player selects language from Settings dropdown
- **THEN** LocalizationService SHALL save selection to PlayerPrefs "Language" and display "Restart required" message (no runtime language switching at v1)

#### Scenario: Language persistence across sessions
- **WHEN** app relaunches
- **THEN** LocalizationService SHALL load language from PlayerPrefs "Language" and apply selected language

### Requirement: String Lookup
The system SHALL provide LocalizationService.GetString(key) for localized text retrieval.

#### Scenario: Valid key lookup
- **WHEN** UI calls LocalizationService.GetString("HUD_SCORE")
- **THEN** service SHALL return tr-TR value if current language is tr-TR, else en-US value

#### Scenario: Missing key fallback
- **WHEN** UI calls LocalizationService.GetString("INVALID_KEY")
- **THEN** service SHALL return "[INVALID_KEY]" placeholder and log warning

#### Scenario: Missing translation fallback
- **WHEN** requested key exists but tr-TR column is empty
- **THEN** service SHALL return en-US value and log warning "Missing tr-TR translation for key"

### Requirement: Supported Languages
The system SHALL support Turkish (tr-TR) and US English (en-US) at launch.

#### Scenario: Turkish language support
- **WHEN** player selects tr-TR language
- **THEN** all UI strings SHALL display in Turkish (e.g., "Oyna" for Play, "Puan" for Score, "Durakla" for Pause)

#### Scenario: US English language support
- **WHEN** player selects en-US language
- **THEN** all UI strings SHALL display in US English (e.g., "Play", "Score", "Pause")

### Requirement: Localized String Keys
The system SHALL define string keys for all UI elements.

#### Scenario: HUD strings
- **WHEN** Localization.csv is created
- **THEN** file SHALL include keys: HUD_SCORE, HUD_RAGE_READY, HUD_PAUSE

#### Scenario: Main Menu strings
- **WHEN** Localization.csv is created
- **THEN** file SHALL include keys: MAINMENU_PLAY, MAINMENU_SHOP, MAINMENU_SETTINGS, MAINMENU_BEST

#### Scenario: Results screen strings
- **WHEN** Localization.csv is created
- **THEN** file SHALL include keys: RESULTS_SCORE, RESULTS_BEST, RESULTS_NEW_BEST, RESULTS_COINS, RESULTS_REPLAY, RESULTS_REVIVE, RESULTS_HOME

#### Scenario: Shop screen strings
- **WHEN** Localization.csv is created
- **THEN** file SHALL include keys: SHOP_TITLE, SHOP_TOTAL_COINS, SHOP_REMOVE_ADS, SHOP_STARTER_PACK, SHOP_UNLOCK_CONFIRM, SHOP_NOT_ENOUGH_COINS

#### Scenario: Settings screen strings
- **WHEN** Localization.csv is created
- **THEN** file SHALL include keys: SETTINGS_TITLE, SETTINGS_SOUND, SETTINGS_HAPTICS, SETTINGS_LANGUAGE, SETTINGS_PRIVACY, SETTINGS_PRIVACY_POLICY, SETTINGS_RESTART_REQUIRED

### Requirement: Dynamic Text Formatting
The system SHALL support string interpolation for dynamic values (score, coins, etc.).

#### Scenario: Score display with placeholder
- **WHEN** Localization.csv defines "RESULTS_SCORE" as "Score: {0}"
- **THEN** UI SHALL call string.Format(LocalizationService.GetString("RESULTS_SCORE"), finalScore) to display "Score: 1234"

#### Scenario: Coins display with placeholder
- **WHEN** Localization.csv defines "RESULTS_COINS" as "+{0} coins"
- **THEN** UI SHALL call string.Format(LocalizationService.GetString("RESULTS_COINS"), sessionCoins) to display "+25 coins"

#### Scenario: Unlock confirmation with placeholder
- **WHEN** Localization.csv defines "SHOP_UNLOCK_CONFIRM" as "Unlock {0} for {1} coins?"
- **THEN** UI SHALL call string.Format(LocalizationService.GetString("SHOP_UNLOCK_CONFIRM"), itemName, cost) to display "Unlock Red Cap for 50 coins?"

### Requirement: Localization Export/Import
The system SHALL support CSV export/import for translation workflow.

#### Scenario: Export string keys to CSV
- **WHEN** developer adds new UI element with string key
- **THEN** developer SHALL add row to Localization.csv with key, tr-TR value, and en-US value

#### Scenario: Import translated CSV
- **WHEN** translator provides updated Localization.csv with tr-TR translations
- **THEN** developer SHALL replace Assets/Localization/Localization.csv and verify in Play Mode

#### Scenario: CSV merge conflict resolution
- **WHEN** multiple developers add keys to Localization.csv
- **THEN** CSV SHALL be sorted alphabetically by Key column to minimize merge conflicts

### Requirement: Localization Testing
The system SHALL provide in-editor language switching for testing.

#### Scenario: Editor language override
- **WHEN** developer sets #define FORCE_TURKISH or FORCE_ENGLISH
- **THEN** LocalizationService SHALL ignore PlayerPrefs and force specified language for testing

#### Scenario: Missing translation detection in Play Mode
- **WHEN** Play Mode is active and missing translation is encountered
- **THEN** LocalizationService SHALL log warning to Console with key name and missing language

### Requirement: Right-to-Left (RTL) Support (Future)
The system SHALL NOT support RTL languages at v1 (tr-TR and en-US are LTR).

#### Scenario: RTL placeholder
- **WHEN** future RTL language (e.g., Arabic) is requested
- **THEN** developer SHALL add RTL layout support in v2 (out of scope for v1)

### Requirement: Localized Asset Naming (Future)
The system SHALL NOT support localized sprite assets for text-heavy images at v1 (deferred to future version).

#### Scenario: Localized sprite placeholder for future
- **WHEN** future version requires UI element with embedded text (v2 or later)
- **THEN** developer SHALL create separate sprites (e.g., "button_play_en.png", "button_play_tr.png") and load based on current language
