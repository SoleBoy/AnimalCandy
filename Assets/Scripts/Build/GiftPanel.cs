using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftPanel : MonoBehaviour
{
    Text nextText;
    Text getText;
    Text giftText1;
    Text giftText2;
    Text giftText3;

    Button getBtn;
    Button bacakBtn;

    Image article;

    public float nextReward;
    public float highDemand;
    private int receiveIndex;

    public float maxDemand;
    private int maxIndex;
    
    public void Init()
    {
        nextText = transform.Find("Diamond/Text").GetComponent<Text>();
        getText = transform.Find("Sum").GetComponent<Text>();
        giftText1 = transform.Find("Bar/Num1").GetComponent<Text>();
        giftText2 = transform.Find("Bar/Num2").GetComponent<Text>();
        giftText3 = transform.Find("Bar/Num3").GetComponent<Text>();

        article = transform.Find("Bar/Article").GetComponent<Image>();

        getBtn = transform.Find("GetBtn").GetComponent<Button>();
        bacakBtn = transform.Find("BackBtn").GetComponent<Button>();

        getBtn.onClick.AddListener(GetReward);
        bacakBtn.onClick.AddListener(ClosePanel);


        receiveIndex = PlayerPrefs.GetInt("ReceiveIndex");
        highDemand = GetGift(receiveIndex);
        nextReward = Mathf.Round(highDemand * 0.4f + 1);
        nextReward = Mathf.Clamp(nextReward, 0, 30);
        maxDemand = highDemand;
    }

    public void SetData()
    {
        Data();
        AudioManager.Instance.PlayTouch("open_1");
        gameObject.SetActive(true);
    }

    public string GetMaxHight(float maxScore)
    {
        if (maxScore <= maxDemand)
        {
            return maxDemand.ToString("F1");
        }
        else
        {
            maxIndex = receiveIndex + 1;
            maxDemand = GetGift(maxIndex);
            while (maxScore >= maxDemand)
            {
                maxIndex += 1;
                maxDemand = GetGift(maxIndex);
            }
            return maxDemand.ToString("F1");
        }
    }

    public string GetMinHight(float minScore)
    {
        int count = 0; float index = 0;
        if (minScore < highDemand)
        {
            count = receiveIndex + 1;
            index = GetGift(count);
            while (minScore >= index)
            {
                count += 1;
                index = GetGift(count);
            }
        }
        else
        {
            count = receiveIndex + 1;
            index = GetGift(count);
            while (minScore >= index)
            {
                count += 1;
                index = GetGift(count);
            }
        }
        if(index >= minScore)
        {
            while (minScore <= index)
            {
                count -= 1;
                index = GetGift(count);
            }
            if (index < 0)
            {
                index = 0;
            }
        }
        return index.ToString("F1");
    }

    private void GetReward()
    {
        if(UIBase.Instance.maxScore >= highDemand)
        {
            AudioManager.Instance.PlayTouch("gift_1");
            UIBase.Instance.SetDiamond(nextReward);
            receiveIndex += 1;
            highDemand = GetGift(receiveIndex);
            nextReward = Mathf.Round(highDemand * 0.4f + 1);
            nextReward = Mathf.Clamp(nextReward,0, 30);
            PlayerPrefs.SetInt("ReceiveIndex", receiveIndex);
            Data();
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }

    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIBase.Instance.JudgHight();
    }

    void Data()
    {
        nextText.text = nextReward.ToString("F0");
        getText.text = receiveIndex.ToString("F0");
        float gift1 = GetGift(receiveIndex-2);
        float gift2 = GetGift(receiveIndex - 1);
        if(gift1 <= 0)
        {
            gift1 = 0;
        }
        if (gift2 <= 0)
        {
            gift2 = 0;
        }
        giftText1.text = gift1.ToString("F1")+"M";
        giftText2.text = gift2.ToString("F1") + "M";
        giftText3.text = highDemand.ToString("F1") + "M";
        if (UIBase.Instance.maxScore >= highDemand)
        {
            article.fillAmount = 1;
            getBtn.GetComponent<Image>().color = Color.white;
        }
        else
        {
            article.fillAmount = 0.5f;
            getBtn.GetComponent<Image>().color = Color.gray;
        }
    }

    float GetGift(int index)
    {
        return 1.5f + 12 * index;
    }
}

