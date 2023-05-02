using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    [SerializeField] GameObject smallDropPrefab;
    [SerializeField] GameObject largeDropPrefab;
    [SerializeField] int bonusMaterial;
    [Tooltip("Percent of HealthPool that can dropped as additional currency when this object is damaged.")]
    [SerializeField] float percentDropOnDamage = .5f;
    float sustainedDamage;
    int materialTotal;

    public void Awake()
    {
        if((materialTotal = GetComponent<HealthPool>().MaxHealth) != 0)
            materialTotal = (int)(materialTotal * percentDropOnDamage);
    }

    public void DropDamageMaterial(float damageTaken)
    {
        sustainedDamage += damageTaken * percentDropOnDamage;
        while (sustainedDamage >= 5 && materialTotal >= 5)
        {
            Instantiate(largeDropPrefab, transform.position, transform.rotation);
            sustainedDamage -= 5;
            materialTotal -= 5;
        }
        while (sustainedDamage >= 1 && materialTotal > 0)
        {
            Instantiate(smallDropPrefab, transform.position, transform.rotation);
            sustainedDamage -= 1;
            materialTotal -= 1;
        }
    }

    public void DropDefeatMaterial()
    {
        while (bonusMaterial >= 5)
        {
            Instantiate(largeDropPrefab, transform.position, transform.rotation);
            bonusMaterial -= 5;
        }
        while (bonusMaterial >= 1)
        {
            Instantiate(smallDropPrefab, transform.position, transform.rotation);
            bonusMaterial -= 1;
        }
    }
}
