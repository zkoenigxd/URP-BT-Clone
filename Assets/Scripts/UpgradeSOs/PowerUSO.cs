using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PowerModule", fileName = "new PowerModule")]
public class PowerUSO : UpgradeSO
{
    [SerializeField] Color storeDisplayColor = new Color(1, 0.8044406f, 0);
    [SerializeField] int capacity;
    [SerializeField] float rechargeRate;

    public int Capacity => capacity;
    public float RechargeRate => rechargeRate;

    public override UpgradeType GetUpgradeType() { return UpgradeType.Power; }
    public override Color GetStoreColor() { return storeDisplayColor; }
}
