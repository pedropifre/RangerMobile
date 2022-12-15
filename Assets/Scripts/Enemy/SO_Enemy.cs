using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Enemy : ScriptableObject
{
   
    public float DamageTime;
    public string nameMonster;
    public float speed;
    public float atack;
    public string type;
    public string weak;

    [Header("Animaiton")]
    public Animator animator;
}
