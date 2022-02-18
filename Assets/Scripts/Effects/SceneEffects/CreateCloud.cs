using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCloud : MonoBehaviour, WeatherMaager
{
    //云朵
    public List<GameObject> clouds = new List<GameObject>();
    List<GameObject> cloudPrefab=new List<GameObject>();
    float cloudTime;
    bool isCreate;
    private void Awake()
    {
        cloudPrefab.Add(transform.Find("cloud1").gameObject);
        cloudPrefab.Add(transform.Find("cloud2").gameObject);
        cloudPrefab.Add(transform.Find("cloud3").gameObject);
    }
    public void Weather(bool iswather,bool hide)
    {
        if (gameObject.activeInHierarchy)
        {
            isCreate = iswather;
            if (hide)
            {
                clouds.Clear();
                for (int i = 0; i < cloudPrefab.Count; i++)
                {
                    ObjectPool.Instance.Clear(cloudPrefab[i].name);
                }
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
        clouds.Clear();
        for (int i = 0; i < cloudPrefab.Count; i++)
        {
            ObjectPool.Instance.Clear(cloudPrefab[i].name);
        }
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(isCreate)
        {
            if (clouds.Count <= 20)
            {
                cloudTime += Time.deltaTime;
                if (cloudTime >= 5f)
                {
                    cloudTime = 0;
                    int ran = Random.Range(0, cloudPrefab.Count);
                    var cloud = ObjectPool.Instance.CreateObject(cloudPrefab[ran].name, cloudPrefab[ran]);
                    cloud.transform.SetParent(transform);
                    cloud.transform.localScale = new Vector3(Random.Range(0.7f, 1f), Random.Range(0.4f, 1f), Random.Range(0.3f, 1f));
                    //cloud.transform.localEulerAngles = new Vector3(Random.Range(-60, -40), 0, 0);
                    cloud.transform.localPosition = new Vector3(Random.Range(-40, 40), 35, 600);
                    clouds.Add(cloud);
                }
            }
        }
    }
}
