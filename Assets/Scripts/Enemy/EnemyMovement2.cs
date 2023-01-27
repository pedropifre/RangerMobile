using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMovement2 : MonoBehaviour
{
    public List<Transform> TriggerPoints;

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

    private bool _canMove = true;
    private bool _isJumper = true;
    private GunBase gunBase;
    private int _point;

    

    private void Start()
    {
        gunBase = objAttack.GetComponent<GunBase>();
    }

    private void Update()
    {
        if (!isJumper)  StartCoroutine(MoveToNextPlace());
        else if(isJumper) StartCoroutine(JumpToNextPlace());
    }

    #region Movement

    IEnumerator MoveToNextPlace()
    {
        if (_canMove && isMover)
        {
            var pointNew = Random.Range(0, TriggerPoints.Count);
            if (pointNew != _point)
            {
                _point = pointNew;
                gunBase.canShootingObjective = false;
                _canMove = false;
                gameObject.transform.DOMove(TriggerPoints[_point].transform.position, movementSpeed).SetEase(ease); 
                yield return new WaitForSeconds(movementSpeed);
                gunBase.canShootingObjective = true;
                yield return new WaitForSeconds(stopDuration);
                _canMove = true;
            }
            
        }
        else if (!isMover)
        {
            gunBase.canShootingObjective = true;
        }
    }
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

}
