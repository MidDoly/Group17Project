using UnityEngine;

// Định nghĩa giao diện "Có thể bị sát thương"
public interface IDamageable
{
    void TakeDamage(int amount);
}

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Enemy hit! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Tìm Spawner và báo cáo
        var spawner = FindObjectOfType<DungeonEnemySpawner>();
        if (spawner != null)
        {
            spawner.OnEnemyKilled();
        }
        Destroy(gameObject);
    }
}