using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer = 1;
    
    [Header("Physics Settings")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isGrounded;
    private bool wasGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float horizontalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        HandleInput();
        CheckGrounded();
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleJump();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
        HandleVariableJumpHeight();
    }
    
    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }
    
    private void CheckGrounded()
    {
        wasGrounded = isGrounded;
        
        // Cast a slightly wider and shorter box below the player
        Vector2 boxSize = new Vector2(boxCollider.size.x * 0.8f, groundCheckDistance);
        Vector2 boxCenter = (Vector2)transform.position + boxCollider.offset + Vector2.down * (boxCollider.size.y / 2 + groundCheckDistance / 2);
        
        isGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer);
        
        // Reset coyote time when landing
        if (!wasGrounded && isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
    }
    
    private void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    
    private void HandleJumpBuffer()
    {
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    
    private void HandleJump()
    {
        // Jump if we have jump input buffered and we can jump (grounded or coyote time)
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            Jump();
        }
    }
    
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpBufferCounter = 0;
        coyoteTimeCounter = 0;
    }
    
    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }
    
    private void HandleVariableJumpHeight()
    {
        // Apply additional downward force when falling for snappier feel
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Apply additional downward force when releasing jump early for variable jump height
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    
    public bool IsGrounded()
    {
        return isGrounded;
    }
    
    public Vector2 GetVelocity()
    {
        return rb.linearVelocity;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;
        
        // Draw ground check box
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector2 boxSize = new Vector2(boxCollider.size.x * 0.8f, groundCheckDistance);
        Vector2 boxCenter = (Vector2)transform.position + boxCollider.offset + Vector2.down * (boxCollider.size.y / 2 + groundCheckDistance / 2);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
} 