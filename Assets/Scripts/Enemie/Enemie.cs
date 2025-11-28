using NUnit.Framework.Constraints;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    public float StanderdmaxHealth = 3f;
    public float StanderdmoveSpeed = 2f;
    public float StanderdchaseRange = 5f;
    public float StanderdattackRange = 1f;
    public float StanderdattackDamage = 1f;

    
    public float distanceToPlayer = 3f;

    public bool isChasing = false;



    public Transform playerTransform;

    void Start()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        //orc stats
        float OrcHealth = StanderdmaxHealth * 2;
        float OrcmoveSpeed = StanderdmoveSpeed /4;
        float OrcchaseRange = StanderdchaseRange *2;
        float OrcattackRange = StanderdattackRange * 2;
        float OrcattackDamage = StanderdattackDamage * 2;

        // skeleton stats
        float SkeletonHealth = StanderdmaxHealth /2;
        float SkeletonmoveSpeed = StanderdmoveSpeed * 2;
        float SkeletonchaseRange = StanderdchaseRange;
        float SkeletonattackRange = StanderdattackRange;
        float SkeletonattackDamage = StanderdattackDamage;


    }


    // Update is called once per frame
    void Update()
    {
       
        if (playerTransform == null)
        {
            return;
        }

        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 directionToPlayer = (playerTransform.position - playerTransform.forward).normalized;

        if (if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange))
        {
            isChasing = true;
            EnemieMovement(OrcchaseRange, SkeletonattackRange, StanderdattackRange)


        }


        if (isChasing)
        {
            // Rotate to face the player
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }


    }

    

    public static EnemieMovement(chaseRange, attackRange, moveSpeed)
    {
       

        
        
            
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            if (attackRange >= distanceToPlayer)
            {
                //attack player saker här
            }

        
        else
        {
            isChasing = false;
        }


    }


    public void TakeDmg(float dmg)
    {
        OrcHealth -= dmg;
        if (OrcHealth < 0)
        {
            Destroy(gameObject);
        }
    }

}
