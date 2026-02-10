using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerQuit : MonoBehaviour
{
    public InputActionReference QuitAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // Subscribe to the input action when this object becomes active
        QuitAction.action.performed += Quit;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent duplicate calls or memory leaks
        QuitAction.action.performed -= Quit;
    }

    public void Quit(InputAction.CallbackContext context)
    {
        // Handle the quit action
        Application.Quit();
        Debug.Log("Game is quitting");
    }
}
