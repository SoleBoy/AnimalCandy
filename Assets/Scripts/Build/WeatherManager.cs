using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeatherManager : MonoBehaviour
{

    public GameObject[] cloudPrefab;
    public GameObject rainPrefab;
    public GameObject sandPrefab;
    public GameObject leafPrefab;
    public int index;
   
    Vector3 rainScale;

    float interval;
    float lastTime;
    float sumTime;
    void Start()
    {
        rainScale = rainPrefab.transform.localScale;
        index = Random.Range(0,4);
        RanIndex();
    }


    void Update()
    {
        sumTime += Time.deltaTime;
        if (sumTime <= 60)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= interval)
            {
                lastTime = 0;
                if (index == 0)
                {
                    CloneRain();
                }
                else if (index == 1)
                {
                    CloneLeft();
                }
                else if (index == 2)
                {
                    CloneSand();
                }
                else if (index == 3)
                {
                    CloneCloud();
                }
            }
        }
        else if (sumTime >= 65)
        {
            sumTime = 0;
            index += 1;
            if (index > 3)
            {
                index = 0;
            }
            RanIndex();
        }
    }
    void RanIndex()
    {
        if (index == 0)
        {
            interval = 0.2f;
        }
        else if (index == 1)
        {
            interval = 0.3f;
        }
        else if (index == 2)
        {
            interval = 0.3f;
        }
        else if (index == 3)
        {
            interval = 5f;
        }
    }
    void CloneRain()
    {
        var rain = ObjectPool.Instance.CreateObject(rainPrefab.name, rainPrefab);
        rain.transform.SetParent(transform);
        rain.transform.localScale = rainScale;
        rain.transform.localPosition = new Vector3(Random.Range(-60, 60), UIBase.Instance.height+20, Random.Range(-60, 60));
    }

    void CloneLeft()
    {
        var leaf = ObjectPool.Instance.CreateObject(leafPrefab.name, leafPrefab);
        leaf.transform.SetParent(transform);
        leaf.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
        leaf.transform.localEulerAngles = new Vector3(Random.Range(20, 60), 0, 0);
        leaf.transform.localPosition = new Vector3(Random.Range(-60, 60), UIBase.Instance.height + 20, Random.Range(-35, 0));
    }

    void CloneSand()
    {
        var sand = ObjectPool.Instance.CreateObject(sandPrefab.name, sandPrefab);
        sand.transform.SetParent(transform);
        sand.transform.localScale = Vector3.one * Random.Range(0.1f, 0.3f);
        sand.transform.localEulerAngles = new Vector3(Random.Range(20, 60), 0, 0);
        sand.transform.localPosition = new Vector3(Random.Range(-60, 60), UIBase.Instance.height + 20, Random.Range(-35, 0));
    }

    void CloneCloud()
    {
        int ran = Random.Range(0, cloudPrefab.Length);
        var cloud = ObjectPool.Instance.CreateObject(cloudPrefab[ran].name, cloudPrefab[ran]);
        cloud.transform.SetParent(transform);
        cloud.transform.localScale = Vector3.zero;
        Vector3 scale= new Vector3(Random.Range(0.8f, 1.5f), Random.Range(0.5f, 1.2f), Random.Range(0.5f, 1.3f));
        cloud.transform.DOScale(scale,0.5f);
        //cloud.transform.localEulerAngles = new Vector3(Random.Range(-60, -40), 0, 0);
        cloud.transform.localPosition = new Vector3(Random.Range(-60, 60), UIBase.Instance.height+30, 300);
    }

    
}
