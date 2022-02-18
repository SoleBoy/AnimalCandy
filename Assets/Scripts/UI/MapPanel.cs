using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : MonoBehaviour
{
    private Text passText;
    private Text starText;
    private Text timeText;
    private Button closeBtn;
    private bool isClick;
    private float lastTime;
    private float timeDown;
    public void Init()
    {
        starText = transform.Find("Star/Text").GetComponent<Text>();
        passText = transform.Find("Pass/Text").GetComponent<Text>();
        timeText = transform.Find("TimeText").GetComponent<Text>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(HideMap);
    }
    public void OpenPanel(bool isOpen)
    {
        KeelAnimation.Instance.Stop();
        if (PlayerPrefs.GetString("Guide" + 16) == "")
        {
            UIManager.Instance.guidePanel.OpenGuide(16, true);
        }
        else
        {
            isClick = isOpen;
            starText.text = CreateModel.Instance.sumLevel.ToString();
            passText.text = PlayerPrefs.GetFloat("PassLevel").ToString();
            if (isClick)
            {
                timeDown = 15;
                lastTime = 0;
                timeText.text = timeDown.ToString("F0") + "s";
            }
            else
            {
                timeText.text = "";
            }
            gameObject.SetActive(true);
            GameManager.Instance.ShowBanner();
        }
    }

    void Update()
    {
        if (isClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClick = false;
                timeText.text = "";
            }
            if (timeDown >= 0)
            {
                lastTime += Time.deltaTime;
                if (lastTime >= 1)
                {
                    timeDown -= lastTime;
                    timeText.text = timeDown.ToString("F0") + "s";
                    if (timeDown <= 0)
                    {
                        NextLevel(CreateModel.Instance.level+1);
                    }
                    lastTime = 0;
                }
            }
        }
    }

    void HideMap()
    {
        AudioManager.Instance.PlayTouch("close_1");
        ClosePanel();
        if (CreateModel.Instance.isScene)
        {
            CreateModel.Instance.isScene = false;
            CreateModel.Instance.SetLevel(CreateModel.Instance.level);
        }
    }

    public void ClosePanel()
    {
        isClick = false;
        GameManager.Instance.HideBanner();
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
    }

    public void NextLevel(float level)
    {
        UIManager.Instance.MaskPanel(true);
        if (CreateModel.Instance.isScene)
        {
            CreateModel.Instance.ChangeScene(level);
            StartCoroutine(DelyHide(1, level));
        }
        else
        {
            if (int.Parse(CreateModel.Instance.ReturnLevel(level).mapId) != CreateModel.Instance.sceneIndex)
            {
                CreateModel.Instance.ChangeScene(level);
                StartCoroutine(DelyHide(1,level));
            }
            else
            {
                StartCoroutine(DelyHide(0,level));
            }
        }
    }

    IEnumerator DelyHide(float day,float level)
    {
        yield return new WaitForSeconds(day);
        CreateModel.Instance.SetLevel(level);
        UIManager.Instance.InitLevel(level);
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.MaskPanel(false);
        CreateModel.Instance.isScene = false;
        ClosePanel();
        if (day == 1)
        {
            Camera.main.GetComponent<CameraView>().InitMove();
        }
    }
}
