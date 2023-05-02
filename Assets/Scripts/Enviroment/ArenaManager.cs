using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] float dragMultiplier;

    bool isPlayerInZone;

    public List<GameObject> units = new();
    public bool IsPlayerInZone => isPlayerInZone;
    public float DragMultiplier => dragMultiplier;

    private void Awake()
    {
        isPlayerInZone = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        units.Add(player);
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            units.Add(enemy);
        }
    }

    public bool AllEnemiesDefeated()
    {

        if (units.Count == 1)
        {
            if (units[0].GetComponent<Player>() != null)
                return true;
        }
        return false;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
            isPlayerInZone = false;
        if (collider.gameObject.GetComponent<Enemy>() != null)
            collider.gameObject.GetComponent<Enemy>().LeftArena();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
            isPlayerInZone = true;
        if (collider.gameObject.GetComponent<Enemy>() != null)
            collider.gameObject.GetComponent<Enemy>().ReturnedToArena();
    }
}
