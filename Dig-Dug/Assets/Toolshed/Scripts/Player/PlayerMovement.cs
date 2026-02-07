using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public Rigidbody2D Rb;
    public Vector2 MoveInput;
    public bool IsMoving;
    public bool IsFacingLeft = false;
    public bool IsFacingUp = false;
    public bool IsFacingDown = false;
    
    [SerializeField] private Animator PlayerAnimator;
    public Vector2 TargetPosition;
    public float TileSize = 1f;
    
    // Add a LayerMask for things you cannot walk through (like walls)
    [SerializeField] private LayerMask ObstacleLayer;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        
        // Ensure we start aligned to the grid
        TargetPosition = transform.position;
    }

    void Update()
    {
        // 1. Smoothly move toward the target
        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, MoveSpeed * Time.deltaTime);

        // 2. Logic gate: only check for new moves when at the destination
        if (Vector2.Distance(transform.position, TargetPosition) < 0.01f)
        {
            transform.position = TargetPosition;

            if (MoveInput != Vector2.zero)
            {
                // Check if the next tile is walkable before setting it as a target
                if (IsPathClear(TargetPosition + (MoveInput * TileSize)))
                {
                    TargetPosition += MoveInput * TileSize;
                }
            }
        }

        FlipSprite();

        #region Animation
        IsMoving = Vector2.Distance(transform.position, TargetPosition) > 0.01f;
        PlayerAnimator.SetBool("IsMoving", IsMoving);
        #endregion
    }

    // New method to check for obstacles
    private bool IsPathClear(Vector2 TargetPos)
    {
        // Checks if there's a collider in the ObstacleLayer at the target tile
        // The radius (0.2f) should be smaller than your TileSize
        return !Physics2D.OverlapCircle(TargetPos, 0.2f, ObstacleLayer);
    }

    public void Move(InputAction.CallbackContext Context)
    {
        Vector2 RawInput = Context.ReadValue<Vector2>();

        if (Mathf.Abs(RawInput.x) > Mathf.Abs(RawInput.y))
        {
            MoveInput = new Vector2(RawInput.x > 0 ? 1 : -1, 0);
        }
        else if (Mathf.Abs(RawInput.y) > Mathf.Abs(RawInput.x))
        {
            MoveInput = new Vector2(0, RawInput.y > 0 ? 1 : -1);
        }
        else
        {
            MoveInput = Vector2.zero;
        }
    }

    private void FlipSprite()
    {
        // Horizontal Scaling
        if (MoveInput.x > 0)
        {
            IsFacingLeft = false;
            transform.localScale = new Vector2(1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (MoveInput.x < 0)
        {
            IsFacingLeft = true;
            transform.localScale = new Vector2(-1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        // Vertical Rotations
        if (MoveInput.y > 0) // Up
        {
            IsFacingUp = true;
            IsFacingDown = false;
            transform.localRotation = IsFacingLeft ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);
        }
        else if (MoveInput.y < 0) // Down
        {
            IsFacingUp = false;
            IsFacingDown = true;
            transform.localRotation = IsFacingLeft ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, -90);
        }

        if (MoveInput.x != 0)
        {
            IsFacingDown = false;
            IsFacingUp = false;
        }
    }
}