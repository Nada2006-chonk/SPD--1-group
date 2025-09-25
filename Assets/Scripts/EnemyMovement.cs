using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 2.0f;
    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float upwardsForce = 100f;
    [SerializeField] private int damageGiven = 1;


    private bool canMove = true;
    private AudioSource audioSource;



    //skapa en variabel
    private SpriteRenderer rend;

    private void Start()
    {
        //tilldela variabeln så att den åkallar funktionen SpriteRenderer
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

    }

    void FixedUpdate()
    {

        if (!canMove)
        {
            return;
        }

        //detta är rörelse
        transform.Translate(new Vector2(MoveSpeed, 0) * Time.deltaTime);

        //detta är en if-sats för att se till att spriten flippas
        if (MoveSpeed < 0)
        {
            rend.flipX = true;
        }

        if (MoveSpeed > 0)
        {
            rend.flipX = false;
        }
    }
    //Se till att fienden vänder sig vid enemybox
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock"))
        {
            MoveSpeed = -MoveSpeed;
        }


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

    // Fiendens kill trigger
/*    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(other.GetComponent<Rigidbody2D>().linearVelocity.x, 0);
            GetComponent<Animator>().SetTrigger("Dead");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            canMove = false;
            Destroy(gameObject, 0.5f);
        }
    }*/
}
