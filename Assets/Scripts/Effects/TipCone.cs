using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TipCone : MonoBehaviour
{
    public bool isLeft;
    public float maxY;
    public float minY;

    float lastTime = 1;
    public void SetPoint(Vector3 point)
    {
        maxY = point.y + 1.5f;
        minY = point.y;
        transform.localPosition = point;
    }
    private void OnEnable()
    {
        lastTime = 1;
    }
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= 1f)
            {
                lastTime = 0;
                if (isLeft)
                {
                    StartCoroutine(LeftMove());
                }
                else
                {
                    StartCoroutine(DelayMove());
                }
            }
        }
    }
    IEnumerator DelayMove()
    {
        transform.DOLocalMoveY(maxY, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOLocalMoveY(minY, 0.5f);
    }
    IEnumerator LeftMove()
    {
        transform.DOLocalMoveX(maxY, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOLocalMoveX(minY, 0.5f);
    }
}
