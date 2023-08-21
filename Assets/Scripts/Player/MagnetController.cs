using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
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

    public void InstallComponent(MagnetUSO magnetUSO)
    {
        magnetType = magnetUSO;
        attractionDistance = magnetUSO.AttractionDistance;
        strength = magnetUSO.Strength;
        magnetCollider.GetComponent<PointEffector2D>().forceMagnitude = -strength;
        magnetCollider.radius = attractionDistance;
    }

    public void RemoveComponent()
    {
        magnetType = null;
        attractionDistance = 0;
        strength = 0;
        magnetCollider.radius = attractionDistance;
    }
}
