using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemSFX;

public class ObjectiveBase : MonoBehaviour
{


    [Header("Sound config")]
    public AudioSource sfxCatch;
    public string audioName = "HitObjective";

    private SFXManager sFXManager;


    private void Awake()
    {
        sFXManager = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
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
}
