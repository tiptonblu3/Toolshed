using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    #region === Inspector References ===

    [SerializeField] private Animator PlayerAnimator;

    [Header("Settings")]
    public GameObject FootballPrefab;   // Projectile to spawn when attacking
    public Transform FirePoint;         // Where the projectile spawns from
    public bool CanAttack = true;          // Whether the player is currently allowed to attack (e.g., not while digging)

    [Header("Input Reference")]
    // Named differently from the Fire() method to avoid confusion
    public InputActionReference FireAction;
    public PlayerMovement MoveSpeedScript;   // Reference to movement so we can slow the player while attacking

    #endregion


    #region === Unity Lifecycle ===

    public void Start()
    {
        // Cache the PlayerMovement script on the same object
        MoveSpeedScript = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        // Subscribe to the input action when this object becomes active
        FireAction.action.performed += Fire;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent duplicate calls or memory leaks
        FireAction.action.performed -= Fire;
    }

    #endregion


    #region === Attack Logic ===

    /// <summary>
    /// Called when the fire input action is performed.
    /// Spawns a football projectile at the fire point.
    /// </summary>
    public void Fire(InputAction.CallbackContext context)
    {
        // Only fire if required references are assigned
        if (FootballPrefab != null && FirePoint != null)
        {
            Instantiate(FootballPrefab, FirePoint.position, FirePoint.rotation);
        }

        PlayerAnimator.SetBool("IsThrowing", true);

        MoveSpeedScript.MoveInput = Vector2.zero;
        MoveSpeedScript.MoveSpeed = 1f; // Slow player while attacking
        MoveSpeedScript.CanMove = false; // Prevent player from moving while attacking
        // add a delay here before allowing the player to move again
        Invoke("ResetPlayerSpeed", .75f); // Reset player speed after seconds

    }

    /// <summary>
    /// Resets the player's movement speed to normal.
    /// </summary>
    private void ResetPlayerSpeed()
    {
        MoveSpeedScript.MoveSpeed = 3f; // Reset player speed to normal value
        MoveSpeedScript.CanMove = true; // Allow player to move again

        PlayerAnimator.SetBool("IsThrowing", false);
    }

    #endregion
}
