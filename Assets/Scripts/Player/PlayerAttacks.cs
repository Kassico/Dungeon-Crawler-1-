using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using UnityEngine;


public class PlayerAttacks : MonoBehaviour
{



    public Transform attackUpPoint;
    public Transform attackDownPoint;
    public Transform attackLeftPoint;
    public Transform attackRightPoint;
    public Transform AttackingPoint;




    public bool isAttacking;

    private Animator anim;

    private Rigidbody2D rd;
    
    public float playerDmg;
    public float attackRadius;
    private float attackCooldown = 0.4f;
    private float attackTimer = 0f;

    private bool allowedToTakeDmg;

    public SpriteRenderer sr;

    public LayerMask enemyLayers;

    private const string Attacking = "Attacking";

    private Vector2 lastMoveDir = Vector2.down;

    public GameObject AttackHitboxP;




    void Start()
    {
        playerDmg = 1;
        attackRadius = 0.5f;

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        allowedToTakeDmg = true;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = InputManager.Movement;

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if (InputManager.Attack && attackTimer <= 0)
        {
            Attack();
        }
        anim.SetBool(Attacking, isAttacking);


        //if (move.magnitude > 0.1)
        //{
        //    lastMoveDir = move.normalized;
        //}

        if (InputManager.Movement.sqrMagnitude > 0.1)
        {
            lastMoveDir = InputManager.Movement.normalized;
        }

        if (InputManager.Attack)
            Debug.Log("PlayerAttacks sees attack input");

        anim.SetBool(Attacking, isAttacking);
    }

    void Attack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        
        if (Mathf.Abs(lastMoveDir.x) > Mathf.Abs(lastMoveDir.y))
        {
            if (lastMoveDir.x > 0)
            {
                anim.SetTrigger("Attack_Right");
                AttackingPoint = attackRightPoint;
            }

            else
            {
                anim.SetTrigger("Attack_Left");
                AttackingPoint = attackLeftPoint;
            }
        }
        else
        { 
           
            if (lastMoveDir.y > 0)
            {
                anim.SetTrigger("Attack_Up");
                AttackingPoint = attackUpPoint;
            }
            else
            {
                anim.SetTrigger("Attack_Down");
                AttackingPoint = attackDownPoint;
            }
            
        }

        if (AttackHitboxP != null)
        {
            GameObject hb = Instantiate(AttackHitboxP, AttackingPoint.position, Quaternion.identity);
            hb.transform.localScale = Vector3.one * attackRadius * 2.5f; // scale matches radius
            Destroy(hb, 0.6f); // destroy after attack duration
            
        }
        Debug.Log("ATTACK FUNCTION RAN");



        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackingPoint.position, attackRadius, enemyLayers);


        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit" +  enemy.name);
            enemy.GetComponent<Enemie>().TakeDmg(playerDmg); 
        }


        Invoke(nameof(StopAttack), 0.2f);

    }

    void StopAttack()
    { isAttacking = false; }


    private void OnDrawGizmos()
    {
      
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackUpPoint.position, attackRadius);
            Gizmos.DrawWireSphere(attackDownPoint.position, attackRadius);
            Gizmos.DrawWireSphere(attackLeftPoint.position, attackRadius);  
            Gizmos.DrawWireSphere(attackRightPoint.position, attackRadius);
    }



}



