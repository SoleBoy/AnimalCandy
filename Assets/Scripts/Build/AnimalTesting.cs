using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalTesting : MonoBehaviour
{
    private MeshRenderer rendererM;
    private bool isTrigger;
    void Start()
    {
        rendererM = GetComponent<MeshRenderer>();
        rendererM.enabled = false;
        isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger && other.CompareTag("Enemy"))
        {
            rendererM.enabled = true;
            isTrigger = false;
            Invoke("AgainRenderer", 30);
            Invoke("HideRenderer", 1);
        }
    }
    void HideRenderer()
    {
        rendererM.enabled = false;
    }
    void AgainRenderer()
    {
        isTrigger = true;
    }
}
