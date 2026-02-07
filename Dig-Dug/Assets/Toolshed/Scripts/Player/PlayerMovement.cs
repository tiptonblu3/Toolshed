using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float MoveSpeed = 5f;
    public Rigidbody2D rb;
    public Vector2 MoveInput;
    public bool IsMoving;
    public bool IsFacingLeft = false;
    public bool IsFacingUp = false;
    public bool IsFacingDown = false;
    [SerializeField] private Animator animator;


    void Start()
    {
       rb =  GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();

    }

    void Update()
    {
        rb.linearVelocity = MoveInput * MoveSpeed;
        // Call the flip logic
        LeftCheck();
        FlipSprite();
        #region Animation
        if (MoveInput != Vector2.zero)
        {
            IsMoving = true;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            IsMoving = false;
            animator.SetBool("IsMoving", false);
        }
        #endregion
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
            IsFacingDown = false;
            IsFacingUp = true;
        }
        else if (MoveInput.y > 0)
        {
            // Face up
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            IsFacingDown = false;
            IsFacingUp = true;
        }

        if (IsFacingLeft == true && MoveInput.y < 0)
        {
            // Ensure the sprite is flipped correctly when moving horizontally
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            IsFacingDown = true;
            IsFacingUp = false;
        }
        else if (MoveInput.y < 0)
        {
            // Face down
            transform.localRotation = Quaternion.Euler(0, 0, -90);
            IsFacingDown = true;
            IsFacingUp = false;
        }

        if (MoveInput.x != 0)
        {
            // Reset rotation when moving horizontally
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            IsFacingDown = false;
            IsFacingUp = false;
        }

        /**/
        /**/
    }

}