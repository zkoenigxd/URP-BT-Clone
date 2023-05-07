using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    const float TRACE_DELAY = 0.06f;

    [SerializeField] WeaponUSO weaponType;
    [Tooltip("Should bullets appear one after another or all at once. Use for turret.")]
    [SerializeField] bool fireInSequence;
    [Tooltip("Lest of empty child GameObjects in the weapon where bullet will appear")]
    [SerializeField] Transform[] bulletStartPoses;
    [SerializeField] Transform traceTarget = null;

    [Space(20)]
    [SerializeField] AudioSource audioSource;

    int fireIndex = 0;
    int layer;

    float bulletSpeed;
    float fireDelay;
    float powerCost;
    float targetConeAngle = 30;
    float deltaTimeStepRotation;
    float deltaAngle;
    float lastTimeRotated = 0.0f;

    bool tracingEnable = true;
    bool oneStepRotation = false;
    protected bool repeatFire = false;

    Coroutine repeatFireC;
    PowerController powerController;
    GameObject bulletPrefab;

    public WeaponUSO WeaponType => weaponType;

    protected void BaseStart()
    {
        if (tracingEnable)
            StartCoroutine(TraceTarget());
    }

    private void Awake()
    {
        if(weaponType != null)
        {
            bulletPrefab = weaponType.BulletPrefab;
            bulletSpeed = weaponType.BulletSpeed;
            fireDelay = weaponType.FireDelay;
            powerCost = weaponType.PowerCost;
            targetConeAngle = weaponType.TargetConeAngle;
            tracingEnable = weaponType.TracingEnable;
            oneStepRotation = weaponType.OneStepRotation;
            deltaTimeStepRotation = weaponType.DeltaTimeStepRotation;
            deltaAngle = weaponType.DeltaAngle;
        }

        powerController = GetComponentInParent<PowerController>();
        if (traceTarget == null)
        {
            traceTarget = FindAnyObjectByType<Player>().transform;
            Debug.Log("Target assigned as: " + traceTarget.gameObject.name);
        }

        if (GetComponentInParent<Player>())
            layer = 10;
        if (GetComponentInParent<EnemyBrain>())
            layer = 11;
        BaseStart();
    }

    public void InstallComponent(WeaponUSO weaponUSO)
    {
        weaponType = weaponUSO;
        bulletPrefab = weaponType.BulletPrefab;
        bulletSpeed = weaponType.BulletSpeed;
        fireDelay = weaponType.FireDelay;
        powerCost = weaponType.PowerCost;
        targetConeAngle = weaponType.TargetConeAngle;
        tracingEnable = weaponType.TracingEnable;
        oneStepRotation = weaponType.OneStepRotation;
        deltaTimeStepRotation = weaponType.DeltaTimeStepRotation;
        deltaAngle = weaponType.DeltaAngle;
    }

    public void RemoveComponent()
    {
        weaponType = null;
        bulletPrefab = null;
        bulletSpeed = 0;
        fireDelay = 0;
        powerCost = 0;
        targetConeAngle = 0;
        tracingEnable = false;
        oneStepRotation = false;
        deltaTimeStepRotation = 0;
        deltaAngle = 0;
    }

    public void SetTarget(Transform target)
    {
        traceTarget = target;
    }

    IEnumerator TraceTarget()
    {
        while(tracingEnable && traceTarget != null)
        {
            if (oneStepRotation)
            {
                if (Time.time - lastTimeRotated > deltaTimeStepRotation)
                {
                    if (Tools.TraceTarget(transform, traceTarget.position, deltaAngle, true))
                        lastTimeRotated = Time.time;
                }
            }
            else
                Tools.TraceTarget(transform, traceTarget.position, deltaAngle);
            yield return new WaitForSeconds(TRACE_DELAY);
        }
    }

    public bool TargetInSight()
    {
        if (traceTarget == null)
            return false;
        Vector2 direct = transform.position - traceTarget.position;
        float angle = Vector3.Angle(Vector3.up, direct);
        if ((traceTarget.position.x - transform.TransformPoint(Vector3.zero).x) < 0)
        {
            angle = 360.0f - angle;
        }
        if (transform.rotation.eulerAngles.z > angle - targetConeAngle / 2 && transform.rotation.eulerAngles.z < angle + targetConeAngle / 2)
            return true;
        return false;
    }
    
    protected void OneShot(int index = 0)
    {
        bool i = IfIndexGood(index);
        if (i && powerController == null)
        {
            if (TargetInSight())
            {
                //Debug.Log(TargetInSight().ToString());
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletStartPoses[index].position, Quaternion.identity);
                bullet.layer = layer;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                audioSource.Play();
                bullet.GetComponent<Collider2D>().enabled = true;
                rb.velocity = bulletSpeed * (bulletStartPoses[index].up);
                bullet.transform.rotation = transform.rotation;
            }
        }
        else if (i && TargetInSight())
        {
            if (powerController.ConsumePower(powerCost))
            {
                //Debug.Log(TargetInSight().ToString());
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletStartPoses[index].position, Quaternion.identity);
                bullet.layer = layer;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                audioSource.Play();
                bullet.GetComponent<Collider2D>().enabled = true;
                rb.velocity = bulletSpeed * (bulletStartPoses[index].up);
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
