using UnityEngine;

public class projectile : MonoBehaviour
{
    private float dmg;
    private float knockbackForce;

    [Header("Sources")]
    public AudioSource fireBallSource;

    [Header("Clips")]

    public AudioClip fireBallClip;


    void Start() //hittar vad saker ska vara lika med , asså vad dmg ocj knockbackforcen ska vara
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
    private void OnTriggerEnter2D(Collider2D collision) //kollar om projektilen träffar spelaren eller en hitbox och gör så att spelaren tar skada och knockback eller att projektilen förstörs
    {
        PlayerHealthManager playerHealth = FindObjectOfType<PlayerHealthManager>();

        PlayerHealthManager player = collision.GetComponent<PlayerHealthManager>();
        if (player != null)
        {
            playerHealth.TakeDmg(dmg, transform.position, knockbackForce);
             Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("HitBox"))
        {
            Destroy(gameObject);
        }

    }


    public void PlayFireBallSound()  //skulle göra ljud när projektilen träffar något men den tas bort innan ljudet hinner spelas så det funkar inte, därav används inte denna funktion längre
    {
        if (fireBallClip == null) return;
        fireBallSource.PlayOneShot(fireBallClip);
    }
}
