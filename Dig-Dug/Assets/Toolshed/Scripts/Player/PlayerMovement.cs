using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float MoveSpeed = 5f;


    public Rigidbody2D rb;

    public Vector2 MoveInput;

    void Start()
    {
       rb =  GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = MoveInput * MoveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}