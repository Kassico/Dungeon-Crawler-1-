using UnityEngine;

public class playerData : MonoBehaviour
{

    public static playerData instance;

    public float Health;
    public float damage;
    public float moveSpeed;
    public float dashCooldown;
    public float knockbackForce;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
