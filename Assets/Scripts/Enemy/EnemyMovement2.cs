using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMovement2 : MonoBehaviour
{
    public List<Transform> TriggerPoints;

    [Header("Movement Config")]
    public float stopDuration;
    public float movementSpeed; //Speed that the enemy moves to one place to another
    public Ease ease = Ease.OutBack;
    public GameObject objAttack;
    public bool canMove;
     

    private bool _canMove = true;
    private GunBase gunBase;
    private int _point;

    

    private void Start()
    {
        gunBase = objAttack.GetComponent<GunBase>();
    }

    private void Update()
    {
        StartCoroutine(MoveToNextPlace());
    }

    IEnumerator MoveToNextPlace()
    {
        if (_canMove && canMove)
        {
            var pointNew = Random.Range(0, TriggerPoints.Count);
            if (pointNew != _point)
            {
                _point = pointNew;
                gunBase.canShootingObjective = false;
                _canMove = false;
                gameObject.transform.DOMove(TriggerPoints[_point].transform.position, movementSpeed).SetEase(ease); ;
                yield return new WaitForSeconds(movementSpeed);
                gunBase.canShootingObjective = true;
                yield return new WaitForSeconds(stopDuration);
                _canMove = true;
            }
            
        }
        else if (!canMove)
        {
            gunBase.canShootingObjective = true;
        }
    }
}
