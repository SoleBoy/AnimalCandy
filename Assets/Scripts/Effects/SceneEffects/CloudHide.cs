using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudHide : MonoBehaviour
{
    void Update()
    {
        if(transform.localPosition.z >= -30)
        {
            transform.Translate(Vector3.back*15*Time.deltaTime,Space.World);
            if(transform.localPosition.z <= -30)
            {
                transform.parent.GetComponent<CreateCloud>().clouds.Remove(gameObject);
                ObjectPool.Instance.CollectObject(gameObject);
            }
        }
    }
}
