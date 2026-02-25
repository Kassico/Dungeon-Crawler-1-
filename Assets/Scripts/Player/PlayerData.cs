using UnityEngine;

public class playerData : MonoBehaviour
{

    public static playerData instance;

    public static bool isInitialized = false;

    public float Health;
    public float damage;
    public float moveSpeed;
    public float dashCooldown;
    public float knockbackForce;
    public float points;
    public float maxHealth;

    


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

    public void InitializedPlayerDefaultData()
    {
        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();    
        damage = playerAttacks.playerDmg;
        knockbackForce = playerAttacks.knockbackForce;

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        moveSpeed = playerMovement._moveSpeed;

        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        dashCooldown = playerDash.dashCooldown;

        PlayerHealthManager playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        maxHealth = playerHealthManager.maxHealth;
        Health = maxHealth;
      

        isInitialized = true;
    }

    public void GetPlayerDefaultData()
    {   
        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        playerAttacks.playerDmg = damage;
        playerAttacks.knockbackForce = knockbackForce;
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement._moveSpeed = moveSpeed;
        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        playerDash.dashCooldown = dashCooldown;
        PlayerHealthManager playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        playerHealthManager.maxHealth = maxHealth;
        playerHealthManager.playerHealth = Health;
    }
}