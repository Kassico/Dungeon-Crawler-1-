using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class vampire : MonoBehaviour
{
    public Transform playerTransform;

    public string enemyType = "NormalEnemy";
    public EnemyStats[] allStats;

    public GameObject damageNumberPrefab;

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
    public float rotateSpeed;
    public float projectileSpeed;

    [Header("Fixed Stats")]

    private float StunDuration = 0.2f;
    private float attackCooldown = 2f;
    public float KnockbackForce = 20f;
    public float AttackDuration = 0.5f;
    public float pointsValue = 1;
    private float knockabactimer = 0f;
    public float scaler = 1f;


    [Header("Needed Floats")]
    private float nextAttackTime;
    private float AttackTimer;
    private float distanceToPlayer;
    private float projectileRotation;

    [Header("Needed Bools")]
    public bool allowedToAttack = true;
    public bool isChasing = false;
    public bool isAttacking = false;
    private bool isDead = false;
    private bool isStunned = false;
    public bool portalActiveOnDeath = false;
    private bool isPreparingAttack = false;

    [Header("stuff")]


    private PlayerHealthManager playerHealth;

    private const string _horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private Vector2 knockbackVelocity;
    private Rigidbody2D _rb;
    private Animator _animator;
    public LayerMask playerLayer;
    public GameObject Portal;
    public GameObject projectilePrefab;

    [Header("Visuals")]

    SpriteRenderer sr;
    private Color originalColor;
    public Color hitColor = Color.red;



    [Header("Sources")]

    public AudioSource takeDmgSource;
    public AudioSource attackSource;
    public AudioSource deathSource;


    [Header("Clips")]

    public AudioClip takeDmgClip;
    public AudioClip attackClip;
    public AudioClip deathClip;


   
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealthManager>();
        }
        float difficulty = Difficulty.CurrentDifficulty;
        foreach (EnemyStats stats in allStats)
        {
            if (stats.enemyType == enemyType)
            {
                maxHealth = stats.maxHealth * (difficulty + 1);
                currentHealth = maxHealth;
                moveSpeed = stats.moveSpeed * (difficulty + 1);
                chaseRange = stats.chaseRange;
                attackDamage = stats.attackDamage * (difficulty + 1);
                attackRange = stats.attackRange;
                attackRate = stats.attackRate;
                KnockbackForceResistans = stats.knockbackForceResistans * (1 - (difficulty / 10));
                attackDeley = stats.attackDeley;
                _attackHitBox = stats.attackHitBox;
                projectileSpeed = stats.projectileSpeed;
            }
        }
        playerTransform = player.transform;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == true) { _rb.linearVelocity = Vector2.zero; return; }

        distanceToPlayer = Vector2.Distance(transform.position, playerHealth.transform.position);
        if (isAttacking)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        if (!isAttacking && !isStunned)
        {
            if (distanceToPlayer <= attackRange && allowedToAttack)
            {
                AttackPlayer();

            }
            else if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer();
            }

        }

        if (isStunned)
        {
            _rb.linearVelocity = Vector2.zero;
        }
        if (!allowedToAttack)
        {
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= attackCooldown)
            {
                allowedToAttack = true;
                AttackTimer = 0;
            }
        }

        if (knockabactimer > 0)
        {
            knockabactimer -= Time.deltaTime;
            _rb.linearVelocity = knockbackVelocity;
        }

        if (!isChasing && !isAttacking)
        {
            _animator.SetBool("isWalking", false);
        }


    }

    public void ChasePlayer()
    {
        if (isStunned) return;

        isChasing = true;

        Vector2 direction = (playerHealth.transform.position - transform.position).normalized;
        _rb.linearVelocity = direction * moveSpeed;

        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_Vertical, direction.y);
        _animator.SetBool("isWalking", true);
    }


    public void AttackPlayer()
    {
        if (isStunned) return;

        allowedToAttack = false;
        isAttacking = true;
        _rb.linearVelocity = Vector2.zero;
        _animator.SetBool("isWalking", false);
        dir();
        _animator.SetBool("isAttacking", true);

    }
    
    public void dir()
    {
        Vector2 direction = (playerHealth.transform.position - transform.position).normalized;
        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_Vertical, direction.y);
    }


    public void PerformAttack()
    {
        MakeSoundOnAttack();
        projectileRotation = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0,0, projectileRotation));
        bullet.GetComponent<Rigidbody2D>().AddForce((playerTransform.position - transform.position).normalized * projectileSpeed,ForceMode2D.Impulse);
    }
     
    public void ResetAttack()
    {
        isAttacking = false;
        _animator.SetBool("isAttacking", false);
    }


    public void TakeDmg(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        GameObject DmgObj = Instantiate(damageNumberPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        DamageNumber damagenuber = GetComponent<DamageNumber>();

        MakeSoundOnDmg();

        FlotingHealthbar floatingHealthBar = GetComponentInChildren<FlotingHealthbar>();
        if (floatingHealthBar != null)
        {
            floatingHealthBar.UppdateHealthBar(currentHealth, maxHealth);
        }

        Debug.Log($"Vampire took {damage} damage, current health: {currentHealth}");
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        ApplyKnockback(-dir);
        StartCoroutine(FlashHitColor());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ApplyKnockback(Vector2 attackDir)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)attackDir).normalized;
        knockbackVelocity = knockbackDirection * KnockbackForce * (1 - KnockbackForceResistans);
        knockabactimer = StunDuration;
        isStunned = true;
        Invoke(nameof(ResetStun), StunDuration);
    }
    private void ResetStun()
    {
        isStunned = false;
    }

    private System.Collections.IEnumerator FlashHitColor()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor;
    }

    public void Die()
    {
        Portal portal = FindObjectOfType<Portal>();

        isDead = true;
        _animator.SetTrigger("Die");
        _rb.linearVelocity = Vector2.zero;
        MakeSoundOnDeath();
        if (portalActiveOnDeath)
        {
            portal.Enable();
        }
        Destroy(gameObject, 1f);
    }



    private void MakeSoundOnDmg()
    {
        if (attackClip != null)
        {
            takeDmgSource.PlayOneShot(takeDmgClip);
        }


    }


    private void MakeSoundOnDeath()
    {

        if (deathClip != null)
        {
            deathSource.PlayOneShot(deathClip);
        }

    }

    private void MakeSoundOnAttack()
    {
        if (attackClip != null)
        {

            attackSource.PlayOneShot(attackClip);
        }

    }



}


