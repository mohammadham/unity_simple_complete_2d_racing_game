using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyCarPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyCarPrefab, spawnPoints[index].position, Quaternion.identity);
    }
}
