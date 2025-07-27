using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public HealthData healthData;
    public int currentHealth;

    void Start()
    {
        currentHealth = healthData.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Remaining Health: " + currentHealth);
    }
}
