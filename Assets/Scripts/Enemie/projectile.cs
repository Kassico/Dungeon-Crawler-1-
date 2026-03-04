using UnityEngine;

public class projectile : MonoBehaviour
{
    private float dmg;
    private float knockbackForce;

    [Header("Sources")]
    public AudioSource fireBallSource;

    [Header("Clips")]

    public AudioClip fireBallClip;


    void Start()
    {
        //dmg = GameObject.FindGameObjectWithTag("Enemie").GetComponent<vampire>().attackDamage;
        vampire vampireScript = FindObjectOfType<vampire>();
        if (vampireScript != null)
        {
            dmg = vampireScript.attackDamage;
            knockbackForce = vampireScript.KnockbackForce;
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
            PlayFireBallSound();// kommer ej att funka eftersom ljudet inte hinner spelas innan projektilen förstörs men ändå valt att det räker med ljudet spelaren gör
            playerHealth.TakeDmg(dmg, transform.position, knockbackForce);
             Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("HitBox"))
        {
            PlayFireBallSound();// kommer ej att funka eftersom ljudet inte hinner spelas innan projektilen förstörs men ändå valt att det räker med ljudet spelaren gör
            Destroy(gameObject);
        }

    }


    public void PlayFireBallSound() 
    {
        if (fireBallClip == null) return;
        fireBallSource.PlayOneShot(fireBallClip);
    }
}
