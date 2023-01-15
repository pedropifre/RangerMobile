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
     

    private bool _canMove = true;
    private GunBase gunBase;

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
        if (_canMove)
        {
            gunBase.canShootingObjective = false;
            _canMove = false;
            var point = Random.Range(0, TriggerPoints.Count);
            gameObject.transform.DOMove(TriggerPoints[point].transform.position, movementSpeed).SetEase(ease); ;
            yield return new WaitForSeconds(movementSpeed);
            gunBase.canShootingObjective = true;
            yield return new WaitForSeconds(stopDuration);
            _canMove = true;
        }
    }
}
