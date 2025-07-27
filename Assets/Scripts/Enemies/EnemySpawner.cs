using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float laneWidth = 2f;
    [SerializeField] private int laneCount = 3;
    [SerializeField] private Vector2 spawnYRange = new Vector2(10f, 15f);

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        int lane = Random.Range(0, laneCount);
        float x = lane * laneWidth - (laneCount - 1) * laneWidth / 2f;
        Vector3 spawnPos = new Vector3(x, Camera.main.orthographicSize + Random.Range(spawnYRange.x, spawnYRange.y), 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
