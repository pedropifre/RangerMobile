using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using NaughtyAttributes;

public class TextDamageBase : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI textToUse;
    public float duration;
    public int health;
    public GameObject enemy;
    void Start()
    {

    }

    [NaughtyAttributes.Button]
    public void spawn()
    {
        setNumber(health);
        StartCoroutine(ShowNumber());
        Destroy(gameObject, duration + 0.1f);
    }

    public void setNumber(int life)
    {
        textToUse.text = life.ToString();
    }

    IEnumerator ShowNumber()
    {
        //animation
        textToUse.transform.localScale = new Vector2(0, 0);
        textToUse.transform.DOScale(new Vector2(1, 1), duration).SetLoops(-1, LoopType.Yoyo);
        textToUse.transform.DOLocalMove(new Vector2(enemy.transform.position.x+
            Random.Range(-44, 44), enemy.transform.position.y+44), duration);
        textToUse.DOColor(Color.red, 0);
        yield return new WaitForSeconds(duration*0.8f);
        textToUse.DOFade(0f,duration*0.2f);
    }

    
}
