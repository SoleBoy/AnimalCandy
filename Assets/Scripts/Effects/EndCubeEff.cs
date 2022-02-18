using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCubeEff : MonoBehaviour
{
    public bool isDir;
    public bool isDeath;
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.y > 1)
        {
            if(isDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if(isDir)
            {
                transform.Translate(Vector3.up * Time.deltaTime * 8);
            }
            else
            {
                transform.Translate(-Vector3.forward * Time.deltaTime * 8);
            }
        }
    }
}
