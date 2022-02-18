using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSnow : MonoBehaviour, WeatherMaager
{
    //雪
    public List<GameObject> snows = new List<GameObject>();
    GameObject snowPrefab;
    float snowTime;
    bool isCreate;
    private void Awake()
    {
        snowPrefab = transform.Find("snow").gameObject;
    }
    public void Weather(bool iswather,bool hide)
    {
        if (gameObject.activeInHierarchy)
        {
            isCreate = iswather;
            if (hide)
            {
                snows.Clear();
                ObjectPool.Instance.Clear(snowPrefab.name);
                gameObject.SetActive(false);
                return;
            }
            if (!iswather)
            {
                StartCoroutine(HideObject());
            }
        }
    }
    IEnumerator HideObject()
    {
        yield return new WaitForSeconds(2);
        snows.Clear();
        ObjectPool.Instance.Clear(snowPrefab.name);
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(isCreate)
        {
            if (snows.Count <= 50)
            {
                snowTime += Time.deltaTime;
                if (snowTime >= 0.2f)
                {
                    snowTime = 0;
                    var snow = ObjectPool.Instance.CreateObject(snowPrefab.name, snowPrefab);
                    snow.transform.SetParent(transform);
                    snow.transform.localScale = Vector3.one * Random.Range(0.5f, 1f);
                    snow.transform.localEulerAngles = new Vector3(Random.Range(-60, -40), 90, -90);
                    snow.transform.localPosition = new Vector3(-30, 30, Random.Range(-5, 80));
                    snows.Add(snow);
                }
            }
        }
    }
}
