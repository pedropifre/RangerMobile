using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public GameObject direction;

    public float side = 1;

    public int damageAmount = 1;

    [Header("Projectile config")]
    public float speedprojectile = 1;
    public float timeToDestroy = 2f;
    



    private GunBase gunBase;
    private void Awake()
    {
        Destroy(gameObject, timeToDestroy);
        gunBase = GameObject.FindObjectOfType<GunBase>();
        direction = gunBase.gameObject;
        

    }
    private void Update()
    {
        transform.Translate(direction.transform.position*speedprojectile * Time.deltaTime*side);
        
    }

    //Implementar dano a stylus aqui
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var stylus = collision.transform.GetComponent<HealthBase>();

        if (stylus != null)
        {
            Debug.Log("Dano Stylus");
            stylus.Damage(1);
            Destroy(gameObject);
            var lineDraw = collision.transform.GetComponent<LineDraw>();
            lineDraw.PlayBreakSound();
            lineDraw.DestroyElement();
        }
        
    }
}