using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSand : MonoBehaviour, WeatherMaager
{
    //沙块
    public List<GameObject> sands = new List<GameObject>();
    GameObject sandPrefab;
    float sandTime;
    bool isCreate;
    private void Awake()
    {
        sandPrefab = transform.Find("sand").gameObject;
    }
    public void Weather(bool iswather,bool hide)
    {
        if (gameObject.activeInHierarchy)
        {
            isCreate = iswather;
            if (hide)
            {
                sands.Clear();
                ObjectPool.Instance.Clear(sandPrefab.name);
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
        sands.Clear();
        ObjectPool.Instance.Clear(sandPrefab.name);
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(isCreate)
        {
            if (sands.Count <= 50)
            {
                sandTime += Time.deltaTime;
                if (sandTime >= 0.5f)
                {
                    sandTime = 0;
                    var sand = ObjectPool.Instance.CreateObject(sandPrefab.name, sandPrefab);
                    sand.transform.SetParent(transform);
                    sand.transform.localScale = Vector3.one * Random.Range(0.1f, 0.3f);
                    sand.transform.localEulerAngles = new Vector3(Random.Range(40, 60), 0, 0);
                    sand.transform.localPosition = new Vector3(Random.Range(-5, 5), 20, -15);
                    sands.Add(sand);
                }
            }
        }
    }
}
