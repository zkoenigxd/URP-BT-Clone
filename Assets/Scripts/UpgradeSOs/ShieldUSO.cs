using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ShieldModule", fileName = "new ShieldModule")]
public class ShieldUSO : UpgradeSO
{
    ShieldUSO()
    { upgradeType = UpgradeType.Shield; }

    [SerializeField] float shieldCapacity;
    [Tooltip("Amount of power drained for each point of shield recharged.")]
    [SerializeField] float powerConsumptionRate;
    [Tooltip("Percent reduction of incoming damage.")]
    [SerializeField] float beginChargePercent;
    [SerializeField] float damageReduction;
    [SerializeField] float rechargeRate;
    [SerializeField] float rechargeDelay;
    [SerializeField] Color shieldColor;

    public float ShieldCapacity => shieldCapacity;
    public float PowerConsumptionRate => powerConsumptionRate;
    public float BeginChargePercent => beginChargePercent;
    public float DamageReduction => damageReduction;
    public float RechargeRate => rechargeRate;
    public float RechargeDelay => rechargeDelay;
    public Color ShieldColor => shieldColor;
}
