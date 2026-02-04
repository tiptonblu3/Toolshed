using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //declare variables
    public float MoveSpeed = 5f;
    public Rigidbody2D rb;
    public Vector2 MoveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Grabs Rigidbody2D from Player
       rb =  GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //Move Calcs
        rb.linearVelocity = MoveInput * MoveSpeed;
    }

    //Movement
    public void Move(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}
