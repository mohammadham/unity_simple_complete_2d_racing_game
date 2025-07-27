using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;
    public float spawnRate = 2f;
    public float spawnRangeX = 2f;

    private float nextSpawnTime;

    void Update()
    {
        if (!GameManager.Instance.isGameActive) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnObstacle()
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, transform.position.z);

        Instantiate(obstaclePrefabs[randomIndex], spawnPos, Quaternion.identity);
    }
}
