using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D rb;

    public Vector2 moveDir;

    public GameObject impactEffect;

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = moveDir.x * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    
    private void OnBecomeInvisible()
    {
        Destroy(gameObject);
    }
}
