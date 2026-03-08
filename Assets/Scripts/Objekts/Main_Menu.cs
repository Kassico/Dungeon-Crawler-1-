using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework.Internal.Filters;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;


public class Main_Menu : MonoBehaviour
{
    private GameObject playerStatsPanel;


    public bool haveStarted;
    void Start()
    {
        playerStatsPanel = GameObject.Find("PlayerStatsPanel");
        playerStatsPanel.SetActive(false);

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
        StatsPanelManeger statsPanelManeger = FindObjectOfType<StatsPanelManeger>(); // detta måste göras efter som att statspanel är i dontdestroyonLoad och då tappar mainmenyu typ bort den referensen, när man dör och det resetats.
        PlayerPowerUpps playerPowerUpps = FindObjectOfType<PlayerPowerUpps>();
        EndGame endGame = FindObjectOfType<EndGame>();


        Debug.Log("Play!");

        playerPowerUpps.playerpoints = 0;

        if (playerData.instance != null && playerData.isInitialized)
                playerData.instance.GetPlayerDefaultData();

        if (!playerData.isInitialized)
                playerData.instance.InitializedPlayerDefaultData();


        statsPanelManeger.ShowPanel();

        Transform spawnPoint = GameObject.Find("PlayerSpawnPoint")?.transform;
        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        Time.timeScale = 1f;

        //UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        SceneManager.LoadScene(2);

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
