using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{

    private Rigidbody2D rb;
    private TMP_Text dmgValue;


    

    public float initialYVelocity = 3.5f;
    public float initialXVelocityRange = 1.5f;
    public float lifeTime = 0.8f;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dmgValue = GetComponentInChildren<TMP_Text>();
    }

    private void Start()  //väljer vart texten ska spawna och hur den ska röra sig, samt när den ska förstöras
    {   
        rb.linearVelocity = new Vector2(Random.Range(-initialXVelocityRange, initialXVelocityRange), initialYVelocity);
        Destroy(gameObject, lifeTime);
        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        if (playerAttacks != null)
        {
            DmgText(playerAttacks.playerDmg);
        }
    }
    public void DmgText(float damageAmount) //sätter vad dmgtexten ska vara
    {
        dmgValue.text = damageAmount.ToString();
    }



}
