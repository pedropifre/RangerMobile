using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTest : MonoBehaviour
{
    public GameObject rockPrefab;

    private bool isTouching = false;
    private Vector2 lastTouchPosition;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // initialize touch state and record the touch position
                isTouching = true;
                lastTouchPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && isTouching)
            {
                // spawn a rock for each segment of the swipe
                List<Vector2> swipePositions = GetSwipePositions();
                foreach (Vector2 swipePosition in swipePositions)
                {
                    // check if there is already a rock at the swipe position
                    Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(swipePosition.x, swipePosition.y, 10));
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 0.5f);
                    bool canSpawn = true;
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.gameObject.CompareTag("Rock"))
                        {
                            canSpawn = false;
                            break;
                        }
                    }

                    // spawn a rock if there is no other rock at the swipe position
                    if (canSpawn)
                    {
                        GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
                        rock.tag = "Rock"; // set the tag to "Rock" for future reference

                        // add a collider component to the rock
                        rock.AddComponent<BoxCollider2D>();
                    }
                }

                // update the last touch position
                lastTouchPosition = Input.GetTouch(0).position;
            }
        }
        else if (isTouching)
        {
            // reset the touch state when the player releases their finger
            isTouching = false;
        }
    }

    private List<Vector2> GetSwipePositions()
    {
        List<Vector2> swipePositions = new List<Vector2>();

        // get the current touch position
        Vector2 currentTouchPosition = Input.GetTouch(0).position;

        // calculate the swipe direction and distance
        Vector2 swipeDirection = (currentTouchPosition - lastTouchPosition).normalized;
        float swipeDistance = Vector2.Distance(currentTouchPosition, lastTouchPosition);

        // divide the swipe distance into segments and add each segment position to the list
        float segmentDistance = 0.5f; // adjust this value to control the spacing between rocks
        float currentDistance = 0.0f;
        while (currentDistance < swipeDistance)
        {
            swipePositions.Add(lastTouchPosition + swipeDirection * currentDistance);
            currentDistance += segmentDistance;
        }

        return swipePositions;
    }
}
