using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.SceneManager;



public class PlayerPowerUpps : MonoBehaviour
{
    [Header("Points")]

    public float playerpoints = 0;
    public float pointsToPowerup = 1;


    [Header("Player Stats")]
    private float extraPlayerHealth;
    private float playerMoveSpeed;
    private float playerDamage;
    private float playerDashCooldown;
    private float playerknockbackForce;

    [Header("UI")]
    public GameObject powerUpPanel;
    public Button[] powerUpButtons;
    public TMP_Text[] powerUpText;
    public TextMeshProUGUI scoreText;

    //// base stats
    //private float baseHealth = 10;
    //private float baseMoveSpeed = 5;
    //private float baseDamage = 1;
    //private float baseDashCooldown = 1;
    //private float baseknockbackForce = 20;

    // stuff for levle

    private float currentLevel;


    //current stats
    private float currentHealth;
    private float currentMoveSpeed;
    private float currentDamage;
    private float currentDashCooldown;
    private float currentknockbackForce;

    private List<int> usedIddices = new List<int>();

    [System.Serializable]
    public class PowerUp
    {
        public string description;
        public System.Action applayEffect;
    }

    public List<PowerUp> powerUps;


    public bool powerupActive = false;
    //private bool powerUpChosen = false;




    void Start()
    {
        powerUpPanel.SetActive(false);
        initializePlayerstats();
        initialzePowerUps();


    }


    void Update()
    {
        pointsToPowerup = 1 + (2 * currentLevel);
        
        scoreText.text = "Score: " + playerpoints.ToString() + "/" + pointsToPowerup;
        

        // kollar efter powerups

        if (playerpoints >= pointsToPowerup)
        {
            currentLevel += 1;
            //powerupActive = true;
            showPowerUps();
            playerpoints = 0;
        }

        


    }

    void initializePlayerstats()
    {


        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        playerDamage = playerAttacks.playerDmg;
        playerknockbackForce = playerAttacks.knockbackForce;

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMoveSpeed = playerMovement._moveSpeed;

        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        playerDashCooldown = playerDash.dashCooldown;
    }
    void initialzePowerUps()
    {
    
        powerUps.Add(new PowerUp
        {
            description = "Health + 5",
            applayEffect = () => extraPlayerHealth += 5
        });
        powerUps.Add(new PowerUp
        {
            description = "Damage + 2",
            applayEffect = () => playerDamage += 2
        });
        powerUps.Add(new PowerUp
        {
            description = "Speed + 0.5",
            applayEffect = () => playerMoveSpeed += 0.5f
        });
        powerUps.Add(new PowerUp
        {
            description = "Health + 10",
            applayEffect = () => extraPlayerHealth += 10
        });
        powerUps.Add(new PowerUp
        {
            description = "Damage + 1",
            applayEffect = () => playerDamage += 1
        });
        powerUps.Add(new PowerUp
        {
            description = "Speed + 0.8",
            applayEffect = () => playerMoveSpeed += 0.8f
        });
        powerUps.Add(new PowerUp
        {
            description = "Dash Cooldown - 0.1",
            applayEffect = () => playerDashCooldown -= 0.1f
        });
        powerUps.Add(new PowerUp
        {
            description = "Knockback Force + 1",
            applayEffect = () => playerknockbackForce += 5
        });
    }

void showPowerUps()
{
        Time.timeScale = 0;
        powerUpPanel.SetActive(true);
        usedIddices.Clear();


        List<int> usedIndices = new List<int>();

        

    for (int i = 0; i < powerUpButtons.Length; i++)
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, powerUps.Count);
        } while (usedIndices.Contains(randomIndex));
        usedIndices.Add(randomIndex);
        powerUpText[i].text = powerUps[randomIndex].description;

        int idexcopy = randomIndex;
        powerUpButtons[i].onClick.RemoveAllListeners();
        powerUpButtons[i].onClick.AddListener(() =>
        {
            ChosePowerUp(idexcopy);

        });
    }
}
void ChosePowerUp(int index)
{
        powerUps[index].applayEffect.Invoke();


        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        PlayerHealthManager playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        PlayerPowerUpps playerPowerUpps = FindObjectOfType<PlayerPowerUpps>();
 

        playerAttacks.playerDmg = playerDamage;
        playerHealthManager.playerHealth += extraPlayerHealth;
        playerMovement._moveSpeed = playerMoveSpeed;
        playerDash.dashCooldown = playerDashCooldown;
        playerAttacks.knockbackForce = playerknockbackForce;
        playerHealthManager.maxHealth += extraPlayerHealth;




        powerUpPanel.SetActive(false);
        extraPlayerHealth = 0;
        usedIddices.Clear();
        //powerupActive = false;
        Time.timeScale = 1;
}
}


