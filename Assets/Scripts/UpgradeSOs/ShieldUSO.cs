using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ShieldModule", fileName = "new ShieldModule")]
public class ShieldUSO : UpgradeSO
{
    [SerializeField] Color storeDisplayColor = new Color(0.2577132f, 0.5274531f, 0.6886792f);
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

    public override UpgradeType GetUpgradeType() { return UpgradeType.Shield; }
    public override Color GetStoreColor() { return storeDisplayColor; }
}