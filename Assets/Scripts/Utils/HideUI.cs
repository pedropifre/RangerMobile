using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    public void HideUIInt(GameObject UI)
    {
        UI.SetActive(!UI.activeSelf);
    }
}
