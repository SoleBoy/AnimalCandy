using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flakes_Bullet : MonoBehaviour
{
    private Material material;
    private Color color;
    private ParticleSystem particle; //MagicExplosionBlue
    private float distance;
    private void Awake()
    {
        particle = transform.Find("MagicExplosionBlue").GetComponent<ParticleSystem>();
        material = transform.Find("xh").GetComponent<Renderer>().material;
        color = material.color;
        color.a = 0.5f;
        transform.localScale = Vector3.zero;
    }
    public void OpenAnimal(float distance)
    {
        this.distance = distance;
        material.color = color;
        StartCoroutine(Animal());
    }

    private IEnumerator Animal()
    {
        particle.Play();
        transform.DOScale(Vector3.one * distance, 0.2f);
        yield return new WaitForSeconds(0.2f);
        material.DOFade(0.8f, 0.5f);
        transform.DOScale(Vector3.one * distance*1.1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        material.DOFade(0, 0.5f);
        transform.DOScale(Vector3.one * distance * 1.15f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        particle.Stop();
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}
