using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> TriggerPoints;

    [Header("Animation")]
    public Animator animator;

    [Header("Movement Config")]
    public bool isMover;
    public float stopDuration;
    public float movementSpeed; //Speed that the enemy moves to one place to another
    public Ease ease = Ease.OutBack;
    public GameObject objAttack;

    [Header("Jump Config")]
    public bool isJumper;
    public float jumpStopTimeMax;
    public float jumpStopTimeMin;

    [Header("Basher Config")]
    public bool isBasher;

    [Header("Flash Config")]
    public bool isFlash;
    public int jumpPoints;
    public float TimeStopPointsFlashs;
    public float TimeBtwnPointsFlashs;

    [Header("Dugtrio Config")]
    public bool isDugtrio;
    public bool undergroundState = false;
    public SpriteRenderer spriteTemp;

    private bool _canMove = true; //movment controll for shooting
    private bool _isJumper = true; //jumper controll
    private bool _isBasher = false; //basher controll
    
    private GunBase gunBase;
    private int _point;
    private bool moverOriginal;
    private IEnumerator enumerator;
    private IEnumerator enumeratorJ;
    

    public void Awake()
    {
        moverOriginal = isMover;
        var triggerList = GameObject.FindGameObjectsWithTag("WalkingTarget");
        foreach (var i in triggerList)
        {
            TriggerPoints.Add(i.transform);
        }
        enumerator = MoveToNextPlace();
        enumeratorJ = MoveToNextPlace();
    }
    private void Start()
    {
        gunBase = objAttack.GetComponent<GunBase>();
    }

    private void Update()
    {

        if (!isJumper) StartCoroutine(MoveToNextPlace());
        else if (isJumper) StartCoroutine(JumpToNextPlace());
    }


    #region Movement

    public string Direction(Vector2 endPosition, Vector2 startPosition)
    {
        Vector2 direction = endPosition - startPosition;

        // Check the direction and prompt accordingly
        if (direction.x > 0)
        {
            Debug.Log("Right");
            return "Right";
        }
        else if (direction.x < 0)
        {
            Debug.Log("Left");
            return "Left";
            
        }
        else if (direction.y > 0)
        {
            Debug.Log("Up");
            return "Up";
            
        }
        else if (direction.y < 0)
        {
            Debug.Log("Down");
            return "Down";
        }
        else
        {
            return null;
        }
    }

    IEnumerator MoveToNextPlace()
    {
        Sequence mySequence = DOTween.Sequence();
        //NORMAL MOVEMENT
        if (_canMove && isMover && !isBasher && !isFlash && !isDugtrio)
        {
            var pointNew = Random.Range(0, TriggerPoints.Count);
            if (pointNew != _point)
            {
                _point = pointNew;
                gunBase.canShootingObjective = false;
                _canMove = false;
                //animation trigger
                animator.SetTrigger(Direction(TriggerPoints[_point].transform.position, gameObject.transform.position));
                gameObject.transform.
                    DOMove(TriggerPoints[_point].transform.position, movementSpeed).SetEase(ease); 
                yield return new WaitForSeconds(movementSpeed);
                animator.SetTrigger("PartialIdle");
                gunBase.canShootingObjective = true;
                yield return new WaitForSeconds(stopDuration);
                animator.SetTrigger("Idle");
                _canMove = true;
            }
            
        }
        else if (!isMover)
        {
            gunBase.canShootingObjective = true;
        }
        //BASHER MOVEMENT
        else if (!_isBasher && isBasher && _canMove)
        {
            var pointNew = Random.Range(0, TriggerPoints.Count);
            if (pointNew != _point)
            {
                _point = pointNew;
                gunBase.canShootingObjective = false;
                _canMove = false;
                _isBasher = true;
                gameObject.transform.
                    DOMove(TriggerPoints[_point].transform.position, movementSpeed).SetEase(ease);
                yield return new WaitForSeconds(movementSpeed);
                gunBase.canShootingObjective = true;
                _isBasher = false;
                yield return new WaitForSeconds(stopDuration);
                _canMove = true;
            }
        }
        //FLASH MOVEMENT
        else if (isFlash && _canMove)
        {
            var pointNew = Random.Range(0, TriggerPoints.Count);
            if (pointNew != _point)
            {
                _point = pointNew;
                gunBase.canShootingObjective = false;
                _canMove = false;
                for (var x = 0; x < jumpPoints; x++)
                {
                    if (_point+x >= TriggerPoints.Count) _point = 0;
                    gameObject.transform.
                        DOMove(TriggerPoints[_point+x].transform.position, TimeBtwnPointsFlashs).SetEase(ease);
                    yield return new WaitForSeconds(TimeStopPointsFlashs);
                }
                gunBase.canShootingObjective = true;
                yield return new WaitForSeconds(stopDuration);
                _canMove = true;
            }
        }
        //DUGTRIO MOVEMENT
        else if (isDugtrio && _canMove)
        {
            var pointNew = Random.Range(0, TriggerPoints.Count);
            if (pointNew != _point)
            {
                _point = pointNew;
                gunBase.canShootingObjective = false;
                _canMove = false;
                undergroundState = true;
                spriteTemp.color = Color.green;
                gameObject.transform.
                    DOMove(TriggerPoints[_point].transform.position, movementSpeed).SetEase(ease);
                yield return new WaitForSeconds(movementSpeed);
                spriteTemp.color = Color.white;
                gunBase.canShootingObjective = true;
                undergroundState = false;
                yield return new WaitForSeconds(stopDuration);
                _canMove = true;
            }
        }
    }

    //JUMPER MOVEMENT
    IEnumerator JumpToNextPlace()
    {
        if (isJumper && _isJumper)
        {
            var pointNew = Random.Range(0, TriggerPoints.Count);
            if (pointNew != _point)
            {
                _isJumper = false;
                float jumpStopTime = Random.Range(jumpStopTimeMin, jumpStopTimeMax);
                _point = pointNew;
                gunBase.canShootingObjective = false;

                gameObject.transform.position = TriggerPoints[_point].transform.position;
                gunBase.canShootingObjective = true;
                yield return new WaitForSeconds(jumpStopTime);
                _isJumper = true;
            }

        }
        else if (!isMover)
        {
            gunBase.canShootingObjective = true;
        }
    }
    #endregion

    public void StartTunder()
    {
        StopCoroutine(enumerator);
        StopCoroutine(enumeratorJ);
        DOTween.KillAll();
        foreach (var i in gunBase.poolingShoots)i.SetActive(false);
        var moverOriginal = isMover;
        isMover = false;
    }
    public void StopTunder()
    {
        isMover = moverOriginal;
        StartCoroutine(enumerator);
        StartCoroutine(enumeratorJ);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LineController" && _isBasher)
        {
            collision.GetComponent<HealthBase>().Damage(1);
        }
    }
}
