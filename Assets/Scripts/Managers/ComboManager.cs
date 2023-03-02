using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class ComboManager : MonoBehaviour
{
    public TextMeshProUGUI txtCombo;
    public Ease easeCombo;

    
    public void SetComboCourotine(int comboInt)
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimationCombo(comboInt));
    }

    IEnumerator AnimationCombo(int combo)
    {
        txtCombo.text = "X " + combo.ToString();
        txtCombo.transform.localScale = new Vector3(0, 0, 0);
        txtCombo.gameObject.transform.DOScale(new Vector3(1,1,1),.5f).SetEase(easeCombo);
        yield return new WaitForSeconds(.5f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
