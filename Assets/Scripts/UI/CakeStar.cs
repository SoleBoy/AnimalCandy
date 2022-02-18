using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CakeStar : MonoBehaviour
{
    public Image spriteImage;
    Text InfoText;
    Text numText;
    Text hpText;
    Text levelText;
    Text starText;
    Text realName;
    Text nameText;
    Button infoBtn;
    Button conBtn;
    Image sprite;
    GameObject tipUp;
    GameObject upMask;
    TurretPanel turret;
    string messgInfo = "";
    public void Init()
    {
        turret = UIManager.Instance.turretPanel;
        spriteImage = transform.Find("model/Image").GetComponent<Image>();
        sprite = transform.Find("model/Image").GetComponent<Image>();
        InfoText = transform.Find("InfoText").GetComponent<Text>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        realName = transform.Find("title/Text").GetComponent<Text>();
        infoBtn = transform.Find("InfoBtn").GetComponent<Button>();
        infoBtn.onClick.AddListener(OpenInfo);
        numText = transform.Find("FortText").GetComponent<Text>();
        hpText = transform.Find("Health").GetComponent<Text>();
        levelText = transform.Find("StarLevel/Text").GetComponent<Text>();
        starText = transform.Find("Diamond/Text").GetComponent<Text>();
        conBtn = transform.Find("UpBtn").GetComponent<Button>();
        tipUp = transform.Find("TipUp").gameObject;
        upMask = transform.Find("UpBtn/Image").gameObject;
        conBtn.onClick.AddListener(CakeUpGrade);
        ExcelTool.LanguageEvent += CutLang;
        InitData();
        if (turret.cakeGrade >= 10)
        {
            conBtn.enabled = false;
            conBtn.GetComponent<Image>().color = Color.gray;
        }
    }
    private void CutLang()
    {
        realName.text = ExcelTool.lang["cakerealname"];
        nameText.text = ExcelTool.lang["cakeipname"];
        numText.text = string.Format("{0}{1}{2}", ExcelTool.lang["cur"], turret.cakeGrade, ExcelTool.lang["pc"]);
        hpText.text = string.Format("{0}:{1}", ExcelTool.lang["life"], turret.cakeGrade * 100);
        InfoText.text = ExcelTool.lang["cakeinfo2"];
        messgInfo = "";
        messgInfo += ExcelTool.lang["cakeinfo1"] + "\n\n" + ExcelTool.lang["cakeinfo2"];
    }
    //初始化数据
    void InitData()
    {
        numText.text = string.Format("{0}{1}{2}",ExcelTool.lang["cur"],turret.cakeGrade, ExcelTool.lang["pc"]);
        hpText.text = string.Format("{0}:{1}", ExcelTool.lang["life"], turret.cakeGrade * 100);
        levelText.text = turret.levelUp_cake.ToString();
        starText.text = turret.diamUp_cake.ToString();
    }
    //购买
    public bool JudgeUp(float level, float diam)
    {
        if(turret.cakeGrade < 10 && level >= turret.levelUp_cake && diam >= turret.diamUp_cake)
        {
            if(PlayerPrefs.GetString("UnLockcake") =="")
            {
                UIManager.Instance.UnlockPanel(-1, turret.levelUp_cake, transform.GetSiblingIndex(), true, "UnLockcake", sprite.sprite);
            }
            tipUp.SetActive(true);
            upMask.SetActive(false);
            return true;
        }
        upMask.SetActive(true);
        tipUp.SetActive(false);
        return false;
    }

    void OpenInfo()
    {
        turret.OpenInfo(messgInfo, ExcelTool.lang["cakerealname"],null, spriteImage.sprite);
    }

    private void CakeUpGrade()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if(turret.cakeGrade < 10 && CreateModel.Instance.sumLevel >= turret.levelUp_cake)
        {
            float number = turret.diamUp_cake * GameManager.multiple;
            if (UIManager.Instance.goldNumber >= number)
            {
                turret.OpenBuyPanel(CakeGrade, turret.diamUp_cake, number);
            }
            else if(UIManager.Instance.starNumber >= turret.diamUp_cake)
            {
                UIManager.Instance.SetStar(-turret.diamUp_cake);
                CakeGrade();
            }
            else
            {
                UIManager.Instance.diamondPanel.OpenPanel();
            }
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }

    private void CakeGrade()
    {
        KeelAnimation.Instance.StarUpArmature();
        turret.SetCake();
        InitData();
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
        if (turret.cakeGrade >= 10)
        {
            conBtn.enabled = false;
            conBtn.GetComponent<Image>().color = Color.gray;
        }
    }
}
