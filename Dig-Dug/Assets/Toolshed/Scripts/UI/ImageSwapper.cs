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
        if (lives == 3)
            displayImage.sprite = LivesThree;
        else if (lives == 2)
            displayImage.sprite = LivesTwo;
        else if (lives == 1)
            displayImage.sprite = LivesOne;
        else if (lives <= 0)
            displayImage.enabled = false; // Hide if dead
            
        Debug.Log("UI Updated to lives: " + lives);
    }
}