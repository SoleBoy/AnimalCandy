using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRecort : MonoBehaviour
{
    private Text recordText;

    private Button recordBtn;
    private Button anewBtn;
    void Start()
    {
        recordBtn = transform.Find("recordBtn").GetComponent<Button>();
        anewBtn = transform.Find("anewBtn").GetComponent<Button>();

        recordText = transform.Find("Image/Text").GetComponent<Text>();
        recordText.text = UIManager.Instance.timeRecord.ToString("F1")+"s";
        recordBtn.onClick.AddListener(OpenRecord);
        anewBtn.onClick.AddListener(OpenAnew);
    }

    private void OpenRecord()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
        CreateModel.Instance.level = PlayerPrefs.GetFloat("TimeRecort",1);
        UIManager.Instance.timeCurret = UIManager.Instance.timeRecord-30;
        if (UIManager.Instance.timeCurret < 0) UIManager.Instance.timeCurret = 0;
        UIManager.Instance.settlePanel.recordTime = UIManager.Instance.timeCurret;
    }

    private void OpenAnew()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
        CreateModel.Instance.level = 1;
    }
}
