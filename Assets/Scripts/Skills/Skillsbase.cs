using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillBase
{

    public class Skillsbase : MonoBehaviour
    {
        public float skillTime = 5f;
        public float skillCoolDown = 5f;
        //private GameObject SkillChoosen;
        public _SOPlayer sOPlayer;

        [Header("Poison")]
        public int PHits;

        private void Awake()
        {
            //SkillChoosen = GameObject.FindGameObjectWithTag("LineRenderer");
        }

        public void DoEffect()
        {
            if (sOPlayer.skill == 1) PoisonEffect();
            else if (sOPlayer.skill == 2) StunEffect();
            else if (sOPlayer.skill == 3) AvadakedavraEffect();
        }

        public void PoisonEffect()
        {
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Pokemon");
            Debug.Log("Click skill");
            StartCoroutine(damagePoison(inimigos));
        }

        IEnumerator damagePoison(GameObject[] arrayInimigos)
        {
            for (var y = 1 ; y <= PHits ; y++)
            {
                Debug.Log("Damage");
                foreach (var i in arrayInimigos)
                {
                    i.GetComponent<EnemyBase>().DamageEnemy(1);
                    yield return new WaitForSeconds(skillTime / PHits);
                }

            }
        }
        public void StunEffect()
        {

        }
        public void AvadakedavraEffect()
        {

        }
    }
}
