using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    #region === Settings ===

    [SerializeField] private float MoveSpeed = 2.5f;               // Normal movement speed
    [SerializeField] private float GhostSpeedMultiplier = 0.4f;    // Speed reduction while ghosting through tiles
    [SerializeField] private float TileSize = 1.0f;                // Size of one grid step

    [Header("Timers & Cooldowns")]
    [SerializeField] private float MinGrace = 3.0f;                // Minimum patrol duration before chasing
    [SerializeField] private float MaxGrace = 8.0f;                // Maximum patrol duration before chasing
    [SerializeField] private float GhostCooldown = 10.0f;          // Cooldown before ghosting can be used again
    
    [Header("Attack Settings")]
    [SerializeField] private float fireRate = 1.5f;               // Time between bubble attacks
    [SerializeField] private float attackRange = 2.0f;            // Only attack if player is within 2 units
    public bool CanAttack = true;                                 // Global toggle for attacking
    private float nextFireTime = 0f;                              // Timestamp for next attack

    [Header("Search Logic")]
    [SerializeField] private int LookAheadRange = 3;               // How far ahead the enemy checks for alternate routes

    [Header("Grace Settings")]
    [SerializeField] private bool MoveVerticalInGrace = false;     // If true, patrol vertically instead of horizontally

    #endregion


    #region === Runtime State ===

    private Transform PlayerTarget;           // Player transform reference
    private Vector2 TargetPosition;           // Grid tile the enemy is currently moving toward
    private Vector2 CurrentMoveDir;           // Current movement direction (for sprite rotation)

    // Used to avoid tight back-and-forth loops
    private Vector2 PreviousLocation1 = Vector2.negativeInfinity;
    private Vector2 PreviousLocation2 = Vector2.negativeInfinity;

    // Sprite facing flags
    public bool IsFacingLeft, IsFacingUp, IsFacingDown;

    // Behavior states
    public bool IsGhosting = false;           // Passing through ground tiles
    public bool IsGrace = true;               // Patrol mode
    public bool IsChasing = false;            // Aggressive chase mode
    public bool CanMove = true;               // Whether the enemy can currently move

    private float StateTimer = 0f;            // Timer controlling grace → chase transition
    private float NextGhostAvailableTime = 0f;

    private int GraceDirection = 1;           // Patrol direction switcher

    #endregion

    [Header("Attack References")]
    public GameObject BubblesPrefab; // Prefab for the attack effect (e.g., bubbles)
    public Transform FirePoint; // Point from which the attack is spawned


    #region === Unity Lifecycle ===

    void Start()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
            PlayerTarget = Player.transform;

        // Snap initial movement target to current position
        TargetPosition = transform.position;

        // Start in grace (patrol) state for a random duration
        IsGrace = true;
        IsChasing = false;
        StateTimer = Random.Range(MinGrace, MaxGrace);
    }

    void Update()
    {
        if (PlayerTarget == null) return;

        HandleStateTimer();
        
        // Only move if not currently locked in an attack animation/slowdown
        if (CanMove)
        {
            HandleGridMovement();
        }

        // Distance Check: Attack if player is within 2 units
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerTarget.position);
        if (distanceToPlayer <= attackRange)
        {
            PuffAttack();
        }

        FlipSprite();
    }

    #endregion


    #region === State Management ===

    /// <summary>
    /// Counts down the grace timer and switches to chase mode when it ends.
    /// </summary>
    private void HandleStateTimer()
    {
        if (StateTimer > 0)
        {
            StateTimer -= Time.deltaTime;

            if (StateTimer <= 0 && IsGrace)
            {
                IsGrace = false;
                IsChasing = true;
            }
        }
    }

    #endregion


    #region === Core Grid Movement ===

    /// <summary>
    /// Handles movement toward the current target tile and selects the next action when reached.
    /// </summary>
    private void HandleGridMovement()
    {
        float Speed = IsGhosting ? (MoveSpeed * GhostSpeedMultiplier) : MoveSpeed;

        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, TargetPosition) < 0.01f)
        {
            // Track last positions to prevent simple loops
            if ((Vector2)transform.position != TargetPosition)
            {
                PreviousLocation2 = PreviousLocation1;
                PreviousLocation1 = transform.position;
            }

            transform.position = TargetPosition;

            if (IsGrace)
                MoveGraceBehavior();
            else if (IsChasing)
                MoveChaseBehavior();
        }
    }

    #endregion


    #region === Movement Behaviors ===

    /// <summary>
    /// Patrol movement used during the grace period.
    /// </summary>
    private void MoveGraceBehavior()
    {
        Vector2 PatrolDir = MoveVerticalInGrace
            ? new Vector2(0, GraceDirection)
            : new Vector2(GraceDirection, 0);

        Vector2 NextTile = (Vector2)transform.position + (PatrolDir * TileSize);

        if (IsPathClear(NextTile))
        {
            TargetPosition = NextTile;
            CurrentMoveDir = PatrolDir;
        }
        else
        {
            GraceDirection *= -1;
            CurrentMoveDir = Vector2.zero;
        }
    }

    /// <summary>
    /// Main chasing behavior with detours and ghosting fallback.
    /// </summary>
    private void MoveChaseBehavior()
    {
        // --- Ghosting override ---
        if (IsGhosting)
        {
            if (IsOverlappingGround())
            {
                ContinueGhostingPath();
                return;
            }
            else
            {
                IsGhosting = false;
                NextGhostAvailableTime = Time.time + GhostCooldown;
            }
        }

        Vector2 CurrentPos = transform.position;

        if (Vector2.Distance(CurrentPos, PlayerTarget.position) < (TileSize * 0.4f))
        {
            CurrentMoveDir = Vector2.zero;
            return;
        }

        float XDiff = PlayerTarget.position.x - CurrentPos.x;
        float YDiff = PlayerTarget.position.y - CurrentPos.y;

        // Determine primary and secondary directions
        bool preferX = Mathf.Abs(XDiff) > Mathf.Abs(YDiff);
        Vector2 PrimaryDir = preferX ? new Vector2(XDiff > 0 ? 1 : -1, 0) : new Vector2(0, YDiff > 0 ? 1 : -1);
        Vector2 SecondaryDir = preferX ? new Vector2(0, YDiff > 0 ? 1 : -1) : new Vector2(XDiff > 0 ? 1 : -1, 0);

        Vector2 NextTile = CurrentPos + (PrimaryDir * TileSize);
        bool IsLooping = (NextTile == PreviousLocation1 || NextTile == PreviousLocation2);

        // 1. Try Primary Direction (Direct Path)
        if (IsPathClear(NextTile) && !IsLooping)
        {
            TargetPosition = NextTile;
            CurrentMoveDir = PrimaryDir;
        }
        else
        {
            // 2. Primary is blocked. Try Secondary Direction (Detour/Cornering)
            Vector2 AltTile = CurrentPos + (SecondaryDir * TileSize);
            bool IsAltLooping = (AltTile == PreviousLocation1 || AltTile == PreviousLocation2);

            if (IsPathClear(AltTile) && !IsAltLooping)
            {
                TargetPosition = AltTile;
                CurrentMoveDir = SecondaryDir;
            }
            // 3. Both are blocked or looping. Try to Ghost.
            else if ((CheckTileForTag(NextTile, "Ground") || IsLooping) && Time.time >= NextGhostAvailableTime)
            {
                IsGhosting = true;
                TargetPosition = NextTile;
                CurrentMoveDir = PrimaryDir;
            }
            // 4. Force a direction change if stuck against a wall to prevent "vibrating"
            else
            {
                // If we are hitting a wall on X, try to move on Y regardless of player pos
                Vector2 fallbackDir = (PrimaryDir.x != 0) ? Vector2.up : Vector2.right;
                Vector2 fallbackTile = CurrentPos + (fallbackDir * TileSize);
                Vector2 fallbackTileRev = CurrentPos + (fallbackDir * -1 * TileSize);

                if (IsPathClear(fallbackTile)) TargetPosition = fallbackTile;
                else if (IsPathClear(fallbackTileRev)) TargetPosition = fallbackTileRev;
                
                CurrentMoveDir = Vector2.zero; // Animation idle while waiting for clear path
            }
        }
    }

    /// <summary>
    /// Continues movement while ghosting through tiles.
    /// </summary>
    private void ContinueGhostingPath()
    {
        float XDiff = PlayerTarget.position.x - transform.position.x;
        float YDiff = PlayerTarget.position.y - transform.position.y;

        Vector2 MoveDir = (Mathf.Abs(XDiff) > Mathf.Abs(YDiff))
            ? new Vector2(XDiff > 0 ? 1 : -1, 0)
            : new Vector2(0, YDiff > 0 ? 1 : -1);

        TargetPosition = (Vector2)transform.position + (MoveDir * TileSize);
        CurrentMoveDir = MoveDir;
    }

    /// <summary>
    /// Checks ahead to see if an alternate direction will lead around an obstacle.
    /// </summary>
    private bool IsDetourViable(Vector2 ScoutDir)
    {
        for (int i = 1; i <= LookAheadRange; i++)
        {
            Vector2 ScoutPos = (Vector2)transform.position + (ScoutDir * TileSize * i);
            if (CheckTileForTag(ScoutPos, "Obstacle")) return false;
            if (!CheckTileForTag(ScoutPos, "Ground")) return true;
        }
        return false;
    }

    #endregion


    #region === Collision & Path Checks ===

    /// <summary>
    /// Returns true if any collider at a position matches the given tag.
    /// </summary>
    private bool CheckTileForTag(Vector2 Pos, string TagName)
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(Pos, TileSize * 0.1f);
        foreach (var Hit in Hits)
            if (Hit.CompareTag(TagName)) return true;
        return false;
    }

    private bool IsOverlappingGround() =>
        CheckTileForTag(transform.position, "Ground");

    /// <summary>
    /// Determines if a tile can be moved into.
    /// </summary>
    private bool IsPathClear(Vector2 TargetPos)
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(TargetPos, 0.2f);

        foreach (var Hit in Hits)
        {
            if (Hit.gameObject == gameObject || Hit.CompareTag("Enemy")) continue;
            if (Hit.CompareTag("Obstacle") || (Hit.CompareTag("Ground") && !IsGhosting)) return false;
        }
        return true;
    }

    #endregion


    #region === Sprite Facing / Rotation ===

    /// <summary>
    /// Rotates and flips the sprite to match movement direction.
    /// </summary>
    private void FlipSprite()
    {
        if (CurrentMoveDir == Vector2.zero) return;

        if (CurrentMoveDir.x > 0)
        {
            IsFacingLeft = false;
            transform.localScale = new Vector2(1, 1);
            transform.localRotation = Quaternion.identity;
        }
        else if (CurrentMoveDir.x < 0)
        {
            IsFacingLeft = true;
            transform.localScale = new Vector2(-1, 1);
            transform.localRotation = Quaternion.identity;
        }

        if (CurrentMoveDir.y > 0)
        {
            IsFacingUp = true;
            IsFacingDown = false;
            transform.localRotation = IsFacingLeft ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);
        }
        else if (CurrentMoveDir.y < 0)
        {
            IsFacingUp = false;
            IsFacingDown = true;
            transform.localRotation = IsFacingLeft ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, -90);
        }

        if (CurrentMoveDir.x != 0)
        {
            IsFacingDown = false;
            IsFacingUp = false;
        }
    }

    #endregion


    #region === Puff ===

    /// <summary>
    /// Triggers the bubble attack if the cooldown is ready and attacking is allowed.
    /// </summary>
    public void PuffAttack()
    {
        // Check if enough time has passed and if attacking is currently allowed
        if (Time.time < nextFireTime || !CanAttack)
        {
            return;
        }

        // Set the next allowed fire time
        nextFireTime = Time.time + fireRate;

        // Only fire if required references are assigned
        if (BubblesPrefab != null && FirePoint != null)
        {
            Instantiate(BubblesPrefab, FirePoint.position, FirePoint.rotation);
        }

        MoveSpeed = 1f; // Slow Puff while attacking
        CanMove = false; // Prevent Puff from moving while attacking
        
        // add a delay here before allowing the player to move again
        Invoke("ResetPuffSpeed", .75f); // Reset Puff speed after seconds
    }

    /// <summary>
    /// Resets the Puff's movement speed to normal.
    /// </summary>
    private void ResetPuffSpeed()
    {
        MoveSpeed = 2.5f; // Reset Puff speed to normal value
        CanMove = true; // Allow Puff to move again
    }

    #endregion
}