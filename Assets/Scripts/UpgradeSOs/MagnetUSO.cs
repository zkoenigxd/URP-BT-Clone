using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/MagnetModule", fileName = "new MagnetModule")]
public class MagnetUSO : UpgradeSO
{
    MagnetUSO()
    { upgradeType = UpgradeType.Magnet; }

    [SerializeField] float attractionDistance;
    [SerializeField] float strength;

    public float AttractionDistance => attractionDistance;
    public float Strength => strength;
}
