using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using TMPro;
using TMPro.Examples;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy: MonoBehaviour
{

    public Transform playerTransform;
    public Transform attackpoint;
    public Transform attackHitBox;
    public GameObject damageNumberPrefab;



    //[SerializeField] private string enemyStats = "NormalEnemyStats";
    public string enemyType = "NormalEnemy";
    public EnemyStats[] allStats;

    //Changeble Stats
    [Header("Enemy Stats")]

    private float maxHealth;
    public float currentHealth;
    public float moveSpeed;
    public float chaseRange;
    public float attackDamage;
    public float attackRange;
    public float attackRate;
    public float KnockbackForceResistans;
    public float health;
    public float attackDeley;
    public float _attackHitBox;





    //Fixed Stats 
    [Header("Fixed Stats")]

    private float StunDuration = 0.2f;
    private float attackCooldown = 2f;
    //private float Knockbackduration = 0.01f;
    public float KnockbackForce = 20f;
    public float AttackDuration = 0.5f;
    public float pointsValue = 1;
    private float knockbackTime = 0.02f;
    private float knockabactimer = 0f;
    public float scaler = 1f;
    public float stopDistance = 0.4f; // detta är det avstånd som fienden kommer att stanna på när den är nära spelaren, så att den inte rör sig fram och tillbaka supersnabbt när den är nära spelaren. detta gör att fienden inte går igenom spelaren
    //private float playerPoints;


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
    public bool portalActiveOnDeath = false;
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



    
    [Header("Sources")]

    public AudioSource takeDmgSource;
    public AudioSource attackSource;
    public AudioSource deathSource;

     
    [Header("Clips")]

    public AudioClip takeDmgClip;
    public AudioClip attackClip;
    public AudioClip deathClip;




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
        isDead = false;
       float difficulty = Difficulty.CurrentDifficulty;

        foreach (var stats in allStats)
        {
            if (stats.enemyType == enemyType)
            {
                maxHealth = stats.maxHealth;
                moveSpeed = stats.moveSpeed;
                attackDamage = stats.attackDamage;
                chaseRange = stats.chaseRange;
                attackRate = stats.attackRate;
                KnockbackForceResistans = stats.knockbackForceResistans;
                attackRange = stats.attackRange;
                pointsValue = stats.scoreValue;
                attackDeley = stats.attackDeley;
                _attackHitBox = stats.attackHitBox;
            }

        }


        maxHealth *= (difficulty + 1);
        moveSpeed *= (difficulty + 1);
        chaseRange *= (difficulty + 1);
        attackDamage = attackDamage + (attackDamage* difficulty)/2; // här kan man tänla attackdamge som base attack, det kommer scala mettra med denna ekvation.
        KnockbackForceResistans *= (1 - (difficulty * 0.1f));
        currentHealth = maxHealth;
      
        attackHitBox.localScale = new Vector3(_attackHitBox, _attackHitBox, scaler);


        


    }
    



    // Update is called once per frame
    void Update()
    {
        if (isDead) { _rb.linearVelocity = Vector2.zero; return; }
        health = currentHealth;

        //Debug.Log("Enemy Health: " + currentHealth);
        

        if (playerTransform == null) {return;}

        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        isChasing = distanceToPlayer <= chaseRange;
        

        if (distanceToPlayer <= stopDistance) // detta löser problemet med att fienden rörsig supersnabtt framochtillbacka i spelaren, detta gör att orcen inte går igenom spelaren men spealren kan fortfarande gå igenom orcen.
        {
            isChasing = false;
            _rb.linearVelocity = Vector2.zero;
        }



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

        Dir();
        _rb.linearVelocity = Vector2.zero;
        nextAttackTime = Time.time + 1f / attackRate;
        AttackTimer = AttackDuration;
        isAttacking = true;

        isChasing = false;
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            _animator.SetFloat(_horizontal, direction.x);
            _animator.SetFloat(_Vertical, direction.y);
            _animator.SetBool("isAttacking", true);

        MakeSoundOnAttack(); // kallar på funktionen som spelar ljudet när fienden attackerar

        //Debug.Log("Enemy Attacks Player");
        //DealDmg() Är i animation event i attack animationen, så att den kallar på den funktionen när den ska göra skada, så att den inte gör skada direkt när den börjar attackera utan när den träffar spelaren i animationen.

        //}
    }

    private IEnumerator PrepareAttack()
    {
        isPreparingAttack = true;

        _rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(attackDeley);

        AttackPlayer();
        isPreparingAttack = false;
    }

    public void DealDmg()
    {
        Debug.Log("DealDmg called");

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackpoint.position, _attackHitBox, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            
            if (playerHealth != null)
            {
                playerHealth.TakeDmg(attackDamage, transform.position, KnockbackForce);
                Debug.Log("Enemy Dealt " + attackDamage + " Damage to Player");
                break; // detta gör att fienden bara kan träffa spelaren en gång per attack, så att den inte träffar flera gånger i samma attack om spelaren är i hitboxen. Det kan hända att den räknar en av playerns andra collider som en träff, och då kan den göra att fienden träffar spelaren flera gånger i samma attack
            }
            else { Debug.LogError("playerHealth IS NULL"); }
            //Debug.Log("Enemy Dealing Damage to Player | Hits found: " + hitPlayers.Length)
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
        if (isDead) { return; }

        currentHealth -= dmg;

        GameObject DmgObj = Instantiate(damageNumberPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        DamageNumber damagenuber = GetComponent<DamageNumber>();

        FlotingHealthbar floatingHealthBar = GetComponentInChildren<FlotingHealthbar>();
        if (floatingHealthBar != null)
        {
            floatingHealthBar.UppdateHealthBar(currentHealth, maxHealth);
        }
        Debug.Log($"Enemy takes " + dmg + $" damage. And Has {currentHealth} Health Left");

        MakeSoundOnDmg(); //kallar på funktionen som spelar ljudet när fienden tar skada

        StartCoroutine(KnockbackCoroutine());

        if (sr != null)
        {
            StartCoroutine(FlashHitColor());
        }

        if (currentHealth <= 0) {Die();}
    }


    IEnumerator FlashHitColor()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        sr.color = originalColor;
    }

    private void Die()
    {
        Portal portal = FindObjectOfType<Portal>();
        if (portalActiveOnDeath) 
        {

            portal.Enable();


        }
        PlayerPowerUpps playerPowerUpps = FindObjectOfType<PlayerPowerUpps>();
        playerPowerUpps.playerpoints += pointsValue;

        
        Debug.Log("Enemy Died");
        _animator.SetBool("isDead", true);

        MakeSoundOnDeath(); // kallar på funktionen som spelar ljudet när fienden dör

        isDead = true;


    }

    IEnumerator KnockbackCoroutine()
    {
        
        allowedToAttack = false;
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        _rb.linearVelocity = dir * (KnockbackForce * (1 - KnockbackForceResistans));

        knockbackVelocity = dir * (KnockbackForce * (1 - KnockbackForceResistans));
        knockabactimer = knockbackTime;

        isStunned = true;
        yield return new WaitForSeconds(StunDuration);

        isStunned = false;

    }
   
    private void OnDrawGizmos()
    {
        if (attackpoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, _attackHitBox);
    }

    private void MakeSoundOnDmg()
    {
        if (attackClip != null)
        {
            if (enemietype.Contains("Comander"))
                takeDmgSource.pitch = 0.8f; // sänker pitch för commander fiender
            else
                takeDmgSource.pitch = 1f; // standard pitch för vanliga ORC fiender


            takeDmgSource.PlayOneShot(takeDmgClip);
        }
        

    }


    private void MakeSoundOnDeath()
    {

        if (deathClip != null)
        {
            if (enemietype.Contains("Comander"))
                deathSource.pitch = 0.8f; // sänker pitch för commander fiender
            else
                deathSource.pitch = 1f; // standard pitch för vanliga ORC fiender



            deathSource.PlayOneShot(deathClip);
        }

    }

    private void MakeSoundOnAttack()
    {
        if (attackClip != null)
        {
            if (enemietype.Contains("Comander"))
                attackSource.pitch = 0.8f; // sänker pitch för commander fiender
            else
                attackSource.pitch = 1f; // standard pitch för vanliga ORC fiender

            attackSource.PlayOneShot(attackClip);
        }

    }

}
