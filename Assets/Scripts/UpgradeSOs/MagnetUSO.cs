using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/MagnetModule", fileName = "new MagnetModule")]
public class MagnetUSO : UpgradeSO
{
    [SerializeField] Color storeDisplayColor = new Color(0.7921569f, 0.5058824f, 0.7882353f);
    [SerializeField] float attractionDistance;
    [SerializeField] float strength;

    public float AttractionDistance => attractionDistance;
    public float Strength => strength;

    public override UpgradeType GetUpgradeType() { return UpgradeType.Magnet; }
    public override Color GetStoreColor() { return storeDisplayColor; }
}
