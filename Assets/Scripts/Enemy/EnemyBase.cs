using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using SystemSFX;

public class EnemyBase : MonoBehaviour
{
    public SO_Enemy sO_Enemy;
    private EnemyController enemymovement;
    public int life = 10;
    public int monsterNumb;
    public ParticleSystem particleCatch;
    


    [Header("LineDraw")]
    public LineDraw lineDraw;
    
    [Header("Sound")]
    public SFXManager sFXManager;
    public AudioSource sfxCatch;

    [Header("Graphs")]
    public SpriteRenderer lifeBar;


    [SerializeField] private float _LifeDropGraph;
    void Start()
    {
        enemymovement = this.GetComponent<EnemyController>();
        _LifeDropGraph = (float)1 / life;
        Debug.Log(_LifeDropGraph);
    }

    public void DamageEnemy(int damage=1)
    {
        life -= damage;
        
        lifeBar.size = new Vector2(lifeBar.size.x - _LifeDropGraph, 1);

        if (life <= 0)
        {
            lineDraw.RemoveFromList(monsterNumb);
            Kill();
        }
    }

    public void Kill()
    {
        StartCoroutine(KillCourotine());
        
        
    }
    IEnumerator KillCourotine()
    {
        sfxCatch.clip = sFXManager.PlaySFX("Catch");
        sfxCatch.Play();
        particleCatch.Play();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
