using System.Collections;
using System.IO.Pipes;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    UnitManager2 unitManager;
    Rigidbody2D unitRB;
    Quaternion fireDirectionQ;

    [SerializeField] GameObject projectile;
    [SerializeField] float reloadTime;
    [SerializeField] float recoil;
    [SerializeField] GameObject weaponVisual;
    [SerializeField] public string weaponName;

    bool reloaded;

    private void Awake()
    {
        unitManager = GetComponentInParent<UnitManager2>();
        unitRB = GetComponentInParent<Rigidbody2D>();
        StartCoroutine(ReloadWeapon());
    }

    public override void OperateWeapon(Vector3 fireDirection)
    {
        if (fireDirection != Vector3.zero)
        {
            weaponVisual.transform.rotation = Quaternion.FromToRotation(Vector3.down, fireDirection);
            if (unitManager.CanFire)
            {
                FireWeapon(fireDirection);
            }
        }
    }

    public override void FireWeapon(Vector3 fireDirection)
    {
        if (reloaded)
        {
            fireDirectionQ.SetLookRotation(fireDirection, Vector3.forward);
            reloaded = false;
            GameObject projectileGO = Instantiate(projectile, transform.position, fireDirectionQ);
            projectileGO.GetComponent<ProjectileBehavior>().Fire(unitRB.velocity);
            Physics2D.IgnoreCollision(projectileGO.GetComponent<Collider2D>(), GetComponentInParent<Collider2D>());
            unitRB.AddForce(fireDirection.normalized * recoil, ForceMode2D.Impulse);
            StartCoroutine(ReloadWeapon());
        }
    }

    public override void FireWeapon(RaycastHit2D hit, float distance)
    {
        throw new System.NotImplementedException();
    }


    IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(reloadTime);
        reloaded = true;
    }
}
