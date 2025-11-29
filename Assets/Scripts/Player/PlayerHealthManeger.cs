using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public float maxHealth = 10f;
    public float playerHealth;

    void Start()
    {
        playerHealth = maxHealth;
    }

    public void TakeDmg(float damage)
    {
        playerHealth -= damage;
        Debug.Log($"Player takes {damage} damage. Current health: {playerHealth}");

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Add respawn, game over, etc.
    }
}
