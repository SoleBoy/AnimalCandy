using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cluster_Bullet : MonoBehaviour
{
    private Transform roud_1;
    private Transform roud_2;

    private Material material_1;
    private Material material_2;

    private Color color;
    private void Awake()
    {
        roud_1 = transform.Find("round1");
        roud_2 = transform.Find("round2");
        material_1 = roud_1.GetComponent<Renderer>().material;
        material_2 = roud_2.GetComponent<Renderer>().material;
        color = material_1.color;
        color.a = 0.5f;
    }
    public void OpenAnimal()
    {
        material_1.color = color;
        material_2.color = color;
        roud_1.localScale = Vector3.zero;
        roud_2.localScale = Vector3.zero;
        StartCoroutine(Animal());
    }

    IEnumerator Animal()
    {
        roud_1.DOScale(Vector3.one*1.5f,0.1f);
        yield return new WaitForSeconds(0.1f);
        roud_1.DOScale(Vector3.one * 1.6f, 0.5f);
        roud_2.DOScale(Vector3.one*2.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        roud_1.DOScale(Vector3.one * 1.7f, 0.5f);
        roud_2.DOScale(Vector3.one*2.6f, 0.5f);
        material_1.DOFade(0,0.5f);
        material_2.DOFade(0,0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
