using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretProperty : MonoBehaviour
{
    public int state;
    public Sprite outHSprite;
    public Sprite outLSprite;
    private GameObject carryTip;
    private GameObject tipUp;
    private GameObject upMask;
    private GameObject tipLock;
    private GameObject lockMask;
    private GameObject lstar;
    private GameObject unlck;
    private Transform modleParent;
    private AudioSource audioSource;
    private DragonBones.UnityArmatureComponent model_Armature;

    private Text fitText;
    private Text starDamText;
    private Text starLevelText;
    private Text diam_StarText;
    private Text levelText;
    private Text diam_UnlckText;
    private Text LevelUnlckText;
    private Text InfoText;
    private Text nameText;
    private Text realText;
    private Text outputText;
    private Text carryText;
    private Text goldText;
    private Image model;
    private Image holdImage;
    private Image gemImage;
    private Button infoBtn;
    private Button outputBtn;
    private Button upBtn;
    private Button carryBtn;
    private Button lockBtn;
    private float diamNum;
    private float goldNum;
    private int keelIndex=1;
    private bool isUnlock;
    private bool isOutput;
    private bool isSelect;
    private bool isRoude;
    private ShooterItem item;
    private string messgInfo = "";
    private string keelName;

    public void Init(int index)
    {
        this.state = index;
        item = ExcelTool.Instance.shooters[state];
        modleParent =transform.Find("model/mask");
        carryTip = transform.Find("LStar/CarryTip").gameObject;
        tipUp = transform.Find("LStar/TipUp").gameObject;
        upMask = transform.Find("LStar/UpBtn/Image").gameObject;
        tipLock = transform.Find("Unlck/TipUp").gameObject;
        lockMask = transform.Find("Unlck/LockBtn/Image").gameObject;
        lstar = transform.Find("LStar").gameObject;
        audioSource = gameObject.GetComponent<AudioSource>();
        model = transform.Find("model/Image").GetComponent<Image>();
        holdImage = transform.Find("model/Hold").GetComponent<Image>();
        gemImage = transform.Find("model/Gem").GetComponent<Image>();
        fitText = transform.Find("LStar/FitText").GetComponent<Text>();
        InfoText = transform.Find("InfoText").GetComponent<Text>();
        starDamText = transform.Find("LStar/StarDamage").GetComponent<Text>();
        starLevelText = transform.Find("LStar/StarLevel").GetComponent<Text>();
        diam_StarText = transform.Find("LStar/Diamond/Text").GetComponent<Text>();
        levelText= transform.Find("LStar/StarLevel/Text").GetComponent<Text>();
        outputText = transform.Find("LStar/OutputBtn/Text").GetComponent<Text>();
        goldText = transform.Find("LStar/Gold/Text").GetComponent<Text>();
        unlck = transform.Find("Unlck").gameObject;
        LevelUnlckText = transform.Find("Unlck/StarLevel/Text").GetComponent<Text>();
        diam_UnlckText = transform.Find("Unlck/Diamond/Text").GetComponent<Text>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        realText = transform.Find("title/Text").GetComponent<Text>();
        infoBtn = transform.Find("InfoBtn").GetComponent<Button>();
        upBtn = transform.Find("LStar/UpBtn").GetComponent<Button>();
        outputBtn = transform.Find("LStar/OutputBtn").GetComponent<Button>();
        lockBtn = transform.Find("Unlck/LockBtn").GetComponent<Button>();
        model_Armature = modleParent.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>();
        infoBtn.onClick.AddListener(OpenInfo);
        upBtn.onClick.AddListener(UpTurret);
        lockBtn.onClick.AddListener(UnlckTurret);
        outputBtn.onClick.AddListener(OutputTurret);
        if (GameManager.Instance.modeSelection == "roude")
        {
            isRoude = true;
            carryText = transform.Find("LStar/CarryBtn/Text").GetComponent<Text>();
            carryBtn = transform.Find("LStar/CarryBtn").GetComponent<Button>();
            carryBtn.onClick.AddListener(SelectCarry);
            carryBtn.gameObject.SetActive(true);
            outputBtn.gameObject.SetActive(false);
        }
        ExcelTool.LanguageEvent += CutLnag;
        //ExcelTool.Instance.lockLevel.Add(item.unlock_level);
        isOutput = PlayerPrefs.GetString("IsOutput" + state) != "";
        UpUnlck(PlayerPrefs.GetString("Unlock" + item.id) != "" || item.unlock_level <= 0);
        SetKeel();

    }
    private void CutLnag()
    {
        Starup();
        nameText.text = ExcelTool.lang["turretipname" + (state + 1)];
        realText.text = ExcelTool.lang["turretrealname"+(state+1)];

        messgInfo = "";
        string[] expl = item.param.Split('-');
        messgInfo += ExcelTool.lang["fitratk"] + ":" + ExcelTool.lang["physical"] + "\n\n";
        if (item.buff != "-1")
        {
            messgInfo += ExcelTool.lang["staratk"] + ":" + ExcelTool.lang["able" + item.type] + "\n\n";
        }
        else
        {
            messgInfo += ExcelTool.lang["staratk"] + ":" + ExcelTool.lang["physical"] + "\n\n";
        }
        string[] distance = item.atk_distance.Split('|');
        if (isRoude)
        {
            messgInfo += ExcelTool.lang["atkdis"] + ":" + distance[1] + "\n\n";
        }
        else
        {
            messgInfo += ExcelTool.lang["atkdis"] + ":" + distance[0] + "\n\n";
        }
        //messgInfo += ExcelTool.lang["atkdis"] + ":" + item.atk_distance + "\n\n";
        messgInfo += ExcelTool.lang["atkint"] + ":" + item.att_interval + "\n\n";
        if (item.speed == "-1")
        {
            messgInfo += ExcelTool.lang["bulletspeed"] + ":max" + "\n\n";
        }
        else
        {
            string[] speed = item.speed.Split('|');
            if (isRoude)
            {
                messgInfo += ExcelTool.lang["bulletspeed"] + ":" + speed[1] + "\n\n";
            }
            else
            {
                messgInfo += ExcelTool.lang["bulletspeed"] + ":" + speed[0] + "\n\n";
            }
        }
        messgInfo += ExcelTool.lang["atknum"] + ":" + item.dam_max + "\n\n";
        if (expl[0] == "up")
        {
            messgInfo += ExcelTool.lang["spepower"] + ":" + ExcelTool.lang["able5"] + "\n\n";
        }
        else if (expl[0] == "back")
        {
            messgInfo += ExcelTool.lang["spepower"] + ":" + ExcelTool.lang["able6"] + "\n\n";
        }
        if (item.buff != "-1")
        {
            InfoText.text = ExcelTool.lang["talent" + item.buff];
            messgInfo += ExcelTool.lang["atktalent"] + ":" + ExcelTool.lang["talent" + item.buff] + "\n\n";
        }
        else
        {
            InfoText.text = ExcelTool.lang["able0"];
            messgInfo += ExcelTool.lang["atktalent"] + ":" + ExcelTool.lang["able0"] + "\n\n";
        }
        if (item.attackrange != "1")
        {
            messgInfo += ExcelTool.lang["tip32"] + ":" + ExcelTool.lang["sky"] + ExcelTool.lang["ground"];
        }
        else
        {
            messgInfo += ExcelTool.lang["tip32"] + ":" + ExcelTool.lang["ground"];
        }

        if (isOutput)
        {
            outputText.text = ExcelTool.lang["stopturret"];
        }
        else
        {
            outputText.text = ExcelTool.lang["inturret"];
        }
    }

    private void OutputTurret()
    {
        if (isOutput)
        {
            isOutput = false;
            outputText.text = ExcelTool.lang["inturret"];
            PlayerPrefs.SetString("IsOutput" + state, "");
            outputBtn.GetComponent<Image>().sprite = outHSprite;
            CreateModel.Instance.ControlProduc(false, state);
            return;
        }
        if (CreateModel.Instance.productions.Count > 1)
        {
            if (!isOutput)
            {
                isOutput = true;
                PlayerPrefs.SetString("IsOutput" + state, "put");
                outputText.text = ExcelTool.lang["stopturret"];
                outputBtn.GetComponent<Image>().sprite = outLSprite;
                CreateModel.Instance.ControlProduc(true, state);
            }
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["turretTip"]);
        }
    }

    private void UpUnlck(bool isUp)
    {
        isUnlock = isUp;
        if (isUp)
        {
            unlck.gameObject.SetActive(false);
            lstar.gameObject.SetActive(true);
            item.starLevel = PlayerPrefs.GetFloat("StarLevel" + state, 1);
            Starup();
            CandyQuality(item.starLevel);
            PlayerPrefs.SetFloat("StarLevel" + state, item.starLevel);
            PlayerPrefs.SetString("Unlock" + item.id, item.ip_name);
            if (!isRoude)
            {
                if (isOutput)
                {
                    outputBtn.GetComponent<Image>().sprite = outLSprite;
                    outputText.text = ExcelTool.lang["stopturret"];
                }
                else
                {
                    outputText.text = ExcelTool.lang["inturret"];
                    outputBtn.GetComponent<Image>().sprite = outHSprite;
                    CreateModel.Instance.ControlProduc(false, state);
                }
            }
        }
        else
        {
            unlck.gameObject.SetActive(true);
            lstar.gameObject.SetActive(false);
            LevelUnlckText.text = string.Format("{0}", item.unlock_level);
            diam_UnlckText.text = item.unlock_diamond.ToString();
        }
    }

    private void UnlckTurret()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if (CreateModel.Instance.sumLevel >= item.unlock_level)
        {
            float number = item.unlock_diamond * GameManager.multiple;
            if (UIManager.Instance.goldNumber >= number)
            {
                UIManager.Instance.turretPanel.OpenBuyPanel(UnlockTurret, item.unlock_diamond, number);
            }
            else if (UIManager.Instance.starNumber >= item.unlock_diamond)
            {
                UIManager.Instance.SetStar(-item.unlock_diamond);
                UnlockTurret();
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

    private void UnlockTurret()
    {
        UpUnlck(true);
        KeelAnimation.Instance.StarUpArmature();
        PlayerPrefs.SetFloat("StarLevel" + state, item.starLevel);
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
        //CreateModel.Instance.UnlockTurret(state + 1);
        //UIManager.Instance.guidePanel.OpenGuide(19, true);
    }

    private void Starup()
    {
        diamNum = ExcelTool.Instance.ConDiamond(state);
        goldNum = diamNum * GameManager.multiple;
        goldText.text = UIManager.Instance.GoldDisplay(goldNum);//goldNum.ToString();
        diam_StarText.text = diamNum.ToString();
        fitText.text = ExcelTool.lang["firtdam"] + ":" + ExcelTool.Instance.FitHurt(state);
        starDamText.text = ExcelTool.lang["stardam"] + ":" + ExcelTool.Instance.StarHurt(state);
        starLevelText.text = ExcelTool.lang["stargrade"] + ":" + item.starLevel.ToString();
        levelText.text = string.Format("{0}", ExcelTool.Instance.UpStar(state));
    }

    private void UpTurret()
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if (ExcelTool.Instance.UpStar(state) <= CreateModel.Instance.sumLevel)
        {
            if(UIManager.Instance.goldNumber >= goldNum && UIManager.Instance.starNumber >= diamNum)
            {
                UIManager.Instance.turretPanel.OpenBuyPanel(TurretGrade, diamNum, goldNum);
            }
            else if(UIManager.Instance.goldNumber >= goldNum)
            {
                UIManager.Instance.SetGold(-goldNum, true);
                TurretGrade();
            }
            else if(UIManager.Instance.starNumber >= diamNum)
            {
                UIManager.Instance.SetStar(-diamNum);
                TurretGrade();
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
    private void TurretGrade()
    {
        KeelAnimation.Instance.StarUpArmature();
        item.starLevel += 1;
        CandyQuality(item.starLevel);
        Starup();
        SetKeel();
        PlayDragon("attack_1");
        CreateModel.Instance.StarTurret();
        PlayerPrefs.SetFloat("StarLevel" + state, item.starLevel);
        AudioManager.Instance.PlaySource("shooter_" + (state + 1), audioSource);
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
    }

    public bool UpGrade(float level, float diam)
    {
        if (isUnlock)
        {
            if (level >= ExcelTool.Instance.UpStar(state) && (UIManager.Instance.goldNumber >= goldNum || diam >= diamNum))
            {
                upMask.SetActive(false);
                tipUp.SetActive(true);
                return true;
            }
            tipUp.SetActive(false);
            upMask.SetActive(true);
        }
        else
        {
            if (level >= item.unlock_level && diam >= item.unlock_diamond)
            {
                tipLock.SetActive(true);
                lockMask.SetActive(false);
                if (PlayerPrefs.GetString("UnLockTurret" + (state + 1)) == "")
                {
                    UIManager.Instance.UnlockPanel(state + 1, item.unlock_level, transform.GetSiblingIndex(), true, "UnLockTurret" + (state + 1));
                }
                return true;
            }
            tipLock.SetActive(false);
            lockMask.SetActive(true);
        }
        return false;
    }
    private void OpenInfo()
    {
        UIManager.Instance.turretPanel.OpenInfo(messgInfo, ExcelTool.lang["turretrealname" + (state + 1)],keelName, null);
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
    //candy_1_1 龙骨动画
    private void SetKeel()
    {
        if (keelIndex == 3 && keelIndex != (int)item.starLevel) return;
        if(isUnlock)
        {
            keelIndex = (int)item.starLevel;
        }
        else
        {
            keelIndex = 1;
        }
        if (keelIndex > 3)
        {
            keelIndex = 3;
        }
        keelName = string.Format("candy_{0}_{1}", state + 1, keelIndex);
        model_Armature = UIManager.Instance.SetArmature(model_Armature, modleParent, keelName, Vector3.one * 60, Vector3.up * -15, false, "");
        if(isUnlock)
        {
            model_Armature.animation.Play("rest");
        }
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

    public void SelectCarry()
    {
        AudioManager.Instance.PlayTouch("gift_1");
        if (isSelect)
        {
            isSelect = false; isOutput = true;
            carryText.text = ExcelTool.lang["carry"];
            outputBtn.GetComponent<Image>().sprite = outLSprite;
            outputText.text = ExcelTool.lang["stopturret"];
            CreateModel.Instance.ControlProduc(true, state);
            UIManager.Instance.turretPanel.CancelSelection(state);
        }
        else
        {
            if (UIManager.Instance.turretPanel.SelectCarry(state, item.realname,keelName))
            {
                isSelect = true; isOutput = false;
                carryText.text = ExcelTool.lang["unload"];
                outputText.text = ExcelTool.lang["inturret"];
                outputBtn.GetComponent<Image>().sprite = outHSprite;
                UIManager.Instance.turretPanel.CarryHide();
                CreateModel.Instance.ControlProduc(false, state);
            }
            else
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip30"]);
            }
        }
    }

    public void CarryInit()
    {
        isSelect = false;
        carryText.text = ExcelTool.lang["carry"];
        carryTip.SetActive(true);
        carryBtn.gameObject.SetActive(true);
        outputBtn.gameObject.SetActive(false);
    }

    public void CarryHideTip(bool isHide)
    {
        if (isUnlock)
        {
            if (isSelect)
            {
                carryTip.SetActive(false);
            }
            else
            {
                carryTip.SetActive(isHide);
            }
        }   
    }

    public void CarryHide()
    {
        carryTip.SetActive(false);
        carryBtn.gameObject.SetActive(false);
        if (isSelect)
        {
            outputBtn.gameObject.SetActive(true);
        }
    }

    public void FirstH()
    {
        outputBtn.gameObject.SetActive(true);
    }
}
