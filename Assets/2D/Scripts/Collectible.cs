using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private int collectValue = 10;
    [SerializeField] private AudioClip collectSound;
    
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }
    
    private void Collect(GameObject player)
    {
        // Play collect sound if available
        if (collectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
        
        // Add score to player
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.AddScore(collectValue);
            Debug.Log($"Collected item worth {collectValue} points!");
        }
        else
        {
            Debug.LogWarning("PlayerController not found on player object!");
        }
        
        // Destroy the collectible
        if (collectSound != null && audioSource != null)
        {
            // Delay destruction to allow sound to play
            Destroy(gameObject, collectSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public int GetValue()
    {
        return collectValue;
    }
} 