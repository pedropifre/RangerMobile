using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using UnityEngine.UI;
using TimeManagementSpace;

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


        [Header("Time to unlock powerups")]
        public TimeManager timeManager;
        public List<ButtonType> btnType;


        private void Start()
        {
            foreach (var t in btnType)
            {
                t.btn.enabled = false;
                t.btn.image.color = Color.red;
            }
        }

        public void turnButtonOn(string nameToUnlock)
        {
            foreach (var t in btnType)
            {
                if (nameToUnlock == t.nameBtn)
                {
                    t.btn.enabled = true;
                    t.btn.image.color = Color.white;
                }
            }
        }
        
        public void turnButtonOff(string nameToUnlock)
        {
            foreach (var t in btnType)
            {
                if (nameToUnlock == t.nameBtn)
                {
                    t.btn.enabled = false;
                    t.btn.image.color = Color.red;
                }
            }
        }


        public void PoisonEffect()
        {
            uiScript.HideUIInt(uiSkills);
            turnButtonOff("Poison");
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Pokemon");
            Debug.Log("Click skill");
            StartCoroutine(damagePoison(inimigos));
            //time manager
            timeManager.isPwpUsed("Poison");
        }    
        public void StunEffect()
        {
            turnButtonOff("Thunder");
            uiScript.HideUIInt(uiSkills);
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Pokemon");
            StartCoroutine(StunCourotine(inimigos));
            //time manager
            timeManager.isPwpUsed("Thunder");
        }
        public void AvadakedavraEffect()
        {
            turnButtonOff("Death");
            uiScript.HideUIInt(uiSkills);
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Pokemon");
            StartCoroutine(AvadaCourotine(inimigos));
            //time manager
            timeManager.isPwpUsed("Death");
        }

        IEnumerator damagePoison(GameObject[] arrayInimigos)
        {
            foreach (var i in arrayInimigos)
            {

                var part = Instantiate(poisonParticles, new Vector3(i.transform.position.x,
                    i.transform.position.y, -1), Quaternion.identity);
                Destroy(part.gameObject, skillTime);
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

    [System.Serializable]
    public class ButtonType
    {
        public Button btn;
        public string nameBtn;
    }
}
