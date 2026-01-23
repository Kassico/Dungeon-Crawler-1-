using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using System.Collections;

using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Constraints;
using System;

public class Enemy: MonoBehaviour
{

    public Transform playerTransform;
    public Transform attackpoint;
    public Transform attackHitBox;



    //[SerializeField] private string enemyStats = "NormalEnemyStats";
    public string enemyType = "NormalEnemy";
    public EnemyStats[] allStats;

    //Changeble Stats
    [Header("Enemy Stats")]

    public float maxHealth;
    public static float currentHealth;
    public float moveSpeed;
    public float chaseRange;
    public float attackDamage;
    public float attackRange;
    public float attackRate;
    public float KnockbackForceResistans;


    //Fixed Stats 
    [Header("Fixed Stats")]

    private float StunDuration = 0.2f;
    private float attackCooldown = 2f;
    private float Knockbackduration = 0.01f;
    public static float KnockbackForce = 20f;
    public float AttackHitBox = 1.4f;
    public float AttackDuration = 0.5f;
    public static float pointsValue = 1;
    private float knockbackTime = 0.02f;
    private float knockabactimer = 0f;
    private float attackDeley = 0.5f;


    //Needed float
    [Header("Needed Floats")]
    private float nextAttackTime;
    private float AttackTimer;
    private float distanceToPlayer;


    //needed bools
    [Header("Needed Bools")]
    public bool allowedToAttack = true;
    public bool isChasing = false;
    public bool isAttacking = false;
    private bool isDead = false;
    private bool facingRight = false;
    private bool facingLeft = false;
    private bool facingUp = false;
    private bool facingDown = false;
    private bool isStunned = false;
    private bool allowedToMove = true;
    public bool portalActiveOnDeath = true;
    private bool isPreparingAttack = false;

    [Header("stuff")]


    private PlayerHealthManager playerHealth;

    private const string _horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private Vector2 knockbackVelocity;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    public LayerMask playerLayer;
    public GameObject Portal;

    [Header("Visuals")]

    SpriteRenderer sr;
    private Color originalColor;
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.1f;



    public static string enemietype = "Enemy";
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        var Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            playerTransform = Player.transform;
            playerHealth = Player.GetComponent<PlayerHealthManager>();
        }
    }

    void Start()
    {

       float difficulty = Difficulty.CurrentDifficulty;

        foreach (var stats in allStats)
        {
            if (stats.enemyType == enemyType)
            {
                maxHealth = stats.normalMaxHealth;
                moveSpeed = stats.normalMoveSpeed;
                attackDamage = stats.normalAttackDamage;
                chaseRange = stats.normalChaseRange;
                attackRate = stats.normalAttackRate;
                KnockbackForceResistans = stats.normalKnockbackForceResistans;
                attackRange = stats.normalAttackRange;
            }

        }



        maxHealth *= (difficulty + 1);
        moveSpeed *= (difficulty + 1);
        chaseRange *= (difficulty + 1);
        attackDamage *= (difficulty + 1);
        KnockbackForceResistans *= (1 - (difficulty * 0.1f));
        currentHealth = maxHealth;
      
 



    }
    



    // Update is called once per frame
    void Update()
    {
        

        //Debug.Log("Enemy Health: " + currentHealth);
        if (isDead) { return; }

        if (playerTransform == null) {return;}

        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        isChasing = distanceToPlayer <= chaseRange;

        //if (isChasing && !isAttacking ) { ChasePlayer(); }

        if (distanceToPlayer <= attackRange && !isAttacking && allowedToAttack && !isPreparingAttack) { StartCoroutine(PrepareAttack()); }

        if (isAttacking)
        {
            AttackTimer -= Time.deltaTime;
            if (AttackTimer <= 0)
            {
                EndAttack();
            }
        }
        if (!allowedToAttack)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                allowedToAttack = true;
                attackCooldown = 1f;
            }
        }

        if (knockabactimer > 0)
        {
            knockabactimer -= Time.deltaTime;
            _rb.linearVelocity = knockbackVelocity;
        }
        else
        {
            if(isChasing && !isAttacking)
            {
                ChasePlayer();
            }
        }

    }


    public void ChasePlayer()
    {
        if (isStunned || !allowedToMove)
            return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_Vertical, direction.y);

        _rb.linearVelocity = direction * moveSpeed;
    }

    private void AttackPlayer() 
    {
        if (isStunned || !allowedToAttack)
            return;
        //if (Time.time >= nextAttackTime)
        //{
            Dir();
            nextAttackTime = Time.time + 1f / attackRate;
            AttackTimer = AttackDuration;
            isAttacking = true;

            isChasing = false;
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            _animator.SetFloat(_horizontal, direction.x);
            _animator.SetFloat(_Vertical, direction.y);
            _animator.SetBool("isAttacking", true);

            //Debug.Log("Enemy Attacks Player");
            DealDmg();

        //}
    }

    private IEnumerator PrepareAttack()
    {
        isPreparingAttack = true;
        _rb.linearVelocity = Vector2.zero;
        //Debug.Log("Enemy is Preparing to Attack");
        yield return new WaitForSeconds(attackDeley);
        AttackPlayer();
        isPreparingAttack = false;
    }

    public void DealDmg()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackpoint.position, AttackHitBox, playerLayer);
        //Debug.Log("Enemy Dealing Damage to Player");

        foreach (Collider2D player in hitPlayers)
        {
            
            //Debug.Log("Enemy Hit Player");
            if (playerHealth != null)
            {
                playerHealth.TakeDmg(attackDamage, transform.position, enemietype);
                Debug.Log("Enemy Dealt " + attackDamage + " Damage to Player");
            }
            else { Debug.LogError("playerHealth IS NULL"); }
            //Debug.Log("Enemy Dealing Damage to Player | Hits found: " + hitPlayers.Length);
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
        //Debug.Log("Enemy Finished Attack");
        allowedToAttack = false;
    }


    public void TakeDmg(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log($"Enemy takes " + dmg + $" damage. And Has {currentHealth} Health Left");
        //transform.position = Vector2.MoveTowards(transform.position, -playerTransform.position, moveSpeed * 10 * Time.deltaTime);
        StartCoroutine(KnockbackCoroutine());

        if (sr != null)
        {
            StartCoroutine(FlashHitColor());
        }

        if (currentHealth <= 0) {Die();}
        if (currentHealth < 0) { currentHealth = 0; Die(); }

    }
    IEnumerator FlashHitColor()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        sr.color = originalColor;
    }

    private void Die()
    {
        //PlayerPowerUpps.playerpoints += pointsValue;
        if (portalActiveOnDeath)
        {
            Portal.SetActive(true);
        }
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
        //_rb.AddForce(dir * (PlayerAttacks.knockbackForce * (1 - KnockbackForceResistans)), ForceMode2D.Impulse);
        _rb.linearVelocity = dir * (KnockbackForce * (1 - KnockbackForceResistans));
        //Debug.Log("FORCE: " + PlayerAttacks.knockbackForce); 

        knockbackVelocity = dir * (KnockbackForce * (1 - KnockbackForceResistans));
        knockabactimer = knockbackTime;


        //yield return new WaitForSeconds(Knockbackduration);
        //_rb.linearVelocity = Vector2.zero;

        //allowedToMove = false;
        isStunned = true;
        yield return new WaitForSeconds(StunDuration);


        isStunned = false;
        //allowedToMove = true;

    }
   
    private void OnDrawGizmos()
    {
        if (attackpoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, AttackHitBox);
    }


}
