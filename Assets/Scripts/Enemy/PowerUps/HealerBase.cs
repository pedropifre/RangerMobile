using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBase : MonoBehaviour
{
    public int healPerSeconds;
    [SerializeField]private bool _isHealing=true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Pokemon")
        {
            var enemyBase = collision.gameObject.GetComponent<EnemyBase>();
        
            if (enemyBase != null)
            {
                //Debug.Log("Healer= "+collision.name);
                StartCoroutine(healCourotine(enemyBase));
            }
        }
    }
  
    IEnumerator healCourotine(EnemyBase hb)
    {
        if (_isHealing)
        {
            _isHealing = false;
            Debug.Log("Healer is healing!");
            hb.HealEnemy(1);
            yield return new WaitForSeconds(healPerSeconds);
            _isHealing = true;
        }


    }

}
