using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public static float CurrentDifficulty = 1f; // Normal
    public TextMeshProUGUI difficultyDisplay;


    // sätter difficulty beroände på villken knapp spelaren trycker på.

    void Update()
    {
        difficultyDisplay.text = "Difficulty: " + (CurrentDifficulty == 0.25f ? "Heven" : CurrentDifficulty == 0.5f ? "Easy" : CurrentDifficulty == 1f ? "Normal" : CurrentDifficulty == 1.5f ? "Hard" : "Hell");
    }
    public void Heven()
    {
        CurrentDifficulty = 0.25f;
    }
    public void Easy()
    {
        CurrentDifficulty = 0.5f;
    }
    public void Normal()
    {
        CurrentDifficulty = 1f;
    }
    public void Hard()
    {
        CurrentDifficulty = 1.5f;

    }
    public void Hell()
    {
        CurrentDifficulty = 2.5f;
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
