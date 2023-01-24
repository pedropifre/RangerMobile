using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBase2 : MonoBehaviour
{
    private CircleCollider2D trigger;
    public float checkInterval = 4.0f; // check every 4 seconds
    private float timer;

    void Start()
    {
        trigger = GetComponent<CircleCollider2D>();
        trigger.isTrigger = true;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0;
            CheckObjectsInsideTrigger();
        }
    }

    void CheckObjectsInsideTrigger()
    {
        // Get all the colliders inside the trigger
        Collider2D[] colliders = Physics2D.OverlapBoxAll(trigger.bounds.center, trigger.bounds.size, 0);

        // Iterate through the colliders
        for (int i = 0; i < colliders.Length; i++)
        {
            // Check the tag of the collider's game object
            if (colliders[i].tag == "Pokemon")
            {
                //Debug.Log("Enemy " + colliders[i].gameObject.name +" inside trigger!");
                colliders[i].gameObject.GetComponent<EnemyBase>().HealEnemy();
            }
        }
    }
}
