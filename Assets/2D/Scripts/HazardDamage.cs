using UnityEngine;
using System.Collections;

public class HazardDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private bool continuousDamage = false;
    [SerializeField] private float damageInterval = 1f;
    
    private AudioSource audioSource;
    private bool playerInRange = false;
    private Coroutine damageCoroutine;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            DealDamage(other.gameObject);
            
            if (continuousDamage)
            {
                damageCoroutine = StartCoroutine(ContinuousDamageCoroutine(other.gameObject));
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
    
    private void DealDamage(GameObject player)
    {
        // Play damage sound if available
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
        
        // Apply damage to player
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage, DamageType.Physical, gameObject);
            Debug.Log($"Player took {damage} damage from hazard!");
        }
    }
    
    private IEnumerator ContinuousDamageCoroutine(GameObject player)
    {
        while (playerInRange)
        {
            yield return new WaitForSeconds(damageInterval);
            
            if (playerInRange)
            {
                DealDamage(player);
            }
        }
    }
    
    public int GetDamage()
    {
        return damage;
    }
} 