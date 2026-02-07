using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehavior : MonoBehaviour
{
    // Variables

    // Float Variables
    private float MoveSpeed = 1f;
    private float PlayerXPosition;
    private float PlayerYPosition;
    private float Step;

    //GameObject Variables
    private GameObject PlayerObj = null;

    // Vector2 Variables
    private Vector2 Target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Refrences the Player for PlayerObj, to track location.
        if (PlayerObj == null)
        {
            PlayerObj = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Records player location for testing
        //Debug.Log("Player Position: X = " + PlayerObj.transform.position.x + " --- Y = " + PlayerObj.transform.position.y);

        // Moves enemy constantly towards Player position
        PlayerXPosition = PlayerObj.transform.position.x;
        PlayerYPosition = PlayerObj.transform.position.y;
        Target = new Vector2(PlayerXPosition, PlayerYPosition);
        Step = MoveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, Target, Step);
    }


    // Collision Test Code
    void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.name == "Lv1 Dirt Top")
        {
            Debug.Log("Touching Lv1 Dirt Top");
            if (PlayerYPosition >= 0)
            {
                //transform.position = Vector2.MoveTowards(transform.position, Target, Step);
                Debug.Log("Above");
            } 
            else if (PlayerYPosition < 0)
            {
                Debug.Log("Below");
            }
        } 
        else if (Collision.gameObject.name == "Lv1 Dirt")
        {
            Debug.Log("Touching Lv1 Dirt");
        } 
        else if (Collision.gameObject.name == "Lv2 Dirt")
        {
            Debug.Log("Touching Lv2 Dirt");
        }
        else if (Collision.gameObject.name == "Lv3 Dirt")
        {
            Debug.Log("Touching Lv3 Dirt");
        }

    }

}
