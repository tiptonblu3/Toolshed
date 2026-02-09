using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Health = 1;
    public int Lives = 3;
    public bool IsDead = false;
    public bool GameOver = false;
    public bool IsInvincible = false; //For Cheat Mode
    public int Score = 0;
    public int HighScore;
    public static PlayerStats Instance { get; private set; }

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        // Reset player stats when enabled
        Health = 1;
        IsDead = false;
        IsInvincible = false;
    }

    void CheatModeToggle()
    {
        if (IsInvincible == true)
        {
            IsInvincible = false;
            Debug.Log("Cheat Mode Deactivated: Player is now vulnerable.");
            Health = 1; // Reset health to normal when cheat mode is turned off
        }
        else
        {
            IsInvincible = true;
            Debug.Log("Cheat Mode Activated: Player is now invincible.");
            Health = 999; // Set health to a high value when cheat mode is turned on
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Example collision handling
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Health -= 1; // Decrease health by 1 for each collision with an enemy
            if (Health <= 0)
            {
                Lives--;
                if (Lives <= 0)
                {
                    IsDead = true;
                    GameOver = true;
                    // Handle player death (e.g., trigger game over)
                    HighScore=Score;
                    Score=0;
                }
                else
                {
                    IsDead = true;


                }
            }
        }
    }

    
}
