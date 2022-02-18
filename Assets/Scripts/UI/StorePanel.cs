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
    GameObject maskPanel;
    GameObject load;

    Button buyBtn_gold;
    Button buyBtn_diam1;
    Button buyBtn_diam2;
    Button buyBtn_diam3;
    Button adsBtn;
    Button backBtn;

    GameObject resPanel;
    Button fixBtn;
    Button closeBtn;

    Transform content;
    Button mergeBtn;
    Button diamBtn;
    Button incomeBtn;
    Button attackBtn;
    Button bankBtn;
    Button vipBtn;
    Button restoreBtn;

    public void Init()
    {
        maskPanel = transform.Find("Mask").gameObject;
        load = transform.Find("Mask/Image").gameObject;
        diamText = transform.Find("Diamond/Text").GetComponent<Text>();
        goldText = transform.Find("Gold/Text").GetComponent<Text>();
        adsText = transform.Find("Ads/Text").GetComponent<Text>();
        buyBtn_gold = transform.Find("BuyGold").GetComponent<Button>();
        buyBtn_diam1 = transform.Find("BuyDiamond1").GetComponent<Button>();
        buyBtn_diam2 = transform.Find("BuyDiamond2").GetComponent<Button>();
        buyBtn_diam3 = transform.Find("BuyDiamond3").GetComponent<Button>();
        adsBtn = transform.Find("AdsNumber").GetComponent<Button>();

        resPanel = transform.Find("RestorePanel").gameObject;
        fixBtn = transform.Find("RestorePanel/DrawBtn").GetComponent<Button>();
        closeBtn = transform.Find("RestorePanel/BackBtn").GetComponent<Button>();

        content = transform.Find("Scroll View").GetComponent<ScrollRect>().content;
        mergeBtn = content.Find("Merge/Button").GetComponent<Button>();
        diamBtn = content.Find("Diamond/Button").GetComponent<Button>();
        incomeBtn = content.Find("Income/Button").GetComponent<Button>();
        attackBtn = content.Find("Attack/Button").GetComponent<Button>();
        bankBtn = content.Find("PiggyBank/Button").GetComponent<Button>();
        vipBtn = content.Find("VIP/Button").GetComponent<Button>();
        restoreBtn = transform.Find("RestoreBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        fixBtn.onClick.AddListener(RestoreBuy);
        closeBtn.onClick.AddListener(CloseRestore);
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
        restoreBtn.onClick.AddListener(Restore);
        backBtn.onClick.AddListener(ClosePanel);
        Hide();
    }
    private void RestoreBuy()
    {
        OpenMak();
        resPanel.SetActive(false);
        GameManager.Instance.GetComponent<Purchaser>().RestorePurchases();
    }

    private void Restore()
    {
        AudioManager.Instance.PlayTouch("other_1");
        resPanel.SetActive(true);
    }
    private void CloseRestore()
    {
        AudioManager.Instance.PlayTouch("close_1");
        resPanel.SetActive(false);
    }
    private void BuyCandy()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_Candy);
    }
    private void AdsDiamond()
    {
        if(GameManager.Instance.adsCount >= 3)
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
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_Diamond1);
    }
    private void BuyDiamond2()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_Diamond2);
    }
    private void BuyDiamond3()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_Diamond3);
    }
    private void ClickMerge()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_Auto);
    }
    private void ClickDiamond()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_task);
    }
    private void ClickIncome()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_Income);
    }
    private void ClickAttack()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_Attack);
    }
    private void ClickBank()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_Bank);
    }
    void ClickVip()
    {
        OpenMak();
        GameManager.Instance.GetComponent<Purchaser>().BuyProductID(AdsConfigure.ProductID_VIP);
    }
    void OpenMak()
    {
        AudioManager.Instance.PlayTouch("other_1");
        maskPanel.SetActive(true);
        StartCoroutine(CloseMask());
    }
    IEnumerator CloseMask()
    {
        yield return new WaitForSeconds(10);
        maskPanel.SetActive(false);
    }
    public void FreeDiamond()
    {
        bool isReward = false;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
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

    public void HideMask()
    {
        StopCoroutine(CloseMask());
        maskPanel.SetActive(false);
    }
    public void HideBtn(string messg)
    {
        maskPanel.SetActive(false);
        GameManager.Instance.CloneTip(ExcelTool.lang["tip2"]);
        if (messg == "Auto")
        {
            mergeBtn.enabled = false;
            mergeBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("mergeDelay", "mergeDelay");
        }
        else if(messg == "Income")
        {
            incomeBtn.enabled = false;
            incomeBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("incomeDou", "incomeDou");
            if (UIManager.Instance)
            {
                UIManager.Instance.SetIncome();
            }
        }
        else if(messg == "Attack")
        {
            attackBtn.enabled = false;
            attackBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("attckDou", "attckDou");
            if (UIManager.Instance)
            {
                UIManager.Instance.SetAttack();
            }
        }
        else if (messg == "Bank")
        {
            bankBtn.enabled = false;
            bankBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("PiggyReward", "PiggyReward");
        }
        else if(messg == "Vip")
        {
            vipBtn.enabled = false;
            vipBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("vipReward", "vipReward");
            if (UIManager.Instance)
            {
                UIManager.Instance.everydayPanel.starText.text = "X10";
            }
        }
        else if(messg == "Task")
        {
            diamBtn.enabled = false;
            diamBtn.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetString("DiamTask", "diamBtn");
        }
    }
    public void OpenPanel(float Y)
    {
        gameObject.SetActive(true);
        if (UIManager.Instance)
        {
            UIManager.Instance.morePanel.ShowTip();
            content.localPosition = Vector3.up * Y;
        } 
    }
    void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        if (UIManager.Instance)
        {
            UIManager.Instance.DetectionPanel();
        }
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
            {
                UIManager.Instance.everydayPanel.starText.text = "X10";
            }
        }
        if (PlayerPrefs.GetString("DiamTask") != "")
        {
            diamBtn.enabled = false;
            diamBtn.GetComponent<Image>().color = Color.gray;
        }
    }
    private void Update()
    {
        if (maskPanel.activeInHierarchy)
        {
            load.transform.Rotate(Vector3.back*5);
        }
    }
}
