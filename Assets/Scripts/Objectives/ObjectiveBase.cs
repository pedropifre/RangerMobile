using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemSFX;
using DG.Tweening;

public class ObjectiveBase : MonoBehaviour
{


    [Header("Sound config")]
    public AudioSource sfxCatch;
    public string audioName = "HitObjective";

    [Header("Destroyed")]
    public float destroyAnimationTime = 2;

    private SFXManager sFXManager;
    private HealthBase healthBase;

    private void Awake()
    {
        sFXManager = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
        healthBase = gameObject.GetComponent<HealthBase>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ProjectileObjective")
        {
            //play sound
            sfxCatch.clip = sFXManager.PlaySFX(audioName);
            sfxCatch.Play();
        }
    }


    public void DamageBase(int damage = 1)
    {
        if (healthBase._currentLife > 0)
        {

            healthBase.Damage(damage);
            

            if (healthBase._currentLife <= 0)
            {
                StartCoroutine(KillCourotine());
            }
        }
    }

 
    IEnumerator KillCourotine()
    {
        gameObject.tag = "Untagged";
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("AttackPoint");
        foreach (var t in enemys)
        {
            t.GetComponent<GunBase>().findObjectives();
        }
        gameObject.transform.DOScale(new Vector3(0, 0, 0), destroyAnimationTime);
        yield return new WaitForSeconds(destroyAnimationTime);
     
        Destroy(gameObject);
    }
}
