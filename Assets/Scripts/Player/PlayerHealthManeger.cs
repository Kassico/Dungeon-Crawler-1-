using TMPro;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static float maxHealth = 10f;
    public static float playerHealth;
    public TextMeshProUGUI scoreText;

    public float knockbackForceResistans = 0.5f;

    private float knockbackForce;

    Rigidbody2D _rb;

    void Start()
    {
        playerHealth = maxHealth;
        if (playerData.instance != null && playerData.isInitialized)
         {
            playerHealth = playerData.instance.Health;
            maxHealth = playerData.instance.maxHealth;
        }
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
     void Update()
    {
        scoreText.text = "Health: " + playerHealth.ToString();
    }
    public void TakeDmg(float damage, Vector2 enemyPos, string enemietype)
    {
        playerHealth -= damage;
        Debug.Log($"Player takes {damage} damage. Current health: {playerHealth}");
        scoreText.text = "Health: " + playerHealth.ToString();

        TakeKnockback(enemyPos, enemietype);


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
    private void TakeKnockback(Vector2 enemyPos, string enemietype)
    {
        
        Vector2 dir = (transform.position - (Vector3)enemyPos).normalized;

        switch (enemietype)
        {
            case "StandardEnemy":
                knockbackForce = 40f;
                break;
            case "BosOrc":
                knockbackForce = 50f;
                break;

        }

        _rb.AddForce(dir * (knockbackForce * (1 - knockbackForceResistans)), ForceMode2D.Impulse);
    }
}
