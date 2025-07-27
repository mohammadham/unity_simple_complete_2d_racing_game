// Player/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float boundary = 2.8f;
    public float slipCoefficient = 0.4f;

    private Rigidbody2D rb;
    private Vector2 startTouch;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update() {
        HandleSwipe();
    }

    void HandleSwipe() {
        if (Input.touchCount <= 0) return;
        Touch t = Input.GetTouch(0);
        switch (t.phase) {
            case TouchPhase.Began:
                startTouch = t.position;
                break;
            case TouchPhase.Moved:
                float dir = Mathf.Sign(t.position.x - startTouch.x);
                rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
                rb.angularVelocity = -dir * slipCoefficient * 100;
                break;
            case TouchPhase.Ended:
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                break;
        }
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -boundary, boundary);
        transform.position = pos;
    }
}
