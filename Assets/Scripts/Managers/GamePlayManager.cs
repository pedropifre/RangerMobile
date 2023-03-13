using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeManagementSpace;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public TimeManager timeManager;
    public GameObject UIFinish;
    public TextMeshProUGUI textTime;

    [Header("GameOver")]
    public GameObject UIFinishGO;
    public TextMeshProUGUI textTime2;


    private bool finish = false;
    private int objectives;

    private void Update()
    {
        if (timeManager.ReturnMonstersLeft() == 0 && !finish)
        {
            finish = true;
            StartCoroutine(FinishLevel());
        }
        objectives = returnObjectives();
        //Debug.Log(objectives);
        if (objectives <= 0 && !finish)
        {
            finish = true;
            StartCoroutine(GameOver());
        }
        
    }

    IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(1f);
        var uiIinitialScale = UIFinish.transform.localScale;
        UIFinish.transform.localScale = new Vector2(0, 0);
        textTime.text = "Your time - " + timeManager.ReturnTimeLeft();
        UIFinish.SetActive(true);
        UIFinish.transform.DOScale(uiIinitialScale, 2f).SetEase(Ease.OutBack);
    }
    
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        var uiIinitialScale = UIFinishGO.transform.localScale;
        UIFinishGO.transform.localScale = new Vector2(0, 0);
        textTime2.text = "Your time - " + timeManager.ReturnTimeLeft();
        UIFinishGO.SetActive(true);
        UIFinishGO.transform.DOScale(uiIinitialScale, 2f).SetEase(Ease.OutBack);
    }
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public int returnObjectives()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Objectives");
        return objs.Length;
    }

}
