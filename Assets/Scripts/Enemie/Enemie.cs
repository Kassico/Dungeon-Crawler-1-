using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using System.Collections;

using Unity.Mathematics;

public class Enemie : MonoBehaviour
{

    public Transform playerTransform;
    public Transform attackpoint;
    public Transform attackHitBox;

    float difficulty = Difficulty.CurrentDifficulty;
    public static float pointsValue = 1;
    public float standardKnockbackForceResistans = 0.5f;
    public float StanderdmaxHealth = 5f / 2;
    public float StanderdmoveSpeed = 2f / 2;
    public float StanderdchaseRange = 5f / 2;
    public float StanderdattackRange = 1.2f;
    public float StanderdattackDamage = 1f;
    public float StanderdattackRate = 0.001f;
    public float StanderdAttackDuration = 0.5f;
    public float StandardAttackHitBox = 1.4f;
    private float nextAttackTime;
    private float StanderdAttackTimer;
    private float standardKnockbackduration = 0.1f;
    private float attackCooldown = 4f;
    private float standardStunDuration = 1f;
    public static float standardKnockbackForce = 20f;

    public bool allowedToAttack = true;
    //private PlayerHealthManeger playerHealth;


    public float currentHealth;
    public float distanceToPlayer;
    public bool isChasing = false;
    public bool isAttacking = false;
    private bool isDead = false;
    private bool facingRight = false;
    private bool facingLeft = false;
    private bool facingUp = false;
    private bool facingDown = false;
    private bool isStunned = false;
    private bool allowedToMove = true;


    private PlayerHealthManager playerHealth;

    private const string _horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    public LayerMask playerLayer;

    public static string enemietype = "StandardEnemy";


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            playerTransform = Player.transform;
            playerHealth = Player.GetComponent<PlayerHealthManager>();
        }



        //StanderdmaxHealth = (int)(StanderdmaxHealth *(PlayerPrefs.GetInt("Difficulty") + 1));
        //StanderdmoveSpeed = (int)(StanderdmoveSpeed * (PlayerPrefs.GetInt("Difficulty") + 1));
        //StanderdchaseRange = (int)(StanderdchaseRange * (PlayerPrefs.GetInt("Difficulty") + 1));
        //StanderdattackRange = (int)(StanderdattackRange * (PlayerPrefs.GetInt("Difficulty") + 1));
        //StanderdattackDamage = (int)(StanderdattackDamage * (PlayerPrefs.GetInt("Difficulty") + 1));
        //StanderdattackRate = (int)(StanderdattackRate *(PlayerPrefs.GetInt("Difficulty") + 1));
        
        StanderdmaxHealth = (int)(StanderdmaxHealth * (difficulty + 1));
        StanderdmoveSpeed = (int)(StanderdmoveSpeed * (difficulty + 1));
        StanderdchaseRange = (int)(StanderdchaseRange * (difficulty + 1));
        StanderdattackDamage = (int)(StanderdattackDamage * (difficulty + 1));
 


        currentHealth = StanderdmaxHealth;
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        

        //Debug.Log("Enemy Health: " + currentHealth);
        if (isDead) { return; }

        if (playerTransform == null) {return;}

        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        isChasing = distanceToPlayer <= StanderdchaseRange;

        if (isChasing && !isAttacking ) { ChasePlayer(); }

        if (distanceToPlayer <= StanderdattackRange && !isAttacking && allowedToAttack) { AttackPlayer(); }

        if (isAttacking)
        {
            StanderdAttackTimer -= Time.deltaTime;
            if (StanderdAttackTimer <= 0)
            {
                EndAttack();
            }
        }
       if (!allowedToAttack)
        {   attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                allowedToAttack = true;
                attackCooldown = 1f;
            }
        }



        //Debug.Log("Distance attackpoint → Player = " +
        //  Vector2.Distance(attackpoint.position, playerTransform.position));
    }


    public void ChasePlayer()
    {
        if (isStunned || !allowedToMove)
            return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_Vertical, direction.y);

        //transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, StanderdmoveSpeed * Time.deltaTime);
        _rb.linearVelocity = direction * StanderdmoveSpeed;
    }

    private void AttackPlayer() 
    {
        if (isStunned || !allowedToAttack)
            return;
        if (Time.time >= nextAttackTime)
        {
            Dir();
            nextAttackTime = Time.time + 1f / StanderdattackRate;
            StanderdAttackTimer = StanderdAttackDuration;
            isAttacking = true;
            isChasing = false;
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            _animator.SetFloat(_horizontal, direction.x);
            _animator.SetFloat(_Vertical, direction.y);
            _animator.SetBool("isAttacking", true);
            Debug.Log("Enemy Attacks Player");
            DealDmg();

        }
    }

    public void DealDmg()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackpoint.position, StandardAttackHitBox, playerLayer);
        Debug.Log("Enemy Dealing Damage to Player");

        foreach (Collider2D player in hitPlayers)
        {
            
            Debug.Log("Enemy Hit Player");
            if (playerHealth != null)
            {
                playerHealth.TakeDmg(StanderdattackDamage, transform.position, enemietype);
                Debug.Log("Enemy Dealt " + StanderdattackDamage + " Damage to Player");
            }
            else { Debug.LogError("playerHealth IS NULL"); }
            Debug.Log("Enemy Dealing Damage to Player | Hits found: " + hitPlayers.Length);
        }
        
    }






    public void Dir()
        {

        // det som händer här är att den kollar vilken riktning fienden borde attackera/kolla i förhållande till spelaren, och sätter bools för det.
        // om fienden är till höger om spelaren så är facingRight true, annars facingLeft true. samma sak för upp och ner.
        // den gör detta genom att jämföra positionerna för fienden och spelaren på x och y axeln. och om spelaren är både höger och under fienden kommer den attackera där man är närmast.

        if (transform.position.x < playerTransform.position.x)
        { 
        facingRight = true; facingLeft = false;
        }
        else
        {
           facingLeft = true; facingRight = false;
        }
        if (transform.position.y < playerTransform.position.y)
        {
           facingUp = true; facingDown = false;
        }
        else
        {
            facingDown = true; facingUp = false;
        }


        if (facingRight && facingDown)
        {
            if (Mathf.Abs(transform.position.x - playerTransform.position.x) > Mathf.Abs(transform.position.y - playerTransform.position.y))
            {
                _animator.SetBool("isFacingRight", true); ;
            }
            else
            {
                _animator.SetBool("isFacingUp", false); ;
            }
        }

        if (facingRight && facingUp)
        {
            if (Mathf.Abs(transform.position.x - playerTransform.position.x) > Mathf.Abs(transform.position.y - playerTransform.position.y))
            {
                _animator.SetBool("isFacingRight", true); ;
            }
            else
            {
                _animator.SetBool("isFacingUp", true); ;
            }
        }
        if (facingLeft && facingDown)
        {
            if (Mathf.Abs(transform.position.x - playerTransform.position.x) > Mathf.Abs(transform.position.y - playerTransform.position.y))
            {
                _animator.SetBool("isFacingRight", false); ;
            }
            else
            {
                _animator.SetBool("isFacingUp", false); ;
            }
        }
        if (facingLeft && facingUp)
        {
            if (Mathf.Abs(transform.position.x - playerTransform.position.x) > Mathf.Abs(transform.position.y - playerTransform.position.y))
            {
                _animator.SetBool("isFacingRight", false); ;
            }
            else
            {
                _animator.SetBool("isFacingUp", true); ;
            }
        }
        }

    

    private void EndAttack()
    {
        isAttacking = false;
        isChasing = true;
        _animator.SetBool("isAttacking", false);
        Debug.Log("Enemy Finished Attack");
        allowedToAttack = false;
    }


    public void TakeDmg(float dmg)
    {
        currentHealth -= dmg;
        //Debug.Log($"Enemy takes " + dmg + $" damage. And Has {playerHealth.playerHealth} Health Left");
        //transform.position = Vector2.MoveTowards(transform.position, -playerTransform.position, StanderdmoveSpeed * 10 * Time.deltaTime);
        StartCoroutine(KnockbackCoroutine());
        if (currentHealth <= 0) {Die();}

    }

    private void Die()
    {
        PlayerPowerUpps.playerpoints += pointsValue;
        Debug.Log("Enemy Died");
        //Destroy(gameObject);
        _animator.SetBool("isDead", true);
        isDead = true;


    }
    IEnumerator KnockbackCoroutine()
    {
        
        allowedToAttack = false;
        Vector2 dir = (transform.position - playerTransform.position).normalized;


        //_rb.velocity = Vector2.zero;
        _rb.AddForce(dir * (PlayerAttacks.knockbackForce * (1 - standardKnockbackForceResistans)), ForceMode2D.Impulse);
        Debug.Log("FORCE: " + PlayerAttacks.knockbackForce);


        yield return new WaitForSeconds(standardKnockbackduration);
        _rb.linearVelocity = Vector2.zero;

        allowedToMove = false;
        isStunned = true;
        yield return new WaitForSeconds(standardStunDuration);

        
        isStunned = false;
        allowedToMove = true;

    }
   
    private void OnDrawGizmos()
    {
        if (attackpoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, StandardAttackHitBox);
    }


}
