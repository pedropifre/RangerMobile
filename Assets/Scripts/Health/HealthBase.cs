using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthBase : MonoBehaviour, IDamagable
{
    public float StartLife = 10f;
    public bool destroyOnKill = false;
    public float _currentLife;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public float damageMultiply = 1f;
    public HealthUI healthUI;





    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        ResetLife();
    }


    public void ResetLife()
    {
        _currentLife = StartLife;
        
    }


    [NaughtyAttributes.Button]
    public void Damage()
    {
        Damage(5);
    }

 
    public void heal(float f)
    {
        _currentLife += f;
        Debug.Log("HealthBase");
    }
    public void Damage(float f)
    {

        _currentLife -= f * damageMultiply;
        OnDamage?.Invoke(this);

        if (healthUI != null)
        {
            healthUI.UpdateLifeUI();
        }
        if (_currentLife <= 0)
        {
            //resolve destroing objective here
        }
    }
   

 
  

   

    
    public void ChangeDamageMultiply(float damageMultiply, float duration)
    {
        StartCoroutine(ChangeDamageMultiplyCourotine(damageMultiply, duration));
    }

    IEnumerator ChangeDamageMultiplyCourotine(float damageMultiply, float duration)
    {
        this.damageMultiply = damageMultiply;
        yield return new WaitForSeconds(duration);
        this.damageMultiply = 1;
    }
}
