using UnityEngine;

public class TriggerInfo : MonoBehaviour
{
    Material material;
    Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<Renderer>().material;
        color = material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            material.color = Color.green;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            material.color = Color.red;
        }
    }
}
