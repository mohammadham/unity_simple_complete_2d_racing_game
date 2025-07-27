using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour {
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    public UnityEvent OnDie;

    private void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            currentHealth = 0;
            Die();
        }
    }

    private void Die() {
        OnDie?.Invoke();
        // In a real game, you might want to disable the player controller, play an animation, etc.
        // For now, we'll just trigger the event.
        GameManager.Instance.TriggerGameOver();
    }
}
