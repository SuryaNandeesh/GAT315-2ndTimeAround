using UnityEngine;

public class MetroidvaniaCollectible : MonoBehaviour
{
    [Header("Collectible Type")]
    public CollectibleType type = CollectibleType.EnergyTank;
    public int value = 100;
    public string abilityToUnlock = "";
    
    [Header("Visual Effects")]
    public float rotationSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.5f;
    public GameObject collectEffect;
    public ParticleSystem particles;
    
    [Header("Audio")]
    public AudioClip collectSound;
    
    private Vector3 startPosition;
    private bool collected = false;
    
    public enum CollectibleType
    {
        EnergyTank,
        MissileExpansion,
        Ability,
        HealthPickup,
        ScoreItem
    }
    
    private void Start()
    {
        startPosition = transform.position;
    }
    
    private void Update()
    {
        if (collected) return;
        
        // Rotation animation
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        
        // Bobbing animation
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        
        if (other.CompareTag("Player"))
        {
            CollectItem();
        }
    }
    
    void CollectItem()
    {
        collected = true;
        
        // Play sound effect
        if (collectSound)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        
        // Handle different collectible types
        switch (type)
        {
            case CollectibleType.EnergyTank:
                GameManager.Instance?.CollectEnergyTank();
                break;
                
            case CollectibleType.MissileExpansion:
                GameManager.Instance?.CollectMissileExpansion();
                break;
                
            case CollectibleType.Ability:
                if (!string.IsNullOrEmpty(abilityToUnlock))
                {
                    GameManager.Instance?.UnlockAbility(abilityToUnlock);
                    ShowAbilityUnlockMessage();
                }
                break;
                
            case CollectibleType.HealthPickup:
                PlayerHealth.Instance?.Heal(value);
                GameManager.Instance?.AddScore(50);
                break;
                
            case CollectibleType.ScoreItem:
                GameManager.Instance?.AddScore(value);
                GameManager.Instance?.CollectItem();
                break;
        }
        
        // Visual effects
        if (collectEffect)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
        
        if (particles)
        {
            particles.Play();
        }
        
        // Hide the visual
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        
        // Destroy after effect
        Destroy(gameObject, 1f);
    }
    
    void ShowAbilityUnlockMessage()
    {
        string message = "";
        switch (abilityToUnlock.ToLower())
        {
            case "doublejump":
                message = "Double Jump Acquired!";
                break;
            case "dash":
                message = "Dash Ability Acquired!";
                break;
            case "walljump":
                message = "Wall Jump Acquired!";
                break;
            default:
                message = $"{abilityToUnlock} Acquired!";
                break;
        }
    }
} 