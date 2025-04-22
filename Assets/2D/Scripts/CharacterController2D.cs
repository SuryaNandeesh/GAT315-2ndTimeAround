using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float jumpHeight = 1;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    Rigidbody2D rb;
    Vector3 force;
    Vector2 direction;

    //public void OnMove(Vector2 v) => direction = v;
    //{
        
    //}

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        force = direction * speed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(force, ForceMode2D.Force);
        rb.linearVelocity = new Vector2(force.x, rb.linearVelocityY);
    }

    public void OnJump()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight), ForceMode2D.Impulse);
    }

    public void OnAttack()
    {

    }

    public void OnHurt()
    {

    }

    public void Onidk()
    {
        
    }
}