using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }
    
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public float invulnerabilityTime = 1.5f;
    public bool isInvulnerable = false;
    
    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public float blinkRate = 0.1f;
    public Color damageColor = Color.red;
    private Color originalColor;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip healSound;
    public AudioClip deathSound;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
        if (spriteRenderer)
            originalColor = spriteRenderer.color;
        
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }
    
    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if (audioSource && damageSound)
            audioSource.PlayOneShot(damageSound);
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityPeriod());
        }
    }
    
    public void Heal(int healAmount)
    {
        if (currentHealth >= maxHealth) return;
        
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if (audioSource && healSound)
            audioSource.PlayOneShot(healSound);
    }
    
    public void IncreaseMaxHealth(int increase)
    {
        maxHealth += increase;
        currentHealth += increase; // Also heal the player
    }
    
    void Die()
    {
        if (audioSource && deathSound)
            audioSource.PlayOneShot(deathSound);
        
        // Disable player movement
        MetroidvaniaPlayer playerController = GetComponent<MetroidvaniaPlayer>();
        if (playerController)
            playerController.enabled = false;
        
        // Trigger game over
        GameManager.Instance?.GameOver();
    }
    
    IEnumerator InvulnerabilityPeriod()
    {
        isInvulnerable = true;
        
        if (spriteRenderer)
        {
            float timer = 0f;
            while (timer < invulnerabilityTime)
            {
                // Blink effect
                spriteRenderer.color = Color.Lerp(originalColor, damageColor, Mathf.PingPong(timer * blinkRate * 10, 1));
                timer += Time.deltaTime;
                yield return null;
            }
            
            spriteRenderer.color = originalColor;
        }
        else
        {
            yield return new WaitForSeconds(invulnerabilityTime);
        }
        
        isInvulnerable = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle damage from hazards
        if (other.CompareTag("Hazard") || other.CompareTag("Enemy"))
        {
            DamageDealer damageDealer = other.GetComponent<DamageDealer>();
            if (damageDealer)
            {
                TakeDamage(damageDealer.damage);
            }
        }
    }
} 