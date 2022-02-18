using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Icecone_Bullet : MonoBehaviour
{
    private Material material;
    private Color color;
    private void Awake()
    {
        material = transform.Find("Cube").GetComponent<Renderer>().material;
        color = material.color;
    }

    public void OpenAnimal()
    {
        material.color = color;
        transform.localScale = Vector3.one;
        StartCoroutine(Animal());
    }

    private IEnumerator Animal()
    {
        transform.DOScale(Vector3.one*2,0.3f);
        yield return new WaitForSeconds(0.3f);
        transform.DOScale(Vector3.one * 3, 0.3f);
        material.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
