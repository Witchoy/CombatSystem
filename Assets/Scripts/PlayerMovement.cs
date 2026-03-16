using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Component references
    public Rigidbody rb;

    // Movement parameters
    public float moveSpeed = 5f;

    // Actions
    InputAction moveAction;

    // State variables

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            moveAction = InputSystem.actions.FindAction("Move");

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
    }

    void FixedUpdate()
    {
    }
}
