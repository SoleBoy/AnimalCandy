using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluatePanel : MonoBehaviour
{
    private Button closeBtn;
    private Button goBtn;
    public void Init()
    {
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        goBtn = transform.Find("GoBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(ClosePanel);
        goBtn.onClick.AddListener(GoPanel);
    }

    public void OpenPanel()
    {
        UIManager.Instance.isTime = true;
        gameObject.SetActive(true);
    }

    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
    }

    private void GoPanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
        PlayerPrefs.SetString("GoEvaluate","true");
#if UNITY_IPHONE
        var url = string.Format(
                "itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}",
                AdsConfigure.APPID);
        Application.OpenURL(url);
#endif
    }
}
