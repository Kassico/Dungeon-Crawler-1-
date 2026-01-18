using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{

    public bool isused { get; private set; }
    public GameObject itemPreFab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered the portal.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(4);

        }
    }

    public void Interact()
    {
        if(CompareTag("Interacteble") && !isused)
        {
            
        }
    }

    public bool CanInteract()
    {
        throw new System.NotImplementedException();
    }

    private void openPortal()
    {
        if (itemPreFab == null)
        {
            
        }

    }
}

