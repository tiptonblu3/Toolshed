using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement speed multiplier (units per second)
    public float MoveSpeed = 5f;

    // Reference to the Rigidbody2D component used for physics-based movement.
    // This is populated in Start() to avoid repeated GetComponent calls.
    public Rigidbody2D rb;

    // Stores the latest movement input read from the Input System (x = horizontal, y = vertical).
    public Vector2 MoveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache the Rigidbody2D component attached to this GameObject.
       rb =  GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Convert the input vector into a velocity and apply it to the rigidbody.
        // Multiplying by MoveSpeed lets you control how fast the player moves.
        rb.linearVelocity = MoveInput * MoveSpeed;
    }

    // Movement input callback used by the Unity Input System.
    // The InputAction should provide a Vector2 (e.g. from a gamepad stick or WASD/arrow keys).
    public void Move(InputAction.CallbackContext context)
    {
        // Read the current Vector2 value from the callback context and store it.
        MoveInput = context.ReadValue<Vector2>();
    }
}