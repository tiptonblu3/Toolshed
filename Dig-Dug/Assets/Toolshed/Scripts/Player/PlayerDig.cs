using UnityEngine;

public class PlayerDig : MonoBehaviour
{
    public Collider2D CurrentTile;
    public float DigTime = 1f;
    public float DigTimer;
    public bool IsDigging;
    
    public PlayerMovement MoveSpeedScript;

    void Start()
    {
        MoveSpeedScript = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if (IsDigging && CurrentTile != null)
        {
            DigTimer += Time.fixedDeltaTime;

            if (DigTimer >= DigTime)
            {
                DestroyCurrentTile();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Ground"))
        {
            CurrentTile = Other;
            IsDigging = true;
            DigTimer = 0f;

            if (MoveSpeedScript != null)
            {
                MoveSpeedScript.MoveSpeed = 1f;
            }
            
            Debug.Log("Started Digging...");
        }
    }

    private void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.CompareTag("Ground"))
        {
            StopDigging();
        }
    }

    public void DestroyCurrentTile()
    {
        if (CurrentTile != null)
        {
            Debug.Log("Tile Destroyed!");
            Destroy(CurrentTile.gameObject);
            StopDigging();
        }
    }

    public void StopDigging()
    {
        IsDigging = false;
        DigTimer = 0f;
        CurrentTile = null; 

        if (MoveSpeedScript != null)
        {
            MoveSpeedScript.MoveSpeed = 3f;
        }
    }
}