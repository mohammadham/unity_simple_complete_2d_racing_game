using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damageOnCrash = 30f;

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            TakeDamage(damageOnCrash);
            audioManager.PlayCrash();
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameManager.GameOver();
    }
}
