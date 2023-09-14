using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CargoModule", fileName = "new CargoModule")]
public class CargoUSO : UpgradeSO
{
    [SerializeField] Color storeDisplayColor = new Color(0.3679245f, 0.1855686f, 0.07867564f);
    [SerializeField] int cargoCapacity;

    public int CargoCapacity => cargoCapacity;

    public override UpgradeType GetUpgradeType() { return UpgradeType.Cargo; }
    public override Color GetStoreColor() { return storeDisplayColor; }
}
