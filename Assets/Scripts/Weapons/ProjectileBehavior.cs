using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    GameObject hitObject;
    Rigidbody2D _rb;
    ShieldController sheild;
    [SerializeField] float projectileForce;
    [SerializeField] float damageMultiplier = 1;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitObject = collision.gameObject;
        if (hitObject != null)
        {
            damageMultiplier = (hitObject.GetComponent<Rigidbody2D>().velocity + _rb.velocity).magnitude * damageMultiplier;
            if (sheild = hitObject.GetComponent<ShieldController>())
            {
                sheild.WeakenSheild(damageMultiplier);
                damageMultiplier = sheild.ReduceDamage(damageMultiplier);
            }
            if (hitObject.GetComponent<HealthPool>() != null)
            {
                hitObject.GetComponent<HealthPool>().TakeDamage(damageMultiplier);
            }
            if(!hitObject.GetComponent<ProjectileBehavior>() && !hitObject.GetComponent<ShieldController>())
                Destroy(this.gameObject);
        }
    }

    public void Fire(Vector2 unitMotion)
    {
        _rb.velocity = unitMotion;
        _rb.AddRelativeForce(Vector2.up * projectileForce, ForceMode2D.Impulse);
        transform.rotation = Quaternion.identity;
    }
}