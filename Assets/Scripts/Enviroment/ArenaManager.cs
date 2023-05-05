using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] Faction faction;
    [SerializeField] float dragMultiplier;
    [SerializeField] EnemySpawner spawner;

    [SerializeField] StationController stationController;

    bool isPlayerInZone;

    public List<EnemyBrain> enemyUnits;
    public bool IsPlayerInZone => isPlayerInZone;
    public float DragMultiplier => dragMultiplier;
    public Faction ArenaFaction => faction;

    private void Awake()
    {
        isPlayerInZone = true;
        CheckAllEnemiesDefeated();
    }

    public void RemoveUnit(EnemyBrain unit)
    {
        if (unit != null)
        {
            enemyUnits.Remove(unit);
        }
    }

    public bool CheckAllEnemiesDefeated()
    {
        Debug.Log("Number of Units: " + enemyUnits.Count);
        if (enemyUnits.Count == 0)
        {
            if (stationController != null)
                stationController.ArenaIsSafe();
            else
                Debug.Log("StationController Not Found");
            return true;
        }
        if(stationController != null)
            stationController.ArenaIsNotSafe();
        return false;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>())
            isPlayerInZone = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>())
            isPlayerInZone = true;
    }

    public void ListAllEnemies()
    {
        foreach (EnemyBrain enemy in spawner.GetComponentsInChildren<EnemyBrain>())
        {
            enemyUnits.Add(enemy);
            Debug.Log("Added " + enemy.name + " to unit list");
        }
    }
}
