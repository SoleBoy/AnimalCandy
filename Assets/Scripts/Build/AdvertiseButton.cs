using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvertiseButton : MonoBehaviour
{
    public ButtonCool ResetCool;
    public ButtonCool RestoreCool;

    GameObject resetBg;
    GameObject restoreBg;
    GameObject reset;
    GameObject restore;

    Image bgImage;
    Button closeBtn;
    Button resetBtn_Diam;
    Button resetBtn_Ad;
    Button restoreBtn_Diam;
    Button restoreBtn_Ad;

    bool isReset;
    void Start()
    {
        resetBg=transform.parent.Find("LeftBg/Reset").gameObject;
        restoreBg = transform.parent.Find("LeftBg/Restore").gameObject;
        reset=transform.Find("Reset").gameObject;
        restore=transform.Find("Restore").gameObject;

        closeBtn =GetComponent<Button>();
        bgImage= GetComponent<Image>();
        resetBtn_Diam = transform.Find("Reset/Reset1").GetComponent<Button>();
        resetBtn_Ad = transform.Find("Reset/Reset2").GetComponent<Button>();

        restoreBtn_Diam = transform.Find("Restore/Restore1").GetComponent<Button>();
        restoreBtn_Ad = transform.Find("Restore/Restore2").GetComponent<Button>();

        bgImage.enabled = false;
        closeBtn.onClick.AddListener(Close);
        resetBtn_Diam.onClick.AddListener(ResetDiamond);
        resetBtn_Ad.onClick.AddListener(ResetAds);
        restoreBtn_Diam.onClick.AddListener(RestoreDiamond);
        restoreBtn_Ad.onClick.AddListener(RestoreAds);
    }
    
    private void Close()
    {
        AudioManager.Instance.PlayTouch("close_1");
        bgImage.enabled = false;
        if (isReset)
        {
            resetBg.SetActive(false);
            reset.SetActive(false);
        }
        else
        {
            restoreBg.SetActive(false);
            restore.SetActive(false);
        }
    }

    public void OpenBtn(bool isHide)
    {
        AudioManager.Instance.PlayTouch("open_1");
        isReset = isHide;
        bgImage.enabled = true;
        if (isHide)
        {
            resetBg.SetActive(true);
            reset.SetActive(true);
        }
        else
        {
            restoreBg.SetActive(true);
            restore.SetActive(true);
        }
    }


    private void ResetDiamond()
    {
        if(UIBase.Instance.diamond >= 5)
        {
            //扣除五钻石
            AudioManager.Instance.PlayTouch("ads_1");
            UIBase.Instance.SetDiamond(-5);
            ClearNew();
        }
        else
        {
            AudioManager.Instance.PlayTouch("tips_1");
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    bool isReward;
    private void ResetAds()
    {
        //看广告奖励
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
                    isReward = false;
                    ClearNew();
                }
                ResetCool.OnClick();
                GameManager.Instance.RequestRewardBasedVideo(AdsType.reset);
            };
        }
        else
        {
            ResetCool.OnClick();
            ClearNew();
        }
    }

    private void RestoreDiamond()
    {
        if (UIBase.Instance.diamond >= 1)
        {
            AudioManager.Instance.PlayTouch("ads_1");
            UIBase.Instance.SetDiamond(-1);
            Rebirth();
        }
        else
        {
            AudioManager.Instance.PlayTouch("tips_1");
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }

    private void RestoreAds()
    {
        //看广告奖励
        AudioManager.Instance.PlayTouch("ads_1");
        if (Application.platform == RuntimePlatform.Android)
        {
            GameManager.Instance.UserChoseToWatchAd(AdsType.restore);
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
                    isReward = false;
                    Rebirth();
                }
                RestoreCool.OnClick();
                GameManager.Instance.RequestRewardBasedVideo(AdsType.restore);
            };
        }
        else
        {
            RestoreCool.OnClick();
            Rebirth();
        }
    }

    void ClearNew()
    {
        UIBase.Instance.ResetAnimal();
        GameManager.Instance.CloneTip(ExcelTool.lang["tip16"]);
        Close();
    }

    void Rebirth()
    {
        UIBase.Instance.Resurrection();
        GameManager.Instance.CloneTip(ExcelTool.lang["tip17"]);
        Close();
    }
}
