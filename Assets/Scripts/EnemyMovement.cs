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
    private float horizontalValue;
    private Vector2 moveDirection = Vector2.right;

    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator animator;


    private void Start()
    {
        //tilldela variabeln så att den åkallar funktionerna (neo)
        rend = GetComponent<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = startingHealth;

    }

    void Update()
    {
        animator.SetFloat("MoveSpeed", Mathf.Abs(rgbd.linearVelocity.x));

        if (Target == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, Target.position);


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

    //Funktion för att få fienden att jaga spelaren (neo)
    private void ChasePlayer()
    {
        isChasing = true;
        isAttacking = false;

        Vector2 direction = (Target.position - transform.position).normalized;
        rgbd.linearVelocity = new Vector2(direction.x * MoveSpeed, rgbd.linearVelocity.y);
        rend.flipX = Target.position.x < transform.position.x;

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
