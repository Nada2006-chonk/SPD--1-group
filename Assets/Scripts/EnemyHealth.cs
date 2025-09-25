using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int currentHealth;
    public int startingHealth;
    private Animator anim;

    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
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
            anim.SetTrigger("Dead");
            Destroy(gameObject, 1f);
        }
    }
}
