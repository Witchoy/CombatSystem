using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    // Manages third-person camera controls with pitch and yaw rotation around a target.
    public class CameraController : MonoBehaviour
    {
        // The transform the camera orbits around (typically the player)
        [SerializeField] private Transform target;

        // Mouse/stick sensitivity multiplier
        [SerializeField] private float sensitivity;

        // Vertical rotation limits (degrees)
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;

        // Orbit distance from the target
        [SerializeField] private float radius;

        // Input actions
        private InputAction lookAction;

        private Vector3 offsetDirection;

        // Current rotation state
        private float pitch;
        private float yaw;

        // Caches the Look input action on initialization.
        private void Awake()
        {
            lookAction = InputSystem.actions.FindAction("Look");
            if (lookAction == null) Debug.LogError("Error occurred while initializing look action");
        }

        // Updates camera rotation and position every frame, after all other updates.
        private void LateUpdate()
        {
            var lookInput = lookAction.ReadValue<Vector2>();

            // Update pitch (vertical) and clamp to prevent flipping
            pitch -= lookInput.y * sensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // Update yaw (horizontal)
            yaw += lookInput.x * sensitivity;

            // Compute the camera's offset direction from pitch and yaw
            offsetDirection = Quaternion.Euler(pitch, yaw, 0) * Vector3.back;

            // Position the camera at orbit distance from the target, then face it
            transform.position = target.position + offsetDirection * radius;
            transform.LookAt(target);
        }
    }
}