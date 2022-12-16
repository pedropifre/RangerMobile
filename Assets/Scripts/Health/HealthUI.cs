using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HealthUI : MonoBehaviour
{

    public HealthBase healthBase;
    public TextMeshProUGUI txtVida;
    
    void Start()
    {
        UpdateLifeUI();
    }

    public void UpdateLifeUI()
    {
        txtVida.text = "X " + healthBase._currentLife.ToString();
    }
}
