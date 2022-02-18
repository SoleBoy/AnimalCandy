using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAttributes : MonoBehaviour
{
    SkillItem item;
    SkillSort skillSort;
    Transform LStar;
    Transform Unlck;
    GameObject advance;
    GameObject advanceTip;
    GameObject advanceMask;
    GameObject lockTip;
    GameObject lockMask;
    GameObject upTip;
    GameObject upMask;
    GameObject carryTip;
    Transform parentModel;
    private AudioSource audioSource;
    Text nameText;
    Text infoText;
    Text ipnameText;

    Text basisText_L;
    Text attachText_L;
    Text upstarText_L;
    Text levelText_L;
    Text diamText_L;
    Text carryText;
    Text goldText;

    Text levelText_U;
    Text diamText_U;
    Text adDiamText_U;
    Button lockBtn_U;
    Button advanBtn_U;
    Button upBtn_L;
    Button infoBtn;
    Button carryBtn;

    Image holdImage;
    Image gemImage;
    Image titleSprite;

    private int skillId;
    private int skillGrade;
    private int keelIndex = 1;
    private float level_up;
    private float diam_up;
    private float gold_up;
    private float skill_hurt;
    private string messInfo = "";
    private bool isSelect;
    private bool isUnlock;
    private bool isRoude;
    private string keelName;
    private DragonBones.UnityArmatureComponent model_Armature;
    public void InitData(SkillSort sort,int id,Color title)
    {
        this.skillId = id;
        item = ExcelTool.Instance.skills[skillId.ToString()];
        this.skillSort = sort;
        LStar = transform.Find("LStar");
        Unlck = transform.Find("Unlck");
        advance = Unlck.Find("Advance").gameObject;
        advanceTip = Unlck.Find("Advance/AdvanceTip").gameObject;
        advanceMask = Unlck.Find("Advance/AdvanceBtn/Image").gameObject;
        lockTip = Unlck.Find("LockTip").gameObject;
        lockMask = Unlck.Find("LockBtn/Image").gameObject;
        upTip = LStar.Find("UpTip").gameObject;
        upMask = LStar.Find("UpBtn/Image").gameObject;
        carryTip = LStar.Find("CarryTip").gameObject;
        audioSource = gameObject.GetComponent<AudioSource>();
        infoText = transform.Find("InfoText").GetComponent<Text>();
        ipnameText = transform.Find("NameText").GetComponent<Text>();
        nameText = transform.Find("title/Text").GetComponent<Text>();
        basisText_L = LStar.Find("FitText").GetComponent<Text>();
        attachText_L = LStar.Find("StarDamage").GetComponent<Text>();
        upstarText_L = LStar.Find("StarLevel").GetComponent<Text>();
        levelText_L = LStar.Find("StarLevel/Text").GetComponent<Text>();
        diamText_L = LStar.Find("Diamond/Text").GetComponent<Text>();
        goldText = LStar.Find("Gold/Text").GetComponent<Text>();

        levelText_U = Unlck.Find("StarLevel/Text").GetComponent<Text>();
        diamText_U = Unlck.Find("Diamond/Text").GetComponent<Text>();
        adDiamText_U = Unlck.Find("Advance/AdvanceDiam/Text").GetComponent<Text>();
        lockBtn_U = Unlck.Find("LockBtn").GetComponent<Button>();
        advanBtn_U = Unlck.Find("Advance/AdvanceBtn").GetComponent<Button>();
        upBtn_L = LStar.Find("UpBtn").GetComponent<Button>();
        
        infoBtn = transform.Find("InfoBtn").GetComponent<Button>(); 
        lockBtn_U.onClick.AddListener(UnlockSkill);
        advanBtn_U.onClick.AddListener(AdvanceSkill);
        upBtn_L.onClick.AddListener(UpSkill);
        infoBtn.onClick.AddListener(InfoTip);
        if(GameManager.Instance.modeSelection =="roude")
        {
            isRoude = true;
            carryText = LStar.Find("CarryBtn/Text").GetComponent<Text>();
            carryBtn = LStar.Find("CarryBtn").GetComponent<Button>();
            carryBtn.onClick.AddListener(SelectCarry);
        }
        else
        {
            isRoude = false;
        }
        parentModel = transform.Find("model/mask");
        holdImage = transform.Find("model/Hold").GetComponent<Image>();
        gemImage = transform.Find("model/Gem").GetComponent<Image>();
        titleSprite = transform.Find("title").GetComponent<Image>();
        model_Armature= parentModel.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>();
        ExcelTool.LanguageEvent += CutLang;
        //ExcelTool.Instance.lockLevel.Add(item.unlock_level);
        titleSprite.color = title;
        advance.SetActive(false);
        SetKeel();
        Initialized();
    }
    private void CutLang()
    {
        UpData();
        nameText.text = ExcelTool.lang["skillrealname" + skillId];
        ipnameText.text = ExcelTool.lang["skillipname" + skillId];
        messInfo = "";
        string[] expl = item.param.Split('-');
        messInfo += ExcelTool.lang["baseatk"] + ":" + ExcelTool.lang["physical"] + "\n\n";
        if (item.type == -1)
        {
            messInfo += ExcelTool.lang["staratk"] + ":" + ExcelTool.lang["able0"] + "\n\n";
        }
        else
        {
            messInfo += ExcelTool.lang["staratk"] + ":" + ExcelTool.lang["able" + (int)item.type] + "\n\n";
        }
        string[] interval = item.att_interval.Split('|');
        if (isRoude)
        {
            messInfo += ExcelTool.lang["cool"] + ":" + interval[0] + "\n\n";
        }
        else
        {
            messInfo += ExcelTool.lang["cool"] + ":" + interval[1] + "\n\n";
        }
        messInfo += ExcelTool.lang["bulletnum"] + ":" + item.num + "\n\n";
        messInfo += ExcelTool.lang["atknum"] + ":" + item.dam_max + "\n\n";
        if (expl[0] == "up")
        {
            messInfo += ExcelTool.lang["spepower"] + ":" + ExcelTool.lang["able5"] + "\n\n";
        }
        else
        {
            messInfo += ExcelTool.lang["spepower"] + ":" + ExcelTool.lang["able6"] + "\n\n";
        }

        if (item.buff_id != "-1")
        {
            infoText.text = ExcelTool.lang["talent" + item.buff_id];
            messInfo += ExcelTool.lang["atktalent"] + ":" + ExcelTool.lang["talent" + item.buff_id];
        }
        else
        {
            infoText.text = ExcelTool.lang["able0"];
            messInfo += ExcelTool.lang["atktalent"] + ":" + ExcelTool.lang["able0"];
        }
    }

    private void Initialized()
    {
        skillGrade = PlayerPrefs.GetInt("SkillGrade" + skillId, 1);
        level_up = item.up_star + (skillGrade-1) * item.up__star_growth;
        diam_up = item.up_diamond + (skillGrade - 1) * item.up_diamond_growth;
        skill_hurt = item.atk_num + item.atk_num * (skillGrade - 1) * item.atk_growth;
        skillSort.skill_hurt = skill_hurt;
        gold_up= diam_up * GameManager.multiple;
        JudeLock(PlayerPrefs.GetInt("Skill" + skillId) == skillId || item.unlock_level <= 0);
    }
    private void JudeLock(bool isLock)
    {
        isUnlock = isLock;
        if (isLock)
        {
            LStar.gameObject.SetActive(true);
            Unlck.gameObject.SetActive(false);
            CandyQuality(skillGrade);
            if(!isRoude)
            {
                skillSort.UnlockSkill();
            }
            model_Armature.animation.Play("rest");
        }
        else
        {
            LStar.gameObject.SetActive(false);
            Unlck.gameObject.SetActive(true);
            LockData();
            skillSort.NotUnlock();
            AdvanceWay(CreateModel.Instance.sumLevel);
        }
    }
   
    public void SelectCarry()
    {
        AudioManager.Instance.PlayTouch("gift_1");
        if (isSelect)
        {
            isSelect = false;
            carryText.text = ExcelTool.lang["carry"];
            skillSort.SkillCarry(false);
            UIManager.Instance.lockSkill.Remove(skillId);
            UIManager.Instance.skillPanel.CancelSelection(skillId);
        }
        else
        {
            if(UIManager.Instance.skillPanel.SelectCarry(skillId, item.realname,skillSort.candy_num,keelName))
            {
                isSelect = true;
                carryText.text = ExcelTool.lang["unload"];
                skillSort.SkillCarry(true);
                UIManager.Instance.skillPanel.CarryHide();
                UIManager.Instance.lockSkill.Add(skillId);
            }
            else
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip30"]);
            }
        }
    }

    void LockData()
    {
        levelText_U.text=item.unlock_level.ToString();
        diamText_U.text = item.unlock_diamond.ToString();
        adDiamText_U.text = item.forcibly_unlock_diamond.ToString();
    }
    void UpData()
    {
        basisText_L.text=ExcelTool.lang["basedam"] +":"+ ExcelTool.lang["pawn"] + item.atk_percentage+"%\n    " +
                         ExcelTool.lang["king"] + item.boss_atk_percentage + "%";
        attachText_L.text= ExcelTool.lang["adddame"] + ":" + skill_hurt.ToString("F0") + ExcelTool.lang["nextgrade"] + ":" +
                        (item.atk_num + item.atk_num * skillGrade * item.atk_growth).ToString("F0");
        upstarText_L.text= ExcelTool.lang["stargrade"] + ":" + skillGrade;
        levelText_L.text = level_up.ToString();
        diamText_L.text = diam_up.ToString();
        goldText.text = UIManager.Instance.GoldDisplay(gold_up);//gold_up.ToString();
    }
    //升星
    void UpSkill()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if (CreateModel.Instance.sumLevel >= level_up)
        {
            if(UIManager.Instance.goldNumber >= gold_up && UIManager.Instance.starNumber >= diam_up)
            {
                UIManager.Instance.skillPanel.OpenBuyPanel(SkillGrade, diam_up, gold_up);
            }
            else if (UIManager.Instance.goldNumber >= gold_up)
            {
                UIManager.Instance.SetGold(-gold_up, true);
                SkillGrade();
            }
            else if (UIManager.Instance.starNumber >= diam_up)
            {
                UIManager.Instance.SetStar(-diam_up);
                SkillGrade();
            }
            else
            {
                UIManager.Instance.diamondPanel.OpenPanel();
            }
        }
        else
        {
            PlayDragon("walk");
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    void SkillGrade()
    {
        KeelAnimation.Instance.StarUpArmature();
        skillGrade++;
        CandyQuality(skillGrade);
        level_up = item.up_star + (skillGrade - 1) * item.up__star_growth;
        diam_up = item.up_diamond + (skillGrade - 1) * item.up_diamond_growth;
        skill_hurt = item.atk_num + item.atk_num * (skillGrade - 1) * item.atk_growth;
        skillSort.skill_hurt = skill_hurt;
        gold_up = diam_up * GameManager.multiple;
        UpData();
        PlayDragon("attack_1");
        AudioManager.Instance.PlaySource("warlock_" + skillId, audioSource);
        PlayerPrefs.SetInt("SkillGrade" + skillId, skillGrade);
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
    }
    //解锁 
    void UnlockSkill()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if (CreateModel.Instance.sumLevel >= item.unlock_level)
        {
            float number = item.unlock_diamond * GameManager.multiple;
            if (UIManager.Instance.goldNumber >= number)
            {
                UIManager.Instance.skillPanel.OpenBuyPanel(Unlock_Skill, item.unlock_diamond, number);
            }
            else if (UIManager.Instance.starNumber >= item.unlock_diamond)
            {
                UIManager.Instance.SetStar(-item.unlock_diamond);
                Unlock_Skill();
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
    //提前 解锁 
    void AdvanceSkill()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        float number = item.forcibly_unlock_diamond * GameManager.multiple;
        if (UIManager.Instance.goldNumber >= number)
        {
            UIManager.Instance.skillPanel.OpenBuyPanel(Unlock_Skill, item.forcibly_unlock_diamond, number);
        }
        else if (UIManager.Instance.starNumber >= item.forcibly_unlock_diamond)
        {
            UIManager.Instance.SetStar(-item.forcibly_unlock_diamond);
            Unlock_Skill();
        }
        else
        {
            UIManager.Instance.diamondPanel.OpenPanel();
        }
    }
    private void Unlock_Skill()
    {
        KeelAnimation.Instance.StarUpArmature();
        PlayerPrefs.SetInt("Skill" + skillId, skillId);
        JudeLock(true);
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
    }
    void InfoTip()
    {
        UIManager.Instance.skillPanel.TurretInfo(messInfo, ExcelTool.lang["skillrealname" + skillId], keelName);
    }
    public bool JudeUp(float level,float diam)
    {
        if (isUnlock)
        {
            if (level >= level_up && (diam >= diam_up||UIManager.Instance.goldNumber >= gold_up))
            {
                upMask.SetActive(false);
                upTip.SetActive(true);
                return true;
            }
            upMask.SetActive(true);
            upTip.SetActive(false);
            return false;
        }
        if(diam >= item.unlock_diamond && level >= item.unlock_level)
        {
            lockTip.SetActive(true);
            lockMask.SetActive(false);
            if (PlayerPrefs.GetString("UnLockSkill" + skillId) == "")
            {
                UIManager.Instance.UnlockPanel(skillId, item.unlock_level, transform.GetSiblingIndex(), false, "UnLockSkill" + skillId);
            }
        }
        else
        {
            lockMask.SetActive(true);
            lockTip.SetActive(false);
        }
        if (advance.activeSelf && diam >= item.forcibly_unlock_diamond)
        {
            advanceMask.SetActive(false);
            advanceTip.SetActive(true);
        }
        else
        {
            advanceMask.SetActive(true);
            advanceTip.SetActive(false);
        }
        return advanceTip.activeSelf || lockTip.activeSelf;
    }
    //播放动画
    private void PlayDragon(string messg)
    {
        model_Armature.animation.Reset();
        model_Armature.animation.Play(messg, 1);
        if (!model_Armature.HasEventListener(DragonBones.EventObject.COMPLETE))
        {
            model_Armature.AddEventListener(DragonBones.EventObject.COMPLETE, HideCandy);
        }
    }
    private void HideCandy(string type, DragonBones.EventObject eventObject)
    {
        model_Armature.animation.Play("rest");
    }
    //candy_101_1 龙骨动画
    private void SetKeel()
    {
        //if (keelIndex == 10) return;
        //int index = UIManager.Instance.ArmatureKeel((int)skillGrade);
        //if (keelIndex != index)
        //{
        //    keelIndex = index;
        //    if (skillId < 10)
        //    {
        //        keelName = string.Format("candy_10{0}_{1}", skillId, keelIndex);
        //    }
        //    else
        //    {
        //        keelName = string.Format("candy_1{0}_{1}", skillId, keelIndex);
        //    }
        //}
        if (skillId < 10)
            keelName = string.Format("candy_10{0}_1", skillId);
        else
            keelName = string.Format("candy_1{0}_1", skillId);
        skillSort.SetKeel(keelName);
        model_Armature = UIManager.Instance.SetArmature(model_Armature, parentModel, keelName, Vector3.one * 40, Vector3.up*-15, false, "");
    }
    //糖果升星品质
    public void CandyQuality(float level)
    {
        if (level >= 100)
        {
            holdImage.sprite = UIManager.Instance.holdSprite[9];
            gemImage.sprite = UIManager.Instance.gemSprite[9];
            gemImage.color = UIManager.Instance.quality[9];
        }
        else
        {
            int ge = 0;
            int shi = 0;
            ge = ((int)level % 10);
            if (ge == 0)
            {
                ge = 9;
            }
            else
            {
                ge -= 1;
            }
            shi = (int)((level % 100) / 10);
            holdImage.sprite = UIManager.Instance.holdSprite[shi];
            gemImage.sprite = UIManager.Instance.gemSprite[shi];
            gemImage.color = UIManager.Instance.quality[ge];
        }
    }

    public void AdvanceWay(float level)
    {
        if (item.unlock_level > level)
        {
            advance.SetActive(true);
        }
    }
    public void CarryInit()
    {
        isSelect = false;
        carryText.text =ExcelTool.lang["carry"];
        carryTip.SetActive(true);
        skillSort.SkillCarry(false);
        carryBtn.gameObject.SetActive(true);
    }
    public void CarryHide()
    {
        carryTip.SetActive(false);
        carryBtn.gameObject.SetActive(false);
    }
    public void CarryHideTip(bool isHide)
    {
        if(isSelect)
        {
            carryTip.SetActive(false);
        }
        else
        {
            carryTip.SetActive(isHide);
        }
    }
}
