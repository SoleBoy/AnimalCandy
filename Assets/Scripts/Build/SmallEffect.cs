using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEffect : MonoBehaviour
{
    bool isBall;
    Rigidbody body;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    public void SetPower(float speed)
    {
        body.AddForce(transform.up * speed, ForceMode.Impulse);//向上
    }

    private void OnEnable()
    {
        isBall = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isBall)
        {
            isBall = true;
            body.AddForce(Vector3.up * 5, ForceMode.Impulse);//向上
        }
        else
        {
            StartCoroutine(HideEffCube());
        }
    }
    IEnumerator HideEffCube()
    {
        yield return new WaitForSeconds(1.5f);
        if (gameObject)
        {
            ObjectPool.Instance.CollectObject(gameObject);
        }
    }
}
