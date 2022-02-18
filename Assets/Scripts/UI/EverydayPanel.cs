using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EverydayPanel : MonoBehaviour
{
    public Sprite hightImage;
    public Sprite dimImage;
    public Button[] buttons;
    public Text[] goldTexts;
    public Text[] dateTexts;
    public Text diamondText;
    public Text goldText;
    private Text vipText;
    private Text infoText;
    public Text starText;
    private Text diamondTime;

    private Transform roteImage;
    private Button adsDimBtn;
    private Button vipBtn;
    private Button closeBtn;
    Image diamShadow;

    private int[] gold = new int[7];
    private int[] date = new int[7];

    private int signNum;//签到次数
    public float adsTime;
    public float vipTime;
    private float adsTemp;
    private float vipTemp;
    public void Init()
    {
        roteImage = transform.Find("RoteImage");
        diamondText = transform.Find("Diamond/Text").GetComponent<Text>();
        goldText = transform.Find("Candy/Text").GetComponent<Text>();
        vipText = transform.Find("vip/Button/TimeText").GetComponent<Text>();
        infoText = transform.Find("vip/Button/Text").GetComponent<Text>();
        starText = transform.Find("DiamondAds/star").GetComponent<Text>();
        diamondTime = transform.Find("DiamondAds/DiamondTime").GetComponent<Text>();
        adsDimBtn = transform.Find("DiamondAds").GetComponent<Button>();
        diamShadow = transform.Find("DiamondAds/Shadow").GetComponent<Image>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        vipBtn = transform.Find("vip/Button").GetComponent<Button>();
        vipBtn.onClick.AddListener(JudgeDimAds);
        closeBtn.onClick.AddListener(OnClose);
        adsDimBtn.onClick.AddListener(OnDiamondAds);
        for (int i = 0; i < gold.Length; i++)
        {
            gold[i] = PlayerPrefs.GetInt("SignReward" + i, (i + 1) * 100);
        }
        for (int i = 0; i < date.Length; i++)
        {
            date[i] = PlayerPrefs.GetInt("DateRecord" + i, i + 1);
        }
        vipTime = PlayerPrefs.GetFloat("vipCandy");
        adsTime = PlayerPrefs.GetFloat("DiamondAds");
        
        if(vipTime > 0)
        {
            vipBtn.enabled = false;
            vipBtn.GetComponent<Image>().color = Color.gray;
        }
        ExcelTool.LanguageEvent += CutLang;
        for (int i = 0; i < goldTexts.Length; i++)
        {
            goldTexts[i].text = gold[i].ToString();
        }
        for (int i = 0; i < dateTexts.Length; i++)
        {
            dateTexts[i].text = date[i].ToString();
        }
        
    }
    public void EventView()
    {
        if (adsTime > 0)
        {
            diamShadow.gameObject.SetActive(true);
            adsDimBtn.enabled = false;
        }
        else if (!GameManager.Instance.isDay)
        {
            Currently(adsDimBtn.transform.localPosition);
        }
        if (!GameManager.Instance.isCansign)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Image>().sprite = dimImage;
                buttons[i].enabled = false;
            }
            return;
        }
        signNum = PlayerPrefs.GetInt("SignNum");
        if (!GameManager.Instance.isDay)
        {
            signNum -= 1;
            RefreshView(false);
        }
        else
        {
            RefreshView(true);
        }
    }
    private void CutLang()
    {
        if (vipTime <= 0)
        {
            infoText.text = ExcelTool.lang["draw"];
        }
        else
        {
            infoText.text = ExcelTool.lang["today"];
        }
    }
    //签到判断
    public void Everyday()
    {
        if (roteImage == null) return;
        UIManager.Instance.isTime = true;
        GameManager.Instance.InitTimeData();
        gameObject.SetActive(true);
        EventView();
    }

    //钻石
    bool isAdrewads;
    void OnDiamondAds()
    {
        AudioManager.Instance.PlayTouch("ads_1");
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GameManager.Instance.UserChoseToWatchAd(AdsType.diamond);
            GameManager.Instance.AdmobRewardCB = delegate
            {
                
            };
            GameManager.Instance.AdmobRewardClose = delegate
            {
                isAdrewads = true;
                roteImage.gameObject.SetActive(false);
                diamShadow.gameObject.SetActive(true);
                adsDimBtn.enabled = false;
                adsTime = 7200;
                PlayerPrefs.SetFloat("DiamondAds", adsTime);
            };
            GameManager.Instance.AdmobClose = delegate
            {
                if(isAdrewads)
                {
                    if (PlayerPrefs.GetString("vipReward") != "")
                    {
                        UIManager.Instance.SetStar(10);
                    }
                    else
                    {
                        UIManager.Instance.SetStar(5);
                    }
                    GameManager.Instance.CloneTip(ExcelTool.lang["tip7"]);
                }
                isAdrewads = false;
                GameManager.Instance.RequestRewardBasedVideo(AdsType.diamond);
            };
        }
        else
        {
            roteImage.gameObject.SetActive(false);
            diamShadow.gameObject.SetActive(true);
            adsDimBtn.enabled = false;
            if (PlayerPrefs.GetString("vipReward") != "")
            {
                UIManager.Instance.SetStar(10);
            }
            else
            {
                UIManager.Instance.SetStar(5);
            }
            GameManager.Instance.CloneTip(ExcelTool.lang["tip7"]);
            adsTime = 7200;
            PlayerPrefs.SetFloat("DiamondAds", adsTime);
        }
    }
    public void Cooling(float time)
    {
        if (adsTime > 0)
        {
            adsTemp += time;
            if(adsTemp >= 1)
            {
                adsTime -= 1;
                adsTemp = 0;
                if (adsTime > 0)
                {
                    int hour = (int)adsTime / 3600;
                    int minute = (int)(adsTime - hour * 3600) / 60;
                    int second = (int)(adsTime - hour * 3600 - minute * 60);
                    diamondTime.text = string.Format("{0:D1}:{1:D2}:{2:D2}", hour, minute, second);
                    diamShadow.fillAmount = adsTime / 7200;
                    PlayerPrefs.SetFloat("DiamondAds", adsTime);
                }
                else
                {
                    diamondTime.text = "";
                    diamShadow.gameObject.SetActive(false);
                    diamShadow.fillAmount = 1;
                    adsDimBtn.enabled = true;
                }
            }
        }
    }
    //视图刷新
    private void RefreshView(bool isRote)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if(i < signNum)
            {
                buttons[i].GetComponent<Image>().sprite = dimImage;
                buttons[i].enabled = false;
            }
            else if(i == signNum)
            {
                if(isRote)
                {
                    buttons[i].GetComponent<Image>().sprite = hightImage;
                    buttons[i].enabled = true;
                    Currently(buttons[i].transform.localPosition);
                }
                else
                {
                    buttons[i].GetComponent<Image>().sprite = dimImage;
                    buttons[i].enabled = isRote;
                }
            }
            else
            {
                buttons[i].GetComponent<Image>().sprite = hightImage;
                buttons[i].enabled = true;
            }
        }
    }
    //签到按钮点击
    public void OnSignClick(int index)
    {
        AudioManager.Instance.PlayTouch("gift_1");
        if (signNum == index)
        {
            //给用户加金币
            UserGift();
            GameManager.Instance.RefreshDate();
            PlayerPrefs.SetInt("SignNum", (signNum+1));
            GameManager.Instance.isDay = false;
            ////刷新视图
            //signNum += 1;
            //RefreshView(true);
            //新的签到周期，需要清除签到存档
            if (signNum >= 6)
            {
                PlayerPrefs.DeleteKey("SignNum");
            }
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip10"]);
            Debug.Log("日期未到");
        }
    }
    void UserGift()
    {
        GameManager.Instance.ClonePrompt(gold[signNum], 0);
        UIManager.Instance.SetGold(gold[signNum]);
        buttons[signNum].GetComponent<Image>().sprite = dimImage;
        buttons[signNum].enabled = false;
        if (adsTime <= 0)
        {
            Currently(adsDimBtn.transform.localPosition);
        }
        else
        {
            roteImage.gameObject.SetActive(false);
        }
        //更新奖励
        gold[signNum] += 700;
        gold[signNum] = Mathf.Clamp(gold[signNum],1,1500);
        goldTexts[signNum].text = gold[signNum].ToString();
        for (int i = 0; i < gold.Length; i++)
        {
            PlayerPrefs.SetInt("SignReward" + i, gold[i]);
        }
        date[signNum] += 7;
        dateTexts[signNum].text = date[signNum].ToString();
        for (int i = 0; i < date.Length; i++)
        {
            PlayerPrefs.SetInt("DateRecord" + i, date[i]);
        }
    }
    void JudgeDimAds()
    {
        AudioManager.Instance.PlayTouch("detdiamond_1");
        if (PlayerPrefs.GetString("vipReward")!="")
        {
            if (vipTime <= 0)
            {
                vipTime = 5400;
                vipBtn.enabled = false;
                vipBtn.GetComponent<Image>().color = Color.gray;
                UIManager.Instance.SetGold(100000);
                GameManager.Instance.ClonePrompt(100000, 0);
                PlayerPrefs.SetFloat("vipCandy", vipTime);
            }
            else
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip13"]);
            }
        }
        else
        {
            //UIManager.Instance.isTime = true;
            UIManager.Instance.storePanel.OpenPanel(820);
        }
    }
    public void VipCandy(float time)
    {
        if(vipTime > 0)
        {
            vipTemp += time;
            if(vipTemp >= 1)
            {
                vipTime -= 1;
                vipTemp = 0;
                if (vipTime > 0)
                {
                    PlayerPrefs.SetFloat("vipCandy", vipTime);
                    int hour = (int)vipTime / 3600;
                    int minute = (int)(vipTime - hour * 3600) / 60;
                    int second = (int)(vipTime - hour * 3600 - minute * 60);
                    vipText.text = string.Format("{0:D1}:{1:D2}:{2:D2}", hour, minute, second);
                    infoText.text = ExcelTool.lang["today"];
                }
                else
                {
                    vipText.text = "";
                    infoText.text = ExcelTool.lang["draw"];
                    vipBtn.enabled = true;
                    vipBtn.GetComponent<Image>().color = Color.white;
                }
            }
        }
    }
    void Currently(Vector3 point)
    {
        point.x += 70;
        point.y -= 50;
        roteImage.localPosition = point;
        roteImage.GetComponent<TipCone>().maxY = point.y + 10;
        roteImage.GetComponent<TipCone>().minY = point.y - 15;
        roteImage.gameObject.SetActive(true);
    }

    void OnClose()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
    }
}
