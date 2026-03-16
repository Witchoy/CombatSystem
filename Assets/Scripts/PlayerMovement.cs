using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Component references
    public Rigidbody rb;

    // Movement parameters
    public float moveSpeed = 5f;
    public float jumpHeight = 0.5f;

    // Actions
    InputAction moveAction;
    InputAction jumpAction;

    // State variables
    private bool isJumpPressed;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");

            rb = GetComponent<Rigidbody>();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occurred while initializing player actions: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        isJumpPressed = jumpAction != null && jumpAction.triggered;
    }

    void FixedUpdate()
    {
        if (isJumpPressed)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on player.");
            return;
        }
        float jumpForce = rb.mass * Mathf.Sqrt(2f * 9.81f * jumpHeight);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
