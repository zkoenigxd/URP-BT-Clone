using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField] float damageMultiplier = .25f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthPool>() && !collision.gameObject.GetComponent<ShieldController>())
            if (collision.gameObject.GetComponent<Rigidbody2D>())
            {
                collision.gameObject.GetComponent<HealthPool>().TakeDamage(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * damageMultiplier);
            }
            else
                collision.gameObject.GetComponent<HealthPool>().TakeDamage(5);
    }
}
