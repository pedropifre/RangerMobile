using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
        private bool running = true;
        public LineDraw lineGuard;

        [SerializeField] private GameObject[] spawnPoints;
        
        void Start()
        {
            timer = timeInSeconds;
            intervalTimer =timeInSeconds/monsterToSpawn.Count;
            spawnPoints = GameObject.FindGameObjectsWithTag("WalkingTarget");
            lineGuard = GameObject.FindGameObjectWithTag("LineController").GetComponent<LineDraw>();

        }

        void Update()
        {
            if (running)
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
                //Para não deixar o jogo parado, se o jogador ja tiver capturado todos os inimigos da wave
                //ela adianta para a próxima wave, assim deixando tempo para o jogador bater o recorde de tempo
                var enemyCount = lineGuard.ReturnEnCount();
                if (enemyCount == 0 && timeInSeconds>0 && monsterToSpawn.Count>0)
                {

                    spawnMonsters(monsterToSpawn[0].pfbEnemys);
                    monsterToSpawn.Remove(monsterToSpawn[0]);
                }
                // update the timer display
                int minutes = Mathf.FloorToInt(timer / 60f);
                int seconds = Mathf.FloorToInt(timer % 60f);
                timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

                

                // end the timer when it reaches 0
                if (timer <= 0f)
                {
                    EndTimer();
                }
            }
        }

        private void spawnMonsters(List<GameObject> list)
        {
            foreach (var c in list)
            {
                var spawnRandom = Random.Range(0, spawnPoints.Length - 1);
                Instantiate(c,spawnPoints[spawnRandom].transform.position,Quaternion.identity);
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
            running=false;
        }

        public int ReturnMonstersLeft()
        {
            var monstersLeft = 0;
            var monsterField = lineGuard.ReturnEnCount();
            if (monsterToSpawn.Count == 0 && monsterField == 0)return monstersLeft;
            else
            {
                //monsters to be spawned
                foreach(var b in monsterToSpawn)
                {
                    monstersLeft += b.pfbEnemys.Count;
                }
                //moster in the field
                monstersLeft += monsterField;
                return monstersLeft;
            }
        }
    }
    [System.Serializable]
    public class MonsterToSpawn
    {
        public float timeToSpawn;
        public List<GameObject> pfbEnemys;
    }
}