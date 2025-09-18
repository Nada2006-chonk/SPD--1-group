using UnityEngine;

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
    private bool isGrounded;



    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();

    }

    //Spelaren kan gå
    private void FixedUpdate()
    {
        rgbd.linearVelocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);

    }

    //För att spriten ska vända sig
    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    //Skapar en funktion för Jump
    private void Jump()
    {
        rgbd.AddForce(new Vector2(0, jumpForce));
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
    }
}
