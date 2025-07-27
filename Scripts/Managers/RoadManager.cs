using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Background Settings")]
    public Sprite backgroundSprite;
    public float scrollSpeed = 5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (backgroundSprite != null)
            spriteRenderer.sprite = backgroundSprite;
    }

    void Update()
    {
        // حرکت پس‌زمینه
        transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);
    }
}
