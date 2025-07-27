using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float laneDistance = 2f;
    public int currentLane = 0; // -1 = چپ، 0 = وسط، 1 = راست

    [Header("Difficulty")]
    public float speedMultiplier = 1f;

    private Vector2 touchStartPos;
    private bool isDragging = false;

    void Update()
    {
        HandleInput();
        MoveCar();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                Vector2 touchEndPos = Input.mousePosition;
                float deltaX = touchEndPos.x - touchStartPos.x;

                if (deltaX > 50f && currentLane < 1)
                    currentLane++;
                else if (deltaX < -50f && currentLane > -1)
                    currentLane--;
            }
            isDragging = false;
        }
    }

    void MoveCar()
    {
        float targetX = currentLane * laneDistance;
        Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
