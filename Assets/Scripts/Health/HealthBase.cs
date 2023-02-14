using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;


public class HealthBase : MonoBehaviour, IDamagable
{
    public float StartLife = 10f;
    public bool destroyOnKill = false;
    public float _currentLife;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public float damageMultiply = 1f;

    [Header("FlashColor")]
    public FlashColor flashColor;

    [Header("UILife")]
    public TextMeshProUGUI currentLife;
    public TextMeshProUGUI MaxLife;
    public Image lifeBar;
    private float _LifeDropGraph;



    private void Awake()
    {
        Init();
        if (flashColor == null)
        {
            flashColor = GetComponent<FlashColor>();
        }
        _LifeDropGraph = StartLife / 1;
    }
    public void Init()
    {
        ResetLife();
        UpdateLife();
        if (MaxLife)
        {
            MaxLife.text = "/" + StartLife.ToString();
        }
        
    }


    public void ResetLife()
    {
        _currentLife = StartLife;
        
    }

    public void UpdateLife()
    {
        if (currentLife != null)
        {
            currentLife.text = _currentLife.ToString();
            lifeBar.fillAmount = _currentLife/StartLife;
            Debug.Log(lifeBar.fillAmount);
        }
    }
 
    public void heal(float f)
    {
        _currentLife += f;
        UpdateLife();
    }
    public void Damage(float f=1)
    {

        _currentLife -= f * damageMultiply;
        OnDamage?.Invoke(this);
        //update UI
        UpdateLife();
        
        if (_currentLife <= 0)
        {
            //resolve destroing objective here
            if (gameObject.tag == "LineController")
            {
                //game over screen
            }
        }
        if (flashColor != null)
        {
            flashColor.Flash();
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
