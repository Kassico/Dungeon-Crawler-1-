using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public static float CurrentDifficulty = 1f; // Normal
    public TextMeshProUGUI difficultyDisplay;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //difficultyDisplay.text = "Difficulty: " + (CurrentDifficulty == 0.5f ? "Easy" : CurrentDifficulty == 1f ? "Normal" : "Hard");

        difficultyDisplay.text = "Difficulty: " + (CurrentDifficulty == 0.25f ? "Heven" : CurrentDifficulty == 0.5f ? "Easy" : CurrentDifficulty == 1f ? "Normal" : CurrentDifficulty == 1.5f ? "Hard" : "Hell");
    }
    public void Heven()
    {
        //PlayerPrefs.SetInt("Difficulty", 0);
        CurrentDifficulty = 0.25f;
    }
    public void Easy()
    {
        //PlayerPrefs.SetInt("Difficulty", 0);
        CurrentDifficulty = 0.5f;
    }
    public void Normal()
    {
        //PlayerPrefs.SetInt("Difficulty", 1);
        CurrentDifficulty = 1f;
    }
    public void Hard()
    {
        //PlayerPrefs.SetInt("Difficulty", 2);
        CurrentDifficulty = 1.5f;

    }
    public void Hell()
    {
        //PlayerPrefs.SetInt("Difficulty", 3);
        CurrentDifficulty = 2.5f;
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
