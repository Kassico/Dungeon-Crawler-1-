using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework.Internal.Filters;
using Unity.VisualScripting;


public class Main_Menu : MonoBehaviour
{
    public GameObject playerStatsPanel;

    public bool haveStarted;
    void Start()
    {
        //playerStatsPanel.SetActive(false);

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
        //if (playerStatsPanel != null)
        //    playerStatsPanel.SetActive(true);
        Debug.Log("Play!");

        if (playerData.instance != null && !playerData.isInitialized)
        {
            playerData.instance.InitializedPlayerDefaultData();
        }
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

    //public void establishBasePlayerStats()
    //{
    //    if (playerData.instance != null && !playerData.isInitialized)
    //    {
    //        playerData.instance.InitializedPlayerDefaultData();
    //    }
    //}
    //public void resetPlayerStats()
    //{
    //    if (playerData.instance != null)
    //    {
    //        playerData.instance.InitializedPlayerDefaultData();
    //    }
    //}
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
