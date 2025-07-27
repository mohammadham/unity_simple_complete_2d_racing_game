// Managers/RoadManager.cs
using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour {
    [Header("Segment")]
    public GameObject segmentPrefab;
    public int segmentHeight = 1920;
    public int poolSize = 5;

    [Header("Parallax")]
    public Transform[] parallaxLayers; // 3 Layer
    public float[] parallaxFactors = {0.3f, 0.6f, 1f};

    private Queue<GameObject> segmentPool = new();
    private Transform player;
    private float nextSpawnY;

    void Start() {
        player = FindObjectOfType<PlayerController>().transform;
        nextSpawnY = player.position.y + segmentHeight;
        for (int i = 0; i < poolSize; i++) {
            var seg = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, transform);
            seg.SetActive(false);
            segmentPool.Enqueue(seg);
        }
    }

    void Update() {
        if (player.position.y + segmentHeight > nextSpawnY) {
            SpawnSegment();
        }
        UpdateParallax();
    }

    void SpawnSegment() {
        var seg = segmentPool.Dequeue();
        seg.transform.position = new Vector3(0, nextSpawnY, 0);
        seg.SetActive(true);
        segmentPool.Enqueue(seg);
        nextSpawnY += segmentHeight;
    }

    void UpdateParallax() {
        float travel = player.position.y;
        for (int i = 0; i < parallaxLayers.Length; i++) {
            Vector3 p = parallaxLayers[i].position;
            p.y = -travel * parallaxFactors[i];
            parallaxLayers[i].position = p;
        }
    }
}
