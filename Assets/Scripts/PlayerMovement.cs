using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
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
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }



    //Spelaren kan gå (Neo)
    private void FixedUpdate()
    {
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

        //Kallar funktionen jump (Neo)
        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true)
        {
            Jump();
            
        }

        //Checking for inputs (neo)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }
    }



    
}
