using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : Controller
{
    [SerializeField] WeaponUSO weaponType;
    [Tooltip("Should bullets appear one after another or all at once. Use for turret.")]
    [SerializeField] bool fireInSequence;
    [Tooltip("List of empty child GameObjects in the weapon where bullet will appear")]
    [SerializeField] Transform[] bulletStartPoses;
    [SerializeField] Transform traceTarget = null;
    

    [Space(20)]
    [SerializeField] AudioSource audioSource;

    int fireIndex = 0;
    int layer;

    float bulletSpeed;
    float accuracy;
    float range;
    float fireDelay;
    float powerCost;
    float targetConeAngle = 30;



    protected bool repeatFire = false;

    Coroutine repeatFireC;
    PowerController powerController;
    GameObject bulletPrefab;

    public WeaponUSO WeaponType => weaponType;
    

    private void Awake()
    {
        if(weaponType != null)
        {
            bulletPrefab = weaponType.BulletPrefab;
            bulletSpeed = weaponType.BulletSpeed;
            fireDelay = weaponType.FireDelay;
            powerCost = weaponType.PowerCost;
            targetConeAngle = weaponType.TargetConeAngle;
            accuracy = weaponType.Accuracy;
            range = weaponType.Range;
        }

        powerController = GetComponentInParent<PowerController>();
        if (powerController == null )
        {
            Debug.LogError("PowerController not found");
        }
        if (traceTarget == null)
        {
            if(GetComponentInParent<Player>() != null)
                traceTarget = GetComponentInParent<Player>().TargetTransform;
            else
                traceTarget = GameObject.Find("Player").transform;
        }

        if (GetComponentInParent<Player>())
        {
            layer = 10;
        }
        if (GetComponentInParent<EnemyBrain>())
        {
            layer = 11;
        }
    }

    public override void InstallComponent(UpgradeSO upgrade)
    {
        weaponType = (WeaponUSO)upgrade;
        bulletPrefab = weaponType.BulletPrefab;
        bulletSpeed = weaponType.BulletSpeed;
        fireDelay = weaponType.FireDelay;
        powerCost = weaponType.PowerCost;
        targetConeAngle = weaponType.TargetConeAngle;
        accuracy = weaponType.Accuracy;
    }

    public override void RemoveComponent()
    {
        weaponType = null;
        bulletPrefab = null;
        bulletSpeed = 0;
        fireDelay = 0;
        powerCost = 0;
        targetConeAngle = 0;
        accuracy = 0;
    }

    public override bool IsAvailable()
    {
        return weaponType == null;
    }

    public override UpgradeSO GetUpgrade()
    {
        return weaponType;
    }

    public override UpgradeType GetSlotType()
    {
        return UpgradeType.Weapon;
    }

    public void SetTarget(Transform target)
    {
        traceTarget = target;
    }

    public bool TargetInEngagementArc()
    {
        if (traceTarget == null)
            return false;
        Vector2 direct = (traceTarget.position - transform.position).normalized;
        float targetRotation = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetRotation)) < targetConeAngle / 2)
            return true;
        return false;
    }

    public bool TargetInEngagementRange()
    {
        if (traceTarget == null)
            return false;
        if (range == 0)
            return true;
        if ((transform.position - traceTarget.position).magnitude <= range)
            return true;
        return false;
    }

    protected void OneShot(int index = 0)
    {
        Vector3 scatter = new (Random.Range(-accuracy / 2.0f, accuracy / 2.0f), Random.Range(-accuracy / 2.0f, accuracy / 2.0f), 0);
        if (IfIndexGood(index) && TargetInEngagementArc() && TargetInEngagementRange())
        {
            if (powerController.ConsumePower(powerCost))
            {
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletStartPoses[index].position, Quaternion.identity);
                bullet.layer = layer;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                audioSource.Play();
                bullet.GetComponent<Collider2D>().enabled = true;
                rb.velocity = bulletSpeed * (bulletStartPoses[index].up + scatter).normalized;
                bullet.transform.rotation = transform.rotation;
            }
        }
    }

    bool IfIndexGood(int index)
    {
        if (bulletStartPoses != null && index >= 0 && index < bulletStartPoses.Length)
        {
            return true;
        }
        else
        {
            Debug.LogWarning("index is out of range in bulletStartPoses");
            return false;
        }
    }

    virtual public void StartRepeatFire()
    {
        if (!repeatFire && repeatFireC == null)
        {
            repeatFire = true;
            fireIndex = 0;
            repeatFireC = StartCoroutine(RepeatFire());
        }
    }

    virtual public void StopRepeatFire()
    {
        if (repeatFireC != null) StopCoroutine(repeatFireC);
        repeatFireC = null;
        repeatFire = false;
    }

    virtual public void MakeOneShot()
    {
        for (int index = 0; index < bulletStartPoses.Length; index++)
            OneShot(index);
    }

    private void OnDestroy()
    {
        StopCoroutine(RepeatFire());
    }

    IEnumerator RepeatFire()
    {
        bool firstShot = true;
        while (repeatFire)
        {
            if(firstShot)
            {
                yield return new WaitForSeconds(.1f);
                firstShot = false;
            }
            if (fireInSequence)
            {
                OneShot(fireIndex);
                if (++fireIndex >= bulletStartPoses.Length)
                    fireIndex = 0;
            }
            else
            {
                MakeOneShot();
            }
            yield return new WaitForSeconds(fireDelay);
        }
        repeatFireC = null;
    }
}
