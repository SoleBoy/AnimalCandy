using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLittle : MonoBehaviour
{
    private Transform bear_Object;
    private Transform ball_Object;
    private ParticleSystem particle;
    private AudioSource source;
    private Vector3 scale_bear;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        particle = transform.Find("MagicChargeBlue").GetComponent<ParticleSystem>();
        bear_Object = transform.Find("Bear");
        ball_Object = transform.Find("Sphere");
        scale_bear = new Vector3(400,600,400);
    }

    public void SetInit(SkillItem item, float hurt)
    {
        bear_Object.localScale = scale_bear;
        bear_Object.localEulerAngles = Vector3.zero;
        ball_Object.localScale = Vector3.zero;
        ball_Object.GetComponent<SkillHurt>().SetInit(item, hurt);
        StartCoroutine(OpenAnimal());
    }

    private IEnumerator OpenAnimal()
    {
        bear_Object.DOLocalMoveY(3, 0.2f);
        bear_Object.DOScaleY(300, 0.2f);
        particle.Play();
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.PlaySource("crocodile_1", source);
        bear_Object.DOLocalRotate(Vector3.right*90,0.5f);
        bear_Object.DOScaleY(600, 0.5f);
        ball_Object.DOScale(Vector3.one*12,0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOMoveZ(90, 3);
        ball_Object.DOScale(Vector3.one * 15, 0.5f);
        yield return new WaitForSeconds(0.6f);
        ball_Object.DOScale(Vector3.one * 12, 0.5f);
        yield return new WaitForSeconds(0.6f);
        ball_Object.DOScale(Vector3.one * 15, 0.5f);
        yield return new WaitForSeconds(2f);
        ball_Object.DOScale(Vector3.zero, 0.5f);
        bear_Object.DOLocalMoveY(35, 0.5f);
        particle.Stop();
        yield return new WaitForSeconds(0.5f);
        GameObject.Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
