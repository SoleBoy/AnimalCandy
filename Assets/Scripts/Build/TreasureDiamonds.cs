using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreasureDiamonds : MonoBehaviour
{
    private int number;
    private int count = 1;
    private bool isTrigg;
    private WaitForSeconds wait = new WaitForSeconds(1);
 
    public void SetTreasure(float maxY)
    {
        count = 0;
        number = 10;
        isTrigg = true;
        transform.localScale = Vector3.one * 65;
        transform.localPosition = new Vector3(-6.6f, maxY, 10.2f);
        transform.Find("TipCon").gameObject.SetActive(true);
        transform.Find("Canvas").gameObject.SetActive(true);
    }

    public void SetInit(float maxY)
    {
        isTrigg = true;
        transform.localScale = Vector3.one * 150;
        transform.localPosition = new Vector3(Random.Range(-950, 950), maxY, Random.Range(-950, 950));
        number = Random.Range(1,11);
        int ranNumber = Random.Range(0, 500);
        if (ranNumber <= 0)
        {
            number += 500;
        }
        else if(ranNumber <= 5)
        {
            number += 50;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isTrigg && other.CompareTag("Bullet"))
        {
            isTrigg = false;
            UIBase.Instance.TurnUpTreasure(transform,number);
            EffectGenerator.Instance.Meage(other.transform);
            transform.localPosition = other.transform.parent.position;
            StartCoroutine(HideDely());
            if(count == 0)
            {
                PlayerPrefs.SetString("TreasureInit", "TreasureInit");
                Destroy(gameObject);
            }
        }
    }

    IEnumerator HideDely()
    {
        transform.DOScale(Vector3.zero, 1);
        yield return wait;
        ObjectPool.Instance.CollectObject(gameObject);
    }
}
