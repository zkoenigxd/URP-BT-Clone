using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    GameObject hitObject;
    ShieldController shield;
    [SerializeField] float damage = 1;
    [SerializeField] int layerOffset = 4;

    Transform playerTransform;
    AIDestinationSetter destinationSetter;

    private void Awake()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = playerTransform;
        Debug.Log("Missile Fired");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitObject = collision.gameObject;
        if (hitObject != null)
        {
            if (hitObject.layer + layerOffset == gameObject.layer)
                return;
            if (shield = hitObject.GetComponent<ShieldController>())
            {
                shield.WeakenSheild(damage);
                damage = shield.ReduceDamage(damage / 2);
            }
            if (hitObject.GetComponent<HealthPool>() != null)
            {
                hitObject.GetComponent<HealthPool>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
