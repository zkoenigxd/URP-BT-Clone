using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CargoModule", fileName = "new CargoModule")]
public class CargoUSO : UpgradeSO
{
    CargoUSO()
    { upgradeType = UpgradeType.Cargo; }

    [SerializeField] int cargoCapacity;

    public int CargoCapacity => cargoCapacity;
}
