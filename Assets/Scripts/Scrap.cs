using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] bool isRare;
    [SerializeField] int dropVectorMultiplier;
    

    public bool IsRare => isRare;

    Rigidbody2D rb;

    private void Awake()
    {
        //Debug.Log("Spawned Scrap worth " + value + " at " + transform.position.ToString());
        rb = GetComponent<Rigidbody2D>();
        if (rb != null )
        {
            rb.AddForce(new Vector2 (Random.Range(1f, -1f), Random.Range(1f, -1f)).normalized * Random.Range(0, dropVectorMultiplier), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<CurrecyController>() != null)
        {
            //Debug.Log("Found Currency Controller");
            if (collision.gameObject.GetComponentInChildren<CurrecyController>().CollectScrap(this, value))
                Destroy(gameObject);
        }
        //if (collision.gameObject.GetComponentInParent<MagnetController>() != null)
        //{
        //    Debug.Log("In magnet area");
        //}
    }
}
