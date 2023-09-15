using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : Controller
{
    [SerializeField] MagnetUSO magnetType;
    [SerializeField] CircleCollider2D magnetCollider;

    float attractionDistance;
    float strength;

    public MagnetUSO MagnetType => magnetType;

    private void Awake()
    {
        Debug.Log("Magnet Activated");
        attractionDistance = magnetType.AttractionDistance;
        strength = magnetType.Strength;
        magnetCollider.GetComponent<PointEffector2D>().forceMagnitude = -strength;
        magnetCollider.radius = attractionDistance;
    }

    public override void InstallComponent(UpgradeSO upgrade)
    {
        magnetType = (MagnetUSO)upgrade;
        attractionDistance = magnetType.AttractionDistance;
        strength = magnetType.Strength;
        magnetCollider.GetComponent<PointEffector2D>().forceMagnitude = -strength;
        magnetCollider.radius = attractionDistance;
    }

    public override void RemoveComponent()
    {
        magnetType = null;
        attractionDistance = 0;
        strength = 0;
        magnetCollider.radius = attractionDistance;
    }

    public override bool IsAvailable()
    {
        return magnetType == null;
    }

    public override UpgradeSO GetUpgrade()
    { return magnetType; }

    public override UpgradeType GetSlotType()
    {
        return UpgradeType.Magnet;
    }
}
