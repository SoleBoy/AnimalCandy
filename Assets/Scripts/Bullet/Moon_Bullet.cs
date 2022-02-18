using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Moon_Bullet : MonoBehaviour
{
    private Transform moon_tran;
    private Transform beam_tran;

    private Material material_moon;
    private Material material_beam;

    private Color moon_color;
    private Color beam_color;

    private Vector3 beam_scale;
    private Collider box_coll;
    private ParticleSystem particle;//CartoonyBodySlam
    private void Awake()
    {
        box_coll = transform.GetComponent<Collider>();
        moon_tran = transform.Find("meteorite_ball");
        beam_tran = transform.Find("Cylinder");
        particle = transform.Find("CartoonyBodySlam").GetComponent<ParticleSystem>();
        material_moon = moon_tran.GetComponent<Renderer>().material;
        material_beam = beam_tran.GetComponent<Renderer>().material;

        moon_color = material_moon.color;
        beam_color = material_beam.color;

        moon_color.a = 0.5f;
        beam_color.a = 0.6f;
        beam_scale = beam_tran.localScale;
    }

    public void OpenAnimal(float diatance)
    {
        material_moon.color = moon_color;
        material_beam.color = beam_color;
        moon_tran.localScale = Vector3.zero;
        beam_tran.localScale = beam_scale;
        transform.localScale = Vector3.one * diatance;
        StartCoroutine(Animal());
    }

    private IEnumerator Animal()
    {
        moon_tran.DOScale(Vector3.one,0.1f);
        beam_tran.DOScaleX(2, 0.1f);
        beam_tran.DOScaleZ(2, 0.1f);
        yield return new WaitForSeconds(0.1f);
        particle.Play();
        box_coll.enabled = true;
        moon_tran.DOScale(Vector3.one * 1.5f, 0.5f);
        material_moon.DOFade(0.7f,0.5f);
        material_beam.DOFade(0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        moon_tran.DOScale(Vector3.one * 2, 0.5f);
        material_moon.DOFade(0, 0.5f);
        material_beam.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        particle.Stop();
        box_coll.enabled = false;
        gameObject.SetActive(false);
    }
}
