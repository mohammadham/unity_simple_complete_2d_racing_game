using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private RoadSettings roadSettings;
    [SerializeField] private Transform roadSegmentPrefab;
    [SerializeField] private int segmentCount = 5;
    private Transform[] segments;
    private float roadLength;

    private void Start()
    {
        InitializeRoad();
    }

    private void InitializeRoad()
    {
        segments = new Transform[segmentCount];
        roadLength = roadSegmentPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        for (int i = 0; i < segmentCount; i++)
        {
            segments[i] = Instantiate(roadSegmentPrefab, transform);
            segments[i].localPosition = Vector3.up * (roadLength * i);
        }
    }

    private void Update()
    {
        float speed = roadSettings.scrollSpeed * Time.deltaTime;
        foreach (var segment in segments)
        {
            segment.localPosition -= Vector3.up * speed;
            if (segment.localPosition.y < -roadLength)
                segment.localPosition += Vector3.up * (roadLength * segmentCount);
        }
    }
}
