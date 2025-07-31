using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float laneChangeSpeed = 5f;
    [SerializeField] private float[] lanePositions;
    private int currentLane = 1;
    private bool isChangingLanes = false;

    void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                if (touch.position.x < Screen.width / 2) {
                    MoveLeft();
                } else {
                    MoveRight();
                }
            }
        }
    }

    void MoveLeft() {
        if (currentLane > 0 && !isChangingLanes) {
            StartCoroutine(ChangeLane(currentLane - 1));
        }
    }

    void MoveRight() {
        if (currentLane < lanePositions.Length - 1 && !isChangingLanes) {
            StartCoroutine(ChangeLane(currentLane + 1));
        }
    }

    IEnumerator ChangeLane(int targetLane)
    {
        isChangingLanes = true;
        float targetX = lanePositions[targetLane];
        while (Mathf.Abs(transform.position.x - targetX) > 0.01f)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * laneChangeSpeed), transform.position.y, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        currentLane = targetLane;
        isChangingLanes = false;
    }
}
