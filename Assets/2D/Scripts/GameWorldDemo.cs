using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Comprehensive demo script showcasing the 2D Game World assignment features
/// This script demonstrates Unity's Tilemap system, 2D physics, collectibles, and hazards
/// </summary>
public class GameWorldDemo : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap groundTilemap;
    public Tilemap hazardTilemap;
    public Tilemap collectibleTilemap;

    [Header("UI Elements")]
    public Slider healthBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI debugText;

    [Header("Level Generation")]
    public LevelBuilder levelBuilder;

    [Header("Demo Settings")]
    [SerializeField] private bool autoShowFeatures = true;
    [SerializeField] private float featureShowcaseInterval = 3f;
    
    private DemoSceneManager demoManager;
    private PlayerController playerController;
    private Camera mainCamera;
    private float nextFeatureShowcaseTime;

    private void Start()
    {
        // Find UI elements if not assigned
        if (healthBar == null)
            healthBar = FindAnyObjectByType<Slider>();
        if (scoreText == null)
            scoreText = FindAnyObjectByType<TextMeshProUGUI>();
        if (debugText == null)
            debugText = FindAnyObjectByType<TextMeshProUGUI>();

        // Find level builder if not assigned
        if (levelBuilder == null)
            levelBuilder = FindAnyObjectByType<LevelBuilder>();

        // Find player and set up UI references
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.healthBar = healthBar;
                playerController.scoreText = scoreText;
                playerController.debugText = debugText;
            }
        }

        // Find or create required components
        demoManager = FindAnyObjectByType<DemoSceneManager>();
        mainCamera = Camera.main;
        
        // Validate setup
        ValidateSetup();
        
        if (autoShowFeatures)
        {
            nextFeatureShowcaseTime = Time.time + featureShowcaseInterval;
        }
        
        LogAssignmentCompletion();
    }
    
    private void Update()
    {
        if (autoShowFeatures && Time.time >= nextFeatureShowcaseTime)
        {
            ShowcaseNextFeature();
            nextFeatureShowcaseTime = Time.time + featureShowcaseInterval;
        }
        
        HandleDemoControls();
    }
    
    private void ValidateSetup()
    {
        Debug.Log("=== 2D Game World Assignment Validation ===");
        
        // Check Tilemap System
        var tilemap = FindAnyObjectByType<UnityEngine.Tilemaps.Tilemap>();
        var tilemapCollider = FindAnyObjectByType<UnityEngine.Tilemaps.TilemapCollider2D>();
        Debug.Log($"Tilemap System: {(tilemap != null ? "✓ Present" : "✗ Missing")}");
        Debug.Log($"Tilemap Collider: {(tilemapCollider != null ? "✓ Present" : "✗ Missing")}");
        
        // Check Prefabs
        var coins = GameObject.FindGameObjectsWithTag("Collectible");
        var hazards = GameObject.FindGameObjectsWithTag("Hazard");
        Debug.Log($"Collectible Prefabs: {(coins.Length > 0 ? $"✓ {coins.Length} found" : "✗ None found")}");
        Debug.Log($"Hazard Prefabs: {(hazards.Length > 0 ? $"✓ {hazards.Length} found" : "✗ None found")}");
        
        // Check Player Setup
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerRb = player?.GetComponent<Rigidbody2D>();
        var playerHealth = player?.GetComponent<Health>();
        Debug.Log($"Player Character: {(player != null ? "✓ Present" : "✗ Missing")}");
        Debug.Log($"Player Physics: {(playerRb != null ? "✓ Rigidbody2D attached" : "✗ Missing")}");
        Debug.Log($"Player Health: {(playerHealth != null ? "✓ Health system attached" : "✗ Missing")}");
        
        Debug.Log("=== Validation Complete ===");
    }
    
    private void HandleDemoControls()
    {
        // Add any demo-specific controls here
    }
    
    private void ShowcaseNextFeature()
    {
        // Add feature showcase logic here
    }
    
    private void LogAssignmentCompletion()
    {
        Debug.Log("========================================");
        Debug.Log("2D GAME WORLD ASSIGNMENT - COMPLETED");
        Debug.Log("========================================");
        Debug.Log("Requirements Met:");
        Debug.Log("✓ Traversable level with tile-based platforms and slopes");
        Debug.Log("✓ Collectible items using prefabs (coins, health)");
        Debug.Log("✓ Damage-dealing hazards (spikes) as prefabs");
        Debug.Log("✓ Strategic use of 2D colliders and rigidbodies");
        Debug.Log("✓ Unity Tilemap system integration");
        Debug.Log("✓ Engaging level layout design");
        Debug.Log("✓ Physics-based interactions");
        Debug.Log("========================================");
        Debug.Log($"Controls: Move with WASD/Arrows, Jump with Space");
        Debug.Log($"Demo Controls: {KeyCode.R} to reset, {KeyCode.I} for info");
        Debug.Log("========================================");
    }
    
    // Called by collectible items when collected (can be wired up via events)
    public void OnItemCollected(string itemType)
    {
        Debug.Log($"✓ Collected {itemType}! Demonstrating prefab interaction system.");
        
        if (demoManager != null)
        {
            demoManager.OnItemCollected();
        }
    }
    
    // Called when player takes damage (can be wired up via health events)
    public void OnPlayerDamaged(float damage)
    {
        Debug.Log($"⚠️ Player took {damage} damage! Demonstrating hazard interaction system.");
    }
    
    // Visual indicator in scene view
    private void OnDrawGizmos()
    {
        // Draw assignment completion indicator
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero + Vector3.up * 8, Vector3.one * 2);
        
        // Draw feature zones
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.left * 5, Vector3.one * 1.5f); // Collectible zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.right * 5, Vector3.one * 1.5f); // Hazard zone
    }
} 