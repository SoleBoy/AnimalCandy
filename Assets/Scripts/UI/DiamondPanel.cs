using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondPanel : MonoBehaviour
{
    Button closeBtn;
    Button atkBtn;
    Button earBtn;
    Button goldBtn;
    Button storeBtn;
    Button taskBtn;
    Button giftBtn;
    //Button diamBtn;
    Button selectBtn;

    public Text diamText;
    public void Init()
    {
        diamText = transform.Find("Diamond/Text").GetComponent<Text>();

        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        atkBtn = transform.Find("atk/Button").GetComponent<Button>();
        earBtn = transform.Find("earnings/Button").GetComponent<Button>();
        goldBtn = transform.Find("sign/Button").GetComponent<Button>();
        storeBtn = transform.Find("Store/Button").GetComponent<Button>();
        taskBtn = transform.Find("Task/Button").GetComponent<Button>();
        giftBtn = transform.Find("Gift/Button").GetComponent<Button>();
        selectBtn = transform.Find("Select/Button").GetComponent<Button>();
        //diamBtn = transform.Find("Diam").GetComponent<Button>();

        closeBtn.onClick.AddListener(ClosePanel);
        atkBtn.onClick.AddListener(DoubleAttack);
        earBtn.onClick.AddListener(DoubleIncome);
        goldBtn.onClick.AddListener(GoSign);
        storeBtn.onClick.AddListener(GoStore);
        taskBtn.onClick.AddListener(GoTask);
        giftBtn.onClick.AddListener(GoGift);
        selectBtn.onClick.AddListener(GoSelect);
        //diamBtn.onClick.AddListener(AdsDiam);
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        UIManager.Instance.isTime = true;
    }
    public void SetDiamond(string messg)
    {
        diamText.text = messg;
    }
    private void GoSelect()
    {
        GameManager.Instance.HideBanner();
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.SwitchScene("Select");
    }

    private void GoGift()
    {
        if (PlayerPrefs.GetFloat("MaxLevel") >= 3)
        {
            UIManager.Instance.SwitchScene("Build");
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void AdsDiam()
    {
        bool isReward = false;
        AudioManager.Instance.PlayTouch("ads_1");
        if (Application.platform == RuntimePlatform.Android)
        {
            GameManager.Instance.UserChoseToWatchAd(AdsType.reset);
            GameManager.Instance.AdmobRewardCB = delegate
            {

            };
            GameManager.Instance.AdmobRewardClose = delegate
            {
                isReward = true;
            };
            GameManager.Instance.AdmobClose = delegate
            {
                if (isReward)
                {
                    UIManager.Instance.SetStar(1);
                    GameManager.Instance.ClonePrompt(1, 1);
                    UIManager.Instance.OpenDiam(diamText.transform.parent.position);
                }
                GameManager.Instance.RequestRewardBasedVideo(AdsType.reset);
            };
        }
        else
        {
            UIManager.Instance.SetStar(1);
            GameManager.Instance.ClonePrompt(1, 1);
            UIManager.Instance.OpenDiam(diamText.transform.parent.position);
        }
    }

    private void GoTask()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.taskPanel.SetPanel(false, false);
    }

    private void GoStore()
    {
        GameManager.Instance.HideBanner();
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.storePanel.OpenPanel(0);
    }

    private void GoSign()
    {
        GameManager.Instance.HideBanner();
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.everydayPanel.gameObject.SetActive(true);
    }

    private void DoubleIncome()
    {
        UIManager.Instance.VideoEarnings();
    }

    private void DoubleAttack()
    {
        UIManager.Instance.VideoAttak();
    }
    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        GameManager.Instance.HideBanner();
        UIManager.Instance.DetectionPanel();
    }
    private void OnEnable()
    {
        GameManager.Instance.ShowBanner();
    }
}
