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


   
    private void Awake() //skafat stats och annat viktigt, kontrolerar så att det finns en player och hämtar den, och sätter upp allt så att det är klart när spelet startar
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
                attackDamage = stats.attackDamage + (stats.attackDamage * difficulty) / 2; // här kan man tänla attackdamge som base attack, det kommer scala mettra med denna ekvation.
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
    void Update() // kollar saker som avståndet till spelaren, om den ska attackera eller jaga, och hanterar cooldowns och stun och sånt
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

    public void ChasePlayer() // jagar spelaren genom att räkna ut riktningen mot spelaren och sätta fiendens velocity i den riktningen, samt uppdaterar animationen så att den ser ut att gå åt rätt håll
    {
        if (isStunned) return;

        isChasing = true;

        Vector2 direction = (playerHealth.transform.position - transform.position).normalized;
        _rb.linearVelocity = direction * moveSpeed;

        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_Vertical, direction.y);
        _animator.SetBool("isWalking", true);
    }


    public void AttackPlayer() // attackerar spelaren genom att först kolla så att den inte är stunned, sedan sätter allowedToAttack till false och isAttacking till true, stoppar fiendens rörelse, uppdaterar animationen så att den ser ut att attackera åt rätt håll, och sedan startar attackanimationen
    {
        if (isStunned) return;

        allowedToAttack = false;
        isAttacking = true;
        _rb.linearVelocity = Vector2.zero;
        _animator.SetBool("isWalking", false);
        dir();
        _animator.SetBool("isAttacking", true);

    }
    
    public void dir() // kollar riktningen mot spelaren och uppdaterar animationen så att den ser ut att attackera åt rätt håll, används i AttackPlayer() för att se till att fienden alltid attackerar i riktning mot spelaren
    {
        Vector2 direction = (playerHealth.transform.position - transform.position).normalized;
        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_Vertical, direction.y);
    }


    public void PerformAttack() // gör sjävla attack, denna ropas som en animationevent i animationen, den spelar attack ljudet, räknar ut rotationen för projektilen så att den riktas mot spelaren, instansierar projektilen och ger den en impuls i riktning mot spelaren
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


    public void TakeDmg(float damage) // gör så vampien tar skada, den kollar först om den redan är död, sedan drar den av skadan från currentHealth, instansierar en damage number prefab som visar hur mycket skada den tog, uppdaterar healthbaren, räknar ut riktningen från spelaren och applicerar knockback i motsatt riktning, startar en coroutine som gör att fienden flashar röd när den tar skada, och kollar sedan om currentHealth är mindre eller lika med 0 för att se om fienden ska dö
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

    private void ApplyKnockback(Vector2 attackDir) // applicierar knockback på fienden i motsatt riktning från där attacken kom ifrån, den räknar ut riktningen från attacken
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)attackDir).normalized;
        knockbackVelocity = knockbackDirection * KnockbackForce * (1 - KnockbackForceResistans);
        knockabactimer = StunDuration;
        isStunned = true;
        Invoke(nameof(ResetStun), StunDuration);
    }
    private void ResetStun() // tar bort stun efter en viss tid, denna ropas av ApplyKnockback() efter StunDuration har gått
    {
        isStunned = false;
    }

    private System.Collections.IEnumerator FlashHitColor() // gör så att fienden flashar röd när den tar skada
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor;
    }

    public void Die() // kollar om den ska spawna portalen när den dör, gör döds ljud sedan förstörs fienden efter en sekund så animation och ljud hinner spelas
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


