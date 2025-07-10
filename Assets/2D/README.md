# 2D Game World - Unity Tilemap Assignment

## Overview
This project demonstrates a comprehensive 2D game world built using Unity's Tilemap system, featuring physics-based player movement, collectible items, environmental hazards, and strategic level design.

## Features Implemented

### 1. Tilemap System
- **Grid-based Level**: Utilizes Unity's Grid and Tilemap components for efficient level construction
- **Green Hills Tileset**: Custom tile palette using the provided Green Hills sprite assets
- **Varied Terrain**: Ground tiles, platforms, slopes, and decorative elements
- **Physics Integration**: TilemapCollider2D provides solid collision surfaces

### 2. Player Character
- **Physics-Based Movement**: Rigidbody2D with responsive controls
- **Advanced Platformer Mechanics**:
  - Variable jump height (hold/tap space)
  - Coyote time (brief jump window after leaving platform)
  - Jump buffering (early jump input registration)
  - Snappy fall physics with customizable gravity multipliers
- **Ground Detection**: Robust ground checking using OverlapBox
- **Health System**: Integration with existing health/damage framework

### 3. Collectible System

#### Coin Prefab (`Assets/2D/Prefabs/Coin.prefab`)
- **Sprite**: Golden circle with appropriate color tinting
- **Physics**: CircleCollider2D set as trigger
- **Behavior**: Rotates continuously for visual appeal
- **Collection**: Destroys on contact with player, provides points
- **Script**: `Collectible.cs` handles trigger detection and cleanup

#### Health Pickup Prefab (`Assets/2D/Prefabs/HealthPickup.prefab`)
- **Sprite**: Red cross/plus symbol
- **Physics**: CircleCollider2D trigger
- **Functionality**: Restores 25 health points to player
- **Visual Feedback**: Slower rotation for distinction from coins
- **Script**: `HealthPickup.cs` integrates with existing Health system

### 4. Hazard System

#### Spike Hazard Prefab (`Assets/2D/Prefabs/SpikeHazard.prefab`)
- **Sprite**: Spike sprite from Green Hills tileset
- **Physics**: BoxCollider2D trigger with adjusted size
- **Damage**: Deals 20 physical damage on contact
- **Advanced Features**: Optional continuous damage with configurable intervals
- **Script**: `HazardDamage.cs` with proper damage type integration

### 5. Level Design Philosophy

#### Strategic Placement
- **Ground Level Collectibles**: Easy-to-reach coins for basic progression
- **Elevated Rewards**: Coins placed to encourage jumping and exploration
- **Risk vs Reward**: High-value items near hazards or in challenging locations
- **Health Management**: Strategic health pickup placement near danger zones

#### Navigation Challenges
- **Platform Jumping**: Various heights requiring different jump techniques
- **Hazard Avoidance**: Spikes placed to create meaningful obstacles
- **Exploration Incentives**: Hidden areas with valuable collectibles
- **Skill Progression**: Increasingly difficult areas as player progresses

### 6. Scripts Overview

#### `PlayerController2D.cs`
- Advanced 2D platformer movement with modern game feel
- Configurable physics parameters for fine-tuning
- Visual debugging with ground check gizmos

#### `Collectible.cs`
- Generic collectible item handler
- Audio support for collection sounds
- Integration with player controller for scoring

#### `CollectibleRotator.cs`
- Simple rotation script for visual appeal
- Configurable rotation speed

#### `HealthPickup.cs`
- Specialized collectible for health restoration
- Integrates with existing Health component
- Debug logging for feedback

#### `HazardDamage.cs`
- Comprehensive damage system
- Support for both instant and continuous damage
- Proper damage type classification
- Coroutine-based continuous damage handling

#### `LevelBuilder.cs`
- Programmatic level construction tool
- Automated prefab placement
- Visual gizmos for level planning
- Context menu integration for easy level building

## Usage Instructions

### Setting Up the Level
1. Open the `Unity2DWorld.unity` scene
2. The GameManager already has the LevelBuilder script with prefab references assigned:
   - Coin Prefab: `Assets/2D/Prefabs/Coin.prefab`
   - Health Pickup Prefab: `Assets/2D/Prefabs/HealthPickup.prefab`
   - Spike Hazard Prefab: `Assets/2D/Prefabs/SpikeHazard.prefab`
   - Player Prefab: `Assets/2D/Prefabs/Player.prefab`
3. "Build On Start" is set to true, so level will build automatically when you press Play
4. Alternatively, click "Build Level" in the GameManager's context menu

### Player Controls
- **Movement**: A/D or Arrow Keys (Left/Right)
- **Jump**: Space, W, or Up Arrow
- **Variable Jump**: Hold jump key for higher jumps, tap for lower jumps

### Level Editing
- Use the Tile Palette window to paint additional tiles
- Existing tilemap has TilemapCollider2D for automatic collision
- Add new prefabs by dragging them into the scene
- Use the LevelBuilder script for systematic placement

## Technical Implementation

### Physics Integration
- **2D Physics**: All objects use Collider2D components
- **Layer Masks**: Ground detection uses configurable layer masks
- **Trigger vs Collision**: Collectibles and hazards use triggers, platforms use colliders
- **Rigidbody2D**: Player has physics body with gravity and drag settings

### Performance Considerations
- **Object Pooling Ready**: Collectible system designed for easy pooling integration
- **Efficient Collision**: Uses appropriate collider shapes and sizes
- **Tilemap Optimization**: Leverages Unity's built-in tilemap batching

### Extensibility
- **Modular Scripts**: Each component handles specific functionality
- **Configuration Exposed**: Inspector-accessible parameters for easy tweaking
- **Event System Ready**: Health and damage systems use UnityEvents
- **Damage Types**: Extensible damage system with multiple damage types

## Asset Credits
- **Green Hills Tileset**: Provided sprite assets
- **Unity Built-in**: Default sprites for player and some collectibles
- **Custom Prefabs**: All prefabs created specifically for this assignment

## Future Enhancements
- **Audio Integration**: Sound effects for all interactions
- **Particle Effects**: Visual feedback for collections and damage
- **Animated Sprites**: Character and object animations
- **Save System**: Level progress and collectible tracking
- **Multiple Levels**: Level progression system
- **Enemy AI**: Moving hazards and intelligent opponents

This implementation demonstrates proficiency with Unity's 2D systems, physics integration, and game design principles while creating an engaging and polished level experience. 