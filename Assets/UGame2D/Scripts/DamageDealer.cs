using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10;
    public bool isHazard = true;
    public bool dealsContinuousDamage = false;
    public float damageInterval = 1f;
    
    private float lastDamageTime;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage(other);
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (dealsContinuousDamage && other.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageInterval)
            {
                DealDamage(other);
            }
        }
    }
    
    void DealDamage(Collider2D player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth)
        {
            playerHealth.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
} 