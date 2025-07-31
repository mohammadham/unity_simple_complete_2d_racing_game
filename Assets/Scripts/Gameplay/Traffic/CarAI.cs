using UnityEngine;

public class CarAI : MonoBehaviour {
    [SerializeField] private float minSpeed = 80f;
    [SerializeField] private float maxSpeed = 120f;
    private float speed;

    void Start() {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update() {
        // Move forward relative to the world, assuming Z is forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Simple cleanup: destroy if far behind the player
        // In a real game, this would be handled by the object pool
        if (Camera.main != null && transform.position.z < Camera.main.transform.position.z - 20f) {
            Destroy(gameObject);
        }
    }
}
