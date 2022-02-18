using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    Button closeBtn;
    public void Init()
    {
        //transform.Find("Scroll View").GetComponent<ScrollPage1>().Init(0);
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(ClosePanel);
    }
    void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
    }
}
