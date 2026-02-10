using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class PlayerDig : MonoBehaviour
{
    #region === Inspector / References ===

    public PlayerMovement MoveSpeedScript;   // Reference to movement so we can slow the player while digging
    [SerializeField] private Animator PlayerAnimator;

    #endregion


    #region === Digging State ===

    public Collider2D CurrentTile;   // The ground tile we are currently digging
    public float DigTime = 1f;       // How long it takes to dig a tile
    public float DigTimer;           // Current progress toward digging
    public bool IsDigging;           // Whether the player is actively digging
    public Vector2 MoveInput;        // For getting the movement from PlayerMovement

    #endregion


    #region === Unity Lifecycle ===

    void Start()
    {
        // Cache the PlayerMovement script on the same object
        MoveSpeedScript = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        HandleDigTimer();
        HandleAnimations();
    }

    #endregion


    #region === Digging Logic ===

    /// <summary>
    /// Increases the dig timer while digging and destroys the tile when complete.
    /// </summary>
    private void HandleDigTimer()
    {
        if (IsDigging && CurrentTile != null)
        {
            DigTimer += Time.fixedDeltaTime;

            if (DigTimer >= DigTime)
            {
                DestroyCurrentTile();
            }
        }
    }

    /// <summary>
    /// Destroys the tile currently being dug up.
    /// </summary>
    public void DestroyCurrentTile()
    {
        if (CurrentTile != null)
        {
            Debug.Log("Tile Destroyed!");
            Destroy(CurrentTile.gameObject);
            StopDigging();
        }
    }


    /// <summary>
    /// Stops digging and resets movement + timers.
    /// </summary>
    public void StopDigging()
    {
        IsDigging = false;
        DigTimer = 0f;
        CurrentTile = null;
        //MoveSpeedScript.CanMove = true; // Allow player to move again

        // Restore normal movement speed
        if (MoveSpeedScript != null)
        {
            MoveSpeedScript.MoveSpeed = 3f;
        }
    }

    #endregion


    #region === Trigger Detection ===

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Ground"))
        {
            CurrentTile = Other;
            IsDigging = true;
            DigTimer = 0f;
            //MoveSpeedScript.CanMove = false; // Prevent player from moving while digging
            //MoveSpeedScript.MoveInput = Vector2.zero; // Stop player movement immediately when starting to dig

            // Slow player while digging
            if (MoveSpeedScript != null)
            {
                MoveSpeedScript.MoveSpeed = 1f;
            }

            Debug.Log("Started Digging...");
        }
    }

    private void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.CompareTag("Ground"))
        {
            StopDigging();
        }
    }

    #endregion

    #region === Animation ===

    private void HandleAnimations()
    {
        if (PlayerAnimator == null || MoveSpeedScript == null)
        {
            return;
        }

        Vector2 Move = MoveSpeedScript.MoveInput;

        PlayerAnimator.SetBool("IsDigging", false);

        if (IsDigging)
        {
            // Digging Up and Down
            if (Move.y < 0)
            {
                PlayerAnimator.SetBool("IsMoving", false);
                PlayerAnimator.SetBool("IsDigging", true);
            }
            else if (Move.y > 0)
            {
                transform.localScale = new Vector2(1, 1);
                PlayerAnimator.SetBool("IsMoving", false);
                PlayerAnimator.SetBool("IsDigging", true);
            }
            // Digging left & right
            else if (Move.x != 0)
            {
                if (Move.x < 0)
                {
                    transform.localScale = new Vector2(-1, 1);
                }
                else
                {
                    transform.localScale = new Vector2(1, 1);
                }
                PlayerAnimator.SetBool("IsMoving", false);
                PlayerAnimator.SetBool("IsDigging", true);
            }
        }
    }

    #endregion

}
