using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;
    [SerializeField] float timeSpawnMax, timeSpawnMin, timerSpawn;
    [SerializeField] List<Transform> spawnPosition = new List<Transform>();

    void Start()
    {
        
    }

    void Update()
    {
        if (timerSpawn > 0.0f)
            timerSpawn -= Time.deltaTime;
        else if(timerSpawn < 0.0f)
        {
            timerSpawn = Random.Range(timeSpawnMin, timeSpawnMax);
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPosition[Random.Range(0, spawnPosition.Count)].position, Quaternion.identity);
    }
}
