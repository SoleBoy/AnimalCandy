using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFall : MonoBehaviour
{
    public int Index;
    GoldRotate leaf;
    GoldRotate snow;
    private void Awake()
    {
        if(Index == 1)
        {
            leaf = transform.GetChild(0).GetComponent<GoldRotate>();
        }
        else if(Index == 2)
        {
            snow = transform.GetChild(0).GetComponent<GoldRotate>();
        }
    }
    private void OnEnable()
    {
        if (Index == 1)
        {
            leaf.enabled = true;
        }
        else if (Index == 2)
        {
            snow.enabled = true;
        }
    }
    void Update()
    {
        switch (Index)
        {
            case 0:
                if (transform.localPosition.y >= 0.5f)
                {
                    transform.Translate(Vector3.down * 20 * Time.deltaTime);
                    if (transform.localPosition.y <= 0.5f)
                    {
                        StartCoroutine(HideRain());
                    }
                }
                break;
            case 1:
                if (transform.localPosition.y >= 0.3f)
                {
                    transform.Translate(transform.forward * 6 * Time.deltaTime);
                    if (transform.localPosition.y <= 0.3f)
                    {
                        leaf.enabled = false;
                        transform.localEulerAngles = Vector3.zero;
                        leaf.transform.localPosition = Vector3.zero;
                        leaf.transform.localEulerAngles = Vector3.zero;
                        StartCoroutine(HideLeft());
                    }
                }
                break;
            case 2:
                if (transform.localPosition.y >= 0.3f)
                {
                    transform.Translate(transform.forward * 10 * Time.deltaTime);
                    if (transform.localPosition.y <= 0.3f)
                    {
                        snow.enabled = false;
                        transform.localEulerAngles = Vector3.zero;
                        snow.transform.localPosition = Vector3.zero;
                        snow.transform.localEulerAngles = Vector3.zero;
                        StartCoroutine(HideSand());
                    }
                }
                break;
            case 3:
                if (transform.localPosition.z >= -100)
                {
                    transform.Translate(Vector3.back * 10 * Time.deltaTime, Space.World);
                    if (transform.localPosition.z <= -100)
                    {
                        ObjectPool.Instance.CollectObject(gameObject);
                    }
                }
                break;
            default:
                break;
        }
        
    }

    IEnumerator HideRain()
    {
        float ran = Random.Range(1f, 1.5f);
        transform.DOScale(new Vector3(ran, 0.05f, ran), 0.2f);
        transform.DOLocalMoveY(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ObjectPool.Instance.CollectObject(gameObject);
    }

    IEnumerator HideLeft()
    {
        transform.DOLocalMoveY(-0.5f, 3f);
        yield return new WaitForSeconds(3f);
        ObjectPool.Instance.CollectObject(gameObject);
    }

    IEnumerator HideSand()
    {
        transform.DOLocalMoveY(-0.5f, 3f);
        yield return new WaitForSeconds(3f);
        ObjectPool.Instance.CollectObject(gameObject);
    }
}
