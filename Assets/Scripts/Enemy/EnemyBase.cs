using System.Collections;
using UnityEngine;
using SystemSFX;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    public SO_Enemy sO_Enemy;
    private EnemyController enemymovement;
    public HealthBase healthBase;
    public ParticleSystem particleCatch;
    public GunBase gunBase;
    


    [Header("LineDraw")]
    public LineDraw lineDraw;
    
    [Header("Sound")]
    public SFXManager sFXManager;
    public AudioSource sfxCatch;

    [Header("Graphs")]
    public SpriteRenderer lifeBar;
    public float vanishTime;


    [SerializeField] private float _LifeDropGraph;
    


    private void Awake()
    {
        //gameObject.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Target").transform;
        lineDraw = GameObject.FindGameObjectWithTag("LineController").GetComponent<LineDraw>();
        sFXManager = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
        lineDraw.AddEnemy(gameObject);
    }
    void Start()
    {

        enemymovement = this.GetComponent<EnemyController>();
        _LifeDropGraph = (float)1 / healthBase._currentLife;
        var scalaInicial = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector2.zero;
        gameObject.transform.DOScale(scalaInicial,1f).SetEase(Ease.OutBack);
    }

    public void ChangeShoting()
    {
        gunBase.canShootAll = (gunBase.canShootAll == true) ? false : true;
    }
  
    public void RemoveListing()
    {
        lineDraw.RemoveEnemy(gameObject);
        
    }
    public void DamageEnemy(int damage=1)
    {
        if (healthBase._currentLife > 0)
        {

            healthBase.Damage(damage);
            if (lifeBar.size.x - (_LifeDropGraph * damage) < 0) lifeBar.size = new Vector2(0, 1);
            else lifeBar.size = new Vector2(lifeBar.size.x - (_LifeDropGraph*damage), 1);

            if (healthBase._currentLife <= 0)
            {
                //remove da lista do line draw e depois deleta o GameObject
            
                Kill();
            }
        }
    }

    public void HealEnemy(int heal=1)
    {
        if (healthBase._currentLife >= healthBase.StartLife)
        {
            return;
        }
        else
        {
            healthBase.heal(heal);
            //Debug.Log("HealEnemy");
            lifeBar.size = new Vector2(lifeBar.size.x + (_LifeDropGraph * heal), 1);
        }

    }

    public void Kill()
    {
        StartCoroutine(KillCourotine());
        
        
    }

    IEnumerator KillCourotine()
    {
        gunBase.StopShoting();
        sfxCatch.clip = sFXManager.PlaySFX("Catch");
        sfxCatch.Play();
        particleCatch.Play();
        yield return new WaitForSeconds(vanishTime);
        RemoveListing();
        Destroy(gameObject);
    }
}
