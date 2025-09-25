using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //neo
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private Transform LeftFoot, RightFoot;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private LightConeDetector lightConeDetector;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float knockbackForce = 200f;

    [SerializeField] private float jumpVelocity = 10f;



    //neo
    public LayerMask enemyLayer;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator animator;
    private Coroutine lavaDamageCoroutine;
    private float horizontalValue;
    private float rayDistance = 0.25f;
    private float timer;
    private string currentState;
    private bool isGrounded;
    private bool isJumpPressed;
    private bool canDoubleJump;
    private bool canMove;
    private bool isAttacking;
    private bool isAttackPressed;
    private bool inLava = false;
    private int startingHealth = 5;
    private int currentHealth = 0;
    private int damage = 1;
    private int facingDirection = 1;


    //Animation States (Neo)
    const string Player_Jump = "Jump";
    const string Player_Attack_1 = "Attack1";
    const string Player_Run = "Run";
    const string Player_Hurt = "Player_Hurt";
    const string Player_Dead = "Dead";
    const string Player_Idle = "Idle";
    const string Player_Fall = "Fall";



    // -- animation state change (neo) --
    void ChangeAnimationState(string newState)
    {
        //stop the animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }


    void Start()
    {
        //när spelet startar är spelaren på full hälsa och kan röra sig (neo)
        canMove = true;
        currentHealth = startingHealth;
        UpdateHealthBar();

        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        //avbryt rörelse om can move inte är aktiv (neo)
        if (!canMove)
        {
            return;
        }

        //spelaren kan gå (neo)
        rgbd.linearVelocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);

        //flip sprite
        horizontalValue = Input.GetAxis("Horizontal");

        if(horizontalValue > 0 && transform.localScale.x < 0)
        {
            FlipSprite();
        }
        else if (horizontalValue < 0 && transform.localScale.x > 0)
        {
            FlipSprite();
        }
        //Axis checker, animator (Neo)
        if (CheckIfGrounded() == true && isAttacking == false)
        {
            if (horizontalValue != 0)
            {
                ChangeAnimationState(Player_Run);
            }
            else
            {
                ChangeAnimationState(Player_Idle);

            }
        }

        //jump animations (neo)
        if (rgbd.linearVelocity.y > 0 && isJumpPressed == true && CheckIfGrounded() == false)
        {
            ChangeAnimationState(Player_Jump);
        }
        if (rgbd.linearVelocity.y < 0 && !isGrounded && isJumpPressed == false)
        {
            ChangeAnimationState(Player_Fall);
        }

        //attack animations (neo)
        if(isAttackPressed == true)
        {
            isAttackPressed = false;

            if(isAttacking == true)
            {
                isAttacking = true;
                ChangeAnimationState(Player_Attack_1);
            }

            attackDelay = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke("AttackComplete", attackDelay);
        }


    }

    //Flip sprite (neo)
    private void FlipSprite()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    //Skapar en funktion för Jump (Neo)
    private void Jump(bool isDouble = false)
    {   
        rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, 0f);
        rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, jumpVelocity);
        if (isDouble)
            canDoubleJump = false;
    }

    //check if ground (Neo)
    private bool CheckIfGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(LeftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(RightFoot.position, Vector2.down, rayDistance, whatIsGround);

        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            return true;
        }
        else
        {
            isGrounded = false;
            return false;
        }


    }

    void Update()
    {
        //kallar funktionen attack (neo)
        if(Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }

        //Kallar funktionen jump + double jump (Neo)
        if (Input.GetButtonDown("Jump"))
        {
            if (CheckIfGrounded())
            {
                canDoubleJump = true;
                Jump();
            }
            else if (canDoubleJump)
            {
                Jump(isDouble: true);
            }
        }
        //se till att timern för cooldown räknar ned (neo)
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        InLight();

        //Checking for inputs (neo)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            isAttackPressed = true;
        }
    }

    //Spelare attackerar (neo)
    public void Attack()
    {
        if(timer <= 0)
        {
            isAttacking = true;
            timer = cooldown;

        }
    }

    //gör damage. Denna funktion kallas på i Attack Animation1 i Unity (neo)
    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-damage);
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce);
        }
    }
    //Attacken klar (neo)
    private void AttackComplete()
    {
        isAttacking = false;
    }


    //spelare ta skada (neo)
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            GameManager.Instance.HandlePlayerDeath(gameObject);
        }
    }

    //spelaren tar knockback (neo)
    public void TakeKnockback(float knockbackForce, float upwards)
    {
        canMove = false;
        rgbd.AddForce(new Vector2(knockbackForce, upwards));
        Invoke("CanMoveAgain", 0.25f);
    }

    private void CanMoveAgain()
    {
        canMove = true;
    }

    //flyttar spelaren till spawn om hälsan blir 0
    private void Respawn()
    {
        currentHealth = startingHealth;
        transform.position = SpawnPoint.position;
        rgbd.linearVelocity = Vector2.zero;
        UpdateHealthBar();
    }

    //Kalle
    private void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / startingHealth;
        healthBar.SetHealth(healthPercent);
    }

    private void InLight()
    {
        if (lightConeDetector != null && lightConeDetector.playerInLight)
        {
            TakeDamage(currentHealth);
            Debug.Log("Player is in the light! Health set to 0");
        }
    }

    //Kalle
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lava"))
        {
            inLava = true;
            rgbd.gravityScale = 0.5f;
            rgbd.linearDamping = 5f;
            rgbd.linearVelocity = rgbd.linearVelocity * 0.2f;

            if (lavaDamageCoroutine == null)
            {
                lavaDamageCoroutine = StartCoroutine(TakeLavaDamage());
            }
        }
    }

    //Kalle
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lava"))
        {
            inLava = false;
            rgbd.gravityScale = 1f;
            rgbd.linearDamping = 0f;
            if (lavaDamageCoroutine != null)
            {
                StopCoroutine(lavaDamageCoroutine);
                lavaDamageCoroutine = null;
            }
        }
    }

    //Kalle
    private IEnumerator TakeLavaDamage()
    {
        while (inLava)
        {
            TakeDamage(1);
            yield return new WaitForSeconds(0.3f);
        }

        lavaDamageCoroutine = null;
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
        UpdateHealthBar();
    }
}
