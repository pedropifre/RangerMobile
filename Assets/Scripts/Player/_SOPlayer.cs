using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class _SOPlayer : ScriptableObject
{
    [Header("Armadura")]
    public SO_Gear varinhaBase;
    public SO_Gear varinhaPonta;
    public SO_Gear Peitoral;
    public SO_Gear Capacete;
    public SO_Gear Pernas;
    public SO_Gear tenis;

    [Header("PowerUp Escolhido")]
    public int powerUp;
    
    [Header("Skill Escolhido")]
    public int skill;
}
