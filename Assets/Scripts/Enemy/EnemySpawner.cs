using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs;

    [SerializeField] List<Transform> spawnLocations;

    [SerializeField] ArenaManager arenaManager;

    int wave = 0;

    private void Awake()
    {
        StartWave();
    }

    public void StartWave()
    {
        wave++;
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            GameObject childObject = Instantiate(enemyPrefabs[Random.Range(0, (wave < enemyPrefabs.Count) ? wave : enemyPrefabs.Count)], // Random enemy
                        spawnLocations[Random.Range(0, spawnLocations.Count)].position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), //Random position from randon waypoint
                        transform.rotation
                ) as GameObject;
            childObject.transform.parent = this.transform;
        }
        arenaManager.ListAllEnemies();
        arenaManager.CheckAllEnemiesDefeated();
    }
}
