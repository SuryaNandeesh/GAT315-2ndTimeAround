using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Behavior")]
    public EnemyType enemyType = EnemyType.Patrol;
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float patrolDistance = 4f;
    
    [Header("Combat")]
    public int health = 30;
    public int damage = 15;
    public float attackCooldown = 1.5f;
    public LayerMask playerLayer = 1 << 8;
    public LayerMask groundLayer = 1;
    
    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Color damageColor = Color.red;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    
    // Private variables
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private Vector3 startPosition;
    private bool isPlayerInRange = false;
    private Transform player;
    private float lastAttackTime;
    private bool isDead = false;
    private Color originalColor;
    
    // Patrol variables
    private bool movingRight = true;
    private Vector3 leftBoundary;
    private Vector3 rightBoundary;
    
    public enum EnemyType
    {
        Patrol,
        Guard,
        Chaser,
        Flying
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        
        if (spriteRenderer)
            originalColor = spriteRenderer.color;
        
        // Set patrol boundaries
        leftBoundary = startPosition - Vector3.right * patrolDistance;
        rightBoundary = startPosition + Vector3.right * patrolDistance;
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
            player = playerObj.transform;
    }
    
    private void Update()
    {
        if (isDead) return;
        
        CheckForPlayer();
        
        switch (enemyType)
        {
            case EnemyType.Patrol:
                PatrolBehavior();
                break;
            case EnemyType.Guard:
                GuardBehavior();
                break;
            case EnemyType.Chaser:
                ChaserBehavior();
                break;
            case EnemyType.Flying:
                FlyingBehavior();
                break;
        }
        
        UpdateAnimations();
    }
    
    void CheckForPlayer()
    {
        if (!player) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isPlayerInRange = distanceToPlayer <= detectionRange;
        
        // Attack if player is in range
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
        }
    }
    
    void PatrolBehavior()
    {
        if (isPlayerInRange && player)
        {
            // Chase player if detected
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
            
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
                Flip();
        }
        else
        {
            // Normal patrol
            if (movingRight)
            {
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
                if (transform.position.x >= rightBoundary.x || !IsGroundAhead())
                {
                    movingRight = false;
                    Flip();
                }
            }
            else
            {
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
                if (transform.position.x <= leftBoundary.x || !IsGroundAhead())
                {
                    movingRight = true;
                    Flip();
                }
            }
        }
    }
    
    void GuardBehavior()
    {
        if (isPlayerInRange && player)
        {
            // Turn to face player but don't chase
            Vector2 direction = (Vector2)player.position - (Vector2)transform.position;
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
                Flip();
        }
        
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
    
    void ChaserBehavior()
    {
        if (isPlayerInRange && player)
        {
            // Always chase player when in range
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed * 1.2f, rb.linearVelocity.y);
            
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
                Flip();
        }
        else
        {
            // Return to start position slowly
            Vector2 direction = ((Vector2)startPosition - (Vector2)transform.position).normalized;
            if (Vector2.Distance(transform.position, startPosition) > 0.5f)
            {
                rb.linearVelocity = new Vector2(direction.x * moveSpeed * 0.5f, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }
    
    void FlyingBehavior()
    {
        if (isPlayerInRange && player)
        {
            // Fly towards player
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
                Flip();
        }
        else
        {
            // Hover around start position
            Vector2 hoverTarget = (Vector2)startPosition + new Vector2(
                Mathf.Sin(Time.time) * 2f,
                Mathf.Cos(Time.time * 0.5f) * 1f
            );
            
            Vector2 direction = (hoverTarget - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed * 0.3f;
        }
    }
    
    bool IsGroundAhead()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection + Vector2.down, 1.5f, groundLayer);
        return hit.collider != null;
    }
    
    void AttackPlayer()
    {
        lastAttackTime = Time.time;
        
        if (audioSource && attackSound)
            audioSource.PlayOneShot(attackSound);
        
        // Deal damage to player
        if (player && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth)
                playerHealth.TakeDamage(damage);
        }
        
        if (animator)
            animator.SetTrigger("Attack");
    }
    
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;
        
        health -= damageAmount;
        
        if (audioSource && damageSound)
            audioSource.PlayOneShot(damageSound);
        
        // Visual feedback
        if (spriteRenderer)
        {
            spriteRenderer.color = damageColor;
            Invoke("ResetColor", 0.2f);
        }
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    void ResetColor()
    {
        if (spriteRenderer)
            spriteRenderer.color = originalColor;
    }
    
    void Die()
    {
        isDead = true;
        
        if (audioSource && deathSound)
            audioSource.PlayOneShot(deathSound);
        
        if (animator)
            animator.SetTrigger("Death");
        
        // Disable colliders and movement
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
            col.enabled = false;
        
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        
        // Notify game manager
        GameManager.Instance?.EnemyDefeated();
        
        // Destroy after animation
        Destroy(gameObject, 2f);
    }
    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        if (spriteRenderer)
            spriteRenderer.flipX = !isFacingRight;
    }
    
    void UpdateAnimations()
    {
        if (animator)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            animator.SetBool("IsPlayerInRange", isPlayerInRange);
            animator.SetBool("IsDead", isDead);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw patrol boundaries for patrol enemies
        if (enemyType == EnemyType.Patrol)
        {
            Gizmos.color = Color.blue;
            Vector3 start = Application.isPlaying ? startPosition : transform.position;
            Gizmos.DrawLine(start - Vector3.right * patrolDistance, start + Vector3.right * patrolDistance);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal contact damage
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth && !playerHealth.isInvulnerable)
            {
                playerHealth.TakeDamage(damage / 2); // Reduced contact damage
            }
        }
    }
} 