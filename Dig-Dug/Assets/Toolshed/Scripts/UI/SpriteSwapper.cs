using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public Sprite LivesOne;
    public Sprite LivesTwo;
    public Sprite LivesThree;
    public SpriteRenderer spriteRenderer;
    
    private PlayerStats playerStats; // Keep a reference to the stats
    private int lastLivesValue = -1; // Tracks changes so we don't update every single frame

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Find the player once at the start
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
            // Only update the sprite if the lives count has actually changed
            if (playerStats.Lives != lastLivesValue)
            {
                lastLivesValue = playerStats.Lives;
                UpdateSprite(lastLivesValue);
            }
        }
    }

    void UpdateSprite(int lives)
    {
        if (lives == 3)
            spriteRenderer.sprite = LivesThree;
        else if (lives == 2)
            spriteRenderer.sprite = LivesTwo;
        else if (lives == 1)
            spriteRenderer.sprite = LivesOne;
        else if (lives <= 0)
            spriteRenderer.sprite = null; // Clear the sprite if dead
            
        Debug.Log("Displaying sprite for lives: " + lives);
    }
}