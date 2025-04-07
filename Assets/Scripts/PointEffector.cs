using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PointEffector : MonoBehaviour
{
	[SerializeField, Range(-100, 100)] float force = 10f;        // The force applied to the objects
	[SerializeField] float minRadius = 1f;     // The minimum radius of the effect
	[SerializeField] float maxRadius = 10f;    // The maximum radius of the effect
	[SerializeField] ForceApply forceApply = ForceApply.Linear;
	[SerializeField] LayerMask affectedLayers = Physics.AllLayers; // The layers that will be affected by the effector

	private SphereCollider sphereCollider;

	enum ForceApply
	{
		Constant,
		Linear,
	}

	void OnValidate()
	{
		// Update radius when values change in inspector
		transform.localScale = Vector3.one * maxRadius;
	}

	void Awake()
	{
		// Cache and setup the collider
		sphereCollider = GetComponent<SphereCollider>();
		sphereCollider.radius = maxRadius;
		sphereCollider.isTrigger = true;
		sphereCollider.radius = 0.5f;
	}

	private void OnTriggerStay(Collider other)
	{
		Rigidbody rb = other.attachedRigidbody;
		if (rb == null || (affectedLayers & (1 << other.gameObject.layer)) == 0) return;

		Vector3 direction = other.transform.position - transform.position;
		float distance = direction.magnitude;

		Debug.DrawLine(transform.position, other.transform.position, Color.red);
		// Calculate the force based on the distance
		Vector3 forceVector;
		if (forceApply == ForceApply.Constant)
		{
			// Apply a constant force
			forceVector = direction.normalized * force;
		}
		else // ForceApply.Linear
		{
			// Apply a linear force that decreases with distance
			float t = Mathf.InverseLerp(minRadius, maxRadius, distance);
			forceVector = direction.normalized * force * (1 - t);
		}

		// Apply the force
		rb.AddForce(forceVector);
	}
}