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
        //Destroy(gameObject, timeToDestroy);
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

        if (stylus != null && collision.gameObject.tag == "LineController")
        {
            Debug.Log("Dano Stylus");
            stylus.Damage(1);
            Destroy(gameObject);
            var lineDraw = collision.transform.GetComponent<LineDraw>();
            if (lineDraw != null)
            {
                lineDraw.PlayBreakSound();
                lineDraw.DestroyElement();
            }
        }
        else if (collision.gameObject.tag == "Objectives")
        {

            var castleHealth = collision.gameObject.GetComponent<HealthBase>();
            if (castleHealth != null)
            {
                StartCoroutine(DamageColor(collision.gameObject));
                castleHealth.Damage();
                
            }
        }
        
    }

    IEnumerator DamageColor(GameObject objColor)
    {
        var color = objColor.GetComponent<SpriteRenderer>().color;
        objColor.GetComponent<SpriteRenderer>().color = new Color32(253, 151, 151, 255);
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
        yield return new WaitForSeconds(.5f);
        objColor.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

    }
}
