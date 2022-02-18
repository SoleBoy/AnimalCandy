using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : MonoBehaviour {
    Text tipText;
    private void Awake()
    {
        tipText = transform.Find("TipText").GetComponent<Text>();
    }
    public void TipMessage(string mess)
    {
        tipText.text = mess;
        StartCoroutine(HideMess());
    }
    IEnumerator HideMess()
    {
        yield return new WaitForSeconds(2);
        ObjectPool.Instance.CollectObject(gameObject);
    }
}
