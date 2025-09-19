using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 2.0f;
    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float upwardsForce = 100f;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private Transform Target;
    [SerializeField] private float visionRange = 4f; //när ser fienden spelaren
    [SerializeField] private float attackRange = 1.0f; //när ska fienden attackera spelaren
    [SerializeField] private float cooldown = 1.0f; //attack cooldown



    private bool canMove = true;
    private bool isChasing = false;
    private bool isAttacking = false;
    private int startingHealth = 5;
    private int currentHealth = 0;
    private float lastAttackTime = 0f;
    private Vector2 moveDirection = Vector2.right;

    private Rigidbody2D rgbd;
    private SpriteRenderer rend;


    private void Start()
    {
        //tilldela variabeln så att den åkallar funktionen SpriteRenderer (neo)
        rend = GetComponent<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();

        currentHealth = startingHealth;

    }


    void FixedUpdate()
    {

        if (!canMove)
        {
            return;
        }
   
        //detta är rörelse (neo)
        transform.Translate(moveDirection * MoveSpeed * Time.deltaTime);

        //detta är en if-sats för att se till att spriten flippas (neo)
        if (moveDirection.x < 0)
        {
            rend.flipX = true;
        }

        if (moveDirection.x > 0)
        {
            rend.flipX = false;
        }
    }

    void Update()
    {
        if (Target == null)
        {
            return;
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, Target.position);

            // Kolla om fienden ska jaga spelaren
            if (distanceToPlayer <= visionRange && distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else if (distanceToPlayer <= attackRange)
            {
                isChasing = false;
                isAttacking = true;

                if (Time.time - lastAttackTime > cooldown)
                {
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                isChasing = false;
                isAttacking = false;

                // Trigga attack om spelaren är nära och cooldown har gått
                if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > cooldown)
                {
                    lastAttackTime = Time.time;

                }
            }
        }


    }

    //Funktion för att få fienden att jaga spelaren (neo)
    private void ChasePlayer()
    {
        isChasing = true;
        isAttacking = false;

        Vector2 direction = (Target.position - transform.position).normalized;
        rgbd.linearVelocity = new Vector2(direction.x * MoveSpeed, rgbd.linearVelocity.y);
    }

    //Se till att fienden vänder sig vid enemybox (neo)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock"))
        {
            moveDirection = -moveDirection;
        }

        //fienden ger playern damage och knockback on collision (neo)
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage(damageGiven);

            if (other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(knockbackForce, upwardsForce);
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(-knockbackForce, upwardsForce);
            }
        }
    }
}
