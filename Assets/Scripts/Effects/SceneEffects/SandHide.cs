using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandHide : MonoBehaviour
{
    void Update()
    {
        if (transform.localPosition.y >= 0f)
        {
            transform.Translate(Vector3.forward * 10 * Time.deltaTime);
            if (transform.localPosition.y <= 0)
            {
                transform.localEulerAngles = Vector3.zero;
                StartCoroutine(HideRain());
            }
        }
    }
    IEnumerator HideRain()
    {
        transform.DOLocalMoveY(-0.5f, 3f);
        yield return new WaitForSeconds(3f);
        ObjectPool.Instance.CollectObject(gameObject);
        transform.parent.GetComponent<CreateSand>().sands.Remove(gameObject);
    }
}
