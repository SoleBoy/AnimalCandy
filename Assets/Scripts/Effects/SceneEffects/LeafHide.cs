using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafHide : MonoBehaviour
{
    GoldRotate leaf;
    private void Awake()
    {
        leaf = transform.Find("leaf").GetComponent<GoldRotate>();
    }
    private void OnEnable()
    {
        leaf.enabled = true;
    }
    void Update()
    {
        if (transform.localPosition.y >= 0f)
        {
            transform.Translate(-Vector3.forward * 6 * Time.deltaTime);
            if (transform.localPosition.y <= 0)
            {
                leaf.enabled = false;
                transform.localEulerAngles = Vector3.zero;
                leaf.transform.localPosition = Vector3.zero;
                leaf.transform.localEulerAngles = Vector3.zero;
                StartCoroutine(HideRain());
            }
        }
    }
    IEnumerator HideRain()
    {
        transform.DOLocalMoveY(-0.5f, 3f);
        yield return new WaitForSeconds(3f);
        ObjectPool.Instance.CollectObject(gameObject);
        transform.parent.GetComponent<CreateLeaf>().leafs.Remove(gameObject);
    }
}
