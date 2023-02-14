using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Gear : ScriptableObject
{
    [Header("Characteristics")]
    public string nameGear;
    public int damageMultiply;
    public int defenseMultiply;
    public int level;

    [Header("Art")]
    public Sprite iconGear;
}
