using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    public Text goldText;
    public Text diamText;
    public Text adsText;

    Button buyBtn_gold;
    Button buyBtn_diam1;
    Button buyBtn_diam2;
    Button buyBtn_diam3;
    Button adsBtn;
    Button backBtn;

    Transform content;
    Button mergeBtn;
    Button diamBtn;
    Button incomeBtn;
    Button attackBtn;
    Button bankBtn;
    Button vipBtn;
    Button freeBtn;
    public void Init()
    {
        diamText = transform.Find("Diamond/Text").GetComponent<Text>();
        goldText = transform.Find("Gold/Text").GetComponent<Text>();
        adsText = transform.Find("Ads/Text").GetComponent<Text>();
        buyBtn_gold = transform.Find("BuyGold").GetComponent<Button>();
        buyBtn_diam1 = transform.Find("BuyDiamond1").GetComponent<Button>();
        buyBtn_diam2 = transform.Find("BuyDiamond2").GetComponent<Button>();
        buyBtn_diam3 = transform.Find("BuyDiamond3").GetComponent<Button>();
        adsBtn = transform.Find("AdsNumber").GetComponent<Button>();

        content = transform.Find("Scroll View").GetComponent<ScrollRect>().content;
        mergeBtn = content.Find("Merge/Button").GetComponent<Button>();
        diamBtn = content.Find("Diamond/Button").GetComponent<Button>();
        incomeBtn = content.Find("Income/Button").GetComponent<Button>();
        attackBtn = content.Find("Attack/Button").GetComponent<Button>();
        bankBtn = content.Find("PiggyBank/Button").GetComponent<Button>();
        vipBtn = content.Find("VIP/Button").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        freeBtn = transform.Find("FreeBtn").GetComponent<Button>();

        buyBtn_gold.onClick.AddListener(BuyCandy);
        adsBtn.onClick.AddListener(AdsDiamond);
        buyBtn_diam1.onClick.AddListener(BuyDiamond1);
        buyBtn_diam2.onClick.AddListener(BuyDiamond2);
        buyBtn_diam3.onClick.AddListener(BuyDiamond3);
        mergeBtn.onClick.AddListener(ClickMerge);
        diamBtn.onClick.AddListener(ClickDiamond);
        incomeBtn.onClick.AddListener(ClickIncome);
        attackBtn.onClick.AddListener(ClickAttack);
        bankBtn.onClick.AddListener(ClickBank);
        vipBtn.onClick.AddListener(ClickVip);
        backBtn.onClick.AddListener(ClosePanel);
        freeBtn.onClick.AddListener(FreeDiamond);
        Hide();
    }

    private void BuyCandy()
    {
        if (GameManager.Instance.adsCount >= 1)
        {
            if (UIManager.Instance)
            {
                UIManager.Instance.SetGold(1600);
            }
            else
            {
                ChoiceControl.Instance.SetGold(1600);
            }
            GameManager.Instance.ClonePrompt(1600, 0);
            GameManager.Instance.SetAdsNumber(-1);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void AdsDiamond()
    {
        if (GameManager.Instance.adsCount >= 3)
        {
            if (UIManager.Instance)
            {
                UIManager.Instance.SetStar(1);
            }
            else
            {
                ChoiceControl.Instance.SetDiamond(1);
            }
            GameManager.Instance.ClonePrompt(1, 1);
            GameManager.Instance.SetAdsNumber(-3);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void BuyDiamond1()
    {
        if (GameManager.Instance.adsCount >= 5)
        {
            if (UIManager.Instance)
            {
                UIManager.Instance.SetStar(3);
            }
            else
            {
                ChoiceControl.Instance.SetDiamond(3);
            }
            GameManager.Instance.ClonePrompt(3, 1);
            GameManager.Instance.SetAdsNumber(-5);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void BuyDiamond2()
    {
        if (GameManager.Instance.adsCount >= 7)
        {
            if (UIManager.Instance)
            {
                UIManager.Instance.SetStar(6);
            }
            else
            {
                ChoiceControl.Instance.SetDiamond(6);
            }
            GameManager.Instance.ClonePrompt(6, 1);
            GameManager.Instance.SetAdsNumber(-7);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void BuyDiamond3()
    {
        if (GameManager.Instance.adsCount >= 9)
        {
            if (UIManager.Instance)
            {
                UIManager.Instance.SetStar(10);
            }
            else
            {
                ChoiceControl.Instance.SetDiamond(10);
            }
            GameManager.Instance.ClonePrompt(10, 1);
            GameManager.Instance.SetAdsNumber(-9);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void ClickMerge()
    {
        if (GameManager.Instance.adsCount >= 49)
        {
            HideBtn("Auto");
            GameManager.Instance.SetAdsNumber(-49);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void ClickDiamond()
    {
        if (GameManager.Instance.adsCount >= 49)
        {
            HideBtn("Task");
            GameManager.Instance.SetAdsNumber(-49);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void ClickIncome()
    {
        if (GameManager.Instance.adsCount >= 49)
        {
            HideBtn("Income");
            GameManager.Instance.SetAdsNumber(-49);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void ClickAttack()
    {
        if (GameManager.Instance.adsCount >= 49)
        {
            HideBtn("Attack");
            GameManager.Instance.SetAdsNumber(-49);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    private void ClickBank()
    {
        if (GameManager.Instance.adsCount >= 99)
        {
            HideBtn("Bank");
            GameManager.Instance.SetAdsNumber(-99);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    void ClickVip()
    {
        if (GameManager.Instance.adsCount >= 49)
        {
            HideBtn("Vip");
            GameManager.Instance.SetAdsNumber(-49);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }

    public void HideBtn(string messg)
    {
        GameManager.Instance.CloneTip(ExcelTool.lang["tip2"]);
        if (messg == "Auto")
        {
            mergeBtn.enabled = false;
            mergeBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("mergeDelay", "mergeDelay");
        }
        else if (messg == "Income")
        {
            incomeBtn.enabled = false;
            incomeBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("incomeDou", "incomeDou");
            if (UIManager.Instance)
            {
                UIManager.Instance.SetIncome();
            }
        }
        else if (messg == "Attack")
        {
            attackBtn.enabled = false;
            attackBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("attckDou", "attckDou");
            if (UIManager.Instance)
                UIManager.Instance.SetAttack();
        }
        else if (messg == "Bank")
        {
            bankBtn.enabled = false;
            bankBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("PiggyReward", "PiggyReward");
        }
        else if (messg == "Vip")
        {
            vipBtn.enabled = false;
            vipBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("vipReward", "vipReward");
            if (UIManager.Instance)
                UIManager.Instance.everydayPanel.starText.text = "X10";
        }
        else if (messg == "Task")
        {
            diamBtn.enabled = false;
            diamBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("DiamTask", "diamBtn");
        }
    }
    public void OpenPanel(float Y)
    {
        content.localPosition = Vector3.up * Y;
        gameObject.SetActive(true);
        if (UIManager.Instance)
            UIManager.Instance.morePanel.ShowTip();
    }
    void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        if (UIManager.Instance)
            UIManager.Instance.DetectionPanel();
    }
    void Hide()
    {
        if (PlayerPrefs.GetString("mergeDelay") != "")
        {
            mergeBtn.enabled = false;
            mergeBtn.GetComponent<Image>().color = Color.gray;
        }
        if (PlayerPrefs.GetString("incomeDou") != "")
        {
            incomeBtn.enabled = false;
            incomeBtn.GetComponent<Image>().color = Color.gray;
        }
        if (PlayerPrefs.GetString("attckDou") != "")
        {
            attackBtn.enabled = false;
            attackBtn.GetComponent<Image>().color = Color.gray;
        }
        if (PlayerPrefs.GetString("PiggyReward") != "")
        {
            bankBtn.enabled = false;
            bankBtn.GetComponent<Image>().color = Color.gray;
        }
        if (PlayerPrefs.GetString("vipReward") != "")
        {
            vipBtn.enabled = false;
            vipBtn.GetComponent<Image>().color = Color.gray;
            if (UIManager.Instance)
                UIManager.Instance.everydayPanel.starText.text = "X10";
        }
        if (PlayerPrefs.GetString("DiamTask") != "")
        {
            diamBtn.enabled = false;
            diamBtn.GetComponent<Image>().color = Color.gray;
        }
    }

    private void FreeDiamond()
    {
        bool isReward = false;
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
                    if (UIManager.Instance)
                    {
                        UIManager.Instance.SetStar(1);
                    }
                    else
                    {
                        ChoiceControl.Instance.SetDiamond(1);
                    }
                    GameManager.Instance.ClonePrompt(1, 1);
                }
                GameManager.Instance.RequestRewardBasedVideo(AdsType.reset);
            };
        }
        else
        {
            if (UIManager.Instance)
            {
                UIManager.Instance.SetStar(1);
            }
            else
            {
                ChoiceControl.Instance.SetDiamond(1);
            }
            GameManager.Instance.ClonePrompt(1, 1);
        }
    }
}
