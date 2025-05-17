using UnityEngine;

public class DmgZone : MonoBehaviour
{
    [SerializeField] int damageAmount = 1; // how much damage to deal
    [SerializeField] Color defaultColor = Color.red;
    [SerializeField] Color activeColor = Color.magenta; // Color when player is inside

    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            spriteRenderer.color = defaultColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered is the player
        if (collision.CompareTag("Player"))
        {
            // Change color when player enters
            if (spriteRenderer)
            {
                spriteRenderer.color = activeColor;
            }

            // Try to find the player's health script and deal damage
            CharacterController2D player = collision.GetComponent<CharacterController2D>();
            if (player != null)
            {
                player.OnHurt(damageAmount);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reset color when player leaves
            if (spriteRenderer)
            {
                spriteRenderer.color = defaultColor;
            }
        }
    }
}
