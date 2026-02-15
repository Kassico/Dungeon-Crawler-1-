using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    public float maxHealth;
    public float playerHealth;
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

        if (FindObjectsOfType<PlayerHealthManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Move player to spawn point in the new scene
        Transform spawnPoint = GameObject.Find("PlayerSpawnPoint")?.transform;
        if (spawnPoint != null)
            transform.position = spawnPoint.position;
    }
    void Update()
    {
        scoreText.text = playerHealth.ToString();
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
        EndGame endGame = FindObjectOfType<EndGame>();
        if (endGame != null)
        {
            endGame.gameEnd = true;
            endGame.EndTheGame();
        }
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
