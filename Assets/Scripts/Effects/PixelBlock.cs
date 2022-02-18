using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class PixelBlock : MonoBehaviour
{
    bool isBall;
    Rigidbody body;
    Color color;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        color=transform.GetComponent<Renderer>().material.color;
        color.a = 1;
    }
    public void SetPower(int speed)
    {
        transform.GetComponent<Renderer>().material.DOFade(1,0);
        body.AddForce(transform.up*speed, ForceMode.Impulse);//向上
    }

    private void OnEnable()
    {
        isBall = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isBall && !collision.gameObject.CompareTag("Enemy") && collision.gameObject.name != "skill5")
        {
            //Debug.Log(collision.gameObject.name);
            isBall = true;
            body.AddForce(Vector3.up * 5, ForceMode.Impulse);//向上
            StartCoroutine(HideEffCube());
        }
    }
    IEnumerator HideEffCube()
    {
        transform.GetComponent<Renderer>().material.DOFade(0,1.5f);
        yield return new WaitForSeconds(1.5f);
        if(gameObject)
        {
            ObjectPool.Instance.CollectObject(gameObject);
        }
    }
}
