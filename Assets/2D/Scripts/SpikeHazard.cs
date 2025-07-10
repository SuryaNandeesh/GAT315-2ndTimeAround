using UnityEngine;

public class SpikeHazard : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 20;
    public float knockbackForce = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // Calculate knockback direction (up and away from the spike)
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                knockbackDirection.y = 1f; // Always knock up
                
                player.TakeDamage(damage, knockbackDirection);
                Debug.Log($"Player hit spike! Damage: {damage}");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Calculate knockback direction (up and away from the spike)
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                knockbackDirection.y = 1f; // Always knock up
                
                player.TakeDamage(damage, knockbackDirection);
                Debug.Log($"Player hit spike! Damage: {damage}");
            }
        }
    }
} 