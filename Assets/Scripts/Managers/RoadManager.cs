using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Background Settings")]
    public Sprite backgroundSprite;
    public float scrollSpeed = 0.5f;

    [Header("Enemy Car Settings")]
    public GameObject enemyCarPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyCarPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }

    void Update()
    {
        MoveBackground();
    }

    void MoveBackground()
    {
        // This requires a material with a texture that can be offset.
        // The default sprite material doesn't support this.
        // A custom shader or a different method (e.g., moving two sprites) would be needed.
        // For now, we'll assume a material that supports tiling is being used.
        if (GetComponent<SpriteRenderer>()?.material.mainTexture != null)
        {
            GetComponent<SpriteRenderer>().material.mainTextureOffset += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }
}
