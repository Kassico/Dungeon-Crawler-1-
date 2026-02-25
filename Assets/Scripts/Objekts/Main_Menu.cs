using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework.Internal.Filters;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;


public class Main_Menu : MonoBehaviour
{
    public GameObject playerStatsPanel;
    public bool alreadyStartedOnce;

    public bool haveStarted;
    void Start()
    {
        //playerStatsPanel.SetActive(false);
        alreadyStartedOnce = false;
        //StatsPanelOf();
        playerStatsPanel.active = false;

    }

    // Update is called once per frame
    void Update()
    {
        EndGame endGame = FindObjectOfType<EndGame>();
        if (endGame != null)
        {

            endGame.endGamePanel.SetActive(false);
            if (endGame.gameEnd)
            {
                haveStarted = false;
                
            }
        }
        Scene currentScene = SceneManager.GetActiveScene();
        
    }

    public void PlayGame() // starts the game and resets player stats to default values, and transfer player to the first level
    {
        if (alreadyStartedOnce)
            playerData.instance.GetPlayerDefaultData();
        Debug.Log("Play!");
        if (playerData.instance != null && !playerData.isInitialized && !alreadyStartedOnce)
        {
            playerData.instance.InitializedPlayerDefaultData();
            alreadyStartedOnce = true;
        }


        //playerStatsPanel.SetActive(true);
        //StatsPanelOn();
        playerStatsPanel.active = true;



        Transform spawnPoint = GameObject.Find("PlayerSpawnPoint")?.transform;
        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        EndGame endGame = FindObjectOfType<EndGame>();
        //endGame.gameEnd = false;
        Time.timeScale = 1f;
        PlayerPowerUpps playerPowerUpps = FindObjectOfType<PlayerPowerUpps>();
        //backToMenu.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));

        //SceneManager.LoadScene(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);

    } 
    public void StatsPanelOf()
    {
        playerStatsPanel.SetActive(false);
    }
    public void StatsPanelOn()
        
    {
        playerStatsPanel.SetActive(true);
        Debug.Log("Stats Panel True");

    }
    public void QuitGame()
    { 
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void Difficulty()
    {
        SceneManager.LoadScene(1);
    }
    
}
