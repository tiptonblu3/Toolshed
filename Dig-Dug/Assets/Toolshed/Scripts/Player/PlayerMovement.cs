using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region === Inspector References ===

    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private LayerMask ObstacleLayer; // What tiles count as walls/blocked

    #endregion


    #region === Movement Settings ===

    public float MoveSpeed = 3f;
    public float TileSize = 1f; // Size of one grid step

    #endregion


    #region === Runtime State ===

    public Rigidbody2D Rb;

    public Vector2 MoveInput;        // Current snapped directional input
    public Vector2 TargetPosition;   // The grid position we are moving toward

    public bool CanMove = true;          // Whether the player can currently move (e.g., not while attacking)
    public bool IsMoving;
    public bool IsFacingLeft = false;
    public bool IsFacingUp = false;
    public bool IsFacingDown = false;

    #endregion


    #region === Unity Lifecycle ===

    void Start()
    {
        // Cache required components
        Rb = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();

        // Ensure we start aligned perfectly to the grid
        TargetPosition = transform.position;
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
        FlipSprite();
    }

    #endregion


    #region === Core Movement Logic ===

    private void HandleMovement()
    {
        // Smoothly move toward the current target tile
        transform.position = Vector2.MoveTowards(
            transform.position,
            TargetPosition,
            MoveSpeed * Time.deltaTime
        );

        // Only allow choosing a new tile once we've reached the current one
        if (Vector2.Distance(transform.position, TargetPosition) < 0.01f)
        {
            // Snap exactly to avoid floating point drift
            transform.position = TargetPosition;

            // If there's input, try to move one tile in that direction
            if (MoveInput != Vector2.zero)
            {
                Vector2 nextTile = TargetPosition + (MoveInput * TileSize);

                // Only move if the path is not blocked
                if (IsPathClear(nextTile))
                {
                    TargetPosition = nextTile;
                }
            }
        }
    }

    /// <summary>
    /// Checks if a tile position is free of objects tagged "Obstacle".
    /// </summary>
    private bool IsPathClear(Vector2 TargetPos)
    {
        // 1. Find any collider at the target position
        Collider2D hit = Physics2D.OverlapCircle(TargetPos, 0.2f);

        // 2. If we hit nothing, the path is clear
        if (hit == null) return true;

        // 3. If we hit ourselves (the player), the path is still technically clear
        if (hit.gameObject == gameObject) return true;

        // 4. If we hit something tagged "Obstacle", the path is blocked
        if (hit.CompareTag("Obstacle"))
        {
            Debug.Log("Path blocked by an Obstacle tag!");
            return false;
        }

        // Default to clear if it's just a background tile or something without the tag
        return true;
    }
    #endregion


    #region === Input System ===

    /// <summary>
    /// Called by the Input System. Snaps movement to cardinal directions only.
    /// </summary>
    public void Move(InputAction.CallbackContext Context)
    {
        Vector2 rawInput = Context.ReadValue<Vector2>();
        if (CanMove == true)
        {
            // Prioritize the axis with the stronger input
            if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
            {
                MoveInput = new Vector2(rawInput.x > 0 ? 1 : -1, 0);
            }
            else if (Mathf.Abs(rawInput.y) > Mathf.Abs(rawInput.x))
            {
                MoveInput = new Vector2(0, rawInput.y > 0 ? 1 : -1);
            }
            else
            {
                MoveInput = Vector2.zero;
            }
        }
    }

    #endregion


    #region === Animation ===

    private void HandleAnimation()
    {
        IsMoving = Vector2.Distance(transform.position, TargetPosition) > 0.01f;
        PlayerAnimator.SetBool("IsMoving", IsMoving);
    }

    #endregion


    #region === Sprite Facing / Rotation ===

    private void FlipSprite()
    {
        // ----- Horizontal Facing (flip scale) -----
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

        // ----- Vertical Facing (rotate sprite) -----
        if (MoveInput.y > 0) // Up
        {
            IsFacingUp = true;
            IsFacingDown = false;
            transform.localRotation = IsFacingLeft
                ? Quaternion.Euler(0, 0, -90)
                : Quaternion.Euler(0, 0, 90);
        }
        else if (MoveInput.y < 0) // Down
        {
            IsFacingUp = false;
            IsFacingDown = true;
            transform.localRotation = IsFacingLeft
                ? Quaternion.Euler(0, 0, 90)
                : Quaternion.Euler(0, 0, -90);
        }

        // If moving horizontally, clear vertical facing flags
        if (MoveInput.x != 0)
        {
            IsFacingDown = false;
            IsFacingUp = false;
        }
    }

    #endregion
}
