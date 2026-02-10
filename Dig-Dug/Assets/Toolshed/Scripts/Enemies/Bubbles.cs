using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Bubbles : MonoBehaviour
{
    #region === Inspector Settings ===

    [Header("Settings")]
    public float Speed = 10f;          // How fast the Bubbles travels
    public float MaxDistance = 3f;     // How far it can travel before despawning
    public int Damage = 1;             // Damage to apply on hit (handled elsewhere)
    public PlayerStats Score;
    private PlayerStats Stats;
    public Transform Puff;            // Reference to the Puff transform

    public int SaveScoreKey; // Key for saving score in PlayerPrefs

    #endregion


    #region === Runtime State ===

    private Vector2 StartPosition;     // Where the Bubbles was spawned
    public Transform Player;           // Reference to the player transform

    #endregion


    #region === Unity Lifecycle ===

    void Start()
    {
        InitializeStartPosition();
        FindPuff();
        FindPlayer();
        LaunchInPuffFacingDirection();
        StatsInfo();
    }

    void Update()
    {
        CheckMaxTravelDistance();
    }

    #endregion


    #region === Initialization ===

    /// <summary>
    /// Records the spawn position so we know how far the Bubbles has traveled.
    /// </summary>
    private void InitializeStartPosition()
    {
        StartPosition = transform.position;
    }

    /// <summary>
    /// Attempts to locate the player in the scene using the "Player" tag.
    /// </summary>
    private void FindPuff()
    {
        GameObject PuffObj = GameObject.FindWithTag("Puff");
        

        if (PuffObj != null)
        {
            Puff = PuffObj.transform;
        }
        else
        {
            Debug.LogWarning("Bubbles could not find a Puff object!");
        }
    }
    private void FindPlayer()
    {
        GameObject PlayerObj = GameObject.FindWithTag("Player");

        if (PlayerObj != null)
        {
            Player = PlayerObj.transform;
        }
        else
        {
            Debug.LogWarning("Bubbles could not find a Player object!");
        }
    }

    public void StatsInfo()
    {
        // Initialize local score counter (player's persistent stats should be handled on the PlayerStats component)
        Stats = Player.GetComponent<PlayerStats>();
    }

    /// <summary>
    /// Sets the Bubbles's velocity based on the direction the player is facing.
    /// </summary>
    private void LaunchInPuffFacingDirection()
    {
        GameObject PuffObj = GameObject.FindWithTag("Puff");
        Enemy2 Puff = PuffObj.GetComponent<Enemy2>();
        

        bool IsFacingDown = Puff.IsFacingDown;
        bool IsFacingUp = Puff.IsFacingUp;
        bool IsFacingLeft = Puff.IsFacingLeft;
        bool IsFacingRight = !IsFacingLeft;

        if (Player != null)
        {
            // Facing Down
            if (IsFacingDown == true)
            {
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * Speed;
                Debug.Log("Bubbles is moving down");
            }

            // Facing Up
            if (IsFacingUp == true)
            {
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * Speed;
                Debug.Log("Bubbles is moving up");
            }

            // Facing Right
            if (IsFacingRight == true && IsFacingLeft == false && IsFacingUp == false && IsFacingDown == false)
            {
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * Speed;
                Debug.Log("Bubbles is moving right");
            }

            // Facing Left
            if (IsFacingLeft == true && IsFacingRight == false && IsFacingUp == false && IsFacingDown == false)
            {
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * Speed;
                Debug.Log("Bubbles is moving left");
            }
        }
    }

    #endregion


    #region === Lifetime Control ===

    /// <summary>
    /// Destroys the Bubbles if it has exceeded its maximum allowed travel distance.
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
            Debug.Log("Bubbles hit the ground");
            Destroy(gameObject);
        }

        // Hit an enemy — damage would be applied in the enemy script
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Bubbles hit the player");
            Stats.Health -= Damage; // Apply damage to player's health
            Destroy(gameObject);
            
        }
    }

    #endregion
}
