using TMPro;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static float maxHealth = 10f;
    public static float playerHealth;
    public TextMeshProUGUI scoreText;

    public float knockbackForceResistans = 0.5f;

    void Start()
    {
        playerHealth = maxHealth;
    }
    private void Update()
    {
        scoreText.text = "Health: " + playerHealth.ToString();
    }
    public void TakeDmg(float damage)
    {
        playerHealth -= damage;
        Debug.Log($"Player takes {damage} damage. Current health: {playerHealth}");
        scoreText.text = "Health: " + playerHealth.ToString();

        TakeKnockback();


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
    private void TakeKnockback()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = -rb.linearVelocity.normalized;
            rb.AddForce(knockbackDirection * knockbackForceResistans, ForceMode2D.Impulse);
        }
    }
}
