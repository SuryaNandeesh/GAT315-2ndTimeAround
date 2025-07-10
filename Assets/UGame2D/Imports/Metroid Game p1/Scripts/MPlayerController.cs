using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;

    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.linearVelocity.y);

        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

        if (Input.GetButtonDown("Jump") && isOnGround)
            {
                theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
            }
    }
}
