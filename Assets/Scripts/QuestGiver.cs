using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    //Natalia

    [SerializeField] private GameObject textPopUp;

    public void OnTriggerEnter(Collider other) 
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
            textPopUp.SetActive(true);
        }
    }
}
