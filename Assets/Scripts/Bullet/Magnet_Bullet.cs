using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Magnet_Bullet : MonoBehaviour
{
    private Transform sphere;
    private Transform ring_1;
    private Transform ring_2;
    private Transform ring_3;

    private Material material_sphere;
    private Material material_ring1;
    private Material material_ring2;
    private Material material_ring3;

    private Color color;

    private Vector3 scale_sphere;
    private Vector3 scale_ring;
    private bool isOpen;
    private void Awake()
    {
        isOpen = true;
        sphere = transform.Find("Sphere");
        ring_1 = transform.Find("Ring1");
        ring_2 = transform.Find("Ring2");
        ring_3 = transform.Find("Ring3");
        material_sphere = sphere.GetComponent<Renderer>().material;
        material_ring1 = ring_1.GetComponent<Renderer>().material;
        material_ring2 = ring_2.GetComponent<Renderer>().material;
        material_ring3 = ring_3.GetComponent<Renderer>().material;
        color = material_sphere.color;
        //color.a = 0.4f;
    }

    public void OpenAnimal(float distance)
    {
        if(isOpen)
        {
            isOpen = false;
            transform.localScale = Vector3.one * distance;
            scale_sphere = sphere.localScale;
            scale_ring = ring_1.localScale;
        }
        material_sphere.color = color;
        material_ring1.color = color;
        material_ring2.color = color;
        material_ring3.color = color;
        transform.DOScale(Vector3.one * distance, 0.1f);
        StartCoroutine(Animal());
    }

    IEnumerator Animal()
    {
        sphere.DOScale(scale_sphere,0.1f);
        ring_1.DOScale(scale_ring,0.1f);
        ring_2.DOScale(scale_ring, 0.1f);
        ring_3.DOScale(scale_ring, 0.1f);
        yield return new WaitForSeconds(0.1f);
        sphere.DOScale(scale_sphere*1.2f, 0.5f);
        ring_1.DOScale(scale_ring*1.5f, 0.5f);
        ring_2.DOScale(scale_ring*1.5f, 0.5f);
        ring_3.DOScale(scale_ring*1.5f, 0.5f);
        material_sphere.DOFade(0.8f,0.5f);
        material_ring1.DOFade(0.6f, 0.5f);
        material_ring2.DOFade(0.6f, 0.5f);
        material_ring3.DOFade(0.6f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        sphere.DOScale(scale_sphere * 1.3f, 0.5f);
        ring_1.DOScale(scale_ring * 1.6f, 0.5f);
        ring_2.DOScale(scale_ring * 1.6f, 0.5f);
        ring_3.DOScale(scale_ring * 1.6f, 0.5f);
        material_sphere.DOFade(0, 0.5f);
        material_ring1.DOFade(0, 0.5f);
        material_ring2.DOFade(0, 0.5f);
        material_ring3.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

}
