using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeManager : MonoBehaviour
{
    public float timeInSeconds = 60f; // set the time in seconds
    public float intervalInSeconds = 5f; // set the interval in seconds
    private float timer; // current timer value
    private float intervalTimer; // current interval timer value
    public TextMeshProUGUI timerText; // reference to the TextMeshProUGUI component to display the timer

    void Start()
    {
        timer = timeInSeconds;
        intervalTimer = intervalInSeconds;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        intervalTimer -= Time.deltaTime;

        // update the timer display
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        // run the function at the set interval
        if (intervalTimer <= 0f)
        {
            FunctionToRun();
            intervalTimer = intervalInSeconds;
        }

        // end the timer when it reaches 0
        if (timer <= 0f)
        {
            EndTimer();
        }
    }

    // function to run at the set interval
    void FunctionToRun()
    {
        Debug.Log("This function runs every " + intervalInSeconds + " seconds.");
    }

    // function to call when the timer ends
    void EndTimer()
    {
        Debug.Log("Timer ended.");
    }
}
