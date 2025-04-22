using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Component that deals damage to health components on contact
/// </summary>
public class DamageInflictor : MonoBehaviour
{
	[Header("Damage Settings")]
	[SerializeField] private float damageAmount = 10f;        // Base damage amount to apply
	[SerializeField] private DamageType damageType = DamageType.Normal; // Type of damage to apply
	[SerializeField] private float damageInterval = 0f;       // Time between damage applications (0 = once only)
	[SerializeField] private bool destroyOnDamage = false;    // Whether to destroy this object after damage is dealt
	[SerializeField] private float destroyDelay = 0;          // Delay before destroying this object

	[Header("Objects to Damage")]
	[SerializeField] private string[] tagsToAffect;           // Only damage objects with these tags
	[SerializeField] private LayerMask affectLayers = Physics.AllLayers; // Only damage objects on these layers

	[Header("Events")]
	public UnityEvent<GameObject> OnDamageDealt;              // Event triggered when damage is successfully applied

	private float lastDamageTime;                             // Tracks when damage was last applied for interval calculations

	/// <summary>
	/// Initialize the last damage time to allow immediate damage on first contact
	/// </summary>
	private void Awake()
	{
		lastDamageTime = 0;
	}

	/// <summary>
	/// Called when another collider enters this object's trigger collider
	/// </summary>
	private void OnTriggerEnter(Collider other)
	{
		TryDamage(other.gameObject);
	}

	/// <summary>
	/// Called every frame while another collider is inside this object's trigger collider
	/// Used for applying damage at intervals
	/// </summary>
	private void OnTriggerStay(Collider other)
	{
		// Skip if no interval is set (one-time damage only)
		if (damageInterval <= 0)
			return;

		// Check if enough time has passed since last damage application
		if (Time.time - lastDamageTime >= damageInterval)
		{
			// Try to apply damage and update timer if successful
			if (TryDamage(other.gameObject))
			{
				lastDamageTime = Time.time;
			}
		}
	}

	/// <summary>
	/// Called when this object collides with another collider
	/// </summary>
	private void OnCollisionEnter(Collision collision)
	{
		TryDamage(collision.gameObject);
	}

	/// <summary>
	/// Called every frame while this object is colliding with another collider
	/// Used for applying damage at intervals
	/// </summary>
	private void OnCollisionStay(Collision collision)
	{
		// Skip if no interval is set (one-time damage only)
		if (damageInterval <= 0)
			return;

		// Check if enough time has passed since last damage application
		if (Time.time - lastDamageTime >= damageInterval)
		{
			// Try to apply damage and update timer if successful
			if (TryDamage(collision.gameObject))
			{
				lastDamageTime = Time.time;
			}
		}
	}

	/// <summary>
	/// Attempt to apply damage to an object if it has a health component
	/// </summary>
	/// <param name="target">The GameObject to potentially damage</param>
	/// <returns>True if damage was applied successfully</returns>
	private bool TryDamage(GameObject target)
	{
		// Check if the other object is on a valid interaction layer
		// The bitwise operation creates a mask for the target's layer and compares it with our affectLayers mask
		if ((affectLayers & (1 << target.layer)) == 0)
		{
			return false; // Object's layer is not in our affect layers mask
		}

		// Check if we should damage this object based on tags
		if (tagsToAffect != null && tagsToAffect.Length > 0)
		{
			bool hasValidTag = false;
			// Loop through all tags that we can affect
			foreach (string tag in tagsToAffect)
			{
				if (target.CompareTag(tag))
				{
					hasValidTag = true;
					break;
				}
			}

			// Return false if the object doesn't have any of our valid tags
			if (!hasValidTag)
				return false;
		}

		// Get health component and apply damage
		Health health = target.GetComponent<Health>();

		if (health != null)
		{
			// Apply damage to the health component
			health.TakeDamage(damageAmount, damageType, this.gameObject);

			// Trigger event to notify listeners that damage was dealt
			OnDamageDealt?.Invoke(target);

			// Destroy this object if configured to do so
			if (destroyOnDamage)
			{
				Destroy(this.gameObject, destroyDelay);
			}

			return true; // Damage was successfully applied
		}

		return false; // No health component found or damage not applied
	}
}