using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortProperty : MonoBehaviour
{
    public Image spriteImage;
    //转化
    Text conText;
    Text cLevelText;
    Text cdimText;
    Button conBtn;
    GameObject ctipUp;
    GameObject ctipMask;
    //拥有
    Text buyText;
    Text bLevelText;
    Text bdimText;
    Button buyBtn;

    Text InfoText;
    Text realName;
    Text nameText;
    Button infoBtn;
    GameObject btipUp;
    GameObject btipMask;
    TurretPanel turret;
    string messgInfo = "";
    public void Init()
    {
        turret = UIManager.Instance.turretPanel;
        spriteImage = transform.Find("model/Image").GetComponent<Image>();
        conText = transform.Find("ConText").GetComponent<Text>();
        cLevelText = transform.Find("StarLevel1/Text").GetComponent<Text>();
        cdimText = transform.Find("Diamond1/Text").GetComponent<Text>();
        InfoText = transform.Find("InfoText").GetComponent<Text>();
        conBtn = transform.Find("ConBtn").GetComponent<Button>();
        ctipUp = transform.Find("TipUp1").gameObject;
        ctipMask = transform.Find("ConBtn/Image").gameObject;
        conBtn.onClick.AddListener(ConUpGrade);

        nameText = transform.Find("NameText").GetComponent<Text>();
        realName = transform.Find("title/Text").GetComponent<Text>();
        infoBtn = transform.Find("InfoBtn").GetComponent<Button>();
        infoBtn.onClick.AddListener(OpenInfo);

        buyText = transform.Find("BuyText").GetComponent<Text>();
        bLevelText = transform.Find("StarLevel2/Text").GetComponent<Text>();
        bdimText = transform.Find("Diamond2/Text").GetComponent<Text>();
        buyBtn = transform.Find("BuyBtn").GetComponent<Button>();
        btipUp = transform.Find("TipUp2").gameObject;
        btipMask = transform.Find("BuyBtn/Image").gameObject;
        buyBtn.onClick.AddListener(BuyUpGrade);
        ExcelTool.LanguageEvent += CutLang;
        if (CreateModel.Instance.spanCount >= 20)
        {
            bLevelText.text = "max";
            bdimText.text = "max";
            buyBtn.enabled = false;
            buyBtn.GetComponent<Image>().color = Color.gray;
        }
        if (CreateModel.Instance.conCount >= 20)
        {
            cLevelText.text = "max";
            cdimText.text = "max";
            conBtn.enabled = false;
            conBtn.GetComponent<Image>().color = Color.gray;
        }
    }
    //语言切换
    private void CutLang()
    {
        nameText.text = ExcelTool.lang["fitipname"];
        realName.text= ExcelTool.lang["fitrealname"];
        if (CreateModel.Instance.spanCount >= 20)
        {
            buyText.text = ExcelTool.lang["alllock"];
        }
        else
        {
            BuyData();
        }
        if (CreateModel.Instance.conCount >= 20)
        {
            conText.text = ExcelTool.lang["allcon"];
        }
        else
        {
            ConData();
        }
        InfoText.text = ExcelTool.lang["firtinfo4"];
        messgInfo = "";
        messgInfo += ExcelTool.lang["firtinfo1"] + "\n\n" + ExcelTool.lang["firtinfo2"]+ "\n\n" +
            ExcelTool.lang["firtinfo3"]+ "\n\n" + ExcelTool.lang["firtinfo4"];
    }
    //详细信息
    void OpenInfo()
    {
        turret.OpenInfo(messgInfo, ExcelTool.lang["fitrealname"], null, spriteImage.sprite);
    }
    //初始化数据
    void BuyData()
    {
        buyText.text = string.Format("{0}{1}{2}", ExcelTool.lang["have"], CreateModel.Instance.spanCount, ExcelTool.lang["desk"]);
        bLevelText.text = string.Format("{0}", turret.levelTerm_span);
        bdimText.text = turret.diamTerm_span.ToString();
    }
    void ConData()
    {
        conText.text = string.Format("{0}{1}{2}", ExcelTool.lang["turn"], CreateModel.Instance.conCount, ExcelTool.lang["desk"]);
        cLevelText.text = string.Format("{0}", turret.levelCon_span);
        cdimText.text = turret.diamCon_span.ToString();
    }
    //购买
    public bool JudgeBuyUp(float level, float diam)
    {
        if (CreateModel.Instance.spanCount < 20 && level >= turret.levelTerm_span && diam >= turret.diamTerm_span)
        {
            btipMask.SetActive(false);
            btipUp.SetActive(true);
            return true;
        }
        btipMask.SetActive(true);
        btipUp.SetActive(false);
        return false;
    }
    private void BuyUpGrade()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if(CreateModel.Instance.sumLevel >= turret.levelTerm_span)
        {
            float number = turret.diamTerm_span * GameManager.multiple;
            if (UIManager.Instance.goldNumber >= number)
            {
                turret.OpenBuyPanel(UnlockFit, turret.diamTerm_span,number);
            }
            else if(UIManager.Instance.starNumber >= turret.diamTerm_span)
            {
                UIManager.Instance.SetStar(-turret.diamTerm_span);
                UnlockFit();
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
    private void UnlockFit()
    {
        KeelAnimation.Instance.StarUpArmature();
        CreateModel.Instance.UnlockSpan();
        turret.SetSpan();
        BuyData();
        UIManager.Instance.IsCreateTureet();
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
        if (CreateModel.Instance.spanCount >= 20)
        {
            buyBtn.enabled = false;
            buyBtn.GetComponent<Image>().color = Color.gray;
            buyText.text = ExcelTool.lang["alllock"];
            btipUp.SetActive(false);
        }
    }

    //转化
    public bool JudgeConUp(float level, float diam)
    {
        if (CreateModel.Instance.conCount < 20 && CreateModel.Instance.conCount <= CreateModel.Instance.spanCount 
            && level >= turret.levelCon_span && diam >= turret.diamCon_span)
        {
            ctipMask.SetActive(false);
            ctipUp.SetActive(true);
            return true;
        }
        ctipMask.SetActive(true);
        ctipUp.SetActive(false);
        return false;
    }
    private void ConUpGrade()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if(CreateModel.Instance.conCount <= CreateModel.Instance.spanCount &&
            CreateModel.Instance.sumLevel >= turret.levelCon_span)
        {
            float number = turret.diamCon_span * GameManager.multiple;
            if (UIManager.Instance.goldNumber >= number)
            {
                turret.OpenBuyPanel(ConvertFit, turret.diamCon_span, number);
            }
            else if (UIManager.Instance.starNumber >= turret.diamCon_span)
            {
                UIManager.Instance.SetStar(-turret.diamCon_span);
                ConvertFit();
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

    private void ConvertFit()
    {
        KeelAnimation.Instance.StarUpArmature();
        CreateModel.Instance.ConvertGun();
        turret.SetConvart();
        ConData();
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
        if (CreateModel.Instance.conCount >= 20)
        {
            conBtn.enabled = false;
            conBtn.GetComponent<Image>().color = Color.gray;
            conText.text = ExcelTool.lang["allcon"];
            ctipUp.SetActive(false);
        }
    }
}
