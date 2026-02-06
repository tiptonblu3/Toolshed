using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float MoveSpeed = 5f;
    public Rigidbody2D rb;
    public Vector2 MoveInput;
    public bool IsMoving;
    public bool IsFacingLeft = false;

    void Start()
    {
       rb =  GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = MoveInput * MoveSpeed;
        // Call the flip logic
        LeftCheck();
        FlipSprite();
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();

        // If both axes are being pressed, we prioritize one
        if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
        {
            // Prioritize Horizontal
            MoveInput = new Vector2(rawInput.x, 0);
        }
        else
        {
            // Prioritize Vertical
            MoveInput = new Vector2(0, rawInput.y);
        }
    }

    public void LeftCheck()
    {
        if (MoveInput.x < 0)
        {
            IsFacingLeft = true;
        }
        else if (MoveInput.x > 0)
        {
            IsFacingLeft = false;
        }
    }

    private void FlipSprite()
    {
        // Check if there is horizontal movement
        if (MoveInput.x > 0)
        {
            // Face right
            transform.localScale = new Vector2(1, 1);
        }
        if (MoveInput.x < 0)
        {
            // Face left
            transform.localScale = new Vector2(-1, 1);
        }
        // Check if there is vertical movement

        if (IsFacingLeft == true && MoveInput.y > 0)
        {
            // Ensure the sprite is flipped correctly when moving horizontally
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else if (MoveInput.y > 0)
        {
            // Face up
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        if (IsFacingLeft == true && MoveInput.y < 0)
        {
            // Ensure the sprite is flipped correctly when moving horizontally
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (MoveInput.y < 0)
        {
            // Face down
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }

        if (MoveInput.x != 0)
        {
            // Reset rotation when moving horizontally
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        /**/
        /**/
    }

}