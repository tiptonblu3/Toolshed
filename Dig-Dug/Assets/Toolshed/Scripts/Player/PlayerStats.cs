using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private Animator PlayerAnimator;
    public int Health = 1;
    public int Lives;
    public bool IsDead = false;
    public bool GameOver = false;
    public bool IsInvincible = false; //For Cheat Mode
    public int Score = 0;
    public int HighScore;
    public static PlayerStats Instance { get; private set; }

    public int SaveScoreKey; // Key for saving score in PlayerPrefs
    public int LivesData; // Variable to hold the number of lives for saving/loading


    void LoadSceneByName(string Level1)
    {
        SceneManager.LoadScene(Level1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
                Lives = PlayerPrefs.GetInt("LivesNum", 3);
                PlayerPrefs.Save(); //update lives in PlayerPrefs
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
        PlayerAnimator.SetBool("IsDead", false);
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

 public void ReloadCurrentScene()
    {
        // Gets the name of the active scene and loads it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("Collision Detected");
        
        // Example collision handling
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Puff"))
        {
            Health -= 1; // Decrease health by 1 for each collision with an enemy
            if (Health <= 0)
            {
                Lives--;
                PlayerPrefs.SetInt("LivesNum", Lives); // Update lives in PlayerPrefs
                PlayerPrefs.Save(); // Save the updated lives to disk

                if (Lives <= 0)
                {
                    IsDead = true;
                    GameOver = true;
                    // Handle player death (e.g., trigger game over)
                    PlayerAnimator.SetBool("IsDead", true);
                    PlayerPrefs.SetInt("LivesNum", 3); // Update PlayerPrefs with reset lives
                    LoadSceneByName("GameOver");
                }
                else
                {
                    IsDead = true;
                    PlayerAnimator.SetBool("IsDead", true);
                    ReloadCurrentScene();


                }
            }
        }
    }

// if (PlayerPrefs.GetInt("Highscore", 0) <= PlayerPrefs.GetInt("PScore", 0))
//         {
//             HighScore = PlayerPrefs.GetInt("PScore", 0);

//             PlayerPrefs.SetInt("Highscore", HighScore);
//             PlayerPrefs.Save(); // Save the new high score to disk
//         }
    
}