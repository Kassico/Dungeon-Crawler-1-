using System;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.SceneManager;




public class PlayerPowerUpps : MonoBehaviour
{
    [Header("Points")]

    public float playerpoints = 0;
    public float pointsToPowerup = 1;
    public float totalPoints = 0;


    [Header("Player Stats")]
 

    [Header("UI")]
    public GameObject powerUpPanel;
    public Button[] powerUpButtons;
    public TMP_Text[] powerUpText;
    public TextMeshProUGUI scoreText;

    // The floats for random extra stats
    private float randomHelath;
    private float randomAttackForce;
    private float randomSpeed;
    private float DecreasePlayerDashCooldown;
    private float extraPlayerHealth;
    private float extraPlayerMoveSpeed;
    private float extraPlayerAttackForce;
    private float extraPlayerDashSpeed;
    private float extraPlayerAttackRadius;


    private bool betterStats = false;
    private bool biggerstats = false;
    private float randomNumber;
    private float extraStats;


    public float currentLevel;


    //current stats
    private float currentHealth;
    private float currentMoveSpeed;
    private float currentDamage;
    private float currentDashCooldown;
    private float currentknockbackForce;
    private float currentPlayerAttackRadius;

    private List<int> usedIddices = new List<int>();

    [System.Serializable]
    public class PowerUp
    {
        public string description;
        public System.Action applayEffect;
    }

    public List<PowerUp> powerUps;


    public bool powerupActive = false;




    void Start() // sätter powerup panelen till false så att den inte syns
    {
        powerUpPanel.SetActive(false);
        initializePlayerstats();
        powerUps = new List<PowerUp>();

    }


    void Update() // uppdaterar score texten och kollar om spelaren har tillräckligt med poäng för att få en powerup, om den har det så läggs poängen till total score i endgame scriptet, leveln ökar, total points ökar, spelarens audio manager spelar en ljud och powerup panelen visas
    {
        PlayerAudioManeger playerAudioManeger = GetComponent<PlayerAudioManeger>();


        scoreText.text = "Score: " + playerpoints.ToString() + "/" + pointsToPowerup;
        

        if (playerpoints >= pointsToPowerup)
        {
            EndGame endGame = FindObjectOfType<EndGame>();
            endGame.totalScore += playerpoints;
            currentLevel += 1;
            totalPoints += playerpoints;
            playerAudioManeger.PlayLevelUp();
            showPowerUps();
            playerpoints = playerpoints- pointsToPowerup;
            pointsToPowerup = 1 + (4 * currentLevel);
        }

        


    }

    void initializePlayerstats() // hämtar spelarens nuvarande stats så att powerups kan addera till dessa, detta är viktigt för att powerups ska fungera som det är tänkt, så att de adderar till spelarens nuvarande stats och inte bara sätter dem till ett nytt värde
    {


        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        currentDamage = playerAttacks.playerDmg;
        currentknockbackForce = playerAttacks.knockbackForce;

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        currentMoveSpeed = playerMovement._moveSpeed;

        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        currentDashCooldown = playerDash.dashCooldown;
    }
    void buffStats() // kollar villekn typ av powerups som ska genereras baserat på en random number mellan 1-12, där 1-6 ger inga extra stats, 7-9 ger lite extra stats och 10-12 ger bättre stats, detta är viktigt för att det ska finnas en variation i powerups som spelaren får, så att det inte alltid är samma sak och att det finns en chans att få bättre powerups
    {
        randomNumber = UnityEngine.Random.Range(1, 13);
        switch (randomNumber)
        {
            case 1 : case 2: case 3: case 4: case 5:case 6:
                betterStats = false;
                biggerstats = false;
                break;
            case 7: case 8: case 9:
                betterStats = false;
                biggerstats = true;
                break;
            case 10: case 11: case 12:
                betterStats = true;
                biggerstats = true;
                break;


        }
        if (biggerstats)
        {
            randomAttackForce = UnityEngine.Random.Range(1,3); // nu blir det alltid 1 men att kunfa f� +3 dmg �r lite f�r OP.
            randomHelath = UnityEngine.Random.Range(5, 11); // d� blir random mellan 5 - 10 extra
            randomSpeed = UnityEngine.Random.Range(2, 8)/10; // mellan 0.2 - 0.7 extra speed
        }
        else 
        {
            randomAttackForce = 0;
            randomHelath = 0;
            randomNumber = 0;
            randomSpeed = 0;
        }
 


    }
    void initialzePowerUps() // skapar powerups baserat på de random stats som genererats i buffstats funktionen, och lägger till dem i powerups listan
    {
    
        powerUps.Add(new PowerUp
        {
            description = $"Health + {5 + randomHelath}",
            applayEffect = () => extraPlayerHealth += 5+randomHelath
        });
        powerUps.Add(new PowerUp
        {
            description = $"AttackForce + {1 + randomAttackForce}",
            applayEffect = () => extraPlayerAttackForce += 1+ randomAttackForce
        });
        powerUps.Add(new PowerUp
        {
            description = $"Speed + {0.5 + randomSpeed}",
            applayEffect = () => extraPlayerMoveSpeed += 0.5f+randomSpeed
        });
        

        if (betterStats)
        {
            powerUps.Add(new PowerUp
            {
                description = "Dash Cooldown - 0.15",
                applayEffect = () => DecreasePlayerDashCooldown -= 0.15f
            });
            powerUps.Add(new PowerUp
            {
                description = "Dash Speed + 2",
                applayEffect = () => extraPlayerDashSpeed += 2
            });
            powerUps.Add(new PowerUp
            {
                description = "Attack Radius + 0.125 ",
                applayEffect = () => extraPlayerAttackRadius += 0.125f
            });
        }
        
    }

void showPowerUps() // visar powerups på knapparna i powerup panelen, och lägger till on click listeners på knapparna, med att det kan finnas mera än 3 powerups tillgänglit så är det random villka som blir valda
    {
        powerUps.Clear();

        Debug.Log(
    $"Buttons: {powerUpButtons.Length}, Texts: {powerUpText.Length}, PowerUps: {powerUps.Count}");

        Debug.Log("PowerUps count: " + powerUps.Count);
        buffStats();
        initialzePowerUps();
        Time.timeScale = 0;
        powerUpPanel.SetActive(true);
        usedIddices.Clear();

        if (powerUps.Count < powerUpButtons.Length)
        {
            Debug.LogError("Not enough power-ups to fill all buttons.");
            return;
        }
        else
        {
            int choices = Mathf.Min(powerUpButtons.Length, powerUps.Count);
            
            List <int> avilebleIndices = new List<int>();

            for (int i = 0; i < powerUps.Count; i++)
            {
                avilebleIndices.Add(i);
            }

            for (int i= 0; i < choices; i++)
            {
                int rand = UnityEngine.Random.Range(0, avilebleIndices.Count);
                int powerUpIndex = avilebleIndices[rand];
                avilebleIndices.RemoveAt(rand);

                powerUpText[i].text = powerUps[powerUpIndex].description;

                int indexCopy = powerUpIndex;
                powerUpButtons[i].onClick.RemoveAllListeners();
                powerUpButtons[i].onClick.AddListener(() =>
                {
                    ChosePowerUp(indexCopy);
                } );

            }

        }
    }
    void ChosePowerUp(int index) // när man väljen en powerupp så upptaterarden/adderar den staten till player
    {
        powerUps[index].applayEffect.Invoke();


        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        PlayerHealthManager playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        PlayerPowerUpps playerPowerUpps = FindObjectOfType<PlayerPowerUpps>();

        if (playerAttacks == null || playerMovement == null || playerDash == null || playerHealthManager == null || playerPowerUpps == null)
        {
            Debug.LogError("One or more player components not found!");
            return;
        }
        if (playerAttacks.playerDmg <= 2) // kan bara addera mera dmg om den inte �r �ver 3, det �r f�r att det inte ska bli f�r l�tt.
        { playerAttacks.playerDmg += extraPlayerAttackForce; }
        playerHealthManager.playerHealth += extraPlayerHealth;
        playerMovement._moveSpeed += extraPlayerMoveSpeed;
        playerDash.dashCooldown -= DecreasePlayerDashCooldown;
        playerAttacks.knockbackForce += extraPlayerAttackForce * 3; //  Attackforce p�verkar knockbacken ocks�, med en multiplikator p� 3

        playerHealthManager.maxHealth += extraPlayerHealth;
        playerDash.dashSpeedmultiplier += extraPlayerDashSpeed;
        playerAttacks.attackRadius += extraPlayerAttackRadius;

        extraPlayerHealth = 0;
        extraPlayerMoveSpeed = 0;
        extraPlayerAttackForce = 0;
        DecreasePlayerDashCooldown = 0;
        extraPlayerDashSpeed = 0;
        extraPlayerMoveSpeed = 0;
        extraPlayerAttackRadius = 0;

        if (playerAttacks.playerDmg >= 3) // en check så att dmg inte överstifger 3, det kan hända att om dmg är 2 och man får +2 så blir det 4 eftersom det överstiger fösta checken då dmg kan öka med mer än 1
        {
            playerAttacks.playerDmg = 3;
        }


        powerUpPanel.SetActive(false);
        extraPlayerHealth = 0;
        usedIddices.Clear();
        //powerupActive = false;
        Time.timeScale = 1;
}
}


