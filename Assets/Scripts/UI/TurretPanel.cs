using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretPanel : MonoBehaviour
{
    public CarryOut[] carryOuts;
    public Image tipImage;
    public DragonBones.UnityArmatureComponent model_Armature;
    private Transform parent;
    public Text starText;
    public Text goldText;
    private Text levelText;
    private Text infoText;
    private Text nameText;
    private Text buyText_diam;
    private Text buyText_gold;
    private Text buyposse_diam;
    private Text buyposse_gold;
    private Image buyImage;
    private Image infoSprite;
    private Button closeBtn;
    private Button bcakBtn;
    private Button infoBtn;
    private Button buyBtn;
    private FortProperty fortData;
    private GunProperty gunData;
    private CakeStar cakeData;
    private GameObject infoPanel;
    private GameObject buyPanel;
    private GameObject goldtip_buy;
    private GameObject diamtip_buy;
    private Transform turretPrefab;
    private Action action_buy;
    private DragonBones.UnityArmatureComponent info_Armature;

    private float diam_Consume;
    private float gold_Consume;

    private int spanGrade;
    public float levelTerm_span;
    public float diamTerm_span;

    private int conGrade;
    public float levelCon_span;
    public float diamCon_span;

    private int sendGrade;
    public float levelTerm_send;
    public float diamTerm_send;
    public int attackGrade;
    public float levelUp_send;
    public float diamUp_send;

    public int cakeGrade;
    public float levelUp_cake;
    public float diamUp_cake;

    private bool isUp;
    private bool isCarryMax;

    private GameObject selectPanel;
    private GameObject selectTip;
    private Text carryText;
    private Button warBtn;
    private int carry_max;
    private int indexCount;
    private List<TurretProperty> attributes = new List<TurretProperty>();
    private Dictionary<int, CarryOut> carryKey = new Dictionary<int, CarryOut>();
    public void Init()
    {
        parent = transform.Find("Scroll View/Viewport/Content");
        turretPrefab = transform.Find("turret");
        starText = transform.Find("Star/StarText").GetComponent<Text>();
        goldText = transform.Find("Gold/Text").GetComponent<Text>();
        levelText = transform.Find("Level/Text").GetComponent<Text>();
        infoText = transform.Find("InfoPanel/InfoText").GetComponent<Text>();
        nameText = transform.Find("InfoPanel/NameText").GetComponent<Text>();
        buyText_diam = transform.Find("BuyPanel/Diamond/Text").GetComponent<Text>();
        buyText_gold = transform.Find("BuyPanel/Gold/Text").GetComponent<Text>();
        buyposse_diam = transform.Find("BuyPanel/DiamText").GetComponent<Text>();
        buyposse_gold = transform.Find("BuyPanel/GoldText").GetComponent<Text>();
        buyImage = transform.Find("BuyPanel/DiamBtn").GetComponent<Image>();
        infoSprite = transform.Find("InfoPanel/Sprite").GetComponent<Image>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        bcakBtn = transform.Find("BackBtn").GetComponent<Button>();
        infoBtn = transform.Find("InfoPanel/Button").GetComponent<Button>();
        buyBtn = transform.Find("BuyPanel/Close").GetComponent<Button>();

        info_Armature = transform.Find("InfoPanel/candy").GetComponent<DragonBones.UnityArmatureComponent>();

        fortData = parent.Find("FitA").GetComponent<FortProperty>();
        cakeData = parent.Find("Cake").GetComponent<CakeStar>();
        gunData = parent.Find("Turret").GetComponent<GunProperty>();

        infoPanel = transform.Find("InfoPanel").gameObject;
        buyPanel = transform.Find("BuyPanel").gameObject;
        goldtip_buy = transform.Find("BuyPanel/goldtip").gameObject;
        diamtip_buy = transform.Find("BuyPanel/diamtip").gameObject;
        closeBtn.onClick.AddListener(CloseClick);
        bcakBtn.onClick.AddListener(BackPanel);
        infoBtn.onClick.AddListener(CloseInfoPanel);
        buyBtn.onClick.AddListener(CloseBuyPanel);
        if (GameManager.Instance.modeSelection == "roude")
        {
            selectPanel = transform.Find("Select").gameObject;
            selectTip = transform.Find("Select/SelectTip").gameObject;
            carryText = transform.Find("Select/Text").GetComponent<Text>();
            warBtn = transform.Find("Select/WarBtn").GetComponent<Button>();
            isCarryMax = PlayerPrefs.GetString("Carryturret") == "";
            warBtn.onClick.AddListener(CloseCarry);
        }
        InitData();
    }
    
    void InitData()
    {
        sendGrade = PlayerPrefs.GetInt("SendGrade", 1);
        levelTerm_send = 4 + (sendGrade - 1) * 1;//炮台 关卡
        diamTerm_send = 5 + (sendGrade - 1) * 3;//钻石

        attackGrade = PlayerPrefs.GetInt("AttackGrade");
        levelUp_send = 19 + (attackGrade) * 25;//升级 所需关卡
        diamUp_send = 10 + (attackGrade) * 30;//钻石

        spanGrade = PlayerPrefs.GetInt("SpanGrade", 1);
        levelTerm_span = 1 + (spanGrade - 1) * 2;//合并 所需关卡
        diamTerm_span = 5 + (spanGrade - 1) * 5;//钻石

        conGrade = PlayerPrefs.GetInt("ConGrade", 1);
        levelCon_span =20+ (conGrade - 1) * 30;//升级所需 关卡
        diamCon_span=5+ (conGrade - 1) * 25;//钻石

        cakeGrade = PlayerPrefs.GetInt("CakeGrade",1);
        levelUp_cake = 50 + (cakeGrade - 1) * 50;//蛋糕升星 关卡
        diamUp_cake = 50 + (cakeGrade - 1) * 80;//钻石
        //if(GameManager.Instance.modeSelection!="roude")
        //{
        //    BaseHealth.Instance.BarMultiple(cakeGrade);
        //}
        //else
        //{
        //    CreateModel.Instance.cakeCon.BarMultiple(cakeGrade);
        //}

        fortData.Init();
        gunData.Init();
        cakeData.Init();
        int[] indexSorts = { 1, 6, 7, 2, 8, 9, 3, 10, 11, 4, 12, 13, 5, 14, 15, 16, 17, 18, 19, 20,21 };
        for (int i = 0; i < indexSorts.Length; i++)
        {
            var turret = Instantiate(turretPrefab);
            turret.gameObject.SetActive(true);
            turret.SetParent(parent);
            turret.localScale = Vector3.one;
            turret.localPosition = Vector3.zero;
            turret.localEulerAngles = Vector3.zero;
            turret.GetComponent<TurretProperty>().Init(i);
            attributes.Add(turret.GetComponent<TurretProperty>());
        }
        for (int i = 0; i < indexSorts.Length; i++)
        {
            attributes[indexSorts[i]-1].transform.SetAsLastSibling();
        }
        //cakeData.transform.SetAsLastSibling();
    }

    //合并 解锁
    public void SetSpan()
    {
        spanGrade += 1;
        levelTerm_span = 1 + (spanGrade - 1) * 2;//合并 所需关卡
        diamTerm_span = 5 + (spanGrade - 1) * 5;//钻石
        PlayerPrefs.SetInt("SpanGrade", spanGrade);
    }
    //转化
    public void SetConvart()
    {
        conGrade += 1;
        levelCon_span = 20 + (conGrade - 1) * 30;
        diamCon_span = 5 + (conGrade - 1) * 25;
        PlayerPrefs.SetInt("ConGrade", conGrade);
    }
    //炮台 解锁 升星解锁的关卡需求=4+（炮塔等级-1）*5炮塔钻石=5+（炮塔等级-1）*5
    public void SetSend()
    {
        sendGrade += 1;
        levelTerm_send = 4 + (sendGrade - 1) * 1;//炮台 关卡
        diamTerm_send = 5 + (sendGrade - 1) * 3;//钻石
        PlayerPrefs.SetInt("SendGrade", sendGrade);
    }
    //炮台 增加伤害 
    public void SetAttack()
    {
        attackGrade += 1;
        levelUp_send = 19 + (attackGrade - 1) * 25;
        diamUp_send = 10 + (attackGrade - 1) * 30;
        PlayerPrefs.SetInt("AttackGrade", attackGrade);
    }
    //蛋糕升星
    public void SetCake()
    {
        cakeGrade += 1;
        levelUp_cake = 50 + (cakeGrade - 1) * 50;//蛋糕升星 关卡
        diamUp_cake = 50 + (cakeGrade - 1) * 80;//钻石
        PlayerPrefs.SetInt("CakeGrade", cakeGrade);
        if (GameManager.Instance.modeSelection != "roude")
        {
            BaseHealth.Instance.BarMultiple(cakeGrade);
            BaseHealth.Instance.AddHp();
        }
        else
        {
            CreateModel.Instance.cakeCon.BarMultiple(cakeGrade);
        }
    }
    public void OpenPanel(int index = -10)
    {
        if(index != -10)
        {
            if(index == -5)
                index = indexCount;
            if(index >= 0)
            {
                Vector3 vector = parent.localPosition;
                vector.y = index * 270 + index * 120;
                parent.localPosition = vector;
            }
        }
        gameObject.SetActive(true);
    }
    public void OpenBuyPanel(Action action,float diam,float gold)
    {
        action_buy = action;
        if(diam <= UIManager.Instance.starNumber)
        {
            buyImage.color = Color.white;
            diamtip_buy.SetActive(true);
        }
        else
        {
            buyImage.color = Color.gray;
            diamtip_buy.SetActive(false);
        }
        diam_Consume = diam;
        gold_Consume = gold;
        buyText_diam.text = diam.ToString("F0");
        buyText_gold.text = gold.ToString("F0");
        buyposse_diam.text = string.Format("{0}:{1}",ExcelTool.lang["possesum"], UIManager.Instance.starNumber);
        buyposse_gold.text = string.Format("{0}:{1}", ExcelTool.lang["possesum"], UIManager.Instance.GoldDisplay(UIManager.Instance.goldNumber));
        buyPanel.SetActive(true);
    }

    public void OpenSelected(int carryNum)
    {
        carry_max = carryNum;
        carryKey.Clear();
        closeBtn.gameObject.SetActive(false);
        bcakBtn.gameObject.SetActive(true);
        gameObject.SetActive(true);
        selectPanel.SetActive(true);
        parent.localPosition = Vector2.zero;
        carryText.text = string.Format("{0}:{1}/{2}", ExcelTool.lang["carry"], 0, carry_max);
        for (int i = 0; i < carryOuts.Length; i++)
        {
            if(i < carryNum)
            {
                carryOuts[i].gameObject.SetActive(true);
                carryOuts[i].NormalState();
            }
            else
            {
                carryOuts[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < attributes.Count; i++)
        {
            attributes[i].CarryInit();
        }
        if(CreateModel.Instance.level != 0)
        {
            string messg = PlayerPrefs.GetString("AnnalCarry", "");
            if (messg != "")
            {
                string[] annal = messg.Split('|');
                int index = 0;
                for (int i = annal.Length - 1; i >= 0; i--)
                {
                    index++;
                    if(index > carryNum)
                    {
                        break;
                    }
                    attributes[int.Parse(annal[i])].SelectCarry();
                }
            }
        }
    }

    public bool SelectCarry(int id, string name,string keelname)
    {
        if (carryKey.Count <= carry_max)
        {
            for (int i = 0; i < carry_max; i++)
            {
                if (carryOuts[i].Unselected())
                {
                    carryOuts[i].SelectedState(name, id, keelname);
                    carryKey.Add(id, carryOuts[i]);
                    carryText.text = string.Format("{0}:{1}/{2}", ExcelTool.lang["carry"], carryKey.Count, carry_max);
                    return true;
                }
            }
        }
        return false;
    }

    public void CarryHide()
    {
        bool isHide = carryKey.Count < carry_max;
        for (int i = 0; i < attributes.Count; i++)
        {
            attributes[i].CarryHideTip(isHide);
        }
        selectTip.SetActive(carryKey.Count > 0);
    }
    public void CancelSelection(int id)
    {
        carryKey[id].NormalState();
        carryKey.Remove(id);
        CarryHide();
        //if (isCarryMax)
        //{
        //    warBtn.enabled = false;
        //    warBtn.GetComponent<Image>().color = Color.gray;
        //}
        //else
        //{
        //    selectTip.SetActive(carryKey.Count > 0);
        //}  
    }

    public void UnloadCarry(int id)
    {
        attributes[id].SelectCarry();
    }
    public void OpenInfo(string mess,string name,string armatureName, Sprite sp = null)
    {
        AudioManager.Instance.PlayTouch("other_1");
        infoPanel.SetActive(true);
        nameText.text = name;
        infoText.text = mess;
        if(sp)
        {
            infoSprite.sprite = sp;
            infoSprite.gameObject.SetActive(true);
            info_Armature.gameObject.SetActive(false);
        }
        else
        {
            //info_Armature = unityArmature;
            infoSprite.gameObject.SetActive(false);
            info_Armature.gameObject.SetActive(true);
            info_Armature = UIManager.Instance.SetArmature(info_Armature, infoPanel.transform, armatureName, Vector3.one * 70, Vector3.up*220, true, "rest");
            info_Armature.transform.SetAsLastSibling();
        }
    }
    private void CloseClick()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
        UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
    }
    private void BackPanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        UIManager.Instance.skillPanel.OpenPanel();
    }
    private void CloseInfoPanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        infoPanel.SetActive(false);
    }
    private void CloseBuyPanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        buyPanel.SetActive(false);
    }
    private void CloseCarry()
    {
        if (CreateModel.Instance.productions.Count <= 0)
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip29"]);
        }
        else
        {
            AudioManager.Instance.PlayTouch("close_1");
            closeBtn.gameObject.SetActive(true);
            bcakBtn.gameObject.SetActive(false);
            selectPanel.SetActive(false);
            gameObject.SetActive(false);
            if(carryKey.Count >= 2)
            {
                UIManager.Instance.guidePanel.OpenGuide(19, true);
            }
            for (int i = 0; i < attributes.Count; i++)
            {
                attributes[i].CarryHide();
            }
            for (int i = 0; i < carryOuts.Length; i++)
            {
                carryOuts[i].gameObject.SetActive(false);
            }
            CreateModel.Instance.SetLevel(0);
            Camera.main.GetComponent<CameraView>().InitMove();
            UIManager.Instance.skillPanel.NextCarry();
            UIManager.Instance.DetectionPanel();
            UIManager.Instance.IsUpTureet(CreateModel.Instance.sumLevel);
            if (isCarryMax)
            {
                isCarryMax = false;
                PlayerPrefs.SetString("Carryturret", "true");
            }
        }
    }
    //public void FirstCarry()
    //{
    //    for (int i = 0; i < attributes.Count; i++)
    //    {
    //        attributes[i].CarryHide();
    //    }
    //    attributes[0].FirstH();
    //    CreateModel.Instance.ChangeScene(1);
    //    CreateModel.Instance.SetLevel(0);
    //    CreateModel.Instance.ControlProduc(true, 0);
    //}

    public void TutorialsCarry()
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            attributes[i].CarryHide();
        }
        CreateModel.Instance.ChangeScene(1);
        CreateModel.Instance.SetLevel(0);
        CreateModel.Instance.productions.Clear();
        int[] numbers = UIManager.Instance.GetRandoms(6, 0,attributes.Count);
        for (int i = 0; i < numbers.Length; i++)
        {
            attributes[numbers[i]].FirstH();
            CreateModel.Instance.ControlProduc(false,numbers[i]);
        }
    }

    public void WaysPurchase(bool isGold)
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if (isGold)
        {
            if (gold_Consume > UIManager.Instance.goldNumber)
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
                return;
            }
            if (action_buy != null)
            {
                action_buy.Invoke();
            }
            UIManager.Instance.SetGold(-gold_Consume, true);
        }
        else
        {
            if (diam_Consume > UIManager.Instance.starNumber)
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
                return;
            }
            if (action_buy != null)
            {
                action_buy.Invoke();
            }
            UIManager.Instance.SetStar(-diam_Consume);
        }
        buyPanel.SetActive(false);
    }
    public bool JudeUpGrade(float level,float diam)
    {
        levelText.text = (level).ToString();
        starText.text = diam.ToString("F0");
        isUp = false; indexCount = -5;
        if (cakeData.JudgeUp(level, diam))
        {
            TipTexture(cakeData.spriteImage.sprite, cakeData.transform.GetSiblingIndex());
        }
        for (int i = attributes.Count-1; i >=0; i--)
        {
            if (attributes[i].UpGrade(level, diam))
            {
                if (!isUp)
                {
                    tipImage.gameObject.SetActive(false);
                    indexCount = attributes[i].transform.GetSiblingIndex();
                    string keelName = string.Format("candy_{0}_1", i + 1);
                    model_Armature = UIManager.Instance.SetArmature(model_Armature,tipImage.transform.parent, keelName, Vector3.one * 15, Vector3.up*-3);
                }
                isUp = true;
            }
        }
        if (fortData.JudgeConUp(level,diam))
        {
            TipTexture(fortData.spriteImage.sprite, fortData.transform.GetSiblingIndex());
        }
        if (fortData.JudgeBuyUp(level, diam))
        {
            TipTexture(fortData.spriteImage.sprite, fortData.transform.GetSiblingIndex());
        }
        if (gunData.JudgeUp(level, diam))
        {
            TipTexture(gunData.spriteImage.sprite, gunData.transform.GetSiblingIndex());
        }
        if (gunData.JudgeBuyUp(level, diam))
        {
            TipTexture(gunData.spriteImage.sprite, gunData.transform.GetSiblingIndex());
        }
        return isUp;
    }

    private void TipTexture(Sprite sprite,int index)
    {
        if (!isUp)
        {
            tipImage.gameObject.SetActive(true);
            model_Armature.gameObject.SetActive(false);
            tipImage.sprite = sprite;
            indexCount = index;
        }
        isUp = true;
    }
}
