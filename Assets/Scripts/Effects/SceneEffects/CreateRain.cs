using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CreateRain : MonoBehaviour, WeatherMaager
{
    //下雨
    public List<GameObject> rains = new List<GameObject>();
    GameObject rainPrefab;
    Vector3 initScal;
    float rainTime;
    bool isCreate;
    void Awake()
    {
        rainPrefab = transform.Find("rain").gameObject;
        initScal = rainPrefab.transform.localScale;
    }
    public void Weather(bool iswather,bool hide)
    {
        if (gameObject.activeInHierarchy)
        {
            isCreate = iswather;
            if (hide)
            {
                rains.Clear();
                ObjectPool.Instance.Clear(rainPrefab.name);
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
        rains.Clear();
        ObjectPool.Instance.Clear(rainPrefab.name);
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (isCreate)
        {
            if (rains.Count <= 50)
            {
                rainTime += Time.deltaTime;
                if (rainTime >= 0.1f)
                {
                    rainTime = 0;
                    var rain = ObjectPool.Instance.CreateObject(rainPrefab.name, rainPrefab);
                    rain.transform.SetParent(transform);
                    rain.transform.localScale = initScal;
                    rain.transform.localPosition = new Vector3(Random.Range(-15, 15), 30, Random.Range(-15, 45));
                    rains.Add(rain);
                }
            }
        }
    }
}
