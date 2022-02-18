using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WindAnimator : MonoBehaviour, WeatherMaager
{
    bool isCreate;
    Transform prefab;
    float lastTime;
    void Start()
    {
        prefab = transform.Find("Wind");
    }

    public void Weather(bool iswather,bool hide)
    {
        if(gameObject.activeInHierarchy)
        {
            isCreate = iswather;
            if(hide)
            {
                ObjectPool.Instance.Clear(prefab.name);
                gameObject.SetActive(false);
                return;
            }
            if (iswather)
            {
                lastTime = 3;
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
            if (lastTime >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    float ran = Random.Range(0, 2);
                    if (ran == 0)
                    {
                        StartCoroutine(HideThunder(i*1, 45));
                    }
                    else
                    {
                        StartCoroutine(HideThunder(i * 1, -45));
                    }
                }
                lastTime = 0;
            }
        }
    }
    WaitForSeconds wait = new WaitForSeconds(5);
    IEnumerator HideThunder(float dely,float num)
    {
        yield return new WaitForSeconds(dely);
        var thunder = ObjectPool.Instance.CreateObject(prefab.name, prefab.gameObject);
        thunder.transform.SetParent(transform);
        thunder.GetComponent<ParticleSystem>().Play();
        thunder.transform.localPosition = new Vector3(num, 0, Random.Range(0, 250));
        thunder.transform.DOLocalMoveX(-num, 5);
        thunder.transform.localScale=Vector3.one*Random.Range(6f,10f);
        yield return wait;
        ObjectPool.Instance.CollectObject(thunder);
    }
}
