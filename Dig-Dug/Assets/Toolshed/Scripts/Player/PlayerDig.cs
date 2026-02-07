using UnityEngine;

public class PlayerDig : MonoBehaviour
{
    #region === Inspector / References ===

    public PlayerMovement MoveSpeedScript;   // Reference to movement so we can slow the player while digging

    #endregion


    #region === Digging State ===

    public Collider2D CurrentTile;   // The ground tile we are currently digging
    public float DigTime = 1f;       // How long it takes to dig a tile
    public float DigTimer;           // Current progress toward digging
    public bool IsDigging;           // Whether the player is actively digging

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
}
