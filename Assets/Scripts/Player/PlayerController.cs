using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHorizontalDistance = 3f;
    [SerializeField] private Vector2 laneRange = new Vector2(-2f, 2f); // چپ و راست

    [Header("Difficulty")]
    [SerializeField] private Difficulty difficulty;

    private Camera mainCam;
    private Vector3 targetPosition;
    private float startingX;

    private void Start()
    {
        mainCam = Camera.main;
        startingX = transform.position.x;
        targetPosition = transform.position;
        moveSpeed *= difficulty.GetSpeedMultiplier();
    }

    private void Update()
    {
        HandleTouchInput();
        SmoothMove();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = mainCam.ScreenToWorldPoint(touch.position);
            float targetX = Mathf.Clamp(touchPos.x, laneRange.x, laneRange.y);
            targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
