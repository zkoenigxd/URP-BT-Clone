using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    const float TRACE_DELAY = 0.06f;

    [SerializeField] GameObject bulletPrefab;
    [Tooltip("Set to true if muzzle has animation. E")]
    [SerializeField] bool muzzleHasAnimation;
    [Tooltip("Lest of empty child GameObjects in the weapon where bullet will appear")]
    [SerializeField] Transform[] bulletStartPoses;
    [SerializeField] float bulletSpeed;
    [Tooltip("Delay between each bullet if repeat fire mode")]
    [SerializeField] float fireDelay;
    [SerializeField] float powerCost;
    [Tooltip("Should bullets appear one after another or all at once. Use for turret.")]
    [SerializeField] bool fireInSequence;
    [Tooltip("Should bullet has the same rotation as weapon. Use for bullets with tale.")]
    [SerializeField] bool weaponRotationForBullet = false;

    [Space(20)]
    [SerializeField] bool tracingEnable = true;
    [Tooltip("Target object to trace")]
    [SerializeField] Transform traceTarget = null;
    [Tooltip("Step angle of rotation when tracing is enable")]
    [SerializeField] float deltaAngle;
    [Tooltip("Set true to emulate slow rotation with constant speed. Good for big heavy cannon.")]
    [SerializeField] bool oneStepRotation = false;
    [Tooltip("Time between each rotate by 'Delta Angle'. Works when 'One Step Rotation' is true. For low level see comments in 'BaseStart' function.")]
    [SerializeField] float deltaTimeStepRotation;

    [Space(20)]
    [SerializeField] AudioSource audioSource;

    float lastTimeRotated = 0.0f;
    protected bool repeatFire = false;
    int fireIndex = 0;
    int layer;
    Animator[] animators;
    Coroutine repeatFireC;
    PowerController powerController;

    protected void BaseStart()
    {
        if (tracingEnable)
            StartCoroutine(TraceTarget());

        if (muzzleHasAnimation)
        {
            animators = new Animator[bulletStartPoses.Length];
            int index = 0;
            foreach (Transform oneStartPos in bulletStartPoses)
                animators[index++] = oneStartPos.parent.GetComponent<Animator>();
        }
    }

    private void Awake()
    {
        BaseStart();
        powerController = GetComponentInParent<PowerController>();
        if (GetComponentInParent<Player>())
            layer = 10;
        if (GetComponentInParent<EnemyBrain>())
            layer = 11;
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
    
    protected void OneShot(int index = 0)
    {
        bool i = IfIndexGood(index);
        if (i && powerController == null)
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletStartPoses[index].position, Quaternion.identity);
            bullet.layer = layer;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            audioSource.Play();
            bullet.GetComponent<Collider2D>().enabled = true;
            rb.velocity = bulletSpeed * (bulletStartPoses[index].up);
            if (weaponRotationForBullet)
                bullet.transform.rotation = transform.rotation;
            if (muzzleHasAnimation)
                animators[index].SetTrigger("fire");
        }
        else if (i && powerController.ConsumePower(powerCost))
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletStartPoses[index].position, Quaternion.identity);
            bullet.layer = layer;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            audioSource.Play();
            bullet.GetComponent<Collider2D>().enabled = true;
            rb.velocity = bulletSpeed * (bulletStartPoses[index].up);
            if (weaponRotationForBullet)
                bullet.transform.rotation = transform.rotation;
            if (muzzleHasAnimation)
                animators[index].SetTrigger("fire");
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
