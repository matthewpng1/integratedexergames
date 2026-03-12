using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Increment()
    {
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);
    }

    public void Decrement()
    {
        currentHealth = Mathf.Max(currentHealth - 1, 0);
        if (currentHealth == 0)
        {
            Debug.Log("💀 Player died!");
            // Trigger death event here
        }
    }

    public bool IsAlive() // ADD THIS ✅
    {
        return currentHealth > 0;
    }
}