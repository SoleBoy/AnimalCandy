using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gasoline_Bullet : MonoBehaviour
{
    private ParticleSystem particle;
    private float distance;
    private void Awake()
    {
        particle = transform.Find("PowerupGlow6").GetComponent<ParticleSystem>();
        transform.localScale = Vector3.zero;
    }
    public void OpenAnimal(float distance)
    {
        this.distance = distance;
        StartCoroutine(Animal());
    }

    private IEnumerator Animal()
    {
        transform.DOScale(Vector3.one* distance, 0.2f);
        yield return new WaitForSeconds(0.2f);
        particle.Play();
        transform.DOScale(Vector3.one * distance*1.2f, 2f);
        yield return new WaitForSeconds(2f);
        transform.DOScale(Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        particle.Stop();
        gameObject.SetActive(false);
    }
}
