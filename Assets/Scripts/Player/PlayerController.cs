using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    // Handles player movement with camera-relative controls and rigidbody physics.
    public class PlayerController : MonoBehaviour
    {
        // Components
        [SerializeField] private Camera mainCamera;

        // Layer
        [SerializeField] private LayerMask walkableLayer;

        // Movement parameters
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float jumpForce;

        // Input actions
        private InputAction moveAction;
        private InputAction jumpAction;

        // State variables
        private Vector2 moveInput;
        private Rigidbody playerRigidbody;
        private CapsuleCollider capsuleCollider;
        private Animator animator;
        private bool jumpInput, isGrounded, isJumping;

        private void Awake()
        {
            // Cache the player input action
            moveAction = InputSystem.actions.FindAction("Move");
            if (moveAction == null) Debug.LogError("Error occured while initializing move action");
            jumpAction = InputSystem.actions.FindAction("Jump");
            if (jumpAction == null) Debug.LogError("Error occured while initializing jump action");

            // Cache the components for physics
            playerRigidbody = GetComponent<Rigidbody>();
            if (playerRigidbody == null) Debug.LogError("Error occurred while initializing rigidbody");
            capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider == null) Debug.LogError("Error occurred while initializing capsuleCollider");

            if (mainCamera == null) Debug.LogError("Error occurred while initializing camera");
            animator = GetComponent<Animator>();
        }

        // Reads input every frame and stores it for physics application.
        private void Update()
        {
            moveInput = moveAction.ReadValue<Vector2>();
            if (jumpAction.WasPressedThisFrame())
            {
                jumpInput = true;
            }
            animator.SetFloat("speed", moveInput.magnitude);
            animator.SetBool("isJumping", isJumping);
        }

        // Applies movement physics every fixed timestep.
        private void FixedUpdate()
        {
            var rayOrigin = transform.position + capsuleCollider.center;
            var ray = new Ray(rayOrigin, Vector3.down);
            var castDistance = capsuleCollider.height / 2;
            isGrounded = Physics.Raycast(ray, castDistance, walkableLayer);

            var dir = ComputeMoveDirection(moveInput);
            Move(dir);
            Rotate(dir);

            if (jumpInput && isGrounded)
            {
                isJumping = true;
                Jump();
                jumpInput = false;
            }
            if (!jumpInput && isGrounded && isJumping && playerRigidbody.linearVelocity.y <= 0)
                isJumping = false;
        }

        // Moves the player in camera-relative directions based on input.
        private void Move(Vector3 moveDirection)
        {
            // Apply movement while preserving vertical velocity
            var yVelocity = moveDirection.magnitude > 0.1f ? playerRigidbody.linearVelocity.y : 0f;
            playerRigidbody.linearVelocity = new Vector3(moveDirection.x * moveSpeed, yVelocity, moveDirection.z * moveSpeed);
        }

        private void Rotate(Vector3 direction)
        {
            if (direction.magnitude < 0.1f) return;
            
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            if (direction.magnitude > 0.1f)
                playerRigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation,
                    rotationSpeed * Time.fixedDeltaTime));
        }

        private Vector3 ComputeMoveDirection(Vector2 input)
        {
            // Get camera directions and flatten to horizontal plane
            var cameraForward = mainCamera.transform.forward;
            var cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Combine camera-relative directions with input
            var direction = cameraRight * input.x + cameraForward * input.y;
            return direction.magnitude > 0.1f ? direction.normalized : Vector3.zero;
        }

        private void Jump()
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}