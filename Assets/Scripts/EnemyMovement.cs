using UnityEngine;
using UnityEngine.Audio;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100;
    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float upwardsForce = 100f;
    [SerializeField] private int damageGiven = 1;

    private bool canMove = true;

    private SpriteRenderer rend;

    private void Start()
    {
        //tilldela variabeln så att den åkallar funktionen SpriteRenderer (neo)
        rend = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {

        if (!canMove)
        {
            return;
        }

        //detta är rörelse (neo)
        transform.Translate(new Vector2(MoveSpeed, 0) * Time.deltaTime);

        //detta är en if-sats för att se till att spriten flippas (neo)
        if (MoveSpeed < 0)
        {
            rend.flipX = true;
        }

        if (MoveSpeed > 0)
        {
            rend.flipX = false;
        }
    }

    //Se till att fienden vänder sig vid enemybox (neo)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock"))
        {
            MoveSpeed = -MoveSpeed;
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
