using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{


    [Header("Wave config")]
    public GameObject enemyPrefab; 
    public float spawnInterval = 5f; 
    public float spawnDuration = 300f; 
    public Transform[] spawnPoints;



    float elapsedTime; 
    public bool isSpawning = false;

    private GameObject EnemysListLineCheck;
    private int SpawnedMonsters=0;

    private void Awake()
    {
        EnemysListLineCheck =  GameObject.FindGameObjectWithTag("LineController");
        
    }

    public void StartRun()
    {
        isSpawning = true;
    }

    public void StopRun()
    {
        isSpawning = false;
    }

    void Update()
    {
        //wave Mode
        if (isSpawning)
        {

            elapsedTime += Time.deltaTime; 

        
            if (elapsedTime >= spawnInterval)
            {
           
                if (elapsedTime < spawnDuration)
                {
                
                    int spawnIndex = Random.Range(0, spawnPoints.Length);
                    Transform spawnPoint = spawnPoints[spawnIndex];

                
                    GameObject enemySpawned =  Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    EnemysListLineCheck.GetComponent<LineDraw>().EnemyOBJ.Add(enemySpawned);
                    enemySpawned.GetComponent<EnemyBase>().monsterNumb = SpawnedMonsters;
                    SpawnedMonsters++;
                }
                else
                {
                    //Make The Boss apear 
                    this.enabled = false;
                }

                // reset the elapsed time
                elapsedTime = 0f;
            }
        }
    
        
    }
    public void spawnEnemy(GameObject enemyToSpawn)
    {
        Instantiate(enemyToSpawn, gameObject.transform);
    }
}
