using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoReward : MonoBehaviour
{
    public AdsType adsType;
    public string bgName;

    GameObject bg;
    Button adsBtn;
    Button diamBtn;
    Button closeBtn;
    Text diamText;
    void Awake()
    {
        bg = transform.parent.Find("bg/"+ bgName).gameObject;
        adsBtn = transform.Find("AdsBtn").GetComponent<Button>();
        diamBtn = transform.Find("DiamBtn").GetComponent<Button>();
        diamText = transform.Find("DiamBtn/Text").GetComponent<Text>();
        closeBtn = GetComponent<Button>();

        adsBtn.onClick.AddListener(AdsReward);
        diamBtn.onClick.AddListener(DiamonReward);
        closeBtn.onClick.AddListener(ClosePanel);
    }
    private void ClosePanel()
    {
        gameObject.SetActive(false);
        bg.SetActive(false);
        AudioManager.Instance.PlayTouch("close_1");
    }

    private void OnEnable()
    {
        if(!bg.activeInHierarchy)
        {
            bg.SetActive(true);
        }
        if(adsType == AdsType.auto)
        {
            if (PlayerPrefs.GetString("mergeDelay") == "")
            {
                diamText.text = "1";
            }
            else
            {
                diamText.text = "5";
            }
        }
    }
    private void DiamonReward()
    {
        AudioManager.Instance.PlayTouch("other_1");
        gameObject.SetActive(false);
        bg.SetActive(false);
        if (UIManager.Instance.starNumber >= 1)
        {
            switch (adsType)
            {
                case AdsType.attack:
                    if (((int)UIManager.Instance.atkTime / 3600) >= 4)
                    {
                        UIManager.Instance.AttakReward(0);
                    }
                    else
                    {
                        UIManager.Instance.SetStar(-1);
                        UIManager.Instance.AttakReward(0);
                    }
                    break;
                case AdsType.earnings:
                    if (((int)UIManager.Instance.earTime / 3600) >= 4)
                    {
                        UIManager.Instance.EarningsReward(0);
                    }
                    else
                    {
                        UIManager.Instance.SetStar(-1);
                        UIManager.Instance.EarningsReward(0);
                    }
                        break;
                case AdsType.auto:
                    if (PlayerPrefs.GetString("mergeDelay") == "")
                    {
                        UIManager.Instance.SetStar(-1);
                        
                    }
                    else if(UIManager.Instance.starNumber >= 5)
                    {
                        UIManager.Instance.SetStar(-5);
                    }
                    else
                    {
                        UIManager.Instance.diamondPanel.OpenPanel();
                        return;
                    }
                    UIManager.Instance.FitReward(0);
                    break;
                case AdsType.sweets:
                    UIManager.Instance.SetStar(-1);
                    UIManager.Instance.CandyReward(0);
                    break;
                default:
                    break;
            }
        }
        else
        {
            UIManager.Instance.freeTip.SetActive(true);
            UIManager.Instance.diamondPanel.OpenPanel();
        }
    }

    private void AdsReward()
    {
        AudioManager.Instance.PlayTouch("ads_1");
        gameObject.SetActive(false);
        bg.SetActive(false);
        switch (adsType)
        {
            case AdsType.attack:
                UIManager.Instance.VideoAttak();
                break;
            case AdsType.earnings:
                UIManager.Instance.VideoEarnings();
                break;
            case AdsType.auto:
                UIManager.Instance.VideoAuto();
                break;
            case AdsType.sweets:
                UIManager.Instance.VideoSweet();
                break;
            default:
                break;
        }
    }
}
