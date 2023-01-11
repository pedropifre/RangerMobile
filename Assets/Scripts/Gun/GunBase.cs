using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Pathfinding;

public class GunBase : MonoBehaviour
{
    public ProjectileBase prefabProjectile;

    [Header("Aim")]
    public Transform positionToShootLine;
    public GameObject[] positionObjectives;
    

    public Transform target;
    private Coroutine _currentCoroutine;

    public KeyCode keyToShoot;
    private bool _isShooting = false;
    [SerializeField] private bool _isShootingObjective = false;

    [Header("Projectile config")]
    public float timeBetweenShotMin = .3f;
    public float timeBetweenShotMax = 5f;
    public int ammount = 1;

    [Header("Raycast config")]
    public float distanceToShoot = 1;

    [SerializeField]private float timeBetweenShot = .3f;
    [SerializeField]private float timeBetweenShotObjective = 5f;
    [SerializeField] private float _angleShoot;

    RaycastHit2D hit;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("LineController").transform;
        positionObjectives = GameObject.FindGameObjectsWithTag("Objectives");
    }

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
        
        Debug.Log(Input.touchCount);
        //raycast start
        if (Input.touchCount > 0 )
        {
            // Get the direction from the current game object to the target game object
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;

            // Rotate the game object to face the target game object
            transform.right = direction;

            Touch touch = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            hit = Physics2D.Raycast(ray.origin, ray.direction);

            //Debug.Log(hit.collider);

            //check the distance between the enemy and the line
            //ksdnjif
            if (hit.collider != null && hit.collider.tag == "LineController")
            {
                float distance = Vector2.Distance(transform.position, hit.point);

                Debug.Log("Monster = " + gameObject.name + " Distance = " + distance);
                if (distance < distanceToShoot)
                {
                    //Debug.Log("Monster "+gameObject.name+" Atirar");
                    StartCoroutine(StartShoot());
                }
                
            }

        }
        else
        {
            StartCoroutine(StartShootObjective());
            
        }

    }

    IEnumerator StartShootObjective()
    {
        if (_isShootingObjective == false)
        {
            _isShootingObjective = true;
            Shoot();
            var objTargeted = Random.Range(0, positionObjectives.Length);
            // Get the direction from the current game object to the target game object
            Vector2 direction = (Vector2)positionObjectives[objTargeted].transform.position - (Vector2)transform.position;
            // Rotate the game object to face the target game object
            transform.right = direction;
            yield return new WaitForSeconds(timeBetweenShotObjective);
            _isShootingObjective = false;
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
            if (turn == 0 && ammount==1)
            {
                var projectile = Instantiate(prefabProjectile);
                projectile.transform.localEulerAngles =
                        new Vector3(
                        positionToShootLine.transform.eulerAngles.x,
                        positionToShootLine.transform.eulerAngles.y,
                        positionToShootLine.transform.eulerAngles.z - (90));
                projectile.transform.position = positionToShootLine.position;
            }
            else
            {

                var projectile = Instantiate(prefabProjectile);
                projectile.transform.localEulerAngles= 
                        new Vector3(
                        projectile.transform.localEulerAngles.x,
                        projectile.transform.localEulerAngles.y,
                        projectile.transform.localEulerAngles.z+(turn * _angleShoot));
                projectile.transform.position = positionToShootLine.position;

            }
            //Debug.Log("Turn = "+turn+" .       Normal rotation = " + positionToShoot.transform.rotation);
            //Debug.Log("Turn = "+turn+" .       added rotation = " + Quaternion.Euler((_angleShoot * turn), (_angleShoot * turn), 0));
           //Debug.Log(_angleShoot * turn);
            turn++;

        }

    }


    //Shot when detects line in collider
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "LineController")
    //    {
    //        //courotine of attack
    //        StartCoroutine(StartShoot());
    //        //Debug.Log("Attack");
    //    }

    //}
    void OnDrawGizmos()
    {
        if (hit.collider != null)
        {
            // Draw a line from the enemy to the touchpoint
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hit.point);
            float distance = Vector2.Distance(transform.position, hit.point);
            
        }
    }
}
