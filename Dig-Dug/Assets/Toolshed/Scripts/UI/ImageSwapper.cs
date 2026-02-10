using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class LiveManager : MonoBehaviour
{
    public Sprite LivesOne;
    public Sprite LivesTwo;
    public Sprite LivesThree;
    
    // Change this from SpriteRenderer to Image
    public Image displayImage; 
    
    private PlayerStats playerStats;
    private int lastLivesValue = -1;
    private int LivesData; // Variable to hold the number of lives for changing life sprite

    void Start()
    {
        // If you didn't drag the Image into the slot, it tries to find it on this object
        if (displayImage == null)
            displayImage = GetComponent<Image>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerStats = playerObj.GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        if (playerStats != null)
        {
            if (playerStats.Lives != lastLivesValue)
            {
                lastLivesValue = playerStats.Lives;
                UpdateUI(lastLivesValue);
            }
        }
    }

    void UpdateUI(int lives)
    {
        // We use .sprite to change the image's source
        PlayerPrefs.GetInt("LivesNum", 0); // Load lives from PlayerPrefs, defaulting to 0 if "LivesNum" doesn't exist
        LivesData = PlayerPrefs.GetInt("LivesNum", 0); // Update LivesData with the loaded value

        if (LivesData == 3)
            displayImage.sprite = LivesThree;
        else if (LivesData == 2)
            displayImage.sprite = LivesTwo;
        else if (LivesData == 1)
            displayImage.sprite = LivesOne;
        else if (LivesData <= 0)
            displayImage.enabled = false; // Hide if dead
            
        Debug.Log("UI Updated to lives: " + LivesData);
    }
}

// PlayerPrefs.SetInt("LivesNum", Lives);
//                 PlayerPrefs.Save(); //update lives in PlayerPrefs
//                 int LivesData = PlayerPrefs.GetInt("LivesNum", Lives); // Equal for purposes


//                 if (LivesData <= 0)
//                 {
//                     IsDead = true;
//                     GameOver = true;
//                     // Handle player death (e.g., trigger game over)
//                     PlayerAnimator.SetBool("IsDead", true);
//                     Lives = 3; // Reset lives for next game
//                     PlayerPrefs.SetInt("LivesNum", Lives); // Update PlayerPrefs with reset lives
//                     LoadSceneByName("GameOver");