using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 技能地震
/// </summary>
public class SkillQuake : MonoBehaviour
{
    private WaitForSeconds wait = new WaitForSeconds(1);
    private Transform prefab;
    private AudioSource source;
    private Color color;
    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        color = transform.GetComponent<Renderer>().material.color;
        prefab = Resources.Load<Transform>("Effects/PixelBlock");
        if (GameManager.Instance.modeSelection == "roude")
        {
            GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }

    public void SetInit(SkillItem item,float hurt,float interval,float index)
    {
        GetComponent<SkillHurt>().SetInit(item, hurt);
        StartCoroutine(QuakeAnim(interval, index));
        if(interval <= 0)
        {
            Debug.Log("skill_7");
            AudioManager.Instance.PlaySource("skill_7", source);
        }
    }
    IEnumerator QuakeAnim(float daly,float index)
    {
        yield return new WaitForSeconds(daly);
        transform.DOLocalMoveY(9,1);
        QuakePiecces(transform.localPosition.z);
        yield return new WaitForSeconds(2+(index * 0.2f));
        transform.DOLocalMoveY(-12, 1);
        yield return new WaitForSeconds(1);
        GameObject.Destroy(gameObject);
        //ObjectPool.Instance.CollectObject(gameObject);
    }

    void QuakePiecces(float z)
    {
        for (int i = 0; i < 15; i++)
        {
            var spray = Instantiate(prefab);//ObjectPool.Instance.CreateObject(prefab.name, prefab.gameObject);
            spray.SetParent(transform);
            spray.gameObject.SetActive(true);
            spray.transform.localScale = prefab.localScale * Random.Range(1f, 2f);
            spray.GetComponent<Renderer>().material.color = color;
            spray.transform.localEulerAngles=new Vector3(-45,0,0);
            spray.transform.localPosition = new Vector3(Random.Range(-4, 4),5, z-10);
            spray.transform.GetComponent<PixelBlock>().SetPower(Random.Range(15, 20));
        }
    }
}
