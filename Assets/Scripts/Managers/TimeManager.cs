using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace TimeManagementSpace
{


    public class TimeManager : MonoBehaviour
    {
        public float timeInSeconds = 60f; // set the time in seconds
        public float intervalInSeconds = 5f; // set the interval in seconds
        private float timer; // current timer value
        private float intervalTimer; // current interval timer value
        public TextMeshProUGUI timerText; // reference to the TextMeshProUGUI component to display the timer

        [Header("Monsters to spawn")]
        public List<MonsterToSpawn> monsterToSpawn;
        private int waveCount;
        
        void Start()
        {
            timer = timeInSeconds;
            intervalTimer =timeInSeconds/monsterToSpawn.Count;

        }

        void Update()
        {
            timer -= Time.deltaTime;
            intervalTimer -= Time.deltaTime;

            //update the monster timer
            foreach (var x in monsterToSpawn)
            {
                x.timeToSpawn -= Time.deltaTime;
                if (x.timeToSpawn <= 0 )
                {
                    spawnMonsters(x.pfbEnemys);
                    monsterToSpawn.Remove(x);
                    break;
                }

            }

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

        private void spawnMonsters(List<GameObject> list)
        {
            foreach (var c in list)
            {
                Instantiate(c);
            }
        }

        // function to run at the set interval
        void FunctionToRun()
        {
            //Debug.Log("This function runs every " + intervalInSeconds + " seconds.");

        }

        // function to call when the timer ends
        void EndTimer()
        {
            Debug.Log("Timer ended.");
        }
    }
    [System.Serializable]
    public class MonsterToSpawn
    {
        public float timeToSpawn;
        public List<GameObject> pfbEnemys;
    }
}