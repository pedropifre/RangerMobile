using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GunBase : MonoBehaviour
{
    public ProjectileBase prefabProjectile;

    public Transform positionToShoot;

    public Transform target;
    private Coroutine _currentCoroutine;

    public KeyCode keyToShoot;
    private bool _isShooting = false;

    [Header("Projectile config")]
    public float timeBetweenShotMin = .3f;
    public float timeBetweenShotMax = 5f;
    public int ammount = 1;

    [SerializeField]private float timeBetweenShot = .3f;
    [SerializeField] private float _angleShoot;



    // Update is called once per frame
    private void Start()
    {
        RandomizerShoot();
        //calcular a diferença de anglo para os tiros
        _angleShoot = 360 / ammount;
    }
    public void RandomizerShoot()
    { 
        timeBetweenShot = Random.Range(timeBetweenShotMin, timeBetweenShotMax);
    }
    void Update()
    {
        // Get the direction from the current game object to the target game object
        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;

        // Rotate the game object to face the target game object
        transform.right = direction;

        if (Input.GetKeyDown(keyToShoot))
        {
            _currentCoroutine = StartCoroutine(StartShoot());
        }
        else if(Input.GetKeyUp(keyToShoot))
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
        } 
    }

    IEnumerator StartShoot()
    {
        if(_isShooting == false)
        {
            _isShooting = true;
            Shoot();
            yield return new WaitForSeconds(timeBetweenShot);
            RandomizerShoot();
            //Debug.Log("shoot");
            _isShooting = false;
        }
    }

    public void Shoot()
    {
        int turn = 0;
        while (turn < ammount)
        {
            Quaternion test = new Quaternion(0,0,0,0);
            if (turn == 0)
            {
                var projectile = Instantiate(prefabProjectile);
                projectile.transform.position = positionToShoot.position;
                projectile.transform.rotation = positionToShoot.transform.rotation;
            }
            else
            {
                positionToShoot.rotation = positionToShoot.rotation * Quaternion.Euler(0,0,(turn*_angleShoot));
                var projectile = Instantiate(prefabProjectile, positionToShoot.position,positionToShoot.rotation);
                
                projectile.transform.rotation = test;

            }
            //Debug.Log("Turn = "+turn+" .       Normal rotation = " + positionToShoot.transform.rotation);
            //Debug.Log("Turn = "+turn+" .       added rotation = " + Quaternion.Euler((_angleShoot * turn), (_angleShoot * turn), 0));
            Debug.Log(_angleShoot * turn);
            turn++;

        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LineController")
        {
            //courotine of attack
            StartCoroutine(StartShoot());
            //Debug.Log("Attack");
        }
        
    }

}
