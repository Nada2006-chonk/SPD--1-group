using UnityEngine;

public class Killzone : MonoBehaviour
{

    [SerializeField] private Transform SpawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = SpawnPoint.position;
            other.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        }

    }
}
