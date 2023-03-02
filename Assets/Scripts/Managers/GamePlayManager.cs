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

    private bool finish = false;

    private void Update()
    {
        if (timeManager.ReturnMonstersLeft() == 0 && !finish)
        {
            finish = true;
            StartCoroutine(FinishLevel());
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
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
