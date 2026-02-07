using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    #region === Inspector References ===

    [Header("Settings")]
    public GameObject FootballPrefab;   // Projectile to spawn when attacking
    public Transform FirePoint;         // Where the projectile spawns from

    [Header("Input Reference")]
    // Named differently from the Fire() method to avoid confusion
    public InputActionReference FireAction;

    #endregion


    #region === Unity Lifecycle ===

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
    }

    #endregion
}
