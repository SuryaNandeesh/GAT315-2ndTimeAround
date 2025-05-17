using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float jumpHeight = 1;
    [SerializeField] int maxHealth = 5;
    [SerializeField] float invincibilityDuration = 1f;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Melee Attack Settings")]
    [SerializeField] Collider2D meleeHitbox; // Reference to your melee hitbox
    [SerializeField] float attackDuration = 0.2f; // How long the hitbox is active

    Rigidbody2D rb;
    Vector3 force;
    Vector2 direction;
    bool isRunning;
    bool isInvincible;
    int currentHealth;

    //public void OnMove(Vector2 v) => direction = v;
    //{

    //}

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (meleeHitbox != null)
            meleeHitbox.enabled = false; // Make sure it's off initially
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        //sprint or run mechanic
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        force = direction * speed;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.F)) // F key for attack
        {
            OnAttack();
        }

        animator.SetFloat("Speed", Mathf.Abs(direction.x));

        if(direction.x > 0.05f) spriteRenderer.flipX = false;
        else if (direction.x < -0.05f) spriteRenderer.flipX = true;

        speed = isRunning ? 5f : 0.9f;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(force.x, rb.linearVelocityY);
        //rb.AddForce(force, ForceMode2D.Force);
    }

    public void OnJump()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight), ForceMode2D.Impulse);
    }

    public void OnAttack()
    {
        animator.SetTrigger("Attack");

        if (meleeHitbox != null)
            StartCoroutine(ActivateMeleeHitbox());
    }

    IEnumerator ActivateMeleeHitbox()
    {
        meleeHitbox.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        meleeHitbox.enabled = false;
    }

    public void OnHurt(int damage)
    {
        if (isInvincible)
            return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            OnDie();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        spriteRenderer.color = Color.red; // Flash red to indicate invincibility
        yield return new WaitForSeconds(invincibilityDuration);
        spriteRenderer.color = Color.white; // Reset color
        isInvincible = false;
    }

    public void OnDie()
    {
        
    }

    bool IsGrounded()
    {
        // Simplified: assume grounded if vertical velocity is near 0 and slightly negative
        return Mathf.Abs(rb.linearVelocity.y) < 0.1f;
    }
}