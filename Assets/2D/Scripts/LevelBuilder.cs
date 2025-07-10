using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap groundTilemap;
    public Tilemap hazardTilemap;
    public Tilemap collectibleTilemap;

    [Header("Tiles")]
    public TileBase groundTile;
    public TileBase spikeTile;
    public TileBase coinTile;
    public TileBase healthTile;

    [Header("Prefabs")]
    public GameObject coinPrefab;
    public GameObject healthPickupPrefab;
    public GameObject spikeHazardPrefab;
    public GameObject playerPrefab;

    [Header("Spawn Points")]
    public Vector3 playerSpawnPoint = new Vector3(-8f, 2f, 0f);

    [Header("Runtime Tile Replacement")]
    [SerializeField] private bool enableTileReplacement = true;
    [SerializeField] private bool clearTilesAfterReplacement = true;

    private void Start()
    {
        // Replace tiles with prefabs if enabled
        if (enableTileReplacement)
        {
            ReplaceTilesWithPrefabs();
        }

        // Spawn the player
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Player prefab not assigned in LevelBuilder!");
        }
    }

    public void ReplaceTilesWithPrefabs()
    {
        if (collectibleTilemap != null)
        {
            ReplaceTilesInTilemap(collectibleTilemap, coinTile, coinPrefab, "Coin");
            ReplaceTilesInTilemap(collectibleTilemap, healthTile, healthPickupPrefab, "Health Pickup");
        }

        if (hazardTilemap != null)
        {
            ReplaceTilesInTilemap(hazardTilemap, spikeTile, spikeHazardPrefab, "Spike Hazard");
        }
    }

    private void ReplaceTilesInTilemap(Tilemap tilemap, TileBase targetTile, GameObject prefab, string itemName)
    {
        if (tilemap == null || targetTile == null || prefab == null)
        {
            Debug.LogWarning($"Cannot replace {itemName} tiles - missing references!");
            return;
        }

        // Get the bounds of the tilemap
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        
        int replacedCount = 0;

        // Iterate through all positions in the tilemap
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int localPosition = new Vector3Int(x + bounds.x, y + bounds.y, 0);
                Vector3Int arrayPosition = new Vector3Int(x, y, 0);
                
                // Check if this position has the target tile
                if (allTiles[arrayPosition.x + arrayPosition.y * bounds.size.x] == targetTile)
                {
                    // Convert tile position to world position
                    Vector3 worldPosition = tilemap.CellToWorld(localPosition) + tilemap.tileAnchor;
                    
                    
                    // Instantiate the prefab
                    GameObject instance = Instantiate(prefab, worldPosition, Quaternion.identity);
                    instance.name = $"{itemName} (Tile Replaced)";
                    
                    // Clear the tile if enabled
                    if (clearTilesAfterReplacement)
                    {
                        tilemap.SetTile(localPosition, null);
                    }
                    
                    replacedCount++;
                }
            }
        }
        
        Debug.Log($"Replaced {replacedCount} {itemName} tiles with prefabs");
    }

    // Manual method to regenerate from tiles (can be called from inspector)
    [ContextMenu("Replace Tiles with Prefabs")]
    public void ManualTileReplacement()
    {
        // Clear existing prefab instances
        ClearExistingPrefabs();
        
        // Replace tiles with prefabs
        ReplaceTilesWithPrefabs();
    }

    private void ClearExistingPrefabs()
    {
        // Find and destroy existing collectibles and hazards
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Collectible");
        GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");
        
        foreach (GameObject coin in coins)
        {
            if (coin.name.Contains("(Tile Replaced)"))
                DestroyImmediate(coin);
        }
        
        foreach (GameObject hazard in hazards)
        {
            if (hazard.name.Contains("(Tile Replaced)"))
                DestroyImmediate(hazard);
        }
    }
} 