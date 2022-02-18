using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCandy : MonoBehaviour
{
    SkillItem skillItem;
    float lastTime;
    float snowTime;
    float hurt;
    float skillNum;
    GameObject skillPrefab;
    bool isTrue;
    public void Init(SkillItem item,GameObject go,float hurt)
    {
        isTrue = true;
        skillItem = item;
        lastTime = 0;
        this.hurt = hurt;
        skillPrefab = go;
        skillNum = item.num;
    }
    public void PlayAnim()
    {
        isTrue = false;
        CreateSkill();
        skillNum = 0;
    }
    private void Update()
    {
        if(gameObject.activeInHierarchy && isTrue)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= 0.1f)
            {
                lastTime = 0;
                gameObject.GetComponent<Collider>().enabled = true;
            }
            snowTime += Time.deltaTime;
            if (snowTime >= 0.5f && skillItem.id == "4")
            {
                snowTime=0;
                for (int i = 0; i < 2; i++)
                {
                    var snow = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
                    snow.gameObject.SetActive(true);
                    snow.transform.SetParent(transform.parent.parent);
                    snow.GetComponent<SkillSnow>().SetInit(new Vector3(Random.Range(-10, -6), 40, Random.Range(1, 80)), i * 0.2f, skillItem, hurt);
                }
                for (int i = 0; i < 2; i++)
                {
                    var snow = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
                    snow.gameObject.SetActive(true);
                    snow.transform.SetParent(transform.parent.parent);
                    snow.GetComponent<SkillSnow>().SetInit(new Vector3(Random.Range(7, 10), 40, Random.Range(1, 80)), i * 0.2f, skillItem, hurt);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && skillNum > 0 && isTrue)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            if (skillItem.id == "2")
            {
                var egg = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
                egg.gameObject.SetActive(true);
                egg.transform.SetParent(transform.parent.parent);
                egg.GetComponent<SkillLaser>().SetInit(new Vector3(other.transform.position.x, 15, other.transform.position.z), 0.1f, skillItem, hurt);
            }
            else if (skillItem.id == "4")
            {
                var snow = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
                snow.gameObject.SetActive(true);
                snow.transform.SetParent(transform.parent.parent);
                snow.GetComponent<SkillSnow>().SetInit(new Vector3(other.transform.position.x, 40, other.transform.position.z), 0.1f, skillItem, hurt);
                snow.GetComponent<SkillSnow>().ColePrefab();
            }
            skillNum -= 1;
        }
    }

    private void CreateSkill()
    {
        if (skillItem.id == "2")
        {
            for (int i = 0; i < skillNum; i++)
            {
                var egg = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);//Instantiate(eggPrefab);
                egg.gameObject.SetActive(true);
                egg.transform.SetParent(transform.parent.parent);
                egg.GetComponent<SkillLaser>().SetInit(new Vector3(Random.Range(-4, 4), 15, Random.Range(70, 90)), i * 0.2f, skillItem, hurt);
            }
        }
        else if (skillItem.id == "4")
        {
            for (int i = 0; i < skillNum; i++)
            {
                var snow = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);//Instantiate(eggPrefab);
                snow.gameObject.SetActive(true);
                snow.transform.SetParent(transform.parent.parent);
                snow.GetComponent<SkillSnow>().SetInit(new Vector3(Random.Range(-4, 4), 40, Random.Range(70, 90)), i * 0.2f, skillItem, hurt);
                snow.GetComponent<SkillSnow>().ColePrefab();
            }
        }
    }
}
