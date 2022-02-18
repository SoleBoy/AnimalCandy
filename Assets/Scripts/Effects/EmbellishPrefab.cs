using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbellishPrefab : MonoBehaviour
{
    bool isBall;
    Rigidbody body;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        isBall = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!isBall && !collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log(collision.gameObject.name);
            isBall = true;
            body.AddForce(Vector3.up*12, ForceMode.Impulse);//向上
            StartCoroutine(HideEffCube());
        }
    }
    IEnumerator HideEffCube()
    {
        yield return new WaitForSeconds(5f);
        ObjectPool.Instance.CollectObject(gameObject);
    }
}
