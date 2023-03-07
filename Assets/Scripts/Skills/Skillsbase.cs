using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

namespace SkillBase
{

    public class Skillsbase : Singleton<Skillsbase>
    {
        public float skillTime = 5f;
        public float skillCoolDown = 5f;
        //private GameObject SkillChoosen;
        public _SOPlayer sOPlayer;
        

        [Header("UI")]
        public GameObject uiSkills;
        public HideUI uiScript;

        [Header("Poison")]
        public int PHits;
        public ParticleSystem poisonParticles;

        
        [Header("Thunder")]
        public int stunTime;
        public ParticleSystem stunParticles;

        [Header("Death")]
        public ParticleSystem DeathParticles;
        public float DeathLagTime;
        public float DeathTime;

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
            uiScript.HideUIInt(uiSkills);
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Pokemon");
            Debug.Log("Click skill");
            StartCoroutine(damagePoison(inimigos));
        }
        
        public void StunEffect()
        {
            uiScript.HideUIInt(uiSkills);
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Pokemon");
            StartCoroutine(StunCourotine(inimigos));
        }
        public void AvadakedavraEffect()
        {
            uiScript.HideUIInt(uiSkills);
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Pokemon");
            StartCoroutine(AvadaCourotine(inimigos));
        }

        IEnumerator damagePoison(GameObject[] arrayInimigos)
        {
            foreach (var i in arrayInimigos)
            {

                var part = Instantiate(poisonParticles, new Vector3(i.transform.position.x,
                    i.transform.position.y, -1), Quaternion.identity);
                Destroy(part, skillTime);
            }
            for (var y = 1 ; y <= PHits ; y++)
            {
                Debug.Log("Damage");
                foreach (var i in arrayInimigos)
                {
                    if (i.GetComponent<HealthBase>()._currentLife > 0)
                    {
                        i.GetComponent<EnemyBase>().DamageEnemy(1);
                        yield return new WaitForSeconds(skillTime / PHits /arrayInimigos.Length);
                    }
                    
                }

            }
           
        }
        IEnumerator StunCourotine(GameObject[] arrayInimigos)
        {
            foreach (var i in arrayInimigos)
            {

                var part = Instantiate(stunParticles, new Vector3(i.transform.position.x,
                    i.transform.position.y, -1), Quaternion.identity);
                Destroy(part.gameObject,stunTime);
            }
            foreach (var i in arrayInimigos)
            {
                if (i.GetComponent<HealthBase>()._currentLife > 0)
                {
                    i.GetComponent<EnemyMovement>().StartTunder();
                    i.GetComponent<EnemyBase>().ChangeShoting();
                }
            }
            yield return new WaitForSeconds(stunTime);
            foreach (var i in arrayInimigos)
            {
                i.GetComponent<EnemyMovement>().StopTunder();
                i.GetComponent<EnemyBase>().ChangeShoting();
            }
        }

        IEnumerator AvadaCourotine(GameObject[] arrayInimigos)
        {
            foreach (var i in arrayInimigos)
            {

                var part = Instantiate(stunParticles, new Vector3(i.transform.position.x,
                    i.transform.position.y, -1), Quaternion.identity);
                Destroy(part.gameObject, stunTime);
            }
            foreach (var i in arrayInimigos)
            {
                if (i.GetComponent<HealthBase>()._currentLife > 0)
                {
                    var curren = i.GetComponent<HealthBase>()._currentLife;
                    i.GetComponent<EnemyBase>().DamageEnemy((int)curren);
                }
                yield return new WaitForSeconds(DeathLagTime);
            }
            
        }
    }
}
