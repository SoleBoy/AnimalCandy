using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class MorePanel : MonoBehaviour
{
    Button clickBtn;
    Transform shadeBg;

    //Button rankBtn;
    //Button evaluateBtn;
    Button lanBtn;
    Button giftBtn;
    Button storBtn;
    Button settBtn;
    Button helpBtn;
    //Button gameBtn;

    GameObject storeTip;
    GameObject settingTip;
    private int evaluateCount;
    void Awake()
    {
        shadeBg = transform.Find("Prent/Image");
        clickBtn = GetComponent<Button>();
        //rankBtn = shadeBg.Find("RankBtn").GetComponent<Button>();
        //evaluateBtn = shadeBg.Find("EvaluateBtn").GetComponent<Button>();
        lanBtn = shadeBg.Find("LanBtn").GetComponent<Button>();
        giftBtn = shadeBg.Find("GiftBtn").GetComponent<Button>();
        storBtn = shadeBg.Find("StoreBtn").GetComponent<Button>();
        settBtn= shadeBg.Find("SettingBtn").GetComponent<Button>();
        helpBtn = shadeBg.Find("HelpBtn").GetComponent<Button>();
        //gameBtn = shadeBg.Find("GameBtn").GetComponent<Button>();

        storeTip = shadeBg.Find("StoreBtn/tip").gameObject;
        settingTip = shadeBg.Find("SettingBtn/tip").gameObject;

        clickBtn.onClick.AddListener(ClickHide);
        //rankBtn.onClick.AddListener(ClickRanking);
        //evaluateBtn.onClick.AddListener(ClickEvaluate);
        lanBtn.onClick.AddListener(ClickLanguage);
        giftBtn.onClick.AddListener(ClickGiftBag);
        storBtn.onClick.AddListener(ClickStor);
        settBtn.onClick.AddListener(ClickSetting);
        helpBtn.onClick.AddListener(ClickHelp);
        //gameBtn.onClick.AddListener(ClickGame);
        if (PlayerPrefs.GetString("SettingTip") == "")
        {
            settingTip.SetActive(true);
        }
        else
        {
            settingTip.SetActive(false);
        }
        if (PlayerPrefs.GetString("StoreTip") == "")
        {
            storeTip.SetActive(true);
        }
        else
        {
            storeTip.SetActive(false);
        }
        evaluateCount = PlayerPrefs.GetInt("EvaluateCount");
    }

    private void ClickGame()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.isTime = true;
        UIManager.Instance.gameLinkPanel.gameObject.SetActive(true);
    }

    private void ClickHelp()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.isTime = true;
        UIManager.Instance.helpPanel.gameObject.SetActive(true);
    }

    private void ClickSetting()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.isTime = true;
        UIManager.Instance.settingsPanel.gameObject.SetActive(true);
        if (PlayerPrefs.GetString("SettingTip") == "")
        {
            PlayerPrefs.SetString("SettingTip", "settingTip");
            settingTip.SetActive(false);
        }
    }

    private void ClickStor()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.isTime = true;
        UIManager.Instance.storePanel.OpenPanel(0);
    }

    public void SetCartoon()
    {
        clickBtn.GetComponent<Image>().enabled = true;
        shadeBg.DOLocalMoveX(-120,0.5f);
    }
    //隐藏自身
    private void ClickHide()
    {
        AudioManager.Instance.PlayTouch("close_1");
        clickBtn.GetComponent<Image>().enabled = false;
        shadeBg.DOLocalMoveX(0, 0.5f);
    }
    ////排行榜
    //private void ClickRanking()
    //{
    //    AudioManager.Instance.PlayTouch("other_1");
    //    GameManager.Instance.GetComponent<GameCenterManager>().ShowLeaderboard();
    //}
//    //评价
//    private void ClickEvaluate()
//    {
//        AudioManager.Instance.PlayTouch("other_1");
//        evaluateBtn.enabled = false;
//        Invoke("RewardDiamonds", 5);
//        if (evaluateCount >= 3 && Device.RequestStoreReview())
//        {
//            return;
//        }
//#if UNITY_IPHONE
//        var url = string.Format(
//                "itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}",
//                AdsConfigure.APPID);
//        Application.OpenURL(url);
//#endif
//    }

//    void RewardDiamonds()
//    {
//        evaluateBtn.enabled = true;
//        if (evaluateCount <= 3)
//        {
//            evaluateCount += 1;
//            PlayerPrefs.GetInt("EvaluateCount", evaluateCount);
//        }
//        if (PlayerPrefs.GetString("EvaluationDiamonds") == "")
//        {
//            UIManager.Instance.SetStar(50);
//            GameManager.Instance.ClonePrompt(50, 1);
//            PlayerPrefs.SetString("EvaluationDiamonds", "Diamonds");
//        }
//    }

    //语言
    private void ClickLanguage()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.isTime = true;
        UIManager.Instance.langePanel.gameObject.SetActive(true);
    }
    //礼包
    private void ClickGiftBag()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.everydayPanel.Everyday();
    }
    public void ShowTip()
    {
        if(PlayerPrefs.GetString("StoreTip") == "")
        {
            PlayerPrefs.SetString("StoreTip", "StoreTip");
            storeTip.SetActive(false);
        }
    }
}
