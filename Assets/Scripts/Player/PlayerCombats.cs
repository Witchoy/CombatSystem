using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    // Handles melee combat: reads attack input, performs an overlap sphere hit-check,
    // and applies damage to any IDamageable within range.
    public class PlayerCombats : MonoBehaviour
    {
        // Attack sphere radius (world units)
        [SerializeField] private float radius = 25f;

        // Flat damage value applied per hit
        [SerializeField] private int damage = 25;

        // How far in front of the player the attack sphere is projected
        [SerializeField] private float range = 4f;

        // Input actions
        private InputAction attackAction;

        private CapsuleCollider capsuleCollider;

        private void Awake()
        {
            // Cache the player input action
            attackAction = InputSystem.actions.FindAction("Attack");
            if (attackAction == null) Debug.LogError("Error occurred while initializing attack action");

            capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider == null) Debug.LogError("Error occurred while initializing capsuleCollider");
        }

        // Polls attack input every frame.
        private void Update()
        {
            if (attackAction.WasPressedThisFrame()) Attack();
        }

        // Performs an overlap sphere at the computed attack origin and damages all IDamageable targets found.
        private void Attack()
        {
            var hitColliders = Physics.OverlapSphere(GetAttackOrigin(), radius);
            foreach (var hitCollider in hitColliders)
                if (hitCollider.TryGetComponent(out IDamageable hit))
                    hit.TakeDamage(damage);
        }

        // Returns the world-space centre of the attack sphere, offset forward and vertically to align with the player's torso.
        private Vector3 GetAttackOrigin()
        {
            var heightOffset = capsuleCollider.center.y;
            return transform.position + Vector3.up * heightOffset + transform.forward * range;
        }
    }
}