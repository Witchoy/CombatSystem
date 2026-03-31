using System;
using UnityEngine;

// Tracks an entity's health, broadcasts changes via event, and destroys the object on death.
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Reduces health by the given amount and triggers death if health reaches zero.
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0) Die();
    }

    // Fired whenever the current health value changes, passing the new value.
    public event Action<int> OnHealthChanged;

    // Destroys this GameObject when health is depleted.
    private void Die()
    {
        Destroy(gameObject);
    }
}