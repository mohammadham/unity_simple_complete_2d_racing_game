using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour {
    [SerializeField] private GameObject[] roadSegments;
    [SerializeField] private float segmentLength = 19.20f; // Adjusted for Unity units
    private List<GameObject> activeSegments = new List<GameObject>();
    private Transform playerTransform;

    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateInitialSegments();
    }

    void Update() {
        // Check if we need to generate a new segment
        if (playerTransform.position.z > activeSegments[activeSegments.Count - 2].transform.position.z) {
            SpawnSegment();
            Destroy(activeSegments[0]);
            activeSegments.RemoveAt(0);
        }
    }

    void GenerateInitialSegments() {
        for (int i = 0; i < 3; i++) {
            SpawnSegment(i * segmentLength);
        }
    }

    void SpawnSegment(float zPosition = 0) {
        int segmentIndex = Random.Range(0, roadSegments.Length);
        Vector3 spawnPosition = new Vector3(0, 0, zPosition);
        if (activeSegments.Count > 0) {
            spawnPosition = activeSegments[activeSegments.Count - 1].transform.position + new Vector3(0, 0, segmentLength);
        }
        GameObject segment = Instantiate(
            roadSegments[segmentIndex],
            spawnPosition,
            Quaternion.identity
        );
        activeSegments.Add(segment);
    }
}
