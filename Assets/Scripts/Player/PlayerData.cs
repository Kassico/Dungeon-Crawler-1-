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
        maxHealth = PlayerHealthManager.maxHealth;
        Health = maxHealth;
        damage = PlayerAttacks.playerDmg;
        moveSpeed = PlayerMovement._moveSpeed;
        dashCooldown = PlayerDash.dashCooldown;
        knockbackForce = PlayerAttacks.knockbackForce;

        isInitialized = true;


    }
}