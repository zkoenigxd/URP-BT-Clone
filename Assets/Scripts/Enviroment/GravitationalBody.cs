using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBody : MonoBehaviour
{
    //ArenaManager arenaManager;
    //public float density;
    //public float RelativeWeight;

    //void Awake()
    //{
    //    arenaManager = GameObject.FindObjectOfType<ArenaManager>();
    //}

    //void FixedUpdate()
    //{
    //    if (arenaManager.units.Count > 0)
    //    {
    //        foreach (var unit in arenaManager.units)
    //        {
    //            Vector3 offset = unit.transform.position - transform.position;
    //            float magsqr = offset.sqrMagnitude;
    //            if (magsqr > 1f)
    //            {
    //                unit.GetComponent<Rigidbody2D>().AddForce(-1 * GetComponent<Rigidbody2D>().mass * (RelativeWeight * offset.normalized / magsqr));
    //            }
    //        }
    //    }
    //}
}