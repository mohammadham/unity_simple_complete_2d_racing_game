// Systems/TrafficSystem.cs
using UnityEngine;
using System.Collections.Generic;

public class TrafficSystem : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform[] lanes;
    public float baseInterval = 2f;
    public AnimationCurve densityCurve; // x = score, y = spawn interval

    private Queue<GameObject> pool = new();
    private int poolSize = 20;
    private float timer;
    private int score;

    void Start() {
        for (int i = 0; i < poolSize; i++) {
            var e = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, transform);
            e.SetActive(false);
            pool.Enqueue(e);
        }
    }

    void Update() {
        score = Mathf.FloorToInt(Time.time * 10);
        timer += Time.deltaTime;
        float interval = Mathf.Max(0.5f, baseInterval - densityCurve.Evaluate(score / 1000f));
        if (timer > interval) {
            Spawn();
            timer = 0;
        }
    }

    void Spawn() {
        var e = pool.Dequeue();
        int lane = Random.Range(0, lanes.Length);
        e.transform.position = lanes[lane].position + Vector3.up * 12f;
        e.SetActive(true);
        pool.Enqueue(e);
    }
}
