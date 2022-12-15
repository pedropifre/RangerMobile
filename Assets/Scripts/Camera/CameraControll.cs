using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;
    [SerializeField]
    float zoomModifierSpeed = 0.1f;

    Vector2 firstTouchPrevPos, secondTouchPrevPos;

    // The speed of the camera panning
    public float panSpeed = 10f;
    
    private Camera mainCamera;

    //pan limit
    [Header("Pan Limit")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Zoom Limit")]
    public bool zoomOn;
    public float maxIn;
    public float maxOut;
    

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    { 
        

        // If the user is touching the screen with two fingers
        if (Input.touchCount == 2 && zoomOn)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
            {
                float zoomOut = mainCamera.orthographicSize + zoomModifier;
                Debug.Log("Zoom out = " + zoomOut);
                if(zoomOut<maxOut) mainCamera.orthographicSize += zoomModifier;

            }
            if (touchesPrevPosDifference < touchesCurPosDifference)
            {
                float zoomIn = mainCamera.orthographicSize - zoomModifier;
                Debug.Log("Zoom in = " +zoomIn);
                if (zoomIn > maxIn) mainCamera.orthographicSize -= zoomModifier;

            }

        }


        // Pan the camera based on the finger movement
        Pan();
    }

    // Calculates the distance between the fingers and returns it
    float GetFingerDistance()
    {
        // Get the touches on the screen
        Touch[] touches = Input.touches;

        // If there are two or more touches, calculate the distance between them
        if (touches.Length >= 2)
        {
            Vector2 finger1 = touches[0].position;
            Vector2 finger2 = touches[1].position;

            return Vector2.Distance(finger1, finger2);
        }
        // Otherwise, return 0
        else
        {
            return 0f;
        }
    }


    void Pan()
    {
        // Get the touches on the screen
        Touch[] touches = Input.touches;

        // If there are two touches, calculate the direction of movement
        if (touches.Length == 2)
        {
            Vector2 finger1 = touches[0].position;
            Vector2 finger1Prev = touches[0].position - touches[0].deltaPosition;
            Vector2 finger2 = touches[1].position;
            Vector2 finger2Prev = touches[1].position - touches[1].deltaPosition;

            Vector3 panDirection = new Vector3(finger1.x - finger1Prev.x + finger2.x - finger2Prev.x,
                                               finger1.y - finger1Prev.y + finger2.y - finger2Prev.y,
                                               0f);

            // Apply the panning to the camera
            Vector3 newPos = transform.position - panDirection * panSpeed * Time.deltaTime;

            if (newPos.x < minX || newPos.x > maxX || newPos.y < minY || newPos.y > maxY)
            {
                // The new position is outside the boundaries, so don't update the camera's position
                return;
            }

            transform.position -= panDirection * panSpeed * Time.deltaTime;
        }
    }

    //utilities
    #region ULTILITIES
    public void CameraCentrer(Camera camera)
    {
        camera.transform.position = new Vector3(0, 1, -10);
    }
    #endregion
}