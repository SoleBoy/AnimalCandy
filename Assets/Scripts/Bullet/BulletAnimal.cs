using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimal : MonoBehaviour
{
    private GameObject effectHalo;
    private Material material;
    private void Awake()
    {
        effectHalo =transform.Find("Halo").gameObject;
        material = transform.Find("Cube").GetComponent<Renderer>().material;
        effectHalo.gameObject.SetActive(false);
        transform.localScale = Vector3.one;
    }

    public void OpenAnimal()
    {
        effectHalo.gameObject.SetActive(true);
        StartCoroutine(Bane());
    }
    IEnumerator Bane()
    {
        transform.DOScale(transform.localScale * 2, 0.2f);
        material.DOFade(0.7f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.DOScale(transform.localScale * 2.5f, 0.8f);
        material.DOFade(0f, 0.8f);
        yield return new WaitForSeconds(0.8f);
        material.DOFade(1f, 0);
        effectHalo.gameObject.SetActive(false);
        transform.localScale = Vector3.one;
        ObjectPool.Instance.CollectObject(gameObject);
    }
}
