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

    // The floats for random extra stats
    private float randomHelath;
    private float randomDmg;
    private float randomSpeed;
    private bool betterStats = false;
    private bool biggerstats = false;
    private float randomNumber;
    private float extraStats;


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
        //buffStats();
        //initialzePowerUps();
        powerUps = new List<PowerUp>();



    }


    void Update()
    {
       
        
        scoreText.text = "Score: " + playerpoints.ToString() + "/" + pointsToPowerup;
        

        // kollar efter powerups

        if (playerpoints >= pointsToPowerup)
        {
            currentLevel += 1;
            //powerupActive = true;
            showPowerUps();
            playerpoints = playerpoints- pointsToPowerup;
            pointsToPowerup = 1 + (4 * currentLevel);
        }

        


    }

    void initializePlayerstats()
    {


        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        currentDamage = playerAttacks.playerDmg;
        currentknockbackForce = playerAttacks.knockbackForce;

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        currentMoveSpeed = playerMovement._moveSpeed;

        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        currentDashCooldown = playerDash.dashCooldown;
    }
    void buffStats()
    {
        randomNumber = UnityEngine.Random.Range(1, 4);
        switch (randomNumber)
        {
            case 1 : case 2: case 3: case 4: case 5:case 6:
                betterStats = false;
                biggerstats = false;
                break;
            case 7:
                betterStats = false;
                biggerstats = true;
                break;
            case 8:
                betterStats = true;
                biggerstats = true;
                break;


        }
        if (biggerstats)
        {
            randomDmg = UnityEngine.Random.Range(1,2); // nu blir det alltid 1 men att kunfa få +3 dmg är lite för OP.
            randomHelath = UnityEngine.Random.Range(5, 10); // då blir random mellan 5 - 10 extra
            randomSpeed = UnityEngine.Random.Range(2, 7)/10; // mellan 0.2 - 0.7 extra speed
        }
        else 
        { 
            randomDmg = 0;
            randomHelath = 0;
            randomNumber = 0;
            randomSpeed = 0;
        }
 


    }
    void initialzePowerUps()
    {
    
        powerUps.Add(new PowerUp
        {
            description = $"Health + {5 + randomHelath}",
            applayEffect = () => extraPlayerHealth += 5+randomHelath
        });
        powerUps.Add(new PowerUp
        {
            description = $"Damage + {1 + randomDmg}",
            applayEffect = () => playerDamage += 1+randomDmg
        });
        powerUps.Add(new PowerUp
        {
            description = $"Speed + {0.5 + randomSpeed}",
            applayEffect = () => playerMoveSpeed += 0.5f+randomSpeed
        });

        //powerUps.Add(new PowerUp
        //{
        //    description = "Health + 10",
        //    applayEffect = () => extraPlayerHealth += 10
        //});
        //powerUps.Add(new PowerUp
        //{
        //    description = "Damage + 2",
        //    applayEffect = () => playerDamage += 1
        //});
        //powerUps.Add(new PowerUp
        //{
        //    description = "Speed + 0.8",
        //    applayEffect = () => playerMoveSpeed += 0.8f
        //});
        if (betterStats)
        {
            powerUps.Add(new PowerUp
            {
                description = "Dash Cooldown - 0.15",
                applayEffect = () => playerDashCooldown -= 0.15f
            });
            powerUps.Add(new PowerUp
            {
                description = "Knockback Force + 2",
                applayEffect = () => playerknockbackForce += 2
            });
        }
        
    }

void showPowerUps()
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


            //List<int> usedIndices = new List<int>();
            //for (int i = 0; i < choices; i++)
            //{
            //    int randomIndex;
            //    do
            //    {
            //        randomIndex = Random.Range(0, usedIndices.Count);
            //    } while (usedIndices.Contains(randomIndex));
            //    usedIndices.Add(randomIndex);
            //    powerUpText[i].text = powerUps[randomIndex].description;

            //    int idexcopy = randomIndex;
            //    powerUpButtons[i].onClick.RemoveAllListeners();
            //    powerUpButtons[i].onClick.AddListener(() =>
            //    {
            //        ChosePowerUp(idexcopy);

            //    });
            //}
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
 

        playerAttacks.playerDmg += playerDamage;
        playerHealthManager.playerHealth += extraPlayerHealth;
        playerMovement._moveSpeed += playerMoveSpeed;
        playerDash.dashCooldown -= playerDashCooldown;
        playerAttacks.knockbackForce += playerknockbackForce;
        playerHealthManager.maxHealth += extraPlayerHealth;

        extraPlayerHealth = 0;
        playerMoveSpeed = 0;
        playerDamage = 0;
        playerDashCooldown = 0;
        playerknockbackForce = 0;
        playerMoveSpeed = 0;



        powerUpPanel.SetActive(false);
        extraPlayerHealth = 0;
        usedIddices.Clear();
        //powerupActive = false;
        Time.timeScale = 1;
}
}


