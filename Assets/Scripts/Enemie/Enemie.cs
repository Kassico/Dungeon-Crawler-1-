using UnityEngine;

public class Enemie : MonoBehaviour
{

    public Transform playerTransform;


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

    private PlayerHealthManager playerHealth;

    private const string _horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;

    void Start()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            playerTransform = Player.transform;
            playerHealth = Player.GetComponent<PlayerHealthManager>();
        }
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {


        if (playerTransform == null) {return;}

        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        isChasing = distanceToPlayer <= StanderdchaseRange;

        if (isChasing) { ChasePlayer(); }

        if (distanceToPlayer <= StanderdattackRange) { AttackPlayer(); }


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
            PerformAttack();
            Debug.Log("Enemy Attacks Player");
        }
        
        
    }

    private void PerformAttack()
    {
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
        Destroy(gameObject);
    }


}
