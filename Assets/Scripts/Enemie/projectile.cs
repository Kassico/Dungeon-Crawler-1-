using UnityEngine;

public class projectile : MonoBehaviour
{
    private float dmg;
    void Start()
    {
        //dmg = GameObject.FindGameObjectWithTag("Enemie").GetComponent<vampire>().attackDamage;
        vampire vampireScript = FindObjectOfType<vampire>();
        if (vampireScript != null)
        {
            dmg = vampireScript.attackDamage;
        }


    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealthManager playerHealth = FindObjectOfType<PlayerHealthManager>();

        PlayerHealthManager player = collision.GetComponent<PlayerHealthManager>();
        if (player != null)
        {
            //playerHealth.playerHealth -= dmg;
            playerHealth.TakeDmg(dmg, transform.position, "vampire");
             Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("HitBox"))
        {
            Destroy(gameObject);
        }

    }
}
