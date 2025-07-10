using UnityEngine;
using System.Collections;

public class PhysicsInteractable : MonoBehaviour
{
    [Header("Interaction Type")]
    public InteractionType type = InteractionType.MovingPlatform;
    
    [Header("Moving Platform")]
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public bool isLooping = true;
    public float waitTime = 1f;
    
    [Header("Spring/Bouncer")]
    public float bounceForce = 20f;
    public AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Destructible")]
    public int health = 3;
    public int damageRequirement = 1;
    public GameObject destructionEffect;
    public AudioClip destructionSound;
    
    [Header("Pressure Plate")]
    public GameObject[] objectsToActivate;
    public bool requiresWeight = true;
    public float activationWeight = 1f;
    
    [Header("Physics")]
    public Rigidbody2D rb;
    public SpringJoint2D springJoint;
    public HingeJoint2D hingeJoint;
    
    [Header("Visual Feedback")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Color activatedColor = Color.green;
    
    private int currentWaypoint = 0;
    private bool movingForward = true;
    private bool isActivated = false;
    private Vector3 originalScale;
    private Color originalColor;
    private int playersOnPlatform = 0;
    
    public enum InteractionType
    {
        MovingPlatform,
        Spring,
        DestructibleBlock,
        PressurePlate,
        Pendulum,
        Seesaw
    }
    
    private void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        if (spriteRenderer) originalColor = spriteRenderer.color;
        
        InitializeByType();
    }
    
    void InitializeByType()
    {
        switch (type)
        {
            case InteractionType.MovingPlatform:
                if (waypoints.Length == 0)
                {
                    Debug.LogWarning("Moving platform needs waypoints!");
                }
                break;
                
            case InteractionType.Spring:
                // Ensure we have a trigger collider for springs
                Collider2D[] colliders = GetComponents<Collider2D>();
                bool hasTrigger = false;
                foreach (var col in colliders)
                {
                    if (col.isTrigger) hasTrigger = true;
                }
                if (!hasTrigger)
                {
                    BoxCollider2D triggerCol = gameObject.AddComponent<BoxCollider2D>();
                    triggerCol.isTrigger = true;
                }
                break;
                
            case InteractionType.Pendulum:
                if (!hingeJoint)
                {
                    hingeJoint = gameObject.AddComponent<HingeJoint2D>();
                    hingeJoint.useMotor = false;
                    hingeJoint.useLimits = true;
                    hingeJoint.limits = new JointAngleLimits2D { min = -45, max = 45 };
                }
                break;
        }
    }
    
    private void Update()
    {
        switch (type)
        {
            case InteractionType.MovingPlatform:
                UpdateMovingPlatform();
                break;
                
            case InteractionType.PressurePlate:
                UpdatePressurePlate();
                break;
        }
    }
    
    void UpdateMovingPlatform()
    {
        if (waypoints.Length < 2) return;
        
        Transform target = waypoints[currentWaypoint];
        float step = moveSpeed * Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAndMoveToNext());
        }
    }
    
    IEnumerator WaitAndMoveToNext()
    {
        yield return new WaitForSeconds(waitTime);
        
        if (isLooping)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
        else
        {
            if (movingForward)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length - 1)
                {
                    movingForward = false;
                }
            }
            else
            {
                currentWaypoint--;
                if (currentWaypoint <= 0)
                {
                    movingForward = true;
                }
            }
        }
    }
    
    void UpdatePressurePlate()
    {
        bool shouldActivate = playersOnPlatform > 0;
        
        if (shouldActivate != isActivated)
        {
            isActivated = shouldActivate;
            
            if (isActivated)
            {
                ActivatePressurePlate();
            }
            else
            {
                DeactivatePressurePlate();
            }
        }
    }
    
    void ActivatePressurePlate()
    {
        if (spriteRenderer)
            spriteRenderer.color = activatedColor;
        
        if (animator)
            animator.SetBool("Activated", true);
        
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj)
            {
                // Try different activation methods
                PhysicsInteractable interactable = obj.GetComponent<PhysicsInteractable>();
                if (interactable && interactable.type == InteractionType.MovingPlatform)
                {
                    // Start moving platform if it's not already moving
                    interactable.enabled = true;
                }
                
                // Simply activate the object
                obj.SetActive(true);
            }
        }
        
        GameManager.Instance?.PlaySFX("PressurePlateActivate");
    }
    
    void DeactivatePressurePlate()
    {
        if (spriteRenderer)
            spriteRenderer.color = originalColor;
        
        if (animator)
            animator.SetBool("Activated", false);
        
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj)
            {
                PhysicsInteractable interactable = obj.GetComponent<PhysicsInteractable>();
                if (interactable && interactable.type == InteractionType.MovingPlatform)
                {
                    // Stop moving platform
                    interactable.enabled = false;
                }
                
                // Deactivate the object if needed (optional)
                // obj.SetActive(false);
            }
        }
        
        GameManager.Instance?.PlaySFX("PressurePlateDeactivate");
    }
    
    public void TakeDamage(int damage)
    {
        if (type != InteractionType.DestructibleBlock) return;
        
        health -= damage;
        
        if (animator)
            animator.SetTrigger("Hit");
        
        if (health <= 0)
        {
            DestroyBlock();
        }
    }
    
    void DestroyBlock()
    {
        if (destructionEffect)
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        
        if (destructionSound)
            AudioSource.PlayClipAtPoint(destructionSound, transform.position);
        
        GameManager.Instance?.AddScore(25);
        
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (type)
        {
            case InteractionType.Spring:
                if (other.CompareTag("Player"))
                {
                    ActivateSpring(other);
                }
                break;
                
            case InteractionType.PressurePlate:
                if (other.CompareTag("Player"))
                {
                    playersOnPlatform++;
                }
                break;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (type == InteractionType.PressurePlate && other.CompareTag("Player"))
        {
            playersOnPlatform--;
            if (playersOnPlatform < 0) playersOnPlatform = 0;
        }
    }
    
    void ActivateSpring(Collider2D player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb)
        {
            // Apply spring force
            Vector2 springDirection = Vector2.up;
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
            
            StartCoroutine(SpringAnimation());
            GameManager.Instance?.PlaySFX("Spring");
        }
    }
    
    IEnumerator SpringAnimation()
    {
        float timer = 0f;
        float animTime = 0.3f;
        
        while (timer < animTime)
        {
            float scaleMultiplier = bounceCurve.Evaluate(timer / animTime);
            transform.localScale = originalScale * (1f + scaleMultiplier * 0.3f);
            timer += Time.deltaTime;
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == InteractionType.Seesaw && collision.gameObject.CompareTag("Player"))
        {
            // Apply force based on collision point
            Vector2 contactPoint = collision.contacts[0].point;
            Vector2 center = transform.position;
            
            float torque = (contactPoint.x - center.x) * collision.rigidbody.mass * 10f;
            rb.AddTorque(torque);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (type == InteractionType.MovingPlatform && waypoints != null)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
                    
                    if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                    }
                    else if (isLooping && waypoints[0] != null)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                    }
                }
            }
        }
    }
} 