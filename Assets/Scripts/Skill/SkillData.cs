using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    public Color color;
    public SkillItem item;
    public float hurt;

    private GameObject snowPrefab;
    private Vector3 snowScale;
    private Color color_yun;
    private void Awake()
    {
        if (transform.name == "Skill13")
        {
            snowPrefab = transform.parent.Find("yun1_1").gameObject;
            snowScale = snowPrefab.transform.localScale;
            color_yun = snowPrefab.GetComponent<Renderer>().material.color;
            color_yun.a = 0.8f;
        }
    }

    public void SetInit(SkillItem item, float hurt)
    {
        this.item = item;
        this.hurt = hurt;
    }

    public float MaxHurt(bool isBoos, float maxHp)
    {
        if (isBoos)
        {
            return 0.01f * item.boss_atk_percentage * maxHp + hurt;
        }
        else
        {
            return item.atk_percentage * 0.01f * maxHp + hurt;
        }
    }

    public void CreateEffeats(Transform enemy)
    {
        if(transform.name == "Skill13")
        {
            int ran = Random.Range(1,11);
            if(ran <= 2)
            {
                CreateSnow(enemy.position);
            }
        }   
    }

    private void CreateSnow(Vector3 point)
    {
        var snow = ObjectPool.Instance.CreateObject("skill13yun", snowPrefab);
        snow.transform.SetParent(null);
        snow.transform.localPosition = point;
        snow.transform.localScale = snowScale * Random.Range(0.5f, 2f);
        Material material = snow.GetComponent<Renderer>().material;
        material.color = color_yun;
        material.DOFade(0.3f,5);
        snow.transform.DOMoveY(15, 4);
        ObjectPool.Instance.CollectObject(snow, 5);
    }

}
