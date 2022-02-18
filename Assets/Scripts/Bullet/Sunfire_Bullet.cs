using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sunfire_Bullet : MonoBehaviour
{
    private Transform dot_sphere;

    private Material material;
    private Color color;
    private Vector3 dot_scale;
    private Transform[] dot_all;

    private ParticleSystem particle;
    private float scaleSize;
    private void Awake()
    {
        particle = transform.Find("PowerupGlow2").GetComponent<ParticleSystem>();
        dot_sphere = transform.Find("Dot");
        dot_all = new Transform[dot_sphere.childCount];
        for (int i = 0; i < dot_sphere.childCount; i++)
        {
            dot_all[i] = dot_sphere.GetChild(i);
        }
        material = GetComponent<Renderer>().material;
        dot_scale = dot_all[0].localScale;
        color = material.color;
        color.a = 0.5f;
    }

    public void OpenAnimal(float distance)
    {
        scaleSize = distance;
        material.color = color;
        for (int i = 0; i < dot_all.Length; i++)
        {
            dot_all[i].gameObject.SetActive(false);
        }
        transform.DOScale(scaleSize, 0.1f);
        StartCoroutine(Animal());
    }
    IEnumerator Animal()
    {
        for (int i = 0; i < dot_all.Length; i++)
        {
            StartCoroutine(CreateDot(dot_all[i],i));
        }
        yield return new WaitForSeconds(0.1f);
        particle.Play();
        transform.DOScale(scaleSize*1.1f, 0.5f);
        material.DOFade(0.3f,0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOScale(scaleSize * 1.2f, 0.5f);
        material.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        particle.Stop();
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    IEnumerator CreateDot(Transform dot,int index)
    {
        yield return new WaitForSeconds(index * 0.1f);
        dot.gameObject.SetActive(true);
        dot.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f));
        yield return new WaitForSeconds(0.1f);
        float max = Random.Range(1f,1.6f);
        dot.DOScale(dot_scale* max, 0.2f);
        yield return new WaitForSeconds(0.2f);
        max += 0.2f;
        dot.DOScale(dot_scale * max, 0.2f);
        yield return new WaitForSeconds(0.2f);
        dot.gameObject.SetActive(false);
    }
}
