using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBase : MonoBehaviour
{
    public static GameObject this_instance;
    private GameObject OBJPlayer;
    //Player player;

    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "Bullet" || otherCollider.gameObject.tag == "Obstacle")
        {
            //player.bonus_nohit = true;
            //player.SelfDamage();
        }
    }
}
