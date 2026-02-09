using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehavior : MonoBehaviour
{
    #region === Variables ===

    // Variables

    // Serialized Fields
    [SerializeField] private Animator EnemyAnimator;
    [SerializeField] private LayerMask ObstacleLayer; // What tiles count as walls/blocked

    // Booleans
    public bool CanMove = true;          // Whether the enemy can currently move (e.g., not while attacking)
    public bool IsMoving;
    public bool IsFacingLeft = false;
    public bool IsFacingUp = false;
    public bool IsFacingDown = false;
    public bool IsPuff;                 // Uses to determine whether the enemy is Puff or Citron
                                        // That way, we can control which enemies use the breath attack

    // Float Variables
    public float MoveSpeed = 1f;
    public float TileSize = 1f;       // Size of one grid step
    public float EnemyXPosition;
    public float EnemyYPosition;
    public float PlayerXPosition;
    public float PlayerYPosition;
    public float GhostChance = 0.10f; // % Chance for Enemies to Ghost.
                                      // Make sure it's low, enemies are gonna smash their heads against walls A LOT

    //GameObject Variables
    public GameObject PlayerObj = null;

    // Rigidbody2D
    public Rigidbody2D Rb;

    // Vector2 Variables
    public Vector2 MoveInput;        // Current snapped directional input
    public Vector2 TargetPosition;   // The grid position we are moving toward
    public Vector2 nextTile;         // Butt ugly hack to get this to work.

    #endregion

    #region === Unity Start ===

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache required components
        Rb = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();

        // Refrences the Player for PlayerObj, to track location.
        if (PlayerObj == null)
        {
            PlayerObj = GameObject.Find("Player");
        }

        // Ensure we start aligned perfectly to the grid
        TargetPosition = transform.position;

        DetectEnemyType();
    }

    #endregion

    #region === Unity Update ===

    // Update is called once per frame
    void Update()
    {
        DetectPlayerLocation();
    }

    #endregion

    // Keep this next region closed if you don't want to see a mess of code...
    #region === Detect Player Location ===

    private void DetectPlayerLocation()
    {
        // All of this calculates where the current enemy is and where the player is.
        EnemyXPosition = this.gameObject.transform.position.x;
        EnemyYPosition = this.gameObject.transform.position.y;
        PlayerXPosition = PlayerObj.transform.position.x;
        PlayerYPosition = PlayerObj.transform.position.y;
        //Debug.Log("Enemy Position: X = " + EnemyXPosition + " --- Y = " + EnemyYPosition);
        //Debug.Log("Player Position: X = " + PlayerXPosition + " --- Y = " + PlayerYPosition);

        if (PlayerXPosition < EnemyXPosition && PlayerYPosition < EnemyYPosition) // Player is to the bottom left
        {
            Debug.Log("Player is at the Bottom Left!");
            MoveInput = new Vector2(0, -1);
            EnemyMove();
            if (!IsPathClear(nextTile))
            {
                MoveInput = new Vector2(-1, 0);
                EnemyMove();
                if (!IsPathClear(nextTile))
                {
                    MoveInput = new Vector2(1, 0);
                    EnemyMove();
                    if (!IsPathClear(nextTile))
                    {
                        MoveInput = new Vector2(0, 1);
                        EnemyMove();
                    }
                }
            }
        }

        if (PlayerXPosition > EnemyXPosition && PlayerYPosition > EnemyYPosition) // Player is to the top right
        {
            Debug.Log("Player is at the Top Right!");
            MoveInput = new Vector2(0, 1);
            EnemyMove();
            if (!IsPathClear(nextTile))
            {
                MoveInput = new Vector2(1, 0);
                EnemyMove();
                if (!IsPathClear(nextTile))
                {
                    MoveInput = new Vector2(-1, 0);
                    EnemyMove();
                    if (!IsPathClear(nextTile))
                    {
                        MoveInput = new Vector2(0, -1);
                        EnemyMove();
                    }
                }
            }
        }

        if (PlayerXPosition < EnemyXPosition && PlayerYPosition > EnemyYPosition) // Player is to the top left
        {
            Debug.Log("Player is at the Top Left!");
            MoveInput = new Vector2(0, 1);
            EnemyMove();
            if (!IsPathClear(nextTile))
            {
                MoveInput = new Vector2(-1, 0);
                EnemyMove();
                if (!IsPathClear(nextTile))
                {
                    MoveInput = new Vector2(1, 0);
                    EnemyMove();
                    if (!IsPathClear(nextTile))
                    {
                        MoveInput = new Vector2(0, -1);
                        EnemyMove();
                    }
                }
            }
        }

        if (PlayerXPosition > EnemyXPosition && PlayerYPosition < EnemyYPosition) // Player is to the bottom right
        {
            Debug.Log("Player is at the Bottom Right!");
            MoveInput = new Vector2(0, -1);
            EnemyMove();
            if (!IsPathClear(nextTile))
            {
                MoveInput = new Vector2(1, 0);
                EnemyMove();
                if (!IsPathClear(nextTile))
                {
                    MoveInput = new Vector2(-1, 0);
                    EnemyMove();
                    if (!IsPathClear(nextTile))
                    {
                        MoveInput = new Vector2(0, -1);
                        EnemyMove();
                    }
                }
            }
        }

        if (PlayerXPosition == EnemyXPosition && PlayerYPosition < EnemyYPosition) // Player is directly below
        {
            Debug.Log("Player is Below!");
            MoveInput = new Vector2(0, -1);
            EnemyMove();
            if (!IsPathClear(nextTile))
            {
                MoveInput = new Vector2(1, 0);
                EnemyMove();
                if (!IsPathClear(nextTile))
                {
                    MoveInput = new Vector2(-1, 0);
                    EnemyMove();
                    if (!IsPathClear(nextTile))
                    {
                        MoveInput = new Vector2(0, 1);
                        EnemyMove();
                    }
                }
            }
        }

        if (PlayerXPosition == EnemyXPosition && PlayerYPosition > EnemyYPosition) // Player is directly above
        {
            Debug.Log("Player is Above!");
            MoveInput = new Vector2(0, 1);
            EnemyMove();
            if (!IsPathClear(nextTile))
            {
                MoveInput = new Vector2(1, 0);
                EnemyMove();
                if (!IsPathClear(nextTile))
                {
                    MoveInput = new Vector2(-1, 0);
                    EnemyMove();
                    if (!IsPathClear(nextTile))
                    {
                        MoveInput = new Vector2(0, -1);
                        EnemyMove();
                    }
                }
            }
        }

        if (PlayerXPosition > EnemyXPosition && PlayerYPosition == EnemyYPosition) // Player is directly to the right
        {
            Debug.Log("Player is to the Right!");
            MoveInput = new Vector2(1, 0);
            EnemyMove();
            if (!IsPathClear(nextTile))
            {
                MoveInput = new Vector2(0, 1);
                EnemyMove();
                if (!IsPathClear(nextTile))
                {
                    MoveInput = new Vector2(0, -1);
                    EnemyMove();
                    if (!IsPathClear(nextTile))
                    {
                        MoveInput = new Vector2(-1, 0);
                        EnemyMove();
                    }
                }
            }
        }

        if (PlayerXPosition < EnemyXPosition && PlayerYPosition == EnemyYPosition) // Player is directly to the left
        {
            Debug.Log("Player is to the Left!");
            MoveInput = new Vector2(-1, 0);
            EnemyMove();
            if (!IsPathClear(nextTile))
            {
                MoveInput = new Vector2(0, 1);
                EnemyMove();
                if (!IsPathClear(nextTile))
                {
                    MoveInput = new Vector2(0, -1);
                    EnemyMove();
                    if (!IsPathClear(nextTile))
                    {
                        MoveInput = new Vector2(1, 0);
                        EnemyMove();
                    }
                }
            }
        }
    }

    #endregion

    #region === Enemy Movement ===

    private void EnemyMove()
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

                // Only move if the path is not blocked, then randomly see if the enemy will 'ghost'
                if (IsPathClear(nextTile))
                {
                    TargetPosition = nextTile;
                } else
                {
                   if (Random.value < GhostChance)
                    {
                        GhostingAbility();
                        Debug.Log("Enemy Ghosted!");
                    } else
                    {
                        Debug.Log("Enemy did not Ghost");
                    }
                }
            }
        }
    }

    // Checks if a tile position is free of objects tagged "Obstacle".
    private bool IsPathClear(Vector2 TargetPos)
    {
        // 1. Find any collider at the target position
        Collider2D hit = Physics2D.OverlapCircle(TargetPos, 0.2f);

        // 2. If we hit nothing, the path is clear
        if (hit == null) return true;

        // 3. If we hit ourselves (another enemy), the path is still technically clear
        if (hit.gameObject == gameObject) return true;

        // 4. If we hit something tagged "Obstacle", the path is blocked
        if (hit.CompareTag("Obstacle"))
        {
            Debug.Log("Path blocked by an Obstacle tag!");
            return false;
        }
        
        if (hit.CompareTag("Ground"))
        {
            Debug.Log("Path blocked by a Ground tag!");
            return false;
        }

        // 5. If we hit the Player, the path is also technically clear, we just need to kill the player
        if (hit.gameObject == PlayerObj)
        {
            return true;
        }

        // Default to clear if it's just a background tile or something without the tag
        return true;
    }

    #endregion

    #region === Enemy Damage ===

    private void EnemyDamage()
    {

    }

    #endregion

    #region === Ghosting Ability ===

    private void GhostingAbility()
    {

    }

    #endregion

    #region === Detect Enemy Type ===

    private void DetectEnemyType()
    {

    }

    #endregion
}