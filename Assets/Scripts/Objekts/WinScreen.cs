using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private int lastScore;
    private string completedDifficulty;
    private int levelReached;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI difficultyText;

    void Start() // tar reda på vissa saker som vad för difficulty som var klarad oh vad för score/level spelaren klarade.
    {
        PlayerPowerUpps playerPowerUpps = FindFirstObjectByType<PlayerPowerUpps>();
        StatsPanelManeger statsPanelManeger = FindFirstObjectByType<StatsPanelManeger>();
        PlayerPowerUpps playerPowerUppsInstance = FindObjectOfType<PlayerPowerUpps>();

        statsPanelManeger.HidePanel();
        playerPowerUpps.powerupActive = false;

        lastScore = (int)playerPowerUpps.totalPoints;
        levelReached = (int)playerPowerUpps.currentLevel;

        completedDifficulty = Difficulty.CurrentDifficulty == 0.25f ? "Heaven" :
                             Difficulty.CurrentDifficulty == 0.5f ? "Easy" :
                             Difficulty.CurrentDifficulty == 1f ? "Normal" :
                             Difficulty.CurrentDifficulty == 1.5f ? "Hard" : "Hell";

        DisplayStats();

    }


    void DisplayStats() // visar upp spelarens score, level och vilken difficulty som klarades.
    {
     

        scoreText.text = "Your totoal score is: " + lastScore +", and your level is " + levelReached;
        difficultyText.text = "Difficulty: " + completedDifficulty;
    }

    public void BackToMenu() // tar än tillback til mainmenu8 när man trycker på en knapp som e kopplad till denna funktion
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
