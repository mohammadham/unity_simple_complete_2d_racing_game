using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float laneChangeSpeed = 5f;
    [SerializeField] private float[] lanePositions = new float[]{-2.5f, 0f, 2.5f}; // Example positions
    private int currentLane = 1;
    private bool isChangingLanes = false;
    private Vector3 targetPosition;

    void Start() {
        transform.position = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
        targetPosition = transform.position;
    }

    void Update() {
        HandleInput();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f) {
            isChangingLanes = false;
        }
    }

    private void HandleInput() {
        if (isChangingLanes) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || GetTouchInput() == -1) {
            MoveLeft();
        } else if (Input.GetKeyDown(KeyCode.RightArrow) || GetTouchInput() == 1) {
            MoveRight();
        }
    }

    private int GetTouchInput() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                if (touch.position.x < Screen.width / 2) {
                    return -1; // Left
                } else {
                    return 1; // Right
                }
            }
        }
        return 0; // No touch
    }

    void MoveLeft() {
        if (currentLane > 0) {
            currentLane--;
            StartLaneChange();
        }
    }

    void MoveRight() {
        if (currentLane < lanePositions.Length - 1) {
            currentLane++;
            StartLaneChange();
        }
    }

    void StartLaneChange() {
        isChangingLanes = true;
        targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
    }
}
