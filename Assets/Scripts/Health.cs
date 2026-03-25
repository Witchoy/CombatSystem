using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth;
    private int currentHealth {get; set;}

    private void Awake()
    {
        
    }

    public void TakeDamage(int amount)
    {
        
    }

    private void Die()
    {
        
    }
}
