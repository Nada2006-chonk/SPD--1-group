using UnityEngine;

public class BreakablePillar : MonoBehaviour
{
    [SerializeField] private BoxCollider2D blockCollider;
    [SerializeField] private float scatterForce = 1f;
    [SerializeField] private float scatterTorque = 1.5f;

    private bool broken = false;

    public void Break()
    {
        if (broken) return;
        broken = true;

        
        if (blockCollider != null)
        {
            Destroy(blockCollider);
        }

        
        foreach (Transform child in transform)
        {
            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;

                
                float xForce = Random.Range(-scatterForce, scatterForce);
                float yForce = Random.Range(0.5f * scatterForce, scatterForce);
                rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);

                
                float torque = Random.Range(-scatterTorque, scatterTorque);
                rb.AddTorque(torque, ForceMode2D.Impulse);
            }
        }
    }
}
