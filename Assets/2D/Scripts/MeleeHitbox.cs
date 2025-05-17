using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] LayerMask targetLayers; // Which layers this hitbox can damage

    Collider2D hitbox;

    void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            // If the object is on a target layer, attempt to damage it
            if (other.TryGetComponent(out CharacterController2D health))
            {
                health.OnHurt(damage);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (hitbox != null && hitbox.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitbox.bounds.center, hitbox.bounds.size);
        }
    }
}

