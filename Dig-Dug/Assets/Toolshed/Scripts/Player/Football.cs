using UnityEngine;

public class Football : MonoBehaviour
{
    [Header("Settings")]
    public float Speed = 10f;
    public float MaxDistance = 3f;
    public int Damage = 1;
    private Vector2 StartPosition;
    public Transform Player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Record the starting position
        StartPosition = transform.position;

        // Automatically find the player by tag
        GameObject PlayerObj = GameObject.FindWithTag("Player");
        PlayerMovement playerMovement = PlayerObj.GetComponent<PlayerMovement>();
        bool IsFacingDown = playerMovement.IsFacingDown;
        bool IsFacingUp = playerMovement.IsFacingUp;
        bool IsFacingLeft = playerMovement.IsFacingLeft;
        bool IsFacingRight = !IsFacingLeft;


       
        if (PlayerObj != null)
        {
            Player = PlayerObj.transform;
        }
        else
        {
            Debug.LogWarning("Football could not find a Player object!");
        }

        // Set the football's initial velocity to move in the direction the player is facing
        if (Player != null)
        {
            if (IsFacingDown == true)
            {
                // Facing down
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * Speed;
                Debug.Log("Football is moving down");
            }
            if (IsFacingUp == true)
            {
                // Facing up
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * Speed;
                Debug.Log("Football is moving up");
            }

            if (IsFacingRight == true && IsFacingLeft == false && IsFacingUp == false && IsFacingDown == false)
            {
                // Facing right
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * Speed;
                Debug.Log("Football is moving right");
            }
            if (IsFacingLeft == true && IsFacingRight == false && IsFacingUp == false && IsFacingDown == false)
            {
                // Facing left
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * Speed;
                Debug.Log("Football is moving left");
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the football has traveled beyond its maximum distance
        if (Vector2.Distance(StartPosition, transform.position) >= MaxDistance)
        {
            Destroy(gameObject); // Destroy the football
        }
    }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ground"))
            {
                Debug.Log("Football hit the ground");
                Destroy(gameObject);
            }

            // Check if the football collides with an enemy
            if (collision.CompareTag("Enemy"))
            {
                // Apply damage to the enemy (you can implement this in your enemy script)
                // For example, if your enemy has a method called TakeDamage(int damage):
    
                Destroy(gameObject); // Destroy the football after hitting an enemy
            }
        }
}
