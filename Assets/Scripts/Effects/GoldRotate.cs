using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRotate : MonoBehaviour
{
    public bool isRotateUp;
    void Update()
    {
        if(gameObject.activeInHierarchy)
        {
            if(isRotateUp)
            {
                transform.Rotate(Vector3.right * 5, Space.World);
            }
            else
            {
                transform.Rotate(Vector3.forward * 5);
            }
        }
    }
}
