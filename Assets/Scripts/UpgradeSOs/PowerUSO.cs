using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PowerModule", fileName = "new PowerModule")]
public class PowerUSO : UpgradeSO
{
    PowerUSO()
    { upgradeType = UpgradeType.Power; }

    [SerializeField] int capacity;
    [SerializeField] float rechargeRate;

    public int Capacity => capacity;
    public float RechargeRate => rechargeRate;
}
