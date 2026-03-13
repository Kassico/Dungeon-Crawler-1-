using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;


public class EndGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject endGamePanel;
    public TMPro.TextMeshProUGUI scoreText;
    public Button backToMenu;
    public Button Quit; // med att man inte kan application.quit i editor så har jag inte gjort att den finns, just för att det inte ska finnas en knapp här som inte gör någont
    public TMPro.TextMeshProUGUI endGameText;
    


    public float totalScore;

    public bool gameEnd = false;
    void Start()
    {
        endGamePanel.SetActive(false);

    }


    public void EndTheGame()
    
    {
        StatsPanelManeger statsPanelManeger = FindObjectOfType<StatsPanelManeger>();


    
        if (gameEnd)
            Time.timeScale = 0f; //pousar spelet

            Debug.Log("Game Over! Total Score: " + totalScore);

            if (statsPanelManeger != null)
            {
                statsPanelManeger.HidePanel();
            }
            endGamePanel.SetActive(true);
            scoreText.text = "Total Score: " + totalScore.ToString();
            backToMenu.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
            Button quitButton = backToMenu.GetComponent<Button>();
            quitButton.onClick.AddListener(() => Application.Quit());

    }

}

