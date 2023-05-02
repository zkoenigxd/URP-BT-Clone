using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BeamWeapon : Weapon
{
    public ParticleSystem laserEndParticles;
    public float lineLength = 10f;
    public LayerMask layerMask;

    private AudioSource audioSource;
    private LineRenderer line;
    private bool sfxIsPlaying = false;
    private bool endParticlesPlaying = false;
    float distance;

    UnitManager2 unitManager;

    [SerializeField] float charge;
    [SerializeField] float chargeTime;
    [SerializeField] GameObject weaponVisual;
    [SerializeField] public string weaponName;
    [SerializeField] float damagePerSecond;

    bool charged;

    private void Awake()
    {
        unitManager = GetComponentInParent<UnitManager2>();
        line = GetComponentInChildren<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ResolveOverheat());
        TurnOffLaser();
    }

    IEnumerator ResolveOverheat()
    {
        yield return new WaitForSeconds(chargeTime);
        charged = true;
    }

    public override void FireWeapon(Vector3 fireDirection)
    {
        throw new System.NotImplementedException();
    }

    public override void FireWeapon(RaycastHit2D hit, float distance)
    {
        if (hit.collider != null)
        {
            Debug.Log("Hit " + hit.collider.name);
            if (hit.collider.GetComponent<HealthPool>() != null)
            {
                float damage;
                if (distance > 1)
                {
                    damage = (damagePerSecond * lineLength * Time.deltaTime) / distance;
                }
                else
                    damage = (damagePerSecond * lineLength * Time.deltaTime);
                hit.collider.GetComponent<HealthPool>().TakeDamage(damage);
            }
        }
    }

    public override void OperateWeapon(Vector3 fireDirection)
    {
        if (fireDirection != Vector3.zero)
        {
            weaponVisual.transform.rotation = Quaternion.FromToRotation(Vector3.down, fireDirection);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -fireDirection, lineLength, layerMask);
            if (unitManager.CanFire && charged)
            {
                line.enabled = true;
                if (sfxIsPlaying == false)
                {
                    sfxIsPlaying = true;
                    audioSource.Play();
                }

                if (hit)
                {
                    if (endParticlesPlaying == false)
                    {
                        endParticlesPlaying = true;
                        laserEndParticles.Play(true);
                    }
                    laserEndParticles.gameObject.transform.position = hit.point;
                    distance = ((Vector2)hit.point - (Vector2)transform.position).magnitude;
                    line.SetPosition(1, new Vector3(distance, 0, 0));
                }
                else
                {
                    line.SetPosition(1, new Vector3(lineLength, 0, 0));
                    endParticlesPlaying = false;
                    laserEndParticles.Stop(true);

                }
                FireWeapon(hit, distance);
            }

            else
            {
                TurnOffLaser();
            }
        }
        else
        {
            TurnOffLaser();
        }
    }

    void TurnOffLaser()
    {
        line.SetPosition(1, new Vector3(lineLength, 0, 0));
        endParticlesPlaying = false;
        laserEndParticles.Stop(true);
        sfxIsPlaying = false;
        audioSource.Stop();
        line.enabled = false;
    }
}