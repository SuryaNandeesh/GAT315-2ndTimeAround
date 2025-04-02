using UnityEngine;

public class CollisionInfo : MonoBehaviour
{
    Material material;
    Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<Renderer>().material;
        color = material.color;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            material.color = Color.green;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            material.color = Color.red;
        }
    }
}
