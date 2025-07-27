using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Road Settings")]
    public GameObject roadPrefab;
    public float roadSpeed = 10f;

    [Header("Background Settings")]
    public SpriteRenderer background;
    public Sprite[] backgroundSprites;

    private void Start()
    {
        // Set a random background
        if (background != null && backgroundSprites.Length > 0)
        {
            background.sprite = backgroundSprites[Random.Range(0, backgroundSprites.Length)];
        }
    }

    private void Update()
    {
        // Move the road
        transform.Translate(Vector3.down * roadSpeed * Time.deltaTime);
    }
}
