using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int currentHealth;
    public int startingHealth;

    void Start()
    {
        currentHealth = startingHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }

        else if (currentHealth <= 0)
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
