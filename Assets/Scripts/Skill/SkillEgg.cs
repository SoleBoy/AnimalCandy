using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 技能鸡蛋
/// </summary>
public class SkillEgg : MonoBehaviour
{
    Transform protein;//蛋白
    Transform yolk;//蛋黄
    ParticleSystem smokeEff;

    Material eggMate;
    Material proteinMate;
    Material yolkMate;

    Color eggColor;
    Color yolkColor;

    AudioSource source;
    Vector3 starScale = new Vector3(1.5f, 2f, 1.5f);
    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        protein = transform.Find("protein");
        yolk = transform.Find("yolk");
        smokeEff = transform.Find("Smoke").GetComponent<ParticleSystem>();
        eggMate = transform.GetComponent<Renderer>().material;
        proteinMate = protein.GetComponent<Renderer>().material;
        yolkMate = yolk.GetComponent<Renderer>().material;
        eggColor = eggMate.color;
        yolkColor = yolkMate.color;
    }
    public void SetInit(float daly,SkillItem item,float hurt)
    {
        this.GetComponent<SkillHurt>().SetInit(item, hurt);
        protein.localPosition = Vector3.zero;
        yolk.localPosition = Vector3.zero;
        transform.localScale = starScale;
        protein.localScale = Vector3.one;
        yolk.localScale = Vector3.one * 0.5f;
        eggMate.color = eggColor;
        proteinMate.color = eggColor;
        yolkMate.color = yolkColor;
        StartCoroutine(Animation(daly));
       
    }

    IEnumerator Animation(float daly)
    {
        yield return new WaitForSeconds(daly);
        transform.DOLocalMoveY(0.5f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        smokeEff.Play();
        AudioManager.Instance.PlaySource("skill_1",source);
        protein.DOLocalMoveY(-0.3f, 0.2f);
        yolk.DOLocalMoveY(-0.2f, 0.2f);
        eggMate.DOFade(0.3f, 0.5f);
        transform.DOScale(starScale * 1.1f, 0.5f);
        protein.DOScale(new Vector3(3, 0.3f, 3), 0.6f);
        yolk.DOScale(new Vector3(1.5f, 0.25f, 1.5f), 0.5f);
        yield return new WaitForSeconds(0.6f);
        eggMate.DOFade(0, 0.7f);
        transform.DOScale(starScale * 1.15f, 0.7f);
        protein.DOScale(new Vector3(3.2f, 0f, 3.2f), 1.5f);
        proteinMate.DOFade(0, 1.5f);
        yolk.DOScale(new Vector3(2f, 0, 2f), 1.5f);
        yolkMate.DOFade(0, 1.5f);
        yield return new WaitForSeconds(1.5f);
        GameObject.Destroy(gameObject);
    }
}
