using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < -10f)
            Destroy(gameObject);
    }
}
