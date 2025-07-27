using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float boundary = 3f; // حد حرکت سمت چپ و راست
    private Vector2 touchStartPos;
    private bool isTouching = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isTouching = true;
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    isTouching = false;
                    break;
            }

            if (isTouching)
            {
                float direction = (touch.position - touchStartPos).x;
                float targetX = Mathf.Clamp(transform.position.x + direction * moveSpeed * Time.deltaTime, -boundary, boundary);
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            }
        }
    }
}
