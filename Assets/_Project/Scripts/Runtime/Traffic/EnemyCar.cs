using UnityEngine;

public class EnemyCar : MonoBehaviour {
    public float speed = 5f;
    public float despawnY = -15f;

    private void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < despawnY) {
            gameObject.SetActive(false);
        }
    }
}
