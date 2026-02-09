using UnityEngine;
using System.Threading;

public class EnemyBehaviorWIP : MonoBehaviour
{
    public float MoveSpeed = 2.5f;
    public float TileSize = 1.0f; // Size of one grid step
   // public float MoveDistance = 1.0f; //how much he moves per step
    
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Rigidbody2D rb;

// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Follow game object with player tag on it
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        //check enemy location to compare to player location
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //if there is no player target, return
        if (playerTarget == null) return;
        
        //store current position as a vector2 to use for movement towards player
        Vector2 currentPosition = rb.position;
        //store player position as vector 2 to use for movement as enemy
        Vector2 targetPosition = playerTarget.position;
        //calculate direction to move towards player
        Vector2 moveDirection = Vector2.zero;
        //Calculate difference in position between enemy and player
        float xDiff = targetPosition.x - currentPosition.x;
        float yDiff = targetPosition.y - currentPosition.y;
        
        //decice whether to move horizontally or vertically based on which distance is lesser
        if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
        {
            
                // Move horizontally towards player
                moveDirection = new Vector2(Mathf.Sign(xDiff), 0).normalized;
            
        }
        else
        {
            
            // Move vertically towards player
            moveDirection = new Vector2(0, Mathf.Sign(yDiff)).normalized;
        }

        // Use Rigidbody2D.MovePosition for smooth, physics-based movement
        Vector2 newPosition = currentPosition + moveDirection * MoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Cast a ray forward
    //RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1.0f, Ground);

    }
                //Vector2 nextTile = TargetPosition + (MoveInput * TileSize);
 





    // Update is called once per frame
    void Update()
    {
        
    }
}
