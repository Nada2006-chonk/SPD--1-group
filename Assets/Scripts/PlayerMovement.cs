using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    //neo
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private Transform LeftFoot, RightFoot;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private HealthBar healthBar;



    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator animator;

    private float horizontalValue;
    private float rayDistance = 0.25f;
    private string currentState;
    private bool isGrounded;
    private bool isJumpPressed;
    private bool canDoubleJump;
    private bool canMove;
    private int startingHealth = 5;
    private int currentHealth = 0;


    //Animation States (Neo)
    const string Player_Jump = "Jump";
    const string Player_Attack_1 = "Attack1";
    const string Player_Run = "Run";
    const string Player_Hurt = "Hurt";
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

    //Spelaren kan gå (Neo)
    private void FixedUpdate()
    {
        //avbryt rörelse om can move inte är aktiv (neo)
        if(!canMove)
        {
            return;
        }

        rgbd.linearVelocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);

        //Axis checker, animator (Neo)
        if (CheckIfGrounded() == true)
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
        if(rgbd.linearVelocity.y > 0 && isJumpPressed == true)
        {
            ChangeAnimationState(Player_Jump);
        }
        if(rgbd.linearVelocity.y < 0 && !isGrounded && isJumpPressed == false)
        {
            ChangeAnimationState(Player_Fall);
        }


    }

    //För att spriten ska vända sig (Neo)
    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    //Skapar en funktion för Jump (Neo)
    private void Jump()
    {
        rgbd.AddForce(new Vector2(0, jumpForce));
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
        //flip sprite (Neo)
        horizontalValue = Input.GetAxis("Horizontal");

        if (horizontalValue < 0)
        {
            FlipSprite(true);
        }

        if (horizontalValue > 0)
        {
            FlipSprite(false);
        }

        //Kallar funktionen jump + double jump (Neo)
        if (Input.GetButtonDown("Jump"))
        {
            if(CheckIfGrounded() == true)
            {
                canDoubleJump = true;
                Jump();
            }
            else
            {
                if(canDoubleJump)
                {
                    rgbd.AddForce(new Vector2(0, jumpForce));
                    canDoubleJump = false;

                }
            }

        }

        //Checking for inputs (neo)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }
    }

    //spelare ta skada (neo)
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        print(currentHealth);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    //spelare ta knockback (neo)
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


    private void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / startingHealth;
        healthBar.SetHealth(healthPercent);
    }
}
