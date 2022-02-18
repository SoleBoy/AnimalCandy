using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RainCrash : MonoBehaviour
{
    void Update()
    {
        if(transform.localPosition.y >= 0f)
        {
            transform.Translate(Vector3.down*20*Time.deltaTime);
            if (transform.localPosition.y <= 0)
            {
                StartCoroutine(HideRain());
            }
        }
    }
    IEnumerator HideRain()
    {
        float ran = Random.Range(1f, 1.5f);
        transform.DOScale(new Vector3(ran, 0.05f, ran), 0.2f);
        transform.DOLocalMoveY(-0.5f, 1f);
        yield return new WaitForSeconds(1f);
        ObjectPool.Instance.CollectObject(gameObject);
        transform.parent.GetComponent<CreateRain>().rains.Remove(gameObject);
    }
}
