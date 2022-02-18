using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAnimator : MonoBehaviour, WeatherMaager
{
    bool isCreate;
    Transform prefab;
    float lastTime;
    void Start()
    {
        prefab = transform.Find("Thunder");
        lastTime = 2;
    }
    public void Weather(bool iswather,bool hide)
    {
        if (gameObject.activeInHierarchy)
        {
            isCreate = iswather;
            if (hide)
            {
                ObjectPool.Instance.Clear(prefab.name);
                gameObject.SetActive(false);
                return;
            }
            if (iswather)
            {
                lastTime = 2;
            }
            else
            {
                StartCoroutine(HideObject());
            }
        }
    }
    IEnumerator HideObject()
    {
        yield return wait;
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (isCreate)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= 2)
            {
                CloneThunder();
                lastTime = 0;
            }
        }
    }

    void CloneThunder()
    {
        for (int i = 0; i < 3; i++)
        {
            var thunder = ObjectPool.Instance.CreateObject(prefab.name,prefab.gameObject);
            thunder.transform.SetParent(transform);
            thunder.transform.localPosition = new Vector3(Random.Range(-6,6),0, Random.Range(0,250));
            StartCoroutine(HideThunder(thunder));
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.1f);
    IEnumerator HideThunder(GameObject game)
    {
        yield return wait;
        ObjectPool.Instance.CollectObject(game);
    }
}
