using UnityEngine;

public class Football : MonoBehaviour
{
    [Header("Settings")]
    public float Speed = 10f;
    public float MaxDistance = 3f;
    public int Damage = 1;
    private Vector2 StartPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Record the starting position
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the football forward
        transform.Translate(Vector2.right * Speed * Time.deltaTime);

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
                // Here you would typically access the enemy's health component and apply damage
                // For example:
                // EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
                // if (enemyHealth != null)
                // {
                //     enemyHealth.TakeDamage(Damage);
                // }
    
                Destroy(gameObject); // Destroy the football after hitting an enemy
            }
        }
}
