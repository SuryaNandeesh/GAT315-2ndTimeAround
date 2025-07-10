# 2D Platformer Game

A Unity 2D platformer game with collectibles, hazards, and physics-based gameplay.

## Features

- Tilemap-based level design
- Player movement with smooth physics
- Collectible coins and health pickups
- Placeable spike hazards
- Score and health system
- Debug messages for game events
- UI elements for health and score

## Setup

1. Open the project in Unity 2022.3.19f1 or later
2. Open the `Unity2DWorld` scene
3. Press Play to start the game

## Controls

- A/D or Left/Right Arrow: Move
- Space: Jump
- Hold Space for higher jumps
- Release Space early for shorter jumps

## Game Elements

### Player
- Fire wizard character with smooth movement
- Health system with invincibility frames
- Score tracking
- Debug messages for game events

### Collectibles
- Coins: Adds 10 points to score
- Health Pickups: Restores 25 health points

### Hazards
- Spikes: Deals 10 damage and knocks the player back
- Can be placed in the scene as needed

## UI Elements

- Health Bar: Shows current health
- Score Text: Displays current score
- Debug Text: Shows game events and messages

## Debug Messages

The game now includes debug messages for:
- Game start
- Jumping
- Collecting coins
- Taking damage
- Healing
- Game over

## Level Design

1. Create a new scene named "Unity2DWorld"
2. Set up the tilemap for the level
3. Place spikes and collectibles manually in the scene
4. Add UI elements for health, score, and debug messages

## Physics Settings

- Gravity Scale: 3
- Linear Drag: 0
- Angular Drag: 0.05
- Collision Detection: Continuous
- Sleeping Mode: Start Awake
- Interpolate: None

## Tags and Layers

- Player: "Player" tag
- Ground: "Ground" layer
- Collectibles: "Collectible" layer
- Hazards: "Hazard" layer
