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

    void Update() // kollar om det finns ett endgame script i scenen och om det finns så döljer den endgame panelen
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

    public void PlayGame() // startar spelet, resetar spelarens poäng och hämtar default data för spelaren, visar statspanelen och flyttar spelaren till spawn pointen. Och laddar in scenen med index 2 som är spelet.
    {
        ResetStuff();
        Transform spawnPoint = GameObject.Find("PlayerSpawnPoint")?.transform;
        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        Time.timeScale = 1f;

        SceneManager.LoadScene(2);

    } 



    public void QuitGame() // stänger av spelet om det inte körs i unity efter som att det inte går att stänga ned genom denna kåd
    { 
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void Difficulty() // laddar in så man kan ändra difficulty, detta är en separat scene
    {
        SceneManager.LoadScene(1);
    }

    private void ResetStuff() //resetar saker så att spelet ska kunna restartas, bland annat level och score.
    {
        StatsPanelManeger statsPanelManeger = FindObjectOfType<StatsPanelManeger>(); // detta m�ste g�ras efter som att statspanel �r i dontdestroyonLoad och d� tappar mainmenyu typ bort den referensen, n�r man d�r och det resetats. å ä ö försvan för någon anledning
        PlayerPowerUpps playerPowerUpps = FindObjectOfType<PlayerPowerUpps>();
        EndGame endGame = FindObjectOfType<EndGame>();

        statsPanelManeger.ShowPanel();


        playerPowerUpps.playerpoints = 0;
        playerPowerUpps.pointsToPowerup = 1;
        playerPowerUpps.currentLevel = 0;
        playerPowerUpps.totalPoints = 0;

        if (playerData.instance != null && playerData.isInitialized)
            playerData.instance.GetPlayerDefaultData();

        if (!playerData.isInitialized)
            playerData.instance.InitializedPlayerDefaultData();



    }
}
