using UnityEngine;

public class TrafficSystem : MonoBehaviour {
    [SerializeField] private GameObject[] carPrefabs;
    [SerializeField] private Transform[] lanes;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private float spawnDistance = 100f;

    private float nextSpawnTime;
    private Transform playerTransform;

    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        if (Time.time > nextSpawnTime) {
            SpawnCar();
            // Difficulty should be a value between 0 and 1
            float difficulty = Mathf.Clamp01(GameManager.Instance.Difficulty);
            nextSpawnTime = Time.time + Mathf.Lerp(maxSpawnInterval, minSpawnInterval, difficulty);
        }
    }

    void SpawnCar() {
        if (playerTransform == null) return;

        int laneIndex = Random.Range(0, lanes.Length);
        int carTypeIndex = Random.Range(0, carPrefabs.Length);

        Vector3 spawnPosition = new Vector3(
            lanes[laneIndex].position.x,
            lanes[laneIndex].position.y,
            playerTransform.position.z + spawnDistance
        );

        // Could use the object pool here
        Instantiate(carPrefabs[carTypeIndex], spawnPosition, Quaternion.identity);
    }
}
