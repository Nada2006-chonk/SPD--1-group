using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float upwardsForce = 5f;
    [SerializeField] private float damageCooldown = 0.5f;

    private float lastDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                PlayerMovement player = collision.GetComponent<PlayerMovement>();

                if (player != null)
                {
                    player.TakeDamage(damageGiven);
                    player.TakeKnockback(knockbackForce, upwardsForce);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}
