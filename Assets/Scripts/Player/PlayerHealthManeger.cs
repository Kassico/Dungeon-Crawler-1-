using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    public float maxHealth;
    public float playerHealth;
    public TextMeshProUGUI scoreText;
    public GameObject playerStatsPanel;

    public float knockbackForceResistans = 0.5f;

    private float knockbackForce;

    Rigidbody2D _rb;




    void Awake() // hämtar rigidbody komponenten och ser till att det bara finns en PlayerHealthManager i scenen,  och sertial at spelaren inte dubbliceras och blir dontdestroyed on load så att spelaren klarar scene bytet
    {
        _rb = GetComponent<Rigidbody2D>();

        if (FindObjectsOfType<PlayerHealthManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start() // hämtar max health från player data om det finns anars anar tar den från inspektorn
    {
        if (playerData.instance != null && playerData.isInitialized)
         {
            playerHealth = playerData.instance.Health;
            maxHealth = playerData.instance.maxHealth;
        }
        else
        {
            playerHealth = maxHealth;
        }
        
    }
    public void UpdateHealthUI() // uppdaterar health texten i UI
    {   
        scoreText.text = "Health: " + playerHealth.ToString();
    }

 
    private void OnEnable() // håller kol på villken scene det är
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()// håller kol på villken scene det är
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // körs varje scne byte och kollar om spelaren ska vara synlig eller inte beroende på villken scene det är, spelare ska inte vara synlig vid menu scener.
    {

        bool inGameScene = scene.buildIndex >= 2 && scene.buildIndex != 6;
        SetPlayerVisible(inGameScene);

        Transform spawnPoint = GameObject.Find("PlayerSpawnPoint")?.transform;

        if (spawnPoint != null)
            transform.position = spawnPoint.position;

    }

    private void SetPlayerVisible(bool visible) // blir kallad av onsceneLoaded  och sätter på eller av alla komponenter för spelaren, 
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = visible;
        }

        PlayerAttacks playerAttacks = GetComponent<PlayerAttacks>();
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement != null) playerMovement.enabled = visible;
        if (playerAttacks != null) playerAttacks.enabled = visible;
        if (_rb != null) _rb.simulated = visible;
    }
    void Update()
    {
        scoreText.text = playerHealth.ToString();
    }
    public void TakeDmg(float damage, Vector2 enemyPos, float enemyKnockbackForce) // tar skada och knockback från fiender, och kollar om spelaren dör eller inte, och uppdaterar UI
    {
        PlayerAudioManeger playerAudioManeger = GetComponent<PlayerAudioManeger>();

        playerHealth -= damage;
        playerAudioManeger.PlayTakeDamage();
        Debug.Log($"Player takes {damage} damage. Current health: {playerHealth}");
        scoreText.text = "Health: " + playerHealth.ToString();

        TakeKnockback(enemyPos, enemyKnockbackForce);


        if (playerHealth <= 0)
        {
            Die();
        }

    }


    private void Die() // kör logiken för när spelaren e död
    {
        EndGame endGame = FindObjectOfType<EndGame>();
        Main_Menu mainMenu = FindObjectOfType<Main_Menu>();
        PlayerAudioManeger playerAudioManeger = GetComponent<PlayerAudioManeger>();

        playerAudioManeger.PlayDeath();
        if (endGame != null)
        {
            endGame.gameEnd = true;
            endGame.EndTheGame();
        }
    }

    private void TakeKnockback(Vector2 enemyPos, float enemyKnockbackForce) // tar knockback.
    {
        
        Vector2 dir = (transform.position - (Vector3)enemyPos).normalized;


        _rb.AddForce(dir * (enemyKnockbackForce * (1 - knockbackForceResistans)), ForceMode2D.Impulse);
    }
}
