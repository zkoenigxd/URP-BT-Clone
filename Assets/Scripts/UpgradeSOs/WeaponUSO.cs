using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Scriptable Objects/WeaponModule", fileName = "new WeaponModule")]
public class WeaponUSO : UpgradeSO
{
    [SerializeField] Color storeDisplayColor = new Color(0.6981132f, 0.1953838f, 0.1953838f);

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] float fireDelay;
    [SerializeField] float powerCost;
    [SerializeField] float targetConeAngle = 30;
    [SerializeField] float accuracy;
    [SerializeField] float range = 0;
    [Tooltip("Starting from bow, going counterclockwise")]
    [SerializeField] float minRotationAngle = 0;
    [SerializeField] float maxRotationAngle = 0;

    [Space(20)]
    [SerializeField] bool tracingEnable = true;
    [Tooltip("Step angle of rotation when tracing is enable")]
    [SerializeField] float maxAngularSpeed;

    public GameObject BulletPrefab => bulletPrefab;
    public float BulletSpeed => bulletSpeed;
    public float FireDelay => fireDelay;
    public float PowerCost => powerCost;
    public float TargetConeAngle => targetConeAngle;
    public bool TracingEnable => tracingEnable;
    public float MaxAngularSpeed => maxAngularSpeed;
    public float Accuracy => accuracy;
    public float Range => range;
    public float MinRotationAngle => minRotationAngle;
    public float MaxRotationAngle => maxRotationAngle;

    public override UpgradeType GetUpgradeType() { return UpgradeType.Weapon; }
    public override Color GetStoreColor() { return storeDisplayColor; }
}
