using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLinkPanel : MonoBehaviour
{

    Button closeBtn;
    public void Init()
    {
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(ClosePanel);
    }
    private void OnEnable()
    {
        GameManager.Instance.ShowBanner();
    }

    public void GoGame(string mess)
    {
        Application.OpenURL(mess);
    }
    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        GameManager.Instance.HideBanner();
        UIManager.Instance.DetectionPanel();
    }
}
