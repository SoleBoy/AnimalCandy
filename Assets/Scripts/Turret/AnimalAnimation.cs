using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AnimalState
{
    Normol,
    Attack,//攻击
    Walk,//走路
    Hit,//受伤
    Frozen, //冰冻
    Knock,//击飞
    Repel,//击退
}

public class AnimalAnimation : MonoBehaviour
{
    private Vector3 bodySize;
    private AnimalState animalState;
    private bool isStart;
    public void SetState()
    {
        isStart = true;
        bodySize = transform.localScale;
        animalState = AnimalState.Normol;
        InitStatus(AnimalState.Walk);
    }

    public void InitStatus(AnimalState state)
    {
        if (!isStart) return;
        StopAllCoroutines();
        transform.DOPause();
        animalState = state;
        switch (state)
        {
            case AnimalState.Attack:
                StartCoroutine(AttackState());
                break;
            case AnimalState.Walk:
                StartCoroutine(WalkState());
                break;
            case AnimalState.Hit:
                StartCoroutine(HitState());
                break;
            case AnimalState.Frozen:
                StartCoroutine(FrozenState()); 
                break;
            case AnimalState.Knock:
                StartCoroutine(KnockState());
                break;
            case AnimalState.Repel:
                StartCoroutine(RepelState());
                break;
            default:
                break;
        }
    }
    
    IEnumerator AttackState()
    {
        transform.DOScale(bodySize * 1.05f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(bodySize, 0.3f);
        yield return new WaitForSeconds(0.3f);
        InitStatus(AnimalState.Walk);
    }
    IEnumerator WalkState()
    {
        transform.DOScaleY(bodySize.y*0.9f,0.3f);
        transform.DOScaleX(bodySize.x*1.1f,0.3f);
        yield return new WaitForSeconds(0.3f);
        transform.DOScaleY(bodySize.y, 0.3f);
        transform.DOScaleX(bodySize.x, 0.3f);
        yield return new WaitForSeconds(0.3f);
        InitStatus(AnimalState.Walk);
    }
    IEnumerator HitState()
    {
        transform.DOScale(bodySize * 0.95f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(bodySize, 0.3f);
        yield return new WaitForSeconds(0.3f);
        InitStatus(AnimalState.Walk);
    }
    IEnumerator FrozenState()
    {
        transform.DOScale(bodySize * 0.95f, 0.1f);
        yield return new WaitForSeconds(1f);
        InitStatus(AnimalState.Walk);
    }
    IEnumerator KnockState()
    {
        transform.DOScaleX(bodySize.x * 1.1f, 0.1f);
        transform.DOScaleY(bodySize.y * 1.1f, 0.1f);
        transform.DOScaleZ(bodySize.z * 0.9f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(bodySize, 0.3f);
        yield return new WaitForSeconds(0.3f);
        InitStatus(AnimalState.Walk);
    }
    IEnumerator RepelState()
    {
        transform.DOScaleX(bodySize.x * 1.1f, 0.1f);
        transform.DOScaleY(bodySize.y * 1.1f, 0.1f);
        transform.DOScaleZ(bodySize.z * 0.9f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(bodySize, 0.3f);
        yield return new WaitForSeconds(0.3f);
        InitStatus(AnimalState.Walk);
    }

    private void OnDisable()
    {
        isStart = false;
        StopAllCoroutines();
        transform.DOPause();
    }
}
