using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBehavior : MonoBehaviour
{
    GameObject hitObject;
    ShieldController shield;
    [SerializeField] float damage = 1;
    [SerializeField] int layerOffset = 4;

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
            }
            if (!hitObject.GetComponent<ArenaManager>() && !hitObject.GetComponent<PlasmaBehavior>() && !hitObject.GetComponent<ShieldController>())
            {
                Destroy(this.gameObject);
            }
        }
    }
}
