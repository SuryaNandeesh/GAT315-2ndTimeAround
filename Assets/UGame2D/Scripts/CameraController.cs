using UnityEngine;

public class CameraController : MonoBehaviour
{
    private MetroidvaniaPlayer player;
    public BoxCollider2D cameraBounds;

    private float halfHeight, halfWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<MetroidvaniaPlayer>();

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(
                Mathf.Clamp(player.transform.position.x, cameraBounds.bounds.min.x + halfWidth, cameraBounds.bounds.max.x - halfWidth),
                Mathf.Clamp(player.transform.position.y, cameraBounds.bounds.min.y + halfHeight, cameraBounds.bounds.max.y - halfHeight),
                transform.position.z
            );
        }
    }
}
