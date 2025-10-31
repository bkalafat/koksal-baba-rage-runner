# Content and Assets Specification

## ADDED Requirements

### Requirement: Biome Themes
The system SHALL support two biome themes: Street and Boardwalk.

#### Scenario: Street biome visual assets
- **WHEN** player selects Street biome
- **THEN** game SHALL load street background (repeating tiles), gray color palette (asphalt, concrete), and street-themed obstacle sprites (traffic poles, barriers, trash bins)

#### Scenario: Boardwalk biome visual assets
- **WHEN** player selects Boardwalk biome
- **THEN** game SHALL load boardwalk background (ocean, pier tiles), blue/yellow color palette, and boardwalk-themed obstacle sprites (beach umbrellas, lifeguard stands, crates)

#### Scenario: Biome selection (v1: both unlocked)
- **WHEN** player is in MainMenu
- **THEN** player MAY select biome via "Biome" button (Street or Boardwalk), both available from start

#### Scenario: Background parallax scrolling
- **WHEN** game is in Playing state
- **THEN** background layers SHALL scroll leftward at 0.5x, 0.75x, 1.0x speeds to create parallax depth effect

### Requirement: Obstacle Sprites
The system SHALL provide sprites for three obstacle types: static poles, moving barriers, breakable crates.

#### Scenario: Static pole sprite
- **WHEN** static pole is spawned
- **THEN** sprite SHALL display vertical pole (64x128 pixels), themed per biome (traffic pole for Street, pier post for Boardwalk)

#### Scenario: Moving barrier sprite
- **WHEN** moving barrier is spawned
- **THEN** sprite SHALL display horizontal barrier (128x64 pixels), with oscillating animation (2-frame idle, 0.5s loop)

#### Scenario: Breakable crate sprite
- **WHEN** breakable crate is spawned
- **THEN** sprite SHALL display wooden crate (64x64 pixels) with "fragile" icon
- **WHEN** crate is broken during dash
- **THEN** system SHALL play particle effect (8 wood shard sprites, 0.5s duration)

### Requirement: Pickup Sprites
The system SHALL provide sprites for Taunt tokens and coin bundles.

#### Scenario: Taunt token sprite
- **WHEN** Taunt token is spawned
- **THEN** sprite SHALL display comedic icon representing Riçıt's prank (32x32 pixels, rotating animation 1 rev/sec)

#### Scenario: Coin bundle sprite
- **WHEN** coin bundle is spawned
- **THEN** sprite SHALL display 3 stacked gold coins (32x32 pixels, bobbing animation ±2 pixels, 1.0s loop)

### Requirement: Player Character Sprites
The system SHALL provide Köksal Baba player sprite with idle and hop animations.

#### Scenario: Player idle sprite
- **WHEN** player is grounded
- **THEN** sprite SHALL display Köksal Baba standing pose (64x128 pixels, 2-frame animation, 0.5s loop)

#### Scenario: Player hop sprite
- **WHEN** player is in mid-air
- **THEN** sprite SHALL display Köksal Baba stretched/jumping pose (64x128 pixels, single frame)

#### Scenario: Player rage dash visual
- **WHEN** player is in Rage Dash state
- **THEN** sprite SHALL apply red tint (Color.red, alpha 0.5) and enable particle trail (red particles, 0.1s spawn rate)

### Requirement: Cosmetic Items
The system SHALL provide 6 cosmetic items (3 hats, 3 outfits) with no gameplay effect.

#### Scenario: Hat cosmetics
- **WHEN** player unlocks Hat A (50 coins)
- **THEN** sprite overlay SHALL display red cap on player sprite (32x32 pixels, positioned at head)
- **WHEN** player unlocks Hat B (100 coins)
- **THEN** sprite overlay SHALL display sunglasses (32x16 pixels)
- **WHEN** player unlocks Hat C (150 coins)
- **THEN** sprite overlay SHALL display top hat (32x48 pixels)

#### Scenario: Outfit cosmetics
- **WHEN** player unlocks Outfit A (75 coins)
- **THEN** player sprite SHALL use "tracksuit" variant texture (64x128 pixels)
- **WHEN** player unlocks Outfit B (125 coins)
- **THEN** player sprite SHALL use "suit and tie" variant texture
- **WHEN** player unlocks Outfit C (200 coins)
- **THEN** player sprite SHALL use "beach attire" variant texture

#### Scenario: Cosmetic persistence
- **WHEN** player equips cosmetic item
- **THEN** system SHALL save selection to PlayerPrefs ("EquippedHat", "EquippedOutfit") and apply on next run

### Requirement: Sprite Atlas Batching
The system SHALL pack all sprites into sprite atlases to reduce draw calls.

#### Scenario: Obstacles atlas
- **WHEN** Unity builds project
- **THEN** all obstacle sprites SHALL be packed into "ObstaclesAtlas" (max 2048x2048 ASTC texture)

#### Scenario: UI atlas
- **WHEN** Unity builds project
- **THEN** all UI sprites (buttons, icons, HUD elements) SHALL be packed into "UIAtlas" (max 2048x2048 ASTC texture)

#### Scenario: Character atlas
- **WHEN** Unity builds project
- **THEN** all player and cosmetic sprites SHALL be packed into "CharacterAtlas" (max 2048x2048 ASTC texture)

### Requirement: Texture Compression and Import Settings
The system SHALL use ASTC compression for iOS to minimize build size and memory.

#### Scenario: Sprite import preset
- **WHEN** sprite asset is imported
- **THEN** Unity SHALL apply import preset: texture type "Sprite (2D and UI)", max size 2048, compression ASTC 6x6 (iOS), filter mode bilinear, generate mipmaps off

#### Scenario: Background import preset
- **WHEN** background texture is imported
- **THEN** Unity SHALL apply import preset: texture type "Default", max size 2048, compression ASTC 6x6 (iOS), filter mode bilinear, generate mipmaps off, wrap mode repeat

#### Scenario: Build size target
- **WHEN** Unity builds iOS IPA
- **THEN** final build size SHALL be < 150 MB (verified in Build Report)

### Requirement: Placeholder Art and Color Palette
The system SHALL provide placeholder art for prototyping with defined color palette.

#### Scenario: Placeholder sprites
- **WHEN** art assets are not final
- **THEN** system SHALL use colored squares (obstacles), circles (pickups), and rectangles (player) with debug labels

#### Scenario: Color palette JSON
- **WHEN** designer defines color palette
- **THEN** system SHALL provide Colors.json with keys: {"primary": "#FF5733", "secondary": "#33C3FF", "background": "#F0F0F0", "obstacle": "#808080", "pickup": "#FFD700", "player": "#FF6347"}

#### Scenario: Palette application
- **WHEN** placeholder art is used
- **THEN** sprites SHALL apply colors from Colors.json via SpriteRenderer.color

### Requirement: Particle Effects
The system SHALL provide lightweight particle effects for juice.

#### Scenario: Crate break particles
- **WHEN** crate is destroyed during Rage Dash
- **THEN** system SHALL spawn 8 wood shard sprites (16x16 pixels) with random velocity (2-4 units/sec), 0.5s lifetime, and fade-out alpha

#### Scenario: Rage dash trail particles
- **WHEN** player is in Rage Dash state
- **THEN** system SHALL spawn red circle particles (8x8 pixels) at 10 particles/sec, 0.3s lifetime, following player position

#### Scenario: Pickup collection particles
- **WHEN** player collects Taunt token or coin
- **THEN** system SHALL spawn 4 star sprites (16x16 pixels) with outward velocity, 0.4s lifetime, and scale-down animation

#### Scenario: New best score particles
- **WHEN** Results screen displays "New Best!" banner
- **THEN** system SHALL spawn gold confetti particles (8x8 pixels) from top of screen, 50 particles over 1.0s duration

### Requirement: Asset Organization
The system SHALL organize assets in Assets/ folder by type.

#### Scenario: Asset folder structure
- **WHEN** project is created
- **THEN** Assets/ SHALL contain folders: Art/Sprites, Art/Backgrounds, Art/Particles, SFX, Music, Prefabs/Obstacles, Prefabs/Pickups, Prefabs/UI, Data/ScriptableObjects

#### Scenario: Naming conventions
- **WHEN** assets are imported
- **THEN** files SHALL follow naming convention: lowercase_snake_case (e.g., "static_pole.png", "street_background.png", "hop.wav")
