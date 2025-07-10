using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int healAmount = 25;
    [SerializeField] private AudioClip healSound;
    
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealPlayer(other.gameObject);
        }
    }
    
    private void HealPlayer(GameObject player)
    {
        // Play heal sound if available
        if (healSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(healSound);
        }
        
        // Heal the player using PlayerController
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.Heal(healAmount);
            Debug.Log($"Player healed for {healAmount} health!");
        }
        else
        {
            Debug.LogWarning("PlayerController not found on player object!");
        }
        
        // Destroy the health pickup
        if (healSound != null && audioSource != null)
        {
            // Delay destruction to allow sound to play
            Destroy(gameObject, healSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public int GetHealAmount()
    {
        return healAmount;
    }
} 