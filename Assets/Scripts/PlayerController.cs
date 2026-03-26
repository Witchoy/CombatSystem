using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles player movement with camera-relative controls and rigidbody physics.
public class PlayerController : MonoBehaviour
{
    // Components
    [SerializeField]
    private Camera mainCamera;
    private Rigidbody playerRigidbody;
    private CapsuleCollider capsuleCollider;

    // Layer
    [SerializeField] 
    private LayerMask walkableLayer;

    // Movement parameters
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;

    // Input actions
    private InputAction moveAction;
    private InputAction jumpAction;

    // State variables
    private Vector2 moveInput;
    private bool jumpInput, isGrounded;

    void Awake()
    {
        // Cache the player input action
        moveAction = InputSystem.actions.FindAction("Move");
        if (moveAction == null)
        {
            Debug.LogError($"Error occured while initializing move action");
        }
        jumpAction = InputSystem.actions.FindAction("Jump");
        if (jumpAction == null) {
            Debug.LogError($"Error occured while initializing jump action");
        }
        
        // Cache the components for physics
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError($"Error occurred while initializing rigidbody");
        }
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            Debug.LogError($"Error occurred while initializing capsuleCollider");
        }
        
        if (mainCamera == null)
        {
            Debug.LogError($"Error occurred while initializing camera");
        }
    }

    // Reads input every frame and stores it for physics application.
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        if (jumpAction.WasPressedThisFrame())
        {
            jumpInput = true;
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.height / 2 + 0.1f, walkableLayer);
    }

    // Applies movement physics every fixed timestep.
    void FixedUpdate()
    {
        Move(moveInput);
        if (jumpInput && isGrounded) 
        {
            Jump();
            jumpInput = false;
        }
        Rotate();
    }

    void Rotate()
    {
        // Get camera directions and flatten to horizontal plane
        Vector3 cameraForward = mainCamera.transform.forward;
        
        cameraForward.y = 0;
        
        cameraForward.Normalize();
        playerRigidbody.rotation = Quaternion.LookRotation(cameraForward);
    }

    // Moves the player in camera-relative directions based on input.
    void Move(Vector2 input)
    {        
        // Get camera directions and flatten to horizontal plane
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        
        cameraForward.y = 0;
        cameraRight.y = 0;
        
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Combine camera-relative directions with input
        Vector3 moveDirection = cameraRight * input.x + cameraForward * input.y;

        // Apply movement while preserving vertical velocity
        playerRigidbody.linearVelocity = new Vector3(moveDirection.x * moveSpeed, playerRigidbody.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    void Jump() 
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
