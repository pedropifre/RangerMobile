using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    // Set the speed at which the enemy moves
    public float speed = 2f;

    // Set the time interval at which the enemy changes direction
    public float changeInterval = 2f;

    // Store the current direction of the enemy
    private Vector2 direction;

    [Header("Still")]
    [SerializeField]private bool isWalking = true;
    public float standingWalkingTime;
    public float standingStillTime;
    


    void Start()
    {
        // Set the initial direction of the enemy
        direction = RandomDirection();

        // Invoke the ChangeDirection function at the specified interval
        //InvokeRepeating("StandStill", standingStillInterval, standingStillInterval);
    }

    void Update()
    {
        Debug.Log(direction);

        if (isWalking)
        {

            //Debug.Log("Width = " + Screen.width);
            //Debug.Log("Height = " + Screen.height);
            // Update the position of the enemy based on the current direction
            transform.position = transform.position + (Vector3)(direction * speed * Time.deltaTime);

            // Get the width and height of the screen in world units
            float screenHeight = (Camera.main.orthographicSize * 2)*.8f ;
            float screenWidth = (screenHeight * Camera.main.aspect)*.8f;

            // Check if the enemy has moved outside the boundaries of the screen
            if (transform.position.x < -screenWidth / 2 || transform.position.x > screenWidth / 2 ||
                transform.position.y < -screenHeight / 4 || transform.position.y > screenHeight / 1.8)
            {
                // If so, change the direction of the enemy
                ChangeDirection();
            }
        }
        else
        {
            //StartCoroutine(StandStill());
        }
    }

    IEnumerator StandStill()
    {
        isWalking = true;
        yield return new WaitForSeconds(standingStillTime);
        isWalking = false;
        yield return new WaitForSeconds(standingWalkingTime);
        isWalking = true;
    }

    // Function to generate a random direction for the enemy
    Vector2 RandomDirection()
    {
        // Generate a random angle in degrees
        float angle = Random.Range(0f, 360f);

        // Convert the angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate the x and y components of the direction vector using trigonometry
        float x = Mathf.Cos(radians)*-1;
        float y = Mathf.Sin(radians)*-1;

        // Return the direction as a Vector2
        return new Vector2(x, y);
    }

    // Function to change the direction of the enemy
    void ChangeDirection()
    {
        // Generate a new random direction for the enemy
        direction = RandomDirection();
    }
}
