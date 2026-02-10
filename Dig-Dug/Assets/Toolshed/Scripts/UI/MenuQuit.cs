using UnityEngine;
using UnityEngine.InputSystem; // Import the Input System
#if UNITY_EDITOR
using UnityEditor; // Import Editor namespace for quitting in editor
#endif

public class QuitManager : MonoBehaviour
{
    void Update()
    {
        // Detect if Escape key is pressed
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Requested"); // Log to console for testing

#if UNITY_EDITOR
        // Stop playing in the Unity Editor
        EditorApplication.isPlaying = false;
#else
        // Quit the built application
        Application.Quit();
#endif
    }
}
