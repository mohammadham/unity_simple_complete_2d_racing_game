using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class CollisionHandler : MonoBehaviour {
    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall")) {
            healthSystem.TakeDamage(1);
        }
    }
}
