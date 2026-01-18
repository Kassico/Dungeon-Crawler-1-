using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using System.Collections;

using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class Enemy: MonoBehaviour
{

    public Transform playerTransform;
    public Transform attackpoint;
    public Transform attackHitBox;



    //Changeble Stats

    public float maxHealth = 10f;
    private float currentHealth;
    private float moveSpeed = 2f;
    private float chaseRange = 6f;
    private float attackDamage = 1f;
    private float attackRange = 1.1f;
    private float attackRate = 2f;
    private float KnockbackForceResistans = 0.5f;
    

    //Fixed Stats 
    private float StunDuration = 1f;
    private float attackCooldown = 2f;
    private float Knockbackduration = 0.1f;
    public static float KnockbackForce = 20f;
    public float AttackHitBox = 1.4f;
    public float AttackDuration = 0.5f;
    public static float pointsValue = 1;


    //Needed float
    private float nextAttackTime;
    private float AttackTimer;
    private float distanceToPlayer;


    //needed bools
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


    private PlayerHealthManager playerHealth;

    private const string _horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    public LayerMask playerLayer;

    public static string enemietype = "Enemy";
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

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

        if (isChasing && !isAttacking ) { ChasePlayer(); }

        if (distanceToPlayer <= attackRange && !isAttacking && allowedToAttack) { AttackPlayer(); }

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

            Debug.Log("Enemy Attacks Player");
            DealDmg();

        //}
    }

    public void DealDmg()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackpoint.position, AttackHitBox, playerLayer);
        Debug.Log("Enemy Dealing Damage to Player");

        foreach (Collider2D player in hitPlayers)
        {
            
            Debug.Log("Enemy Hit Player");
            if (playerHealth != null)
            {
                playerHealth.TakeDmg(attackDamage, transform.position, enemietype);
                Debug.Log("Enemy Dealt " + attackDamage + " Damage to Player");
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
        //transform.position = Vector2.MoveTowards(transform.position, -playerTransform.position, moveSpeed * 10 * Time.deltaTime);
        StartCoroutine(KnockbackCoroutine());
        if (currentHealth <= 0) {Die();}

    }

    private void Die()
    {
        //PlayerPowerUpps.playerpoints += pointsValue;


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
        _rb.AddForce(dir * (PlayerAttacks.knockbackForce * (1 - KnockbackForceResistans)), ForceMode2D.Impulse);
        Debug.Log("FORCE: " + PlayerAttacks.knockbackForce);


        yield return new WaitForSeconds(Knockbackduration);
        _rb.linearVelocity = Vector2.zero;

        allowedToMove = false;
        isStunned = true;
        yield return new WaitForSeconds(StunDuration);

        
        isStunned = false;
        allowedToMove = true;

    }
   
    private void OnDrawGizmos()
    {
        if (attackpoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, AttackHitBox);
    }


}
