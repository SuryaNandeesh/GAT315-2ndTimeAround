using UnityEngine;

public class CollectibleRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 90f;
    
    private void Update()
    {
        // Rotate the collectible around the Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
} 