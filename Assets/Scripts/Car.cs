using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            FindObjectOfType<MenuManager>().GameOver();
        }
    }

    public GameObject crashEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyCar"))
        {
            TakeDamage(25);
            FindObjectOfType<AudioManager>().PlayCarCrashSound();
            if (crashEffect != null)
            {
                Instantiate(crashEffect, transform.position, Quaternion.identity);
            }
            Destroy(collision.gameObject);
        }
    }
}
