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
    }


    private void savePlayerData()
    {
        playerData.instance.Health = PlayerHealthManager.playerHealth;
        playerData.instance.maxHealth = PlayerHealthManager.maxHealth;
        playerData.instance.moveSpeed = PlayerMovement._moveSpeed;
        playerData.instance.damage = PlayerAttacks.playerDmg;
        playerData.instance.knockbackForce = PlayerAttacks.knockbackForce;
        playerData.instance.points = PlayerPowerUpps.playerpoints;
        playerData.instance.dashCooldown = PlayerDash.dashCooldown;




        PlayerPrefs.SetFloat("PlayerHealth", PlayerHealthManager.playerHealth);
        PlayerPrefs.SetFloat("PlayerMoveSpeed", PlayerMovement._moveSpeed);
        PlayerPrefs.SetFloat("PlayerDamage", PlayerAttacks.playerDmg);
        PlayerPrefs.SetFloat("PlayerKnockbackForce", PlayerAttacks.knockbackForce);
        PlayerPrefs.SetFloat("PlayerPoints", PlayerPowerUpps.playerpoints);
    }
}


