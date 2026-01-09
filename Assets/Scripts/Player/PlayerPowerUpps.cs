using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.SceneManager;



public class PlayerPowerUpps : MonoBehaviour
{
    [Header("Points")]

    public static float playerpoints = 0;
    public  float pointsToPowerup = 1;
    public  bool powerupActive = false;
    public  bool powerUppselected;



    [Header("Player Stats")]
    private float playerHealth;
    private float playerMoveSpeed;
    private float playerDamage;
    private float playerDashCooldown;

    [Header("UI")]
    public GameObject powerUpPanel;
    public Button[] powerUpButtons;
    public TMP_Text[] powerUpText;


    string[] powerUpps = { "Health + 5", "Damage + 2", " Speed + 0.5", "Health + 10", "damage + 1", "Speed + 0.8", "dashCooldown -0.1" };

    public static Scene Currentscene;



    void Start()
    {
        powerUpPanel.SetActive(false);


    }

    void Update()
    {
        Points();
        PlayerAttacks.playerDmg += playerDamage;
        PlayerMovem._moveSpeed += playerMoveSpeed;
        PlayerDash.dashCooldown += playerDashCooldown;
        PlayerHealthManager.maxHealth += playerHealth;
        PlayerHealthManager.playerHealth += playerHealth;
    }
    public void Points()
    {
        if (playerpoints >= pointsToPowerup)
        {
            playerpoints = 0;
            showPowerUps();
        }


    }
    void showPowerUps()
    {
        Time.timeScale = 0;
        powerUpPanel.SetActive(true);

        List<int> usedIndices = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, powerUpps.Length);
            } while (usedIndices.Contains(randomIndex));
            usedIndices.Add(randomIndex);
            powerUpText[i].text = powerUpps[randomIndex];

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
        switch (index)
        {
            case 0:
                playerHealth += 5;
                break;
            case 1:
                playerDamage += 2;
                break;
            case 2:
                playerMoveSpeed += 0.5f;
                break;
            case 3:
                playerHealth += 10;
                break;
            case 4:
                playerDamage += 1;
                break;
            case 5:
                playerMoveSpeed += 0.8f;
                break;
            case 6:
                playerDashCooldown -= 0.1f;
                break;
        }
        powerUppselected = true;
        powerUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
}


