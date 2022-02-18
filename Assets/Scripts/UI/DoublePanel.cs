using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoublePanel : MonoBehaviour
{
    public Color[] colors;
    Text titleText;
    Text infoText1;
    Text infoText2;
    Text timeText;
    Image titleImage;
    Image barImage;
    Button getBtn;
    Button closeBtn;
    AdsType adsType;

    private float countdown;
    private int atkIndex;
    private int earIndex;
    private int autoIndex;
    private int candyIndex;
    private int skillIndex;
    public void Init()
    {
        titleText = transform.Find("TitleImage/Text").GetComponent<Text>();
        infoText1 = transform.Find("InfoText1").GetComponent<Text>();
        infoText2 = transform.Find("InfoText2").GetComponent<Text>();
        timeText = transform.Find("TimeText").GetComponent<Text>();
        titleImage = transform.Find("TitleImage").GetComponent<Image>();
        barImage = transform.Find("ImageBar/Bar").GetComponent<Image>();
        getBtn = transform.Find("GetBtn").GetComponent<Button>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        getBtn.onClick.AddListener(GetDiamond);
        closeBtn.onClick.AddListener(ClosePanel);
        atkIndex = PlayerPrefs.GetInt("AttackIndex");
        earIndex = PlayerPrefs.GetInt("EarningsIndex");
        autoIndex = PlayerPrefs.GetInt("AutomaticIndex");
        candyIndex = PlayerPrefs.GetInt("CandyIndex");
        skillIndex = PlayerPrefs.GetInt("SkillIndex");
    }
    private void GetDiamond()
    {
        AudioManager.Instance.PlayTouch("detdiamond_1");
        switch (adsType)
        {
            case AdsType.attack:
                atkIndex = 0;
                PlayerPrefs.SetInt("AttackIndex", atkIndex);
                break;
            case AdsType.earnings:
                earIndex = 0;
                PlayerPrefs.SetInt("EarningsIndex", earIndex);
                break;
            case AdsType.auto:
                autoIndex = 0;
                PlayerPrefs.SetInt("AutomaticIndex", autoIndex);
                break;
            case AdsType.sweets:
                candyIndex = 0;
                PlayerPrefs.SetInt("CandyIndex", candyIndex);
                break;
            case AdsType.skill:
                skillIndex = 0;
                PlayerPrefs.SetInt("SkillIndex", skillIndex);
                break;
            default:
                break;
        }
        getBtn.enabled = false;
        getBtn.GetComponent<Image>().color=Color.gray;
        barImage.fillAmount = 0;
        UIManager.Instance.guidePanel.OpenGuide(17, true);
        if (PlayerPrefs.GetString("DiamTask") != "")
        {
            UIManager.Instance.SetStar(10);
            GameManager.Instance.ClonePrompt(10, 1);
        }
        else
        {
            UIManager.Instance.SetStar(5);
            GameManager.Instance.ClonePrompt(5, 1);
        }
    }

    public void SetPanel(AdsType type,int index,float time)
    {
        adsType = type;
        countdown = time;
        int num = 0;
        titleImage.color = colors[(int)type];
        switch (type)
        {
            case AdsType.attack:
                InfoMessg(ExcelTool.lang["atkgrow"], ExcelTool.lang["efftime"],
                    ExcelTool.lang["adsinfo1_1"] +"\n"+ ExcelTool.lang["adsinfo1_2"]);
                 atkIndex += index;
                num = atkIndex;
                PlayerPrefs.SetInt("AttackIndex", atkIndex);
                break;
            case AdsType.earnings:
                InfoMessg(ExcelTool.lang["eardouble"], ExcelTool.lang["efftime"],
                    ExcelTool.lang["adsinfo2_1"] + "\n" + ExcelTool.lang["adsinfo2_2"]);
                earIndex += index;
                num = earIndex;
                PlayerPrefs.SetInt("EarningsIndex", earIndex);
                break;
            case AdsType.auto:
                InfoMessg(ExcelTool.lang["freefit"], ExcelTool.lang["cooltime"],
                    ExcelTool.lang["adsinfo3_1"] + "\n" + ExcelTool.lang["adsinfo3_2"]);
                autoIndex += index;
                num = autoIndex;
                PlayerPrefs.SetInt("AutomaticIndex", autoIndex);
                break;
            case AdsType.sweets:
                InfoMessg(ExcelTool.lang["candy"], ExcelTool.lang["cooltime"],
                    ExcelTool.lang["adsinfo4"]);
                candyIndex += index;
                num = candyIndex;
                PlayerPrefs.SetInt("CandyIndex", candyIndex);
                break;
            case AdsType.skill:
                InfoMessg(ExcelTool.lang["freeskill"], ExcelTool.lang["cooltime"],
                    ExcelTool.lang["adsinfo5"]);
                skillIndex += index;
                num = skillIndex;
                PlayerPrefs.SetInt("SkillIndex", skillIndex);
                break;
            default:
                break;
        }
        num = Mathf.Clamp(num,0, 5);
        barImage.fillAmount = num * 0.2f;
        if (num >= 5)
        {
            getBtn.enabled = true;
            getBtn.GetComponent<Image>().color = Color.white;
        }
        else
        {
            getBtn.enabled = false;
            getBtn.GetComponent<Image>().color = Color.gray;
        }
        TimeFormat();
    }
    private void OnEnable()
    {
        GameManager.Instance.ShowBanner();
    }
    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        GameManager.Instance.HideBanner();
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
    }
    void TimeFormat()
    {
        int hour = (int)countdown / 3600;
        int minute = (int)(countdown - hour * 3600) / 60;
        int second = (int)(countdown - hour * 3600 - minute * 60);
        timeText.text = string.Format("{0:D1}:{1:D2}:{2:D2}", hour, minute, second);
    }

    //信息描述
    void InfoMessg(string title,string info1,string info2)
    {
        titleText.text = title;
        infoText1.text = info1;
        infoText2.text = info2;
    }

}
