using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] bool isBoss;

    void EndAnimation()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 5.0f;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
