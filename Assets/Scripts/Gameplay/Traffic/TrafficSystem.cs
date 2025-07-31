using UnityEngine;

public class TrafficSystem : MonoBehaviour {
    [SerializeField] private GameObject[] carPrefabs;
    [SerializeField] private Transform[] lanes;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;

    private float nextSpawnTime;

    void Update() {
        if (Time.time > nextSpawnTime) {
            SpawnCar();
            // This is a placeholder for difficulty. You'd get this from GameManager
            float difficulty = 0.5f;
            nextSpawnTime = Time.time +
                Mathf.Lerp(maxSpawnInterval, minSpawnInterval, difficulty);
        }
    }

    void SpawnCar() {
        int lane = Random.Range(0, lanes.Length);
        int carType = Random.Range(0, carPrefabs.Length);
        Instantiate(carPrefabs[carType], lanes[lane].position, Quaternion.identity);
    }
}
