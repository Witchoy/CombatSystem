using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    // Manages third-person camera controls with pitch and yaw rotation around a target.
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private float sensitivity;

        [SerializeField] private float minPitch;

        [SerializeField] private float maxPitch;

        [SerializeField] private float radius;

        private InputAction lookAction;
        private Vector3 offsetDirection;

        private float pitch;
        private float yaw;

        // Caches the Look input action on initialization.
        private void Awake()
        {
            lookAction = InputSystem.actions.FindAction("Look");
            if (lookAction == null) Debug.LogError("Error occured while initializing look action");
        }

        // Updates camera rotation and position every frame after logic updates.
        private void LateUpdate()
        {
            // Read look input from player
            var lookInput = lookAction.ReadValue<Vector2>();

            // Update pitch (vertical rotation) and clamp to prevent flipping
            pitch -= lookInput.y * sensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // Update yaw (horizontal rotation)
            yaw += lookInput.x * sensitivity;

            // Calculate camera offset direction based on pitch and yaw
            offsetDirection = Quaternion.Euler(pitch, yaw, 0) * Vector3.back;

            // Position camera at offset distance from target
            transform.position = target.position + offsetDirection * radius;

            // Look at the target
            transform.LookAt(target);
        }
    }
}