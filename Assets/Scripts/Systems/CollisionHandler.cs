using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public HealthSystem healthSystem;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Wall"))
        {
            healthSystem.TakeDamage(1);
            CheckGameOver();
        }
    }

    void CheckGameOver()
    {
        if (healthSystem.currentHealth <= 0)
        {
            GameOverManager.Instance.ShowGameOverPanel();
        }
    }
}
