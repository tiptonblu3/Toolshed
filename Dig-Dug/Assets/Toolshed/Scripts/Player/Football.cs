using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Football : MonoBehaviour
{
    #region === Inspector Settings ===

    [Header("Settings")]
    public float Speed = 10f;          // How fast the football travels
    public float MaxDistance = 3f;     // How far it can travel before despawning
    public int Damage = 1;             // Damage to apply on hit (handled elsewhere)
    public PlayerStats Score;
    private PlayerStats Stats;

    public int SaveScoreKey; // Key for saving score in PlayerPrefs

    #endregion


    #region === Runtime State ===

    private Vector2 StartPosition;     // Where the football was spawned
    public Transform Player;           // Reference to the player transform

    #endregion


    #region === Unity Lifecycle ===

    void Start()
    {
        InitializeStartPosition();
        FindPlayer();
        LaunchInPlayerFacingDirection();
        ScoreInfo();
    }

    void Update()
    {
        CheckMaxTravelDistance();
    }

    #endregion


    #region === Initialization ===

    /// <summary>
    /// Records the spawn position so we know how far the football has traveled.
    /// </summary>
    private void InitializeStartPosition()
    {
        StartPosition = transform.position;
    }

    /// <summary>
    /// Attempts to locate the player in the scene using the "Player" tag.
    /// </summary>
    private void FindPlayer()
    {
        GameObject PlayerObj = GameObject.FindWithTag("Player");

        if (PlayerObj != null)
        {
            Player = PlayerObj.transform;
        }
        else
        {
            Debug.LogWarning("Football could not find a Player object!");
        }
    }

    public void ScoreInfo()
    {
        // Initialize local score counter (player's persistent stats should be handled on the PlayerStats component)
        Stats = Player.GetComponent<PlayerStats>();
    }

    /// <summary>
    /// Sets the football's velocity based on the direction the player is facing.
    /// </summary>
    private void LaunchInPlayerFacingDirection()
    {
        GameObject PlayerObj = GameObject.FindWithTag("Player");
        PlayerMovement playerMovement = PlayerObj.GetComponent<PlayerMovement>();
        

        bool IsFacingDown = playerMovement.IsFacingDown;
        bool IsFacingUp = playerMovement.IsFacingUp;
        bool IsFacingLeft = playerMovement.IsFacingLeft;
        bool IsFacingRight = !IsFacingLeft;

        if (Player != null)
        {
            // Facing Down
            if (IsFacingDown == true)
            {
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * Speed;
                Debug.Log("Football is moving down");
            }

            // Facing Up
            if (IsFacingUp == true)
            {
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * Speed;
                Debug.Log("Football is moving up");
            }

            // Facing Right
            if (IsFacingRight == true && IsFacingLeft == false && IsFacingUp == false && IsFacingDown == false)
            {
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * Speed;
                Debug.Log("Football is moving right");
            }

            // Facing Left
            if (IsFacingLeft == true && IsFacingRight == false && IsFacingUp == false && IsFacingDown == false)
            {
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * Speed;
                Debug.Log("Football is moving left");
            }
        }
    }

    #endregion


    #region === Lifetime Control ===

    /// <summary>
    /// Destroys the football if it has exceeded its maximum allowed travel distance.
    /// </summary>
    private void CheckMaxTravelDistance()
    {
        if (Vector2.Distance(StartPosition, transform.position) >= MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    #endregion


    #region === Collision Handling ===

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit the ground — just despawn
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Football hit the ground");
            Destroy(gameObject);
        }

        // Hit an enemy — damage would be applied in the enemy script
        if (collision.CompareTag("Enemy") || collision.CompareTag("Puff"))
        {
            int currentTotal = PlayerPrefs.GetInt("PScore", 0);
            int newTotal = currentTotal + 100;
            Debug.LogWarning("Football hit an enemy, score increased by 100! New Score = " + newTotal);
            
            // Save the score with a key name "PlayerScore"
            PlayerPrefs.SetInt("PScore", newTotal);
            // Highly recommended: Force a save to disk immediately
            PlayerPrefs.Save();

            Destroy(gameObject);
            Destroy(collision.gameObject); // This destroys the enemy on hit, can be removed if you want to handle enemy health separately
        }
    }

    #endregion
}
