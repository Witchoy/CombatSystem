using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCombats : MonoBehaviour
    {
        // Player variables
        [SerializeField] private float radius = 0.1f;

        [SerializeField] private int damage = 25;

        [SerializeField] private float range = 0.25f;

        // Input actions
        private InputAction attackAction;

        private void Awake()
        {
            // Cache the player input action
            attackAction = InputSystem.actions.FindAction("Attack");
            if (attackAction == null) Debug.LogError("Error occured while initializing attack action");
        }

        private void Update()
        {
            if (attackAction.WasPressedThisFrame()) Attack();
        }

        private void Attack()
        {
            Debug.Log("baguette");
            var hitColliders = Physics.OverlapSphere(transform.position + transform.forward * range, radius);
            foreach (var hitCollider in hitColliders)
                if (hitCollider.TryGetComponent(out IDamageable hit))
                    hit.TakeDamage(damage);
        }
    }
}