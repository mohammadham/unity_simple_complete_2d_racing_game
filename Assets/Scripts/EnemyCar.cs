using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    [Header("Enemy Car Settings")]
    public float moveSpeed = 5f;

    private void Update()
    {
        // Move the car forward
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
}
