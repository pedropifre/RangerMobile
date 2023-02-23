using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeManagementSpace;
public class GamePlayManager : MonoBehaviour
{
    public TimeManager timeManager;

    private void Update()
    {
        if (timeManager.ReturnMonstersLeft() == 0)
        {
            Debug.Log("acabou - monsters Left = timeManager.ReturnMonstersLeft()");
        }
    }
}
