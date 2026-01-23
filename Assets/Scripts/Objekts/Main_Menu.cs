using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework.Internal.Filters;


public class Main_Menu : MonoBehaviour
{


    public bool haveStarted;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        if (playerData.instance != null && !playerData.isInitialized)
        {
            playerData.instance.InitializedPlayerDefaultData();
        }
       
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
