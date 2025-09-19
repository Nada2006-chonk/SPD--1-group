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

    private float horizontalValue;
    private float rayDistance = 0.25f;
   

    private Animator animator;
    private string currentState;
    private bool isGrounded;
    private bool jumpStarted;
    




    //Animation states
    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_JUMPSTART = "PlayerJumpStart";
    const string PLAYER_JUMPRISE = "PlayerJumpRise";
    const string PLAYER_JUMPFALL = "PlayerJumpFall";
    const string PLAYER_RUN = "PlayerRun";
    const string PLAYER_WALK = "PlayerWalk";
    const string PLAYER_HURT = "PlayerHurt";
    const string PLAYER_DEAD = "PlayerDead";
    const string PLAYER_ATTACK1 = "PlayerAttack1";
    const string PLAYER_ATTACK2 = "PlayerAttack2";
    const string PLAYER_ATTACK3 = "PlayerAttack3";
    const string PLAYER_ATTACK4 = "PlayerAttack4";



    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    //Spelaren kan gå
    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        rgbd.linearVelocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);

        if (isGrounded && !jumpStarted) //idle och run om player på marken och inte påbörjat hopp
        {
            if (horizontalValue != 0f)
                ChangeAnimationState(PLAYER_RUN);
            else
                ChangeAnimationState(PLAYER_IDLE);
        }
        else if (!isGrounded)
        {
            //jumpStarted sätts till false i IEnumerator vilket har en delay.
            //Hindrar att någon animation kan köras under delayen vilket skyddar jumpStart så att den inte blir overridad.
            if (!jumpStarted)
            {
                float vy = rgbd.linearVelocity.y;

                if (vy > 0.2f && currentState != PLAYER_JUMPRISE) //sätter jump rise vid positiv y velocity
                {
                    ChangeAnimationState(PLAYER_JUMPRISE);
                }    
                    
                else if (vy < -0.2f && currentState != PLAYER_JUMPFALL) //sätter jump fall vid negativ y velocity
                {
                    ChangeAnimationState(PLAYER_JUMPFALL);
                }    
                   
            }
        }
    }






    //För att spriten ska vända sig
    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    //Skapar en funktion för Jump
    private void Jump()
    {
        ChangeAnimationState(PLAYER_JUMPSTART);
        jumpStarted = true;
        //StartCoroutine och IEnumerator tillsammans kan skapa delay.
        StartCoroutine(DoJumpAfterDelay(0.1f));
    }


    private bool CheckIfGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(LeftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(RightFoot.position, Vector2.down, rayDistance, whatIsGround);

        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            
            return true;
        }
        else
        {
            
            return false;
        }


    }

    void Update()
    {
        horizontalValue = Input.GetAxis("Horizontal");
        bool wasGrounded = isGrounded;

        isGrounded = CheckIfGrounded();

        if (horizontalValue < 0)
        {
            FlipSprite(true);
        }

        if (horizontalValue > 0)
        {
            FlipSprite(false);
        }

        //Kallar funktionen jump
        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true)
        {
            Jump();
        }

        if (!wasGrounded && isGrounded)
        {
            jumpStarted = false;
        }

    }

    //Animator grejen fast i script
    void ChangeAnimationState(string newState)
    {
        if(currentState == newState)
        {
            return;
        }
        animator.CrossFadeInFixedTime(newState, 0.05f);

        currentState = newState;
    }

//Delay i hoppet för att hinna spela jump start animationen
    private IEnumerator DoJumpAfterDelay(float delay)
    {
       
        yield return new WaitForSeconds(delay);

        rgbd.AddForce(new Vector2(0, jumpForce));

        jumpStarted = false;
    }
}
