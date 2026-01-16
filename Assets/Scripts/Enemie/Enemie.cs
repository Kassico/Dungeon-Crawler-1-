using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using System.Collections;

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
    private float standardKnockbackduration = 10f;
    private float attackCooldown = 1f;

    public bool allowedToAttack = true;
    //private PlayerHealthManeger playerHealth;


    public float currentHealth;
    public float distanceToPlayer;
    public bool isChasing = false;
    public bool isAttacking = false;
    private bool isDead = false;


    private PlayerHealthManager playerHealth;

    private const string _horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    public LayerMask playerLayer;


    void Start()
    {
        
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
        //_rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {

        Debug.Log("Enemy Health: " + currentHealth);
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
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_Vertical, direction.y);

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, StanderdmoveSpeed * Time.deltaTime);
    }

    private void AttackPlayer() // makes the enemy attack the player once every second and makes the attack take 0.5 seconds
    {
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
                playerHealth.TakeDmg(StanderdattackDamage);
                Debug.Log("Enemy Dealt " + StanderdattackDamage + " Damage to Player");
            }
            else { Debug.LogError("playerHealth IS NULL"); }
        }
        Debug.Log("Enemy Dealing Damage to Player | Hits found: " + hitPlayers.Length);
    }


    //public void DealDmg()
    //{
    //    Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(
    //        attackpoint.position,
    //        StandardAttackHitBox,
    //        playerLayer
    //    );

    //    Debug.Log("Enemy Dealing Damage to Player | Hits found: " + hitPlayers.Length);

    //    foreach (Collider2D player in hitPlayers)
    //    {
    //        Debug.Log("Enemy Hit Player: " + player.name);

    //        if (playerHealth != null)
    //        {
    //            playerHealth.TakeDmg(StanderdattackDamage);
    //            Debug.Log("Enemy Dealt " + StanderdattackDamage + " Damage");
    //        }
    //        else
    //        {
    //            Debug.LogError("playerHealth IS NULL");
    //        }
    //    }
    //}





    public void Dir()
        {
       
        if (transform.position.x < playerTransform.position.x )
        {             _animator.SetBool("isFacingRight", true);         }
        else
        {
            _animator.SetBool("isFacingRight", false);
        }
        if (transform.position.y < playerTransform.position.y)
        {
            _animator.SetBool("isFacingUp", true);
        }
        else
        {
            _animator.SetBool("isFacingUp", false);
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
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        //_rb.AddForce(new Vector2(direction.x * PlayerAttacks.knockbackForce * standardKnockbackForceResistans, direction.y * PlayerAttacks.knockbackForce* standardKnockbackForceResistans), ForceMode2D.Impulse);
        transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(direction.x, direction.y), PlayerAttacks.knockbackForce * standardKnockbackForceResistans);
        yield return new WaitForSeconds(standardKnockbackduration);
    }
    //IEnumerator knockback(float knockbackDuration, float knockbackPower, Transform obj)
    //{
    //    float timer = 0;
    //    while (knockbackDuration > timer)
    //    {
    //        timer += Time.deltaTime;
    //        Vector2 direction = (obj.position - playerTransform.position).normalized;
    //        _rb.AddForce(new Vector2(direction.x * knockbackPower, direction.y * knockbackPower));
    //    }
    //    yield return 0;
    //}
    private void OnDrawGizmos()
    {
        if (attackpoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, StandardAttackHitBox);
    }


}
