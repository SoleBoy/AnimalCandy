using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowHide : MonoBehaviour
{
    GoldRotate snow;
    private void Awake()
    {
        snow = transform.Find("Snow").GetComponent<GoldRotate>();
    }
    private void OnEnable()
    {
        snow.enabled = true;
    }
    void Update()
    {
        if (transform.localPosition.y >= 0f)
        {
            transform.Translate(Vector3.right * 10 * Time.deltaTime);
            if (transform.localPosition.y <= 0)
            {
                snow.enabled = false;
                transform.localEulerAngles = Vector3.zero;
                snow.transform.localPosition = Vector3.zero;
                snow.transform.localEulerAngles = Vector3.zero;
                StartCoroutine(HideRain());
            }
        }
    }
    IEnumerator HideRain()
    {
        transform.DOLocalMoveY(-0.5f, 3f);
        yield return new WaitForSeconds(3f);
        ObjectPool.Instance.CollectObject(gameObject);
        transform.parent.GetComponent<CreateSnow>().snows.Remove(gameObject);
    }
}
