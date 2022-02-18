using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Frozen_Bullet : MonoBehaviour
{
    private float maxY;
    private Material material;
    private Color init_color;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        init_color = material.color;
    }
    public void InitState(Vector3 vector)
    {
        maxY = vector.y;
        transform.localPosition = vector;
        transform.localScale = Vector3.one * Random.Range(0.3f,1);
        material.color = init_color;
        StartCoroutine(Animal());
    }
    IEnumerator Animal()
    {
        transform.DOLocalMoveY(maxY + 3, 0.5f);
        transform.DOScale(Vector3.one * 1.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOLocalMoveY(maxY + 5, 0.5f);
        transform.DOScale(Vector3.one*1.6f, 0.5f);
        material.DOFade(0,0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
