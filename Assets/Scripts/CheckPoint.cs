using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator animator;
    private bool isActivated = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            GameManager.Instance.SetCheckpoint(transform.position);
            animator.SetTrigger("Activate");
        }
    }
}
