using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 技能雪崩
/// </summary>
public class SkillSnow : MonoBehaviour
{
    List<Transform> snow_block=new List<Transform>();
    Transform snow_cloud;
    Vector3 sizeScale;
    GameObject snowPrefab;
    AudioSource source;
    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        for (int i = 1; i < 6; i++)
        {
            snow_block.Add(transform.Find("snow"+i));
        }
        snowPrefab = transform.Find("Sphere").gameObject;
        snow_cloud = transform.Find("snow_cloud");
    }
    public void SetInit(Vector3 point, float interval,SkillItem item, float Hurt)
    {
        transform.localPosition = point;
        int ran = Random.Range(0, 5);
        snow_block[ran].gameObject.SetActive(true);
        snow_block[ran].DOLocalMoveY(2f, 0);
        sizeScale = new Vector3(Random.Range(150, 200), 800, Random.Range(50, 100));
        snow_block[ran].GetComponent<Renderer>().material.DOFade(1, 0);
        snow_block[ran].localScale = sizeScale;
        snow_cloud.localScale = Vector3.zero;
        snow_cloud.DOLocalMoveY(-1.5f, 0);
        snow_cloud.GetComponent<Renderer>().material.DOFade(0.8f, 0);
        snow_cloud.GetComponent<SkillHurt>().SetInit(item,Hurt);
        StartCoroutine(SnowAnimator(ran, interval));
    }
    IEnumerator SnowAnimator(int index,float daly)
    {
        yield return new WaitForSeconds(daly);
        transform.DOLocalMoveY(0, 0.2f);
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.PlaySource("skill_4", source);
        snow_block[index].DOScale(new Vector3(sizeScale.x*1.5f,50, sizeScale.z * 1.5f), 1);
        snow_cloud.DOLocalMoveY(-1f, 1);
        snow_cloud.DOScale(Vector3.one * 200, 1);
        snow_cloud.GetComponent<Renderer>().material.DOFade(0.3f, 1);
        yield return new WaitForSeconds(1);
        snow_block[index].DOScale(new Vector3(sizeScale.x * 1.7f, 0, sizeScale.z * 1.7f), 3);
        snow_block[index].GetComponent<Renderer>().material.DOFade(0.1f, 3);
        snow_cloud.DOLocalMoveY(1, 0.5f);
        snow_cloud.DOScale(Vector3.one * 250, 3);
        snow_cloud.GetComponent<Renderer>().material.DOFade(0, 3);
        yield return new WaitForSeconds(3);
        GameObject.Destroy(gameObject);
        //snow_block[index].gameObject.SetActive(false);
        //ObjectPool.Instance.CollectObject(gameObject);
    }
    public void ColePrefab()
    {
        for (int i = 0; i < 3; i++)
        {
            var spray = Instantiate(snowPrefab);//ObjectPool.Instance.CreateObject("Snowflakes", snowPrefab.gameObject);
            spray.gameObject.SetActive(true);
            spray.transform.SetParent(transform);
            spray.transform.eulerAngles = new Vector3(0, 0, Random.Range(-30, 40));
            spray.transform.localScale = Vector3.one * Random.Range(0.5f, 0.9f);
            spray.transform.localPosition = Vector3.zero;
            spray.transform.GetComponent<PixelBlock>().SetPower(15);
        }
    }
}
