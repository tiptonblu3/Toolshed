using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Settings")]
    public GameObject FootballPrefab;
    public Transform FirePoint; // Best practice: fire from a specific spot
    
    [Header("Input Reference")]
    // Renamed this so it doesn't clash with the method name 'Fire'
    public InputActionReference FireAction; 

    private void OnEnable()
    {
        // Subscribe to the performed event
        FireAction.action.performed += Fire;
    }

    private void OnDisable()
    {
        // Unsubscribe from the performed event
        FireAction.action.performed -= Fire;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        // Instantiate logic
        if (FootballPrefab != null && FirePoint != null)
        {
            Instantiate(FootballPrefab, FirePoint.position, FirePoint.rotation);
        }
    }
}