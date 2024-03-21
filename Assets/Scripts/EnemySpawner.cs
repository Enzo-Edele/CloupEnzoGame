using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] float timeSpawnMax, timeSpawnMin, timerSpawn;
    [SerializeField] List<Transform> spawnPosition = new List<Transform>();

    [SerializeField] List<Transform> bossSpawnPos = new List<Transform>();

    private void Start()
    {
        GameManager.Instance.spawner = this;
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
        Vector3 pos = spawnPosition[Random.Range(0, spawnPosition.Count)].position;
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }

    public void SpawnBoss()
    {
        for(int i = 0; i < bossSpawnPos.Count; i++)
        {
            Instantiate(bossPrefab, bossSpawnPos[i].position, Quaternion.identity);
        }
    }

    public void AddBossSpawn(Transform spawnPoint)
    {
        for (int i = 0; i < bossSpawnPos.Count; i++)
            if (bossSpawnPos[i] == spawnPoint)
                return;
        bossSpawnPos.Add(spawnPoint);
    }
    public void ClearBossSpawn()
    {
        for (int i = 0; i < bossSpawnPos.Count; i++)
        {
            Destroy(bossSpawnPos[i].gameObject);
        }
        bossSpawnPos.Clear();
    }
}
