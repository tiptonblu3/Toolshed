using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject FootballPrefab;
    public GameObject FireInput;

    void Fire(InputAction.CallbackContext context)
    {
        FireInput = (Instantiate(FootballPrefab, transform.position, transform.rotation));
        
    }
}
