using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLeaf : MonoBehaviour, WeatherMaager
{
    //叶子
    public List<GameObject> leafs = new List<GameObject>();
    GameObject leafPrefab;
    float leafsTime;
    bool isCreate;
    private void Awake()
    {
        leafPrefab = transform.Find("leaf").gameObject;
    }
    public void Weather(bool iswather,bool hide)
    {
        if (gameObject.activeInHierarchy)
        {
            isCreate = iswather;
            if (hide)
            {
                leafs.Clear();
                ObjectPool.Instance.Clear(leafPrefab.name);
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
        leafs.Clear();
        ObjectPool.Instance.Clear(leafPrefab.name);
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(isCreate)
        {
            if (leafs.Count <= 10)
            {
                leafsTime += Time.deltaTime;
                if (leafsTime >= 0.3f)
                {
                    leafsTime = 0;
                    var leaf = ObjectPool.Instance.CreateObject(leafPrefab.name, leafPrefab);
                    leaf.transform.SetParent(transform);
                    leaf.transform.localScale = Vector3.one * Random.Range(0.5f, 1f);
                    leaf.transform.localEulerAngles = new Vector3(Random.Range(-50, -15), 0, 0);
                    leaf.transform.localPosition = new Vector3(Random.Range(-8, 8), 25f, Random.Range(30, 40));
                    leafs.Add(leaf);
                }
            }
        }
    }
}
