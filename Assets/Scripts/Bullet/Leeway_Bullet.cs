using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Leeway_Bullet : MonoBehaviour
{
    private Transform[] circle;

    private Vector3 circle_scale;
    private Vector3 self_scale;
    private ParticleSystem particle;//CartoonyBodySlam
    private void Awake()
    {
        particle = transform.Find("MagicFieldYellow").GetComponent<ParticleSystem>();
        circle = new Transform[transform.childCount];
        for (int i = 0; i < 8; i++)
        {
            circle[i] = transform.GetChild(i);
        }
        circle_scale = circle[0].localScale;
        transform.localScale = Vector3.zero;
    }

    public void OpenAnimal(float distance)
    {
        self_scale = Vector3.one*distance;
        StartCoroutine(Animal());
    }
    IEnumerator Animal()
    {
        for (int i = 0; i < circle.Length; i++)
        {
            circle[i].DOScale(circle_scale,0.1f);
        }
        transform.DOScale(self_scale, 0.1f);
        yield return new WaitForSeconds(0.1f);
        particle.Play();
        for (int i = 0; i < circle.Length; i++)
        {
            circle[i].DOScale(circle_scale*1.5f, 0.3f);
        }
        transform.DOScale(self_scale * 1.2f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < circle.Length; i++)
        {
            circle[i].DOScale(circle_scale * 1.6f, 0.3f);
        }
        transform.DOScale(Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        particle.Stop();
        gameObject.SetActive(false);
    }

}
