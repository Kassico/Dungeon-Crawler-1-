using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    public static float CurrentDifficulty = 1f; // Normal

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        CurrentDifficulty = 2f;

    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
