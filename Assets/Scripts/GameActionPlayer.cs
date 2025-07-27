using UnityEngine;

public class GameActionPlayer : MonoBehaviour
{
    [Header("Car Movement")]
    public float moveSpeed = 5f;
    public float horizontalSpeed = 10f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Move the car forward
        rb.velocity = new Vector2(rb.velocity.x, moveSpeed);

        // Move the car left and right with touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                rb.velocity = new Vector2(touch.deltaPosition.x * horizontalSpeed, rb.velocity.y);
            }
        }
    }
}
