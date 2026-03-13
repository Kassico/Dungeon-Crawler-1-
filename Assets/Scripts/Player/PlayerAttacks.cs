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
    public float attackDuration = 0.5f;  // hur länge hitboxen ska vara aktiv
    public float knockbackForce = 15;

    private float attackTimer = 0f;
    private float attackCooldown = 0.4f;


    public SpriteRenderer sr;

    public LayerMask enemyLayers;

    private const string Attacking = "Attacking";

    private Vector2 lastMoveDir = Vector2.down;

    public GameObject AttackHitboxP;
    public GameObject activeHitBox;





    void Start() // samlar saker som stats  och komponenter, och sätter default värden, detta är viktigt för att spelet ska fungera, så det inte blir null referenses eller att spelaren inte gör någon skada när den attackerar
    {
        playerDmg = 1;
        attackRadius = 0.5f;

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //allowedToTakeDmg = true;

        if (playerData.instance != null && playerData.isInitialized)
        {            
            knockbackForce = playerData.instance.knockbackForce;
            playerDmg = playerData.instance.damage;
        }

    }

    // Update is called once per frame
    void Update() // kollar efter input och gör så att spelaren attackerar
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

        if (InputManager.Movement.sqrMagnitude > 0.1)
        {
            lastMoveDir = InputManager.Movement.normalized;
        }

        if (InputManager.Attack)
            //Debug.Log("PlayerAttacks sees attack input");

      if (activeHitBox != null && isAttacking)
        {
            activeHitBox.SetActive(true);
            activeHitBox.transform.position = AttackingPoint.position;
        }

        anim.SetBool(Attacking, isAttacking);
    }

    void Attack() // gör själva attacken, den kollar vilken riktning spelaren rör sig i och spelar rätt attack animation, den kollar också efter fiender i närheten av attackpunkten och gör skada på dem, den skapar också en hitbox som är aktiv under attackens duration, detta är viktigt för att spelet ska fungera som det ska, så att spelaren kan skada fiender och att attacken ser bra ut
    {
        PlayerAudioManeger playerAudioManeger = GetComponent<PlayerAudioManeger>();

        isAttacking = true;
        attackTimer = attackCooldown;
        playerAudioManeger.PlayAttack();


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
        
        //Debug.Log("ATTACK FUNCTION RAN");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackingPoint.position, attackRadius, enemyLayers);


        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out Enemy enemyHealth))
            {
                enemyHealth.TakeDmg(playerDmg);
            }
            else if (enemy.TryGetComponent(out vampire vampireHealth))
            {
                vampireHealth.TakeDmg(playerDmg);
            }
            if (enemy != null)
                    playerAudioManeger.PlayHit();

        }
        
        if (AttackHitboxP != null && activeHitBox == null)
                activeHitBox = Instantiate(AttackHitboxP, AttackingPoint.position, Quaternion.identity);

        
        if (activeHitBox != null)
                activeHitBox.SetActive(true); activeHitBox.transform.position = AttackingPoint.position;

        

        Invoke(nameof(StopAttack), attackDuration);

    }


    void StopAttack() // stopar attacken
    { 
        isAttacking = false;
        if (activeHitBox != null)   {activeHitBox.SetActive(false);}
    }


    //private void OnDrawGizmos() // detta är bara för att visa attackens räckvidd, det är inte nödvändigt för spelet att fungera, så det kan kommenteras bort, används för att se attackens strolek altså, detta ska inte vara synligt i det färdiga spelet
    //{

    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(attackUpPoint.position, attackRadius);
    //        Gizmos.DrawWireSphere(attackDownPoint.position, attackRadius);
    //        Gizmos.DrawWireSphere(attackLeftPoint.position, attackRadius);  
    //        Gizmos.DrawWireSphere(attackRightPoint.position, attackRadius);
    //}



}



