using System;
using UnityEngine;

public class Mount : MonoBehaviour
{
    enum mountSize
    {
        Small,
        Medium,
        Large,
        Mega
    };

    [Tooltip("Set the tranform rotation of this object to represent the center of the turret's rotation limit")]
    [SerializeField] mountSize size;

    [Space(20)]
    [SerializeField] bool debug;

    void Awake()
    {

    }

    void Update()
    {
        if (debug)
        {
            DebugDraw();
        }
    }

    void DebugDraw()
    {
        Debug.DrawRay(transform.position, transform.right * 10, Color.white);
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Debug.DrawRay(transform.position, transform.right * 10, Color.white);
#endif
    }

}
