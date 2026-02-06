using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{   
    

    [SerializeField] private string sceneToLoad = "Level 2";
    [SerializeField] private Transform SpawnPoint;


    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        savePlayerData();
        SceneManager.LoadScene("Level 2");

        Transform spawnPoint = GameObject.Find("PlayerSpawnPoint")?.transform;
        if (spawnPoint != null)
            transform.position = spawnPoint.position;
    }


    private void savePlayerData()
    {
        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        playerData.instance.damage = playerAttacks.playerDmg;
        playerData.instance.knockbackForce = playerAttacks.knockbackForce;

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerData.instance.moveSpeed = playerMovement._moveSpeed;

        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        playerData.instance.dashCooldown = playerDash.dashCooldown;

        PlayerHealthManager playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        playerData.instance.Health = playerHealthManager.playerHealth;
        playerData.instance.maxHealth = playerHealthManager.maxHealth;

        PlayerPowerUpps playerPowerUpps = FindObjectOfType<PlayerPowerUpps>();
        playerData.instance.points = playerPowerUpps.playerpoints;




        PlayerPrefs.SetFloat("PlayerHealth", playerHealthManager.playerHealth);
        PlayerPrefs.SetFloat("PlayerMoveSpeed", playerMovement._moveSpeed);
        PlayerPrefs.SetFloat("PlayerDamage", playerAttacks.playerDmg);
        PlayerPrefs.SetFloat("PlayerKnockbackForce", playerAttacks.knockbackForce);
        PlayerPrefs.SetFloat("PlayerPoints", playerPowerUpps.playerpoints);
    }
}


