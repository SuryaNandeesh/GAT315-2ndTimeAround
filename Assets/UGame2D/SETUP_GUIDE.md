# Metroidvania Game Setup Guide

## 🎮 Manual Level Building Mode

**Procedural generation is now DISABLED by default.** This allows you to manually design and build your own custom levels using the provided prefabs and tools.

## 🚀 Quick Start for Manual Building

1. **Open the Scene**: Load `Assets/UGame2D/Scenes/MetroidvaniaMain.unity`
2. **Your Canvas**: The scene contains only essential objects:
   - Main Camera (orthographic, size 20)
   - GameManager (handles game state)
   - UIManager (creates top-left HUD automatically)
   - WorldBuilder (disabled but available for reference)
   - Player (spawned at origin 0,0,0)

3. **Start Building**: Drag prefabs from the project to create your level!

## 🏗️ Manual Level Building

### Available Prefabs
Located in `Assets/UGame2D/Prefabs/`:
- **Platform.prefab**: Basic platform elements
- **Ground.prefab**: Ground/floor tiles  
- **Enemy.prefab**: Enemy characters with AI
- **Player.prefab**: Player character (already in scene)
- **Collectibles**: Energy tanks, health pickups, ability orbs, score items

### WorldBuilder Tools
The WorldBuilder component has useful tools available via right-click context menu:

#### Context Menu Options:
- **Generate World**: Enable procedural generation temporarily to create a base level
- **Clear Generated World**: Remove all procedurally generated objects
- **Spawn Player Only**: Spawn just the player at origin (useful if deleted)

### Building Your Level

1. **Layout the Foundation**:
   ```
   - Place Ground prefabs for floors and platforms
   - Use Platform prefabs for jumping challenges
   - Position at strategic locations for gameplay flow
   ```

2. **Add Enemies**:
   ```
   - Drag Enemy prefabs into the scene
   - Each enemy will automatically get random AI behavior
   - Place them on platforms or patrol areas
   ```

3. **Place Collectibles**:
   ```
   - Energy Tank prefabs: Permanent health upgrades
   - Health Pickup prefabs: Temporary healing
   - Ability Pickup prefabs: Unlock movement abilities
   - Score Item prefabs: Point-based rewards
   ```

4. **Test and Iterate**:
   ```
   - Press Play to test your level
   - Use the beautiful top-left HUD to monitor progress
   - Adjust platform positions for difficulty balance
   ```

## 🎛️ WorldBuilder Settings

In the WorldBuilder component, you can:

### Enable/Disable Procedural Generation
- **enableProceduralGeneration**: Set to `true` to use automatic generation
- **Default**: `false` for manual building

### Reference Dimensions
- **worldWidth**: 200 units (for reference)
- **worldHeight**: 120 units (for reference)  
- **roomSize**: 80x60 units (suggested room dimensions)

### Prefab References
All prefab slots are pre-configured for easy dragging:
- **platformPrefab**: Platform elements
- **groundPrefab**: Ground tiles
- **enemyPrefab**: Enemy characters
- **collectible prefabs**: Various pickup types

## 🎨 Level Design Tips

### Camera Considerations
- **Orthographic Size**: 20 units (shows large area)
- **Player Movement**: Design around player's abilities
- **Visibility**: Ensure important elements are visible on screen

### Platform Spacing
- **Horizontal**: 4-8 units apart for jumping challenges
- **Vertical**: 6-12 units for different difficulty levels
- **Ability Gates**: Design areas requiring specific abilities

### Enemy Placement
- **Patrol Areas**: Give enemies clear paths to walk
- **Guard Points**: Place near important collectibles
- **Chasers**: In open areas where they can pursue effectively

### Collectible Strategy
- **Energy Tanks**: Hidden or challenging locations
- **Health Pickups**: Before/after difficult sections
- **Abilities**: Gate progression, place strategically
- **Score Items**: Reward exploration

## 🔧 Advanced Manual Building

### Using Inspector for Precise Placement
1. **Select objects** in scene hierarchy
2. **Transform component** shows exact position
3. **Snap to grid**: Hold Ctrl while moving for alignment
4. **Duplicate**: Ctrl+D to copy positioned objects

### Creating Rooms Manually
1. **Define boundaries** with Ground prefabs
2. **Add platforms** at various heights
3. **Place 3-5 enemies** per room area
4. **Distribute collectibles** strategically
5. **Test connectivity** between areas

### Layer Management
- **Default**: Most game objects
- **Player**: Player character
- **Enemy**: Enemy characters  
- **Collectible**: Pickup items
- **Platform**: Ground and platform elements

## 🎮 Testing Your Level

### Pre-Flight Checklist
✅ Player spawned at reasonable starting location  
✅ Ground platforms provide clear paths  
✅ Enemies placed on accessible platforms  
✅ Collectibles distributed throughout level  
✅ No floating objects or gaps that break progression  
✅ Camera size (20) provides good view of level  

### Testing Process
1. **Press Play** in Unity
2. **Move around** with WASD/Arrow keys
3. **Jump** with Spacebar
4. **Collect items** to test progression
5. **Observe UI** - health, score, timer in top-left
6. **Check enemy behavior** - patrol, guard, chase patterns

### Iterative Design
- **Play frequently** during building
- **Adjust spacing** based on player movement
- **Balance difficulty** with strategic collectible placement
- **Ensure exploration rewards** with hidden items

## 🏆 Level Design Goals

### Core Metroidvania Elements
- **Interconnected areas** accessible via different routes
- **Ability-gated progression** requiring specific powers
- **Hidden secrets** rewarding thorough exploration  
- **Balanced challenge** with health/upgrade management

### Technical Goals
- **Smooth performance** with reasonable object counts
- **Clear visual hierarchy** using positioning and spacing
- **Intuitive navigation** with visual landmarks
- **Reward discovery** with well-placed collectibles

## 🔄 Converting to Procedural (Optional)

If you want to switch back to procedural generation:

1. **Select WorldBuilder** in hierarchy
2. **Check "Enable Procedural Generation"** in inspector  
3. **Press Play** - world generates automatically
4. **Use "Clear Generated World"** context menu to reset

## 📚 Resources

### Unity 2D Documentation
- [2D Physics](https://docs.unity3d.com/Manual/Physics2D.html)
- [Sprite Renderer](https://docs.unity3d.com/Manual/class-SpriteRenderer.html)
- [Tilemap System](https://docs.unity3d.com/Manual/class-Tilemap.html)

### Level Design Principles  
- **Flow**: Guide player movement naturally
- **Pacing**: Alternate challenge and relief
- **Clarity**: Make interactable elements obvious
- **Rewards**: Balance risk and reward appropriately

---

*Happy level building! Create the metroidvania world of your dreams with complete creative control.*

## Controls
- **WASD/Arrow Keys**: Move
- **Spacebar**: Jump
- **Shift/X**: Dash (when unlocked)
- **Escape**: Pause
- **R**: Restart (when game over)

## What You'll See
- **6 procedurally generated rooms** connected by platforms
- **Enemies** with different AI behaviors patrolling areas
- **Collectible items** scattered throughout the world
- **Physics interactions** like springs and moving platforms
- **Ability progression** - collect abilities to unlock new movement options

## Troubleshooting

**"Missing Prefab" Errors:**
- Make sure all prefabs are assigned in the WorldBuilder component
- Create simple placeholder prefabs if needed (just colored cubes work)

**Player Falls Through Ground:**
- Check that Ground prefabs have Box Collider 2D components
- Ensure Ground layer is set correctly in enemy AI ground detection

**No UI Visible:**
- The scene needs a Canvas with UI elements
- For now, the game will work without UI (check Console for score updates)

**Compilation Errors:**
- Make sure all scripts are in the `Assets/UGame2D/Scripts/` folder
- Check that no duplicate class names exist with existing project scripts

## Next Steps
1. **Add Sprites**: Replace colored rectangles with actual 2D sprites
2. **Create UI**: Add Canvas with health bars, score display, etc.
3. **Add Audio**: Assign audio clips to the GameManager audio sources
4. **Customize Levels**: Modify the WorldBuilder parameters for different layouts
5. **Add Animations**: Create Animator Controllers for character animations

## File Structure
```
Assets/UGame2D/
├── Scenes/MetroidvaniaMain.unity     # Main game scene
├── Scripts/                          # All game scripts
├── Prefabs/                          # Game object prefabs  
├── Sprites/                          # 2D artwork (add your own)
├── Audio/                            # Sound files (add your own)
└── Materials/                        # Physics materials
```

The system is designed to be modular and extensible - start with the basics and add complexity as needed! 