using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Pathfinding;
using DG.Tweening;

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
    public bool canShootingObjective = false; //to other scrit can deactivate the shooting bit
    public bool canShootAll = true; //to other scrit can deactivate the shooting bit
    [SerializeField] private bool _isShootingObjective = false;

    [Header("Projectile config")]
    public float timeBetweenShotMin = .3f;
    public float timeBetweenShotMax = 5f;
    public int ammount = 1;


    [Header("Pooling Config")]
    public List<GameObject> poolingShoots;

    [Header("Raycast config")]
    public float distanceToShoot = 1;

    [Header("Objective Shooting")]
    [SerializeField]private float timeBetweenShotObjective = 5f;
    public float velocityObjective =3f;
    private float timeBetweenShot = .3f;
    private float _angleShoot;
    public float recoil = .1f;

    RaycastHit2D hit;

    //to do not star shooting 
    [SerializeField] private float shootDelay;
    private bool _bDelay = true;


    private void Awake()
    {
        shootDelay = Random.Range(3f, 7f);
        target = GameObject.FindGameObjectWithTag("LineController").transform;
        findObjectives();
    }

    public void findObjectives()
    {
        positionObjectives = GameObject.FindGameObjectsWithTag("Objectives");
    }


    // Update is called once per frame
    private void Start()
    {
        RandomizerShoot();
        //calcular a diferenša de anglo para os tiros
        _angleShoot = 360 / ammount;
    }
    public void RandomizerShoot()
    { 
        timeBetweenShot = Random.Range(timeBetweenShotMin, timeBetweenShotMax);
    }
    void Update()
    {

        if(canShootAll)
        {
            StartCoroutine(StartShootObjective());
            
        }

    }

    IEnumerator StartShootObjective()
    {
        if (!_isShootingObjective && canShootingObjective)
        {
            //delay
            if (_bDelay)
            {
                yield return new WaitForSeconds(shootDelay);
                _bDelay = false;
            }
            else
            {

                _isShootingObjective = true;
                var objTargeted = Random.Range(0, positionObjectives.Length);
                foreach (var d in poolingShoots)
                {
                    d.transform.position = positionToShootLine.position;
                    d.SetActive(true);
                    d.transform.DOMove((Vector2)positionObjectives[objTargeted].transform.position, velocityObjective).SetEase(Ease.Linear);    
                    yield return new WaitForSeconds(recoil);
                }
                yield return new WaitForSeconds(timeBetweenShotObjective);
                foreach (var a in poolingShoots)
                {
                    a.SetActive(false);
                }
                _isShootingObjective = false;
            }
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

            turn++;

        }

    }
    //when dead
    public void StopShoting()
    {
        foreach (var b in poolingShoots)
        {
            Destroy(b);
        }
        StopAllCoroutines();
        canShootAll = false;
    }
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
