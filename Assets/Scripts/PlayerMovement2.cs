using UnityEngine;
using System.Collections;
public class PlayerMovement2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private Transform LeftFoot, RightFoot;
    [SerializeField] private LayerMask whatIsGround;



    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator animator;

    private float horizontalValue;
    private float rayDistance = 0.25f;
    private string currentState;
    private bool isGrounded;
    private bool isJumpPressed;
    private bool jumpStarted;


    //Animation States (Neo)
    const string Player_Jump = "Jump";
    const string PLAYER_JUMPSTART = "PlayerJumpStart";
    const string Player_Attack_1 = "Attack1";
    const string Player_Run = "Run";
    const string Player_Hurt = "Hurt";
    const string Player_Dead = "Dead";
    const string Player_Idle = "Idle";
    const string Player_Fall = "Fall";



    // -- animation state change (neo) --
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        animator.CrossFadeInFixedTime(newState, 0.05f);

        currentState = newState;
    }


    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }



    //Spelaren kan gå (Neo)
    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        rgbd.linearVelocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);

        if (isGrounded && !jumpStarted) //idle och run om player på marken och inte påbörjat hopp
        {
            if (horizontalValue != 0f)
                ChangeAnimationState(Player_Run);
            else
                ChangeAnimationState(Player_Idle);
        }
        else if (!isGrounded)
        {
            //jumpStarted sätts till false i IEnumerator vilket har en delay.
            //Hindrar att någon animation kan köras under delayen vilket skyddar jumpStart så att den inte blir overridad.
            if (!jumpStarted)
            {
                float vy = rgbd.linearVelocity.y;

                if (vy > 0.2f && currentState != Player_Jump) //sätter jump rise vid positiv y velocity
                {
                    ChangeAnimationState(Player_Jump);
                }    
                    
                else if (vy < -0.2f && currentState != Player_Fall) //sätter jump fall vid negativ y velocity
                {
                    ChangeAnimationState(Player_Fall);
                }    
                   
            }
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
        ChangeAnimationState(PLAYER_JUMPSTART);
        jumpStarted = true;
        StartCoroutine(DoJumpAfterDelay(0.1f));
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
        bool wasGrounded = isGrounded;
        

        if (horizontalValue < 0)
        {
            FlipSprite(true);
        }

        if (horizontalValue > 0)
        {
            FlipSprite(false);
        }

        //Kallar funktionen jump (Neo)
        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true)
        {
            Jump();
        
        }

        //Checking for inputs (neo)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }

        if (!wasGrounded && isGrounded)
        {
            jumpStarted = false;
        }
    }



    private IEnumerator DoJumpAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        rgbd.AddForce(new Vector2(0, jumpForce));

        jumpStarted = false;
    }
}
