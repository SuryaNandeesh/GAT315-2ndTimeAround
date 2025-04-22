using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Health component for any damageable entity in the game
/// </summary>
[RequireComponent(typeof(Collider))]
public class Health : MonoBehaviour
{
	[Header("Health Settings")]
	[SerializeField] private float maxHealth = 100f;
	[SerializeField] private float currentHealth;
	[SerializeField] private bool destroyOnDeath = true;
	[SerializeField] private float destroyDelay = 0f;
	[SerializeField] private bool isInvulnerable = false;

	[Header("Events")]
	public UnityEvent OnDamaged;
	public UnityEvent OnHealed;
	public UnityEvent OnDeath;

	public float CurrentHealth => currentHealth;
	public float MaxHealth => maxHealth;
	public float HealthPercentage => currentHealth / maxHealth;
	public bool IsDead() => isDead;

	public void SetInvulnerable(bool value) => isInvulnerable = value;
	public void SetMaxHealth(float value)
	{
		maxHealth = value;
		currentHealth = Mathf.Min(currentHealth, maxHealth);
	}


	// Event with damage data
	public UnityEvent<DamageData> OnDamagedWithData;

	private bool isDead = false;

	private void Awake()
	{
		currentHealth = maxHealth;
	}

	/// <summary>
	/// Apply damage to this health component
	/// </summary>
	/// <param name="damage">Amount of damage to apply</param>
	/// <param name="damageType">Type of damage (optional)</param>
	/// <param name="damageSource">Source of the damage (optional)</param>
	public void TakeDamage(float damage, DamageType damageType = DamageType.Normal, GameObject damageSource = null)
	{
		// Don't apply damage if already dead or invulnerable
		if (isDead || isInvulnerable)
			return;

		currentHealth -= damage;

		// Create damage data for the event
		DamageData damageData = new DamageData
		{
			Amount = damage,
			DamageType = damageType,
			Source = damageSource,
			Target = this.gameObject
		};

		// Invoke damage events
		OnDamaged?.Invoke();
		OnDamagedWithData?.Invoke(damageData);

		// Check for death
		if (currentHealth <= 0 && !isDead)
		{
			Die();
		}
	}

	/// <summary>
	/// Heal the entity by the specified amount
	/// </summary>
	/// <param name="amount">Amount to heal</param>
	public void Heal(float amount)
	{
		if (isDead)
			return;

		currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
		OnHealed?.Invoke();
	}

	/// <summary>
	/// Kill this entity immediately
	/// </summary>
	public void Die()
	{
		if (isDead)
			return;

		isDead = true;
		currentHealth = 0;

		OnDeath?.Invoke();

		if (destroyOnDeath)
		{
			Destroy(gameObject, destroyDelay);
		}
	}
}