using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public float invincibilityDuration = 1f;
    public float knockbackForce = 5f;

    [Header("UI")]
    public Slider healthBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI debugText;

    [Header("Animation")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;
    private float invincibilityTimer;
    private int score = 0;
    private float debugMessageTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateScoreUI();
        ShowDebugMessage("Game Started! Use A/D to move and Space to jump");
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movement input
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Update animations
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
        }

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            ShowDebugMessage("Jumped!");
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }

        // Flip character based on movement direction
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Better jump physics
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Update invincibility timer
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            // Flash the sprite when invincible
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1, 1, 1, Mathf.PingPong(Time.time * 10, 1));
            }
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        // Update debug message timer
        if (debugMessageTimer > 0)
        {
            debugMessageTimer -= Time.deltaTime;
            if (debugMessageTimer <= 0)
            {
                debugText.text = "";
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (invincibilityTimer <= 0)
        {
            currentHealth -= damage;
            invincibilityTimer = invincibilityDuration;
            UpdateHealthUI();

            // Apply knockback
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);

            // Trigger hurt animation
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }

            ShowDebugMessage($"Took {damage} damage! Health: {currentHealth}/{maxHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        ShowDebugMessage($"Collected coin! Score: {score}");
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
        ShowDebugMessage($"Healed for {amount}! Health: {currentHealth}/{maxHealth}");
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    private void ShowDebugMessage(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
            debugMessageTimer = 2f; // Show message for 2 seconds
        }
        Debug.Log(message);
    }

    private void Die()
    {
        ShowDebugMessage("Game Over!");
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        // Add game over logic here
    }
} 