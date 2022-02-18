using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float number = 5;
    public Transform cube1;
    public Transform cube2;
    private void Awake()
    {
        Debug.Log(number);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Vector3.Dot(cube1.forward, cube2.position));
        }
    }
    public void Click()
    {

    }
}
