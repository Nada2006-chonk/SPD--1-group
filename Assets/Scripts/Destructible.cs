using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private int startingHealth = 1;
    private int currentHealth;

    
    public System.Action OnDestroyed;

    void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}
