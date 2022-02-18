using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalElimination : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
