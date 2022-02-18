using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod_Bullet : MonoBehaviour
{
    private Vector3 rod_scale;

    private bool isRotate;
    private ParticleSystem particle;
    private void Awake()
    {
        particle = transform.Find("SparkleYellow").GetComponent<ParticleSystem>();
        rod_scale = new Vector3(0,0,1);
    }
    public void OpenAnimal(float distance)
    {
        rod_scale.z = distance;
        transform.localScale = Vector3.one;
        transform.DOScaleZ(distance, 0.2f);
        transform.localEulerAngles = -transform.parent.localEulerAngles;
        StartCoroutine(Animal());
    }

    private IEnumerator Animal()
    {
        particle.Play();
        yield return new WaitForSeconds(0.2f);
        isRotate = true;
        yield return new WaitForSeconds(2);
        isRotate = false;
        transform.DOScale(rod_scale, 0.2f);
        yield return new WaitForSeconds(0.2f);
        particle.Stop();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(isRotate)
        {
            transform.Rotate(-Vector3.up * Time.deltaTime* 180, Space.World);
        }
    }
}
