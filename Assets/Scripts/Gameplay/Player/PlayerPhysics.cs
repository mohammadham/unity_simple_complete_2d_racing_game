using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysics : MonoBehaviour {
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Top-down perspective
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy")) {
            // Handle collision with traffic or obstacles
            GameManager.Instance.LoadSceneAsync("GameOver"); // Example
            HapticFeedback.Vibrate(100);
            // FindObjectOfType<CameraEffects>().Shake();
        }
    }

    public void ApplySlip(float slipCoefficient) {
        // This would modify the car's handling based on the road surface
        // For example, by reducing the effectiveness of lane changes
    }
}
