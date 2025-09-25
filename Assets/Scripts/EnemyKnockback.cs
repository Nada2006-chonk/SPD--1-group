using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rgbd;

    private void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
    }

    public void Knockback(Transform playerTransform, float knockbackForce)
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rgbd.linearVelocity = direction * knockbackForce;
    }
}
