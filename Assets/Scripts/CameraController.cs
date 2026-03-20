using UnityEngine;
using UnityEngine.InputSystem;

// Manages third-person camera controls with pitch and yaw rotation around a target.
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float minPitch;
    [SerializeField]
    private float maxPitch;
    [SerializeField]
    private float radius;

    private InputAction lookAction;

    private float pitch;
    private float yaw;
    private Vector3 offsetDirection;

    // Caches the Look input action on initialization.
    void Awake()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        if (lookAction == null)
        {
            Debug.LogError($"Error occured while initializing look action");
        }
    }

    // Updates camera rotation and position every frame after logic updates.
    void LateUpdate()
    {
        // Read look input from player
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

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
