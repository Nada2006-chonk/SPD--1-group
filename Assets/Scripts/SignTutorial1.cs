using UnityEngine;

public class SignTutorial1 : MonoBehaviour
{
    [SerializeField] private GameObject textPopUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textPopUp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textPopUp.SetActive(false);
        }
    }
}
