using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 技能激光
/// </summary>
public class SkillLaser : MonoBehaviour
{
    Transform columnA;//内光柱
    Transform columnB;//外光柱
    Transform columnC;//地光柱
    Transform columnD;//小光柱

    AudioSource source;
    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        columnA = transform.Find("1");
        columnB = transform.Find("2");
        columnC = transform.Find("3");
        columnD = transform.Find("4");
    }

    public void SetInit(Vector3 point,float interval,SkillItem item,float Hurt)
    {
        this.GetComponent<SkillHurt>().SetInit(item,Hurt);
        transform.localPosition = point;
        AudioManager.Instance.PlaySource("skill_2",source);
        int ran = Random.Range(2, columnD.childCount);
        for (int i = 0; i < ran; i++)
        {
            columnD.GetChild(i).DOLocalMoveY(-20,0);
            columnD.GetChild(i).GetComponent<Renderer>().material.DOFade(0.7f, 0f);
            StartCoroutine(SmallAnim(columnD.GetChild(i),i*0.3f));
        }
        StartCoroutine(Animation(interval));
    }
    IEnumerator Animation(float daly)
    {
        yield return new WaitForSeconds(daly);
        transform.GetComponent<Collider>().enabled = true;
        columnA.DOScale(new Vector3(1.1f,15, 1.1f),0.2f);
        columnA.GetComponent<Renderer>().material.DOFade(0.8f,0.2f);
        yield return new WaitForSeconds(0.2f);
        columnA.DOScale(new Vector3(0.9f, 15, 0.9f), 0.5f);
        columnA.GetComponent<Renderer>().material.DOFade(0.9f, 0.5f);
        columnB.DOScale(new Vector3(2f, 15f, 2f), 0.2f);
        columnB.GetComponent<Renderer>().material.DOFade(0.6f, 0.2f);
        columnC.DOScale(new Vector3(4f, 0.2f, 4f), 0.2f);
        columnC.GetComponent<Renderer>().material.DOFade(1f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        columnB.DOScale(new Vector3(2.4f, 15f, 2.4f), 0.5f);
        columnB.GetComponent<Renderer>().material.DOFade(0.3f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        columnA.DOScale(new Vector3(0, 15, 0), 0.5f);
        columnA.GetComponent<Renderer>().material.DOFade(0f, 0.5f);
        columnB.DOScale(new Vector3(0, 15f, 0), 0.8f);
        columnB.GetComponent<Renderer>().material.DOFade(0, 0.8f);
        columnC.DOScale(new Vector3(4f, 0, 4f), 1);
        columnC.GetComponent<Renderer>().material.DOFade(0f, 1);
        yield return new WaitForSeconds(1f);
        transform.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(gameObject);
        //ObjectPool.Instance.CollectObject(gameObject);
    }
    IEnumerator SmallAnim(Transform small,float daly)
    {
        yield return new WaitForSeconds(daly);
        small.DOLocalMoveY(45, 6);
        small.GetComponent<Renderer>().material.DOFade(0f, 6);
    }
}
