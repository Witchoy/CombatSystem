using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombats : MonoBehaviour
{
    // Input actions
    private InputAction attackAction;

    // Player variables
    [SerializeField]
    private float radius = 0.1f;
    [SerializeField]
    private int damage = 25;
    [SerializeField]
    private float range = 0.25f;

    void Awake()
    {
        // Cache the player input action
        attackAction = InputSystem.actions.FindAction("Attack");
        if (attackAction == null)
        {
            Debug.LogError($"Error occured while initializing attack action");
        }
    }

    void Update()
    {
        if(attackAction.WasPressedThisFrame())
        {
            Attack();
        }
    }

    void Attack()
    {
        Debug.Log("baguette");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * range, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                hit.TakeDamage(damage);
            }
        }
    }
}
