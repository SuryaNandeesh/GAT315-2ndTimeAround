using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MetroidvaniaPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 30f;
    public float deceleration = 50f;
    public float airAcceleration = 15f;
    
    [Header("Jumping")]
    public float jumpForce = 16f;
    public float jumpCutMultiplier = 0.5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    
    [Header("Wall Movement")]
    public float wallSlideSpeed = 3f;
    public float wallJumpForce = 15f;
    public Vector2 wallJumpDirection = new Vector2(1, 1.5f);
    public float wallJumpTime = 0.2f;
    
    [Header("Dash")]
    public float dashForce = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    
    [Header("Ground/Wall Detection")]
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayerMask = 1;
    public float groundCheckRadius = 0.2f;
    public float wallCheckDistance = 0.5f;
    
    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public BulletController bulletPrefab;
    public Transform bulletSpawnPoint;

    // Private variables
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isOnWall;
    private bool isFacingRight = true;
    private bool canDoubleJump;
    private bool hasDoubleJumped;
    private bool isWallSliding;
    private bool isDashing;
    private bool canDash = true;
    
    // Timers
    private float coyoteCounter;
    private float jumpBufferCounter;
    private float wallJumpCounter;
    private float dashCooldownTimer;
    
    // Input
    private float horizontalInput;
    private bool jumpInput;
    private bool jumpInputDown;
    private bool dashInputDown;
    private bool fireInputDown;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Create ground check if not assigned
        if (groundCheck == null)
        {
            groundCheck = new GameObject("GroundCheck").transform;
            groundCheck.SetParent(transform);
            groundCheck.localPosition = new Vector3(0, -0.5f, 0);
        }
        
        // Create wall check if not assigned
        if (wallCheck == null)
        {
            wallCheck = new GameObject("WallCheck").transform;
            wallCheck.SetParent(transform);
            wallCheck.localPosition = new Vector3(0.5f, 0, 0);
        }
    }
    
    private void Update()
    {
        if (GameManager.Instance && GameManager.Instance.isGamePaused) return;
        
        HandleInput();
        UpdateTimers();
        CheckEnvironment();
        HandleMovement();
        HandleJumping();
        HandleWallSliding();
        HandleDashing();
        UpdateAnimations();
    }
    
    void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButton("Jump");
        jumpInputDown = Input.GetButtonDown("Jump");
        dashInputDown = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.X);
        fireInputDown = Input.GetButtonDown("Fire1");
        //shooting
        if (fireInputDown && bulletPrefab && bulletSpawnPoint)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).moveDir = new Vector2(transform.localScale.x, 0f);

            animator.SetTrigger("ShotFired");
        }

        if(dashInputDown && canDash && GameManager.Instance.HasAbility("dash"))
        {
            StartCoroutine(Dash());
            animator.SetTrigger("IsDashing");
        }
    }
    
    void UpdateTimers()
    {
        coyoteCounter -= Time.deltaTime;
        jumpBufferCounter -= Time.deltaTime;
        wallJumpCounter -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        
        if (jumpInputDown)
            jumpBufferCounter = jumpBufferTime;
    }
    
    void CheckEnvironment()
    {
        // Ground check
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);
        
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            hasDoubleJumped = false;
        }
        
        // Wall check
        Vector2 wallCheckDir = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, wallCheckDir, wallCheckDistance, groundLayerMask);
        isOnWall = wallHit.collider != null;
        
        // Wall sliding
        isWallSliding = isOnWall && !isGrounded && rb.linearVelocity.y < 0 && GameManager.Instance.HasAbility("walljump");
    }
    
    void HandleMovement()
    {
        if (isDashing || wallJumpCounter > 0) return;
        
        float targetSpeed = horizontalInput * moveSpeed;
        float accel = isGrounded ? acceleration : airAcceleration;
        
        if (horizontalInput == 0)
            accel = deceleration;
        
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float movement = speedDiff * accel;
        
        rb.AddForce(Vector2.right * movement * Time.deltaTime, ForceMode2D.Force);
        
        // Handle sprite flipping
        if (horizontalInput > 0 && !isFacingRight)
            Flip();
        else if (horizontalInput < 0 && isFacingRight)
            Flip();
    }
    
    void HandleJumping()
    {
        if (isDashing) return;
        
        // Regular jump
        if (jumpBufferCounter > 0 && (coyoteCounter > 0 || isWallSliding))
        {
            Jump();
            jumpBufferCounter = 0;
        }
        // Double jump
        else if (jumpInputDown && !hasDoubleJumped && !isGrounded && GameManager.Instance.HasAbility("doublejump"))
        {
            DoubleJump();
        }
        // Wall jump
        else if (jumpInputDown && isOnWall && GameManager.Instance.HasAbility("walljump"))
        {
            WallJump();
        }
        
        // Variable jump height
        if (!jumpInput && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
        
        // Better falling physics
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jumpInput)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    
    void HandleWallSliding()
    {
        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }
    
    void HandleDashing()
    {
        if (dashInputDown && canDash && GameManager.Instance.HasAbility("dash"))
        {
            StartCoroutine(Dash());
        }
        
        if (dashCooldownTimer <= 0 && isGrounded)
        {
            canDash = true;
        }
    }
    
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        coyoteCounter = 0;
        GameManager.Instance?.PlaySFX("Jump");
    }
    
    void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        hasDoubleJumped = true;
        GameManager.Instance?.PlaySFX("DoubleJump");
    }
    
    void WallJump()
    {
        Vector2 jumpDir = isFacingRight ? new Vector2(-wallJumpDirection.x, wallJumpDirection.y) : wallJumpDirection;
        rb.linearVelocity = jumpDir * wallJumpForce;
        wallJumpCounter = wallJumpTime;
        GameManager.Instance?.PlaySFX("WallJump");
    }
    
    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        dashCooldownTimer = dashCooldown;
        
        Vector2 dashDirection = new Vector2(isFacingRight ? 1 : -1, 0);
        if (horizontalInput != 0)
            dashDirection.x = horizontalInput;
        
        rb.linearVelocity = dashDirection.normalized * dashForce;
        
        GameManager.Instance?.PlaySFX("Dash");
        
        yield return new WaitForSeconds(dashTime);
        
        isDashing = false;
    }
    
    void UpdateAnimations()
    {
        if (animator)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            animator.SetFloat("VelocityY", rb.linearVelocity.y);
            animator.SetBool("IsOnGround", isGrounded);
            animator.SetBool("IsWallSliding", isWallSliding);
            animator.SetBool("IsDashing", isDashing);
        }
    }
    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        if (spriteRenderer)
            spriteRenderer.flipX = !isFacingRight;
        
        // Flip wall check position
        Vector3 wallCheckPos = wallCheck.localPosition;
        wallCheckPos.x *= -1;
        wallCheck.localPosition = wallCheckPos;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        
        if (wallCheck)
        {
            Gizmos.color = isOnWall ? Color.blue : Color.white;
            Vector3 direction = isFacingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + direction * wallCheckDistance);
        }
    }
} 