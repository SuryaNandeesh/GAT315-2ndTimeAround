using UnityEngine;

public class DmgBox : MonoBehaviour
{
    public int damageAmount = 1;
    public Color defaultColor = Color.red;
    public Color activeColor = Color.magenta;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Calculate knockback direction (up and away from the damage box)
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                knockbackDirection.y = 1f; // Always knock up
                
                player.TakeDamage(damageAmount, knockbackDirection);
                Debug.Log($"Player hit damage box! Damage: {damageAmount}");

                // Change color when active
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = activeColor;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset color when player leaves
            if (spriteRenderer != null)
            {
                spriteRenderer.color = defaultColor;
            }
        }
    }
} 