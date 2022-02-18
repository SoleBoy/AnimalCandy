using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunProperty : MonoBehaviour
{
    public Image spriteImage;
    Text gradeText;
    Text attackText;
    Text upStarText;
    Text upDimText;
    Button upBtn;
    GameObject tipUp;
    GameObject upMask;

    Text InfoText;
    Text fortText;
    Text levelText;
    Text starText;
    Button buyBtn;
    Text realName;
    Text nameText;
    Button infoBtn;
    GameObject tipBuy;
    GameObject buyMask;
    TurretPanel turret;
    string messgInfo = "";
    public void Init()
    {
        turret = UIManager.Instance.turretPanel;
        spriteImage = transform.Find("model/Image").GetComponent<Image>();
        gradeText = transform.Find("Grade").GetComponent<Text>();
        attackText = transform.Find("Attack").GetComponent<Text>();
        upStarText = transform.Find("StarLevel1/Text").GetComponent<Text>();
        upDimText = transform.Find("Diamond1/Text").GetComponent<Text>();
        upBtn = transform.Find("UpBtn").GetComponent<Button>();
        tipUp = transform.Find("TipUp").gameObject;
        upMask = transform.Find("UpBtn/Image").gameObject;
        upBtn.onClick.AddListener(TurretUpGrade);

        InfoText = transform.Find("InfoText").GetComponent<Text>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        realName = transform.Find("title/Text").GetComponent<Text>();
        infoBtn = transform.Find("InfoBtn").GetComponent<Button>();
        infoBtn.onClick.AddListener(OpenInfo);

        fortText = transform.Find("FortText").GetComponent<Text>();
        starText = transform.Find("Diamond2/Text").GetComponent<Text>();
        levelText = transform.Find("StarLevel2/Text").GetComponent<Text>();
        buyBtn = transform.Find("BuyBtn").GetComponent<Button>();
        tipBuy = transform.Find("TipBuy").gameObject;
        buyMask = transform.Find("BuyBtn/Image").gameObject;
        buyBtn.onClick.AddListener(TurretBuyGrade);
        ExcelTool.LanguageEvent += CutLang;
        //if (CreateModel.Instance.sendCount >= 17)
        //{
        //    levelText.text = "max";
        //    starText.text = "max";
        //    buyBtn.enabled = false;
        //    buyBtn.GetComponent<Image>().color = Color.gray;
        //}
    }
    //语言切换
    private void CutLang()
    {
        nameText.text = ExcelTool.lang["fortipname"];
        realName.text = ExcelTool.lang["fortrealname"];
        BuyData();
        //if (CreateModel.Instance.sendCount >= 17)
        //{
        //    fortText.text = ExcelTool.lang["alllock"];
        //}
        //else
        //{
        //    BuyData();
        //}
        UpData();
        InfoText.text = ExcelTool.lang["turretinfo3"];
        messgInfo = "";
        messgInfo += ExcelTool.lang["turretinfo1"] + "\n\n" + ExcelTool.lang["turretinfo2"] + "\n\n" +
            ExcelTool.lang["turretinfo3"];
    }
    void BuyData()
    {
        starText.text = turret.diamTerm_send.ToString();
        fortText.text = string.Format("{0}{1}{2}", ExcelTool.lang["cur"], CreateModel.Instance.sendCount, ExcelTool.lang["desk"]);
        levelText.text = string.Format("{0}", turret.levelTerm_send);
    }
    void UpData()
    {
        gradeText.text = string.Format("{0}{1}{2}", ExcelTool.lang["cur"],turret.attackGrade, ExcelTool.lang["grade"]);
        attackText.text = string.Format("{0}:{1}", ExcelTool.lang["addatk"], turret.attackGrade);
        upStarText.text = turret.levelUp_send.ToString();
        upDimText.text = turret.diamUp_send.ToString();
    }
    void OpenInfo()
    {
        turret.OpenInfo(messgInfo, ExcelTool.lang["fortrealname"], null, spriteImage.sprite);
    }

    //升级条件
    public bool JudgeUp(float level, float diam)
    {
        float number = turret.diamUp_send * GameManager.multiple;
        if (level >= turret.levelUp_send && (diam >= turret.diamUp_send || UIManager.Instance.goldNumber >= number))
        {
            upMask.SetActive(false);
            tipUp.SetActive(true);
            return true;
        }
        upMask.SetActive(true);
        tipUp.SetActive(false);
        return false;
    }
    private void TurretUpGrade()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if (CreateModel.Instance.sumLevel >= turret.levelUp_send)
        {
            float number = turret.diamUp_send * GameManager.multiple;
            if(UIManager.Instance.goldNumber >= number && UIManager.Instance.starNumber >= turret.diamUp_send)
            {
                turret.OpenBuyPanel(GradeAttack, turret.diamUp_send, number);
            }
            else if (UIManager.Instance.goldNumber >= number)
            {
                UIManager.Instance.SetGold(-number,true);
                GradeAttack();
            }
            else if (UIManager.Instance.starNumber >= turret.diamUp_send)
            {
                UIManager.Instance.SetStar(-turret.diamUp_send);
                GradeAttack();
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
    private void GradeAttack()
    {
        KeelAnimation.Instance.StarUpArmature();
        turret.SetAttack();
        UpData();
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
    }
    //购买条件
    public bool JudgeBuyUp(float level, float diam)
    {
        if (level >= turret.levelTerm_send && diam >= turret.diamTerm_send)
        {
            buyMask.SetActive(false);
            tipBuy.SetActive(true);
            return true;
        }
        buyMask.SetActive(true);
        tipBuy.SetActive(false);
        return false;
    }
    private void TurretBuyGrade()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if (CreateModel.Instance.sumLevel >= turret.levelTerm_send)
        {
            float number = turret.diamTerm_send * GameManager.multiple;
            if (UIManager.Instance.goldNumber >= number)
            {
                turret.OpenBuyPanel(UnlockFire, turret.diamTerm_send, number);
            }
            else if (UIManager.Instance.starNumber >= turret.diamTerm_send)
            {
                UIManager.Instance.SetStar(-turret.diamTerm_send);
                UnlockFire();
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

    private void UnlockFire()
    {
        KeelAnimation.Instance.StarUpArmature();
        turret.SetSend();
        CreateModel.Instance.UnlockSend();
        BuyData();
        UIManager.Instance.IsCreateTureet();
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
        //if (CreateModel.Instance.sendCount >= 17)
        //{
        //    buyBtn.enabled = false;
        //    buyBtn.GetComponent<Image>().color = Color.gray;
        //    fortText.text = ExcelTool.lang["alllock"];
        //    tipBuy.SetActive(false);
        //}
    }
}
