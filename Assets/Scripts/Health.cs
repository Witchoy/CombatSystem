using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth;
    private int currentHealth;

    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Croissant");
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
