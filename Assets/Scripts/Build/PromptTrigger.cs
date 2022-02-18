using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptTrigger : MonoBehaviour
{
    public Transform tempGame;
    private void OnTriggerEnter(Collider other)
    {
        if(transform.parent.parent.parent != other)
        {
            tempGame.localPosition = other.transform.localPosition;
        }
       // Debug.Log(other.name);
        //Debug.Log(other.transform.localPosition.y);
    }
}
