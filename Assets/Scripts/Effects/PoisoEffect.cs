using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoisoEffect : MonoBehaviour
{
    private Material material;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;

    }
    public void SetPoint(Transform enemy,Color color)
    {
        Vector3 point= enemy.localPosition;
        point.x += Random.Range(0f,0.5f);
        point.y += 0.5f;
        transform.localPosition = point;
        transform.DOLocalMoveY(point.y+2,0.8f);
        color.a = 0.5f;
        material.color = color;
        StartCoroutine(HideObject());
    }

    IEnumerator HideObject()
    {
        yield return new WaitForSeconds(0.8f);
        ObjectPool.Instance.CollectObject(gameObject);
    }
}
