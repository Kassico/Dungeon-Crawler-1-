using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemie : MonoBehaviour
{

    public Transform playerTransform;
    public Transform attackpoint;


    public float StanderdmaxHealth = 5f;
    public float StanderdmoveSpeed = 2f;
    public float StanderdchaseRange = 5f;
    public float StanderdattackRange = 1.2f;
    public float StanderdattackDamage = 1f;
    public float StanderdattackRate = 1f;

    private float nextAttackTime = 0f;
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
        currentHealth = StanderdmaxHealth;
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            playerTransform = Player.transform;
            playerHealth = Player.GetComponent<PlayerHealthManager>();
        }
    }
    private void Awake()
    {
        //_rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        if (isDead) { return; }

        if (playerTransform == null) {return;}

        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        isChasing = distanceToPlayer <= StanderdchaseRange;

        if (isChasing && !isAttacking ) { ChasePlayer(); }

        if (distanceToPlayer <= StanderdattackRange && !isAttacking) { AttackPlayer(); }



    }


    public void ChasePlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_Vertical, direction.y);

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, StanderdmoveSpeed * Time.deltaTime);
    }

    private void AttackPlayer() // makes the enemy attack the player once every second
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / StanderdattackRate;
            isAttacking = true;
            isChasing = false;
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            _animator.SetFloat(_horizontal, direction.x);
            _animator.SetFloat(_Vertical, direction.y);
            _animator.SetTrigger("Attack");

            PerformAttack();
            Debug.Log("Enemy Attacks Player");
        }
        
        
    }

    private void PerformAttack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackpoint.position, StanderdattackRange, playerLayer);
        playerHealth.TakeDmg(StanderdattackDamage);
    }


    public void TakeDmg(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log($"Enemy takes " + dmg + " damage. And Has {playerHealth} Health Left");

        if (currentHealth <= 0) {Die();}
    }

    private void Die()
    {
        Debug.Log("Enemy Died");
        //Destroy(gameObject);
        _animator.SetBool("isDead", true);
        isDead = true;


    }

    private void OnDrawGizmosSelected()
    {
        if (attackpoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, StanderdattackRange);
    }


}
