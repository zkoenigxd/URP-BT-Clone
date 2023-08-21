using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/WeaponModule", fileName = "new WeaponModule")]
public class WeaponUSO : UpgradeSO
{
    WeaponUSO()
    { upgradeType = UpgradeType.Weapon; }

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] float fireDelay;
    [SerializeField] float powerCost;
    [SerializeField] float targetConeAngle = 30;
    [SerializeField] float accuracy;

    [Space(20)]
    [SerializeField] bool tracingEnable = true;
    [Tooltip("Step angle of rotation when tracing is enable")]
    [SerializeField] float deltaAngle;
    [Tooltip("Set true to emulate slow rotation with constant speed. Good for big heavy cannon.")]
    [SerializeField] bool oneStepRotation = false;
    [Tooltip("Time between each rotate by 'Delta Angle'. Works when 'One Step Rotation' is true. For low level see comments in 'BaseStart' function.")]
    [SerializeField] float deltaTimeStepRotation;

    public GameObject BulletPrefab => bulletPrefab;
    public float BulletSpeed => bulletSpeed;
    public float FireDelay => fireDelay;
    public float PowerCost => powerCost;
    public float TargetConeAngle => targetConeAngle;
    public bool TracingEnable => tracingEnable;
    public float DeltaAngle => deltaAngle;
    public bool OneStepRotation => oneStepRotation;
    public float DeltaTimeStepRotation => deltaTimeStepRotation;
    public float Accuracy => accuracy;

}
