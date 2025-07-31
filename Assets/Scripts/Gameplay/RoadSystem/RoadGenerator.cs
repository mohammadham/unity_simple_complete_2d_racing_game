using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {
    [SerializeField] private GameObject[] roadSegments;
    [SerializeField] private float segmentLength = 19.20f; // Adjusted for Unity units
    private List<GameObject> activeSegments = new();

    void Start() {
        GenerateInitialSegments();
    }

    void GenerateInitialSegments() {
        for (int i = 0; i < 3; i++) {
            SpawnSegment(i * segmentLength);
        }
    }

    void SpawnSegment(float zPosition) {
        GameObject segment = Instantiate(
            roadSegments[Random.Range(0, roadSegments.Length)],
            new Vector3(0, 0, zPosition),
            Quaternion.identity
        );
        activeSegments.Add(segment);
    }
}
