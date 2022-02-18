using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get => instance; }
    public Sprite[] holdSprite;
    public Sprite[] gemSprite;
    public Color[] quality;

    public Sprite goldHigh;
    public Sprite goldNor;
    public Sprite turrHight;
    public Sprite turrNor;
    public GameObject qrCode;
    public ButtonCool attackCool;
    public ButtonCool earCool;
    public GameObject confirmPanel;
    public Transform gamePanel;
    public TurretPanel turretPanel;
    public LangePanel langePanel;
    public FailurePanel failurePanel;
    public StorePanel storePanel;
    public EverydayPanel everydayPanel;
    public MorePanel morePanel;
    public DoublePanel doublePanel;
    public SettingsPanel settingsPanel;
    public TaskPanel taskPanel;
    public SkillPanel skillPanel;
    public RewardsPanel rewardsPanel;
    public DiamondPanel diamondPanel;
    public GuidePanel guidePanel;
    public HelpPanel helpPanel;
    public HeadPanel headPanel;
    public MapPanel mapPanel;
    public GameLinkPanel gameLinkPanel;
    public ArmsPanel armsPanel;
    public SettlePanel settlePanel;
    public EvaluatePanel evaluatePanel;
    public RouteMapPanel routeMapPanel;
    public PausePanel pausePanel;
    public EnemyInfoPanel enemyInfoPanel;
    public RisePanel risePanel;

    public GameObject leaddestroy;
    private GameObject leadObject;
    private GameObject snowingPrefab;
    private GameObject snowingPrent;
    private GameObject leverPrefab;
    private GameObject leverPrent;
    public GameObject glodPrefab;
    public GameObject bottomImage;
    private GameObject shareTip;
    private GameObject earPanel;
    private GameObject autoPanel;
    private GameObject attackPanel;
    private GameObject sweetPanel;
    private GameObject tipAttk;
    private GameObject tipEar;
    private GameObject tipSweet;
    private GameObject maskPanel;
    private GameObject tipUpgrade;
    private GameObject tipTureet;
    private GameObject taskTip;
    private GameObject autoTip;
    private GameObject skillTip;
    private GameObject bankTip;
    private GameObject produceTip;
    private GameObject timePattern;
    private GameObject lockTime;
    private GameObject lockHook;
    private GameObject lockTower;
    private GameObject timePanel;
    private GameObject routePattern;
    private GameObject unlockSkill;
    private GameObject unlockturret;
    public GameObject statePanel;
    public GameObject freeTip;
    private GameObject guideInfo;
    private GameObject diamAnimal;
    private GameObject guideMask;
    private DragonBones.UnityArmatureComponent guide_Armature;

    private AudioSource source;

    private Button shareBtn;
    private Button yesBtn;
    private Button noBtn;
    private Button mapBtn;
    private Button atkBtn;
    private Button earBtn;
    private Button sweetBtn;
    private Button gunBtn;
    private Button moreBtn;
    private Button autoBtn;
    private Button goldBtn;
    private Button moneyBtn;
    private Button taskBtn;
    private Button skillBtn;
    private Button skillBtn_tip;
    private Button gunBtn_tip;
    private Button produceBtn;

    private Image produceImage;
    private Image titleImage;
    private Image skillCool;
    private Image sweetCool;
    private Image autoCool;
    private Image goldImage;

    private Text adsText;
    private Text diamNumText;
    private Text levelNum;
    private Text goldText;
    private Text goldNeedText;
    private Text levelText;
    private Text atkText;
    private Text earText;
    private Text autoText;
    private Text savingText;
    private Text sweetText;
    private Text produceText;
    private Text current_time;
    private Text recort_time;
    private Text current_level;
    private Text skillsText;
    private Text plusText;
    private Text guideText;
    private Text hpText;

    private int starPanel_index;
    private int endPanel_index;
    private int lead_index;
    private int modeType;
    public float add_suger;
    private float releaseTime;
    private float leadTime;
    private float candyTiming;
    private float produceTime = -5;
    public float skillTime;
    public float sweetTime;
    public float autoTime;
    public float autoSumTime;
    public float timeRecord;
    public float timeCurret;
    private float autoTimewait;
    public float atkTime;
    public float earTime;
    private float atk_Temp;
    private float ear_Temp;
    private float routeGold_max;
    //private float builiteTime;
    private int autoIndex;
    public float atkDouble;
    public int earDouble = 1;
    public float boxGold;
    public float routeGold;
    public float goldNumber;
    public float starNumber;
    public float skillNumber;
    public float maxDiam;
    public float sunDiam;
    public float turretGold;
    public float customs = 1;
    public bool isTime;
    private bool isAdReward;
    private bool isProduce;
    private bool isRoute;
    public bool skillFirst;
    private Vector3 leadPoint1;
    private Vector3 leadPoint2;
    private Vector3 leadPoint3;
    private Vector3 leadPoint4;
    private List<GameObject> leverObject = new List<GameObject>();
    public int[] keelAnim = { 2, 3, 4, 6, 8, 10, 13, 16, 20 };
    public List<int> lockSkill = new List<int>();
    private Camera UICamera;
    private void Awake()
    {
        if (instance)
        {
            Debug.Log("CHISHI");
            Destroy(instance.gameObject);
        }
        instance = this;
        UICamera = Camera.main;
        source = GetComponent<AudioSource>();
        glodPrefab = Resources.Load<GameObject>("Effects/Gold");
        snowingPrefab = transform.Find("Snowing/SnowingText").gameObject;
        snowingPrent = transform.Find("Snowing").gameObject;
        bottomImage = transform.Find("bottomImage").gameObject;
        if (GameManager.Instance.modeSelection == "hook")
        {
            modeType = 1;
        }
        else if(GameManager.Instance.modeSelection == "time")
        {
            modeType = 2;
        }
        else
        {
            isRoute = true;
            modeType = 3;
        } 
        InitData();
        InitPanel();
    }
    void Start()
    {
        langePanel.Init();
        guidePanel.Init("Guide");
        everydayPanel.Init();
        failurePanel.Init(isRoute);
        turretPanel.Init();
        doublePanel.Init();
        settingsPanel.Init();
        taskPanel.Init();
        skillPanel.Init();
        storePanel.Init();
        rewardsPanel.Init();
        diamondPanel.Init();
        helpPanel.Init();
        headPanel.Init();
        mapPanel.Init();
        gameLinkPanel.Init();
        evaluatePanel.Init();
        if (modeType == 1)
        {
            armsPanel.Init(3);
            leverPrent.SetActive(true);
            mapBtn.gameObject.SetActive(true);
            guidePanel.OpenGuide(1, true);
        }
        else if (modeType == 2)
        {
            isTime = true;
            leverPrent.SetActive(false);
            mapBtn.gameObject.SetActive(false);
            timePanel.SetActive(true);
            timePattern.SetActive(true);
            current_time.text = "0";
            recort_time.text = timeRecord.ToString("F1") + "s";
        }
        else if (modeType == 3)
        {
            isTime = true;
            pausePanel.Init();
            enemyInfoPanel.Init(1);
            routeMapPanel.Init();
            routePattern.SetActive(true);
            leverPrent.SetActive(false);
            mapBtn.gameObject.SetActive(false);
            taskBtn.gameObject.SetActive(false);
        }
        guideInfo.SetActive(false);
        starPanel_index = failurePanel.transform.GetSiblingIndex();
        endPanel_index = langePanel.transform.GetSiblingIndex();
        ExcelTool.LanguageEvent += CutLang;
        ExcelTool.Instance.CutLanguage(PlayerPrefs.GetString("PackageLanguage"));
        GameManager.Instance.skipPanel.ClosePanel();
        GameManager.Instance.AdmobAdsCount = null;
        GameManager.Instance.AdmobAdsCount = AdsCount;
        adsText.text = GameManager.Instance.adsCount.ToString();
        storePanel.adsText.text = adsText.text;
        InitializeData();
        produceImage.gameObject.SetActive(false);
        if (isRoute)
        {
            produceTime = 1;
        }
        else
        {
            produceTime = 3;
        }
        if (isProduce)
        {
            produceImage.fillAmount = 1;
            produceText.text = produceTime.ToString();
            produceTip.gameObject.SetActive(false);
            produceImage.gameObject.SetActive(true);
        }
        else
        {
            produceText.text = produceTime.ToString();
            produceTip.gameObject.SetActive(true);
        }
    }
    void AdsCount(int count)
    {
        adsText.text = count.ToString();
        storePanel.adsText.text = adsText.text;
    }
    void InitPanel()
    {
        //各个面板
        guideMask = transform.Find("GuideMask").gameObject;
        maskPanel = transform.Find("MaskPanel").gameObject;
        gamePanel = transform.Find("GamePanel");
        earPanel = gamePanel.Find("right/EarPanel").gameObject;
        autoPanel = gamePanel.Find("right/AutoPanel").gameObject;
        attackPanel = gamePanel.Find("right/AttackPanel").gameObject;
        sweetPanel = gamePanel.Find("right/SweetPanel").gameObject;
        shareTip = gamePanel.Find("right/ShareBtn/Image").gameObject;
        produceTip = gamePanel.Find("ProduceBtn/TipImage").gameObject;
        unlockSkill = transform.Find("UnlockSkill").gameObject;
        unlockturret = transform.Find("UnlockTurret").gameObject;
        diamAnimal = transform.Find("DiamAnimal").gameObject;
        turretPanel = transform.Find("TurretPanel").GetComponent<TurretPanel>();
        langePanel = transform.Find("LangePanel").GetComponent<LangePanel>();
        failurePanel = transform.Find("FailurePanel").GetComponent<FailurePanel>();
        storePanel = transform.Find("StorePanel").GetComponent<StorePanel>();
        everydayPanel = transform.Find("EverydayPanel").GetComponent<EverydayPanel>();
        mapPanel = transform.Find("MapPanel").GetComponent<MapPanel>();
        morePanel = transform.Find("MorePanel").GetComponent<MorePanel>();
        doublePanel = transform.Find("DoublePanel").GetComponent<DoublePanel>();
        settingsPanel = transform.Find("SettingsPanel").GetComponent<SettingsPanel>();
        taskPanel = transform.Find("TaskPanel").GetComponent<TaskPanel>();
        skillPanel = transform.Find("SkillPanel").GetComponent<SkillPanel>();
        rewardsPanel = transform.Find("RewardsPanel").GetComponent<RewardsPanel>();
        diamondPanel = transform.Find("DiamondPanel").GetComponent<DiamondPanel>();
        guidePanel = transform.Find("GuidePanel").GetComponent<GuidePanel>();
        helpPanel = transform.Find("HelpPanel").GetComponent<HelpPanel>();
        headPanel = transform.Find("HeadPanel").GetComponent<HeadPanel>();
        gameLinkPanel = transform.Find("GameLinkPanel").GetComponent<GameLinkPanel>();
        evaluatePanel = transform.Find("EvaluatePanel").GetComponent<EvaluatePanel>();
        risePanel = transform.Find("RisePanel").GetComponent<RisePanel>();
        confirmPanel = transform.Find("ConfirmPanel").gameObject;
        statePanel = transform.Find("ProductionPanel").gameObject;
        freeTip = gamePanel.Find("FreeBtn/TipImage").gameObject;
        guideInfo = gamePanel.Find("Guide").gameObject;
        guide_Armature= gamePanel.Find("Guide/pause").GetComponent<DragonBones.UnityArmatureComponent>();
        lockTime = gamePanel.Find("left/Select/BtnParent/TimeBtn/Lock").gameObject;
        lockHook = gamePanel.Find("left/Select/BtnParent/LevelBtn/Lock").gameObject;
        lockTower = gamePanel.Find("left/Select/BtnParent/HuntBtn/Lock").gameObject;
        if (modeType == 1)
        {
            armsPanel = transform.Find("ArmsPanel").GetComponent<ArmsPanel>();
            gamePanel.Find("left/Select/BtnParent/RoudeBtn").gameObject.SetActive(true);
            gamePanel.Find("left/Select/BtnParent/TimeBtn").gameObject.SetActive(true);
            lockHook.gameObject.SetActive(false);
        }
        else if (modeType == 2)
        {
            lockTime.gameObject.SetActive(false);
            timePattern = gamePanel.Find("left/TimeGame").gameObject;
            timePanel = transform.Find("TimePanel").gameObject;
            settlePanel = transform.Find("SettlePanel").GetComponent<SettlePanel>();
            gamePanel.Find("left/Select/BtnParent/RoudeBtn").gameObject.SetActive(true);
            gamePanel.Find("left/Select/BtnParent/LevelBtn").gameObject.SetActive(true);
            current_time = gamePanel.Find("left/TimeGame/CurrentText").GetComponent<Text>();
            recort_time = gamePanel.Find("left/TimeGame/Record").GetComponent<Text>();
            lockTower.gameObject.SetActive(false);
            lockHook.gameObject.SetActive(false);
            lockTime.gameObject.SetActive(false);
        }
        else if (modeType == 3)
        {
            isRoute = true;
            armsPanel = transform.Find("ArmsPanel").GetComponent<ArmsPanel>();
            routePattern = gamePanel.Find("left/RouteGame").gameObject;
            pausePanel = transform.Find("PausePanel").GetComponent<PausePanel>();
            enemyInfoPanel = transform.Find("EnemyInfoPanel").GetComponent<EnemyInfoPanel>();
            gamePanel.Find("left/Select/BtnParent/LevelBtn").gameObject.SetActive(true);
            gamePanel.Find("left/Select/BtnParent/TimeBtn").gameObject.SetActive(true);
            settlePanel = transform.Find("RouteSettlePanel").GetComponent<SettlePanel>();
            routeMapPanel = transform.Find("RouteMapPanel").GetComponent<RouteMapPanel>();
            current_time = gamePanel.Find("left/RouteGame/Record").GetComponent<Text>();
            recort_time = gamePanel.Find("left/RouteGame/wave").GetComponent<Text>();
            current_level = gamePanel.Find("left/RouteGame/CurrentText").GetComponent<Text>();
            skillsText = transform.Find("SkillsText").GetComponent<Text>();
        }
       
        //提示标识
        leadObject = gamePanel.Find("Lead").gameObject;
        leaddestroy = gamePanel.Find("Leaddestroy").gameObject;
        tipAttk = gamePanel.Find("right/AtkBtn/TipImage").gameObject;
        tipEar = gamePanel.Find("right/EarBtn/TipImage").gameObject;
        tipSweet = gamePanel.Find("right/SweetBtn/TipImage").gameObject;
        skillTip = gamePanel.Find("left/SkillBtn/Image").gameObject;
        autoTip = gamePanel.Find("right/AutoBtn/Image").gameObject;
        bankTip = gamePanel.Find("right/MoneyBoxBtn/Tip").gameObject;
        taskTip = gamePanel.Find("left/TaskBtn/Image").gameObject;
        tipTureet = gamePanel.Find("GoldBtn/TipImage").gameObject;
        tipUpgrade = gamePanel.Find("left/GunBtn/TipImage").gameObject;
        leverPrent = gamePanel.Find("left/LevelPrent").gameObject;
        leverPrefab = gamePanel.Find("LevelSprite").gameObject;

        skillCool = transform.Find("Skill/SkillCool").GetComponent<Image>();
        sweetCool = gamePanel.Find("right/SweetBtn/Image").GetComponent<Image>();
        autoCool = gamePanel.Find("right/AutoBtn/AutoShade").GetComponent<Image>();
        goldImage = gamePanel.Find("left/GoldImage").GetComponent<Image>();
        produceImage = gamePanel.Find("ProduceBtn/Image").GetComponent<Image>();
        titleImage = transform.Find("TitleImage").GetComponent<Image>();

        guideText = gamePanel.Find("Guide/GuideText").GetComponent<Text>();
        adsText = gamePanel.Find("left/AdsNum/Text").GetComponent<Text>();
        diamNumText = gamePanel.Find("left/DiamNum/Text").GetComponent<Text>();
        levelNum = gamePanel.Find("left/LevelNum/Text").GetComponent<Text>();
        goldText = gamePanel.Find("left/GoldImage/Text").GetComponent<Text>();
        goldNeedText = gamePanel.Find("GoldBtn/GoldText").GetComponent<Text>();
        levelText = gamePanel.Find("GoldBtn/LevelText").GetComponent<Text>();
        atkText = gamePanel.Find("right/AtkBtn/Text").GetComponent<Text>();
        earText = gamePanel.Find("right/EarBtn/Text").GetComponent<Text>();
        autoText = gamePanel.Find("right/AutoBtn/Text").GetComponent<Text>();
        savingText = gamePanel.Find("right/MoneyBoxBtn/GoldText").GetComponent<Text>();
        sweetText = gamePanel.Find("right/SweetBtn/Text").GetComponent<Text>();
        produceText = gamePanel.Find("ProduceBtn/TimeText").GetComponent<Text>();
        plusText = gamePanel.Find("right/AtkBtn/Plus").GetComponent<Text>();
        hpText = transform.Find("HPImage/Text").GetComponent<Text>();
        hpText.transform.parent.gameObject.SetActive(true);

        shareBtn = gamePanel.Find("right/ShareBtn").GetComponent<Button>();
        mapBtn = gamePanel.Find("left/MapBtn").GetComponent<Button>();
        atkBtn = gamePanel.Find("right/AtkBtn").GetComponent<Button>();
        earBtn = gamePanel.Find("right/EarBtn").GetComponent<Button>();
        sweetBtn = gamePanel.Find("right/SweetBtn").GetComponent<Button>();
        gunBtn = gamePanel.Find("left/GunBtn").GetComponent<Button>();
        moreBtn = gamePanel.Find("right/MoreBtn").GetComponent<Button>();
        autoBtn = gamePanel.Find("right/AutoBtn").GetComponent<Button>();
        goldBtn = gamePanel.Find("GoldBtn").GetComponent<Button>();
        taskBtn = gamePanel.Find("left/TaskBtn").GetComponent<Button>();
        moneyBtn = gamePanel.Find("right/MoneyBoxBtn").GetComponent<Button>();
        skillBtn = gamePanel.Find("left/SkillBtn").GetComponent<Button>();
        skillBtn_tip = gamePanel.Find("left/SkillBtn/TipBtn").GetComponent<Button>();
        gunBtn_tip = gamePanel.Find("left/GunBtn/TipBtn").GetComponent<Button>();
        produceBtn = gamePanel.Find("ProduceBtn").GetComponent<Button>();
        yesBtn = transform.Find("ConfirmPanel/YesBtn").GetComponent<Button>();
        noBtn = transform.Find("ConfirmPanel/NoBtn").GetComponent<Button>();
        //绑定事件
        shareBtn.onClick.AddListener(ShareEvent);
        goldBtn.onClick.AddListener(CreateTurrent);
        gunBtn.onClick.AddListener(TurretClick);
        moreBtn.onClick.AddListener(ClickMore);
        atkBtn.onClick.AddListener(AttackDouble);
        earBtn.onClick.AddListener(EarningsDouble);
        autoBtn.onClick.AddListener(ClickAuto);
        mapBtn.onClick.AddListener(ClickMap);
        sweetBtn.onClick.AddListener(ClickAdsSweet);
        moneyBtn.onClick.AddListener(ClickMonery);
        taskBtn.onClick.AddListener(ClickTask);
        skillBtn.onClick.AddListener(ClickSkill);
        produceBtn.onClick.AddListener(ProduceClick);
        skillBtn_tip.onClick.AddListener(OpenSkill);
        gunBtn_tip.onClick.AddListener(OpenTurret);
    }
    void InitData()
    {
        routeGold = 200;
        goldNumber = PlayerPrefs.GetFloat("GoldNumber", 200);
        starNumber = PlayerPrefs.GetFloat("DiamondNumber");
        maxDiam = PlayerPrefs.GetFloat("maxDiamond");
        sunDiam = PlayerPrefs.GetFloat("sumDiamond");
        boxGold = PlayerPrefs.GetFloat("piggyBank");
        autoTime = PlayerPrefs.GetFloat("AutoTime");
        atkTime = PlayerPrefs.GetFloat("AttackTime");
        earTime = PlayerPrefs.GetFloat("EarningsTime");
        sweetTime = PlayerPrefs.GetFloat("SweetTime");
        autoIndex = PlayerPrefs.GetInt("autofrequency");
        timeRecord = PlayerPrefs.GetFloat("PatternTime");
        isProduce = PlayerPrefs.GetString("isProduce") == "True";
        skillFirst = PlayerPrefs.GetString("SkillFirst") == "True";
    }
    void InitializeData()
    {
        if (PlayerPrefs.GetString("mergeDelay") == "")
        {
            autoSumTime = 600;
        }
        else
        {
            autoSumTime = 3600;
        }
        if(PlayerPrefs.GetString("Automerge") == "")
        {
            PlayerPrefs.SetString("Automerge", "auto");
            autoTime = autoSumTime;
        }
        if (autoTime > 0)
        {
            autoTip.SetActive(false);
            autoBtn.enabled = false;
            autoCool.gameObject.SetActive(true);
        }
        else
        {
            autoText.text = ExcelTool.lang["auto"];
            autoTip.SetActive(autoIndex >= 6);
        }
        SetAttack();
        SetIncome();
        if (sweetTime > 0)
        {
            tipSweet.SetActive(false);
            failurePanel.RewardTiming(false);
            sweetBtn.enabled = false;
            sweetCool.gameObject.SetActive(true);
        }
        else
        {
            tipSweet.SetActive(true);
            sweetText.text= ExcelTool.lang["candy"];
        }
        SetGold(0);
        if (modeType != 3)
        {
            SetStar(0);
            HideTask();
            if (modeType == 1)
                InitLevel(CreateModel.Instance.level);
        }
    }
    public void SaveData()
    {
        PlayerPrefs.SetFloat("AutoTime", autoTime);
        PlayerPrefs.SetFloat("AttackTime", atkTime);
        PlayerPrefs.SetFloat("EarningsTime", earTime);
        PlayerPrefs.SetFloat("SweetTime", sweetTime);
    }
    private void CutLang()
    {
        if (sweetTime <= 0)
            sweetText.text = ExcelTool.lang["candy"];
        if (autoTime <= 0)
            autoText.text = ExcelTool.lang["auto"];
        if (PlayerPrefs.GetString("attckDou") == "")
        {
            atkText.text = ExcelTool.lang["attack"] + "+5";
        }
        else
        {
            atkText.text = ExcelTool.lang["attack"] + "+10";
        }
        if (PlayerPrefs.GetString("incomeDou") == "")
        {
            earText.text = ExcelTool.lang["income"] + "X2";
        }
        else
        {
            earText.text = ExcelTool.lang["income"] + "X3";
        }
    }
    private void Update()
    {
        if (everydayPanel.adsTime > 0)
        {
            everydayPanel.Cooling(Time.deltaTime);
        }
        if (everydayPanel.vipTime > 0)
        {
            everydayPanel.VipCandy(Time.deltaTime);
        }
        if (isTime) return;
        if(modeType == 2 || modeType == 3)
        {
            timeCurret += Time.deltaTime;
            current_time.text = timeCurret.ToString("F1")+ "s";
        }
        if(isRoute)
        {
            candyTiming += Time.deltaTime;
            if(candyTiming >= 1)
            {
                candyTiming = 0;
                SetGold(add_suger);
            }
            if(skillTime <= 0)
            {
                releaseTime += Time.deltaTime;
                if (releaseTime >= 30)
                {
                    releaseTime = -5;
                    skillPanel.TipAnimal(lockSkill);
                }
            }
        }
        if (atkTime >= 0)
        {
            atk_Temp += Time.deltaTime;
            if(atk_Temp >= 1)
            {
                atkTime -= 1;
                atk_Temp = 0;
                if (atkTime <= 0)
                {
                    if (PlayerPrefs.GetString("attckDou") == "")
                    {
                        atkText.text = ExcelTool.lang["attack"] + "+5";
                    }
                    else
                    {
                        atkText.text = ExcelTool.lang["attack"] + "+10";
                    }
                    atkDouble = 0;
                    plusText.text = "";
                    tipAttk.SetActive(true);
                }
                else
                {
                    TimeFormat(atkTime, atkText);
                }
            }
        }
        if (autoTime > 0)
        {
            autoTimewait += Time.deltaTime;
            if (autoTimewait >= 0.6f)
            {
                autoTime -= autoTimewait;
                autoTimewait = 0;
                if (autoTime <= 0)
                {
                    autoBtn.enabled = true;
                    autoCool.gameObject.SetActive(false);
                    if (autoIndex >= 6)
                    {
                        autoTip.SetActive(true);
                    }
                    autoText.text = ExcelTool.lang["auto"];
                    guidePanel.OpenGuide(5, true);
                }
                else
                {
                    TimeFormat(autoTime, autoText);
                    autoCool.fillAmount = autoTime / autoSumTime;
                    CreateModel.Instance.MergeTurret();
                }
            }
        }
        if(skillTime > 0)
        {
            skillTime -= Time.deltaTime;
            skillCool.fillAmount = skillTime * 0.2f;
            if (skillTime <= 0)
            {
                skillCool.fillAmount = 0;
                skillPanel.PublicCooling(false);
            }
        }
        if (sweetTime > 0)
        {
            sweetTime -= Time.deltaTime;
            if (sweetTime <= 0)
            {
                sweetText.text = ExcelTool.lang["candy"];
                sweetCool.gameObject.SetActive(false);
                sweetCool.fillAmount = 1;
                failurePanel.RewardTiming(true);
                sweetBtn.enabled = true;
                tipSweet.SetActive(true);
            }
            else
            {
                TimeFormat(sweetTime, sweetText);
                sweetCool.fillAmount = sweetTime / 900;
            }
        }
        if (earTime > 0)
        {
            ear_Temp += Time.deltaTime;
            if(ear_Temp >= 1)
            {
                earTime -= 1;
                ear_Temp = 0;
                if (earTime <= 0)
                {
                    earDouble = 1;
                    tipEar.SetActive(true);
                    if (PlayerPrefs.GetString("incomeDou") == "")
                    {
                        earText.text = string.Format("{0}X2", ExcelTool.lang["income"]);
                    }
                    else
                    {
                        earText.text = string.Format("{0}X3", ExcelTool.lang["income"]);
                    }
                }
                else
                {
                    TimeFormat(earTime, earText);
                }
            }
        }
        if(isProduce)
        {
            produceTime -= Time.deltaTime;
            produceImage.fillAmount = produceTime / 3;
            produceText.text = produceTime.ToString("F0");
            if (produceTime <= 0)
            {
                CreateTurrent();
            }
        }

    }
    public void PlaySound(string sound)
    {
        AudioManager.Instance.PlayTouch(sound);
    }
    public void OpenPattern()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if(modeType == 1)
        {
            if (CreateModel.Instance.maxLevel >= 1)
            {
                lockTower.SetActive(false);
            }
            else
            {
                lockTower.SetActive(true);
            }
            if (CreateModel.Instance.maxLevel >= 2)
            {
                lockTime.SetActive(false);
            }
            else
            {
                lockTime.SetActive(true);
            }
        }
        else if (modeType == 3)
        {
            if (CreateModel.Instance.maxLevel >= 2)
            {
                lockHook.SetActive(false);
            }
            else
            {
                lockHook.SetActive(true);
            }

            if (PlayerPrefs.GetFloat("MaxLevel") >= 2)
            {
                lockTime.SetActive(false);
            }
            else
            {
                lockTime.SetActive(true);
            }

            if (PlayerPrefs.GetFloat("MaxLevel") >= 1)
            {
                lockTower.SetActive(false);
            }
            else
            {
                lockTower.SetActive(true);
            }
        }

    }
    public void CurretHp(float hp)
    {
        if(hp <= 0)
        {
            hp = 0;
        }
        hpText.text = hp.ToString("F0");
    }
    public void CloseState()
    {
        isTime = false;
        statePanel.SetActive(false);
        AudioManager.Instance.PlayTouch("close_1");
    }
    public void CloseTime()
    {
        if(timeRecord > 0)
        {
            transform.Find("SelectRecort").gameObject.SetActive(true);
        }
        else
        {
            isTime = false;
        }
        timePanel.SetActive(false);
        AudioManager.Instance.PlayTouch("close_1");
    }
    //打开暂停界面
    public void OpenPause()
    {
        isTime = true;
        pausePanel.OpenPause();
    }
    //设置波次
    public void SetWave(int wavemax,int wave)
    {
        recort_time.text = string.Format("{0}/{1}",wave,wavemax);
    }
    //设置关卡
    public void SetRoudeLevel(float level,string gold,float addgold)
    {
        routeGold = 0;
        timeCurret = 0;
        skillNumber = 0;
        add_suger = addgold;
        current_time.text = "0";
        current_level.text = level.ToString();
        skillsText.text = string.Format("{0}:{1}",ExcelTool.lang["use"],skillNumber);
        routeGold_max = float.Parse(gold);
        routeGold = routeGold_max;
        Sequence mScoreSequence = DOTween.Sequence();
        mScoreSequence.SetAutoKill(false);
        mScoreSequence.Append(DOTween.To(delegate (float value) {
            var temp = Math.Floor(value);
            goldText.text = temp + "";
        }, 0, routeGold_max, 3f));
        //SetGold(routeGold_max,false);
    }
    //路线金币获取
    public void GetRoudeGold(bool isvictory)
    {
        if(!isvictory)
        {
            routeGold = routeGold - routeGold_max;
        }
        if (routeGold < 0)
            routeGold = 0;
        SetGold(routeGold, true);
    }
    //金币需求
    public void SetGoldNeed(int grade)
    {
        turretGold = 3 + 3 * (grade - 1);
        levelText.text = string.Format("Lv.{0}", grade);
        goldNeedText.text = turretGold.ToString();
    }
    //设置钻石
    public void SetStar(float star)
    {
        starNumber += star;
        diamNumText.text = starNumber.ToString("F0");
        everydayPanel.diamondText.text = diamNumText.text;
        taskPanel.diamText.text = diamNumText.text;
        storePanel.diamText.text = diamNumText.text;
        diamondPanel.SetDiamond(diamNumText.text);
        IsUpTureet(CreateModel.Instance.sumLevel);
        PlayerPrefs.SetFloat("DiamondNumber", starNumber);
        if (star >= 0)
        {
            maxDiam += star;
            sunDiam += star;
            PlayerPrefs.SetFloat("sumDiamond", sunDiam);
            PlayerPrefs.SetFloat("maxDiamond", maxDiam);
        }
    }
    //设置金币
    public void SetGold(float coin,bool isDiam=false,bool isnum=true)
    {
        string goldMessg = "";
        if (isRoute && !isDiam)
        {
            routeGold += coin;
            goldText.text = GoldDisplay(routeGold);
        }
        else
        {
            goldNumber += coin;
            PlayerPrefs.SetFloat("GoldNumber", goldNumber);
        }
        goldMessg = GoldDisplay(goldNumber);
        storePanel.goldText.text = goldMessg;
        taskPanel.goldText.text = goldMessg;
        everydayPanel.goldText.text = goldMessg;
        turretPanel.goldText.text = goldMessg;
        skillPanel.goldText.text = goldMessg;
        if (isRoute)
        {
            if (skillTime <= 0)
                skillPanel.JudeFreed(routeGold);
        }
        else
        {
            if (skillTime <= 0)
                skillPanel.JudeFreed(goldNumber);
            goldText.text = goldMessg;
        }
        if (coin >= 0 && isnum)
        {
            boxGold += coin;
            savingText.text = GoldDisplay(boxGold);
            PlayerPrefs.SetFloat("piggyBank", boxGold);
        }
        IsCreateTureet();
    }
    public void SwitchScene(string scene)
    {
        if(scene == "Build" && PlayerPrefs.GetString("Guide18") == "")
        {
            guidePanel.OpenGuide(18, false);
        }
        else
        {
            if (scene == "level")
            {
                if (PlayerPrefs.GetFloat("RouteMaxLevel") < 2)
                {
                    GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
                    return;
                }
                GameManager.Instance.SwitchScene("main", "hook");
            }
            else if (scene == "time")
            {
                if (PlayerPrefs.GetFloat("MaxLevel") < 2)
                {
                    GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
                    return;
                }
                GameManager.Instance.SwitchScene("main", "time");
            }
            else if (scene == "roude")
            {
                GameManager.Instance.SwitchScene("main", "roude");
            }
            else if (scene == "Build")
            {
                if (PlayerPrefs.GetFloat("MaxLevel") < 1)
                {
                    GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
                    return;
                }
                GameManager.Instance.SwitchScene(scene, "");
            }
            else
            {
                GameManager.Instance.SwitchScene(scene, "");
            }
            if(modeType == 2)
            {
                if (timeCurret >= 30)
                {
                    int number = (int)(timeCurret / 30);
                    SetStar(number);
                    GameManager.Instance.ClonePrompt(number, 1);
                }
                if (timeCurret > timeRecord)
                {
                    PlayerPrefs.SetFloat("PatternTime", timeCurret);
                }
            }
            else if (modeType == 3)
            {
                GetRoudeGold(false);
            }
            isTime = true;
            CreateModel.Instance.SaveData();
            Application.quitting -= CreateModel.Instance.SaveData;
            //ObjectPool.Instance.ClearAll();
            //ExcelTool.Instance.RemoveEvent();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
    //分享事件
    private void ShareEvent()
    {
        AudioManager.Instance.PlayTouch("other_1");
        titleImage.sprite = ExcelTool.Instance.tempSprite;
        titleImage.SetNativeSize();
        titleImage.gameObject.SetActive(true);
        qrCode.SetActive(true);
        shareTip.SetActive(false);
        GJCNativeShare.Instance.ShareWithNative(titleImage.gameObject, qrCode);
        if (PlayerPrefs.GetInt("ShareEvent") < 20)
        {
            GameManager.Instance.ShareEvent();
        }
    }
    //生成炮塔
    private void CreateTurrent()
    {
        AudioManager.Instance.PlayTouch("Production_1");
        if (isProduce)
        {
            if (isRoute)
            {
                produceTime = 1;
            }
            else
            {
                produceTime = 3;
            }
            produceImage.fillAmount = 1;
        }
        float gold = 0;
        if(isRoute)
            gold = routeGold;
        else
            gold = goldNumber;
        if (gold >= turretGold)
        {
            if (CreateModel.Instance.CloneTurret())
            {
                SetGold(-turretGold);
                gold -= turretGold;
                if (gold < turretGold)
                {
                    if (PlayerPrefs.GetString("Guide" + 9) == "")
                    {
                        guidePanel.OpenGuide(9, false);
                    }
                    else if (PlayerPrefs.GetString("Guide" + 11) == "")
                    {
                        guidePanel.OpenGuide(11, false);
                    }
                }
            }
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip11"]);
        }
    }
    //是否可以生成炮塔
    public void IsCreateTureet()
    {
        float gold = 0;
        if (isRoute)
            gold = routeGold;
        else
            gold = goldNumber;
        if (gold >= turretGold)
        {
            bankTip.SetActive(false);
        }
        else
        {
            bankTip.SetActive(true);
        }
        if (CreateModel.Instance.HintTurret() && gold >= turretGold)
        {
            goldBtn.enabled = true;
            goldBtn.GetComponent<Image>().sprite = turrHight;
            tipTureet.SetActive(true);
        }
        else
        {
            if (autoTime <= 0)
            {
                guidePanel.OpenGuide(3, false);
            }
            goldBtn.enabled = false;
            goldBtn.GetComponent<Image>().sprite = turrNor;
            tipTureet.SetActive(false);
        }
    }
    //炮塔是否可以升级
    public void IsUpTureet(float level)
    {
        bool isTip_T = turretPanel.JudeUpGrade(level, starNumber);
        tipUpgrade.SetActive(isTip_T);
        gunBtn_tip.gameObject.SetActive(isTip_T);
        bool isTip_S = skillPanel.JudeUpGrade(level, starNumber);
        skillTip.SetActive(isTip_S);
        skillBtn_tip.gameObject.SetActive(isTip_S);
        levelNum.text = level.ToString();
    }
    //任务面板
    public void HideTask(bool ishide = true)
    {
        if (ishide)
        {
            taskTip.SetActive(taskPanel.Judge());
        }
        else
        {
            taskTip.SetActive(ishide);
        }
    }
    public void OpenMission()
    {
        if(taskPanel.Judge())
        {
            taskPanel.SetPanel(false, false);
        }
    }
    public void IsTask(bool isboos)
    {
        shareTip.SetActive(true);
        bool israw = taskPanel.Judge();
        if (isboos)
        {
            taskTip.SetActive(israw);
            if (!israw)
            {
                if (CreateModel.Instance.SceneChange())
                {
                    isTime = true;
                    mapPanel.OpenPanel(true);
                    return;
                }
                CreateModel.Instance.LevelJudge(true);
            }
            else
            {
                isTime = true;
                taskPanel.SetPanel(true,true);
            }
        }
        else
        {
            CreateModel.Instance.LevelJudge(true);
            taskTip.SetActive(israw);
        }
    }
    //关卡教程
    public void Tutorials(bool isHide)
    {
        gunBtn.gameObject.SetActive(isHide);
        skillBtn.gameObject.SetActive(isHide);
    }
    //技能冷却
    public void SetSkill()
    {
        skillNumber += 1;
        skillCool.fillAmount = 1;
        if(isRoute)
        {
            skillTime = 2.5f;
            releaseTime = 0;
            skillsText.text = string.Format("{0}:{1}", ExcelTool.lang["use"], skillNumber);
        }
        else
        {
            skillTime = 5;
        }
        skillPanel.PublicCooling(true);
        if(!skillFirst)
        {
            skillFirst = true;
            CreateModel.Instance.sendTroops = false;
            PlayerPrefs.SetString("SkillFirst", "True");
            if (guideInfo.activeInHierarchy)
            {
                GuideInfo("");
            }
        }
    }
    //面板遮罩
    public void MaskPanel(bool isHide)
    {
        maskPanel.SetActive(isHide);
    }
    //炮塔
    private void OpenTurret()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isTime = true;
        turretPanel.OpenPanel(-5);
    }
    private void TurretClick()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isTime = true;
        turretPanel.OpenPanel();
    }
    //更多
    private void ClickMore()
    {
        AudioManager.Instance.PlayTouch("other_1");
        morePanel.SetCartoon();
    }
    //地图关卡
    public void ClickMap()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isTime = true;
        mapPanel.OpenPanel(false);
    }
    //任务面板
    void ClickTask()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isTime = true;
        taskPanel.Judge();
        taskPanel.SetPanel(false,false);
    }
    //技能面板
    private void OpenSkill()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isTime = true;
        skillPanel.OpenPanel(-5);
    }
    void ClickSkill()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isTime = true;
        skillPanel.OpenPanel();
    }
    //攻击双倍
    void AttackDouble()
    {
        AudioManager.Instance.PlayTouch("other_1");
        attackPanel.SetActive(true);
    }
    //自动生成
    void ProduceClick()
    {
        if (isRoute)
        {
            produceTime = 1;
            produceText.text = "1";
        }
        else
        {
            produceTime = 3;
            produceText.text = "3";
        }
        if (isProduce)
        {
            isProduce = false;
            produceTime = -5;
            produceImage.gameObject.SetActive(false);
            produceTip.gameObject.SetActive(true);
            PlayerPrefs.SetString("isProduce","");
        }
        else
        {
            isProduce = true;
            produceImage.fillAmount = 1;
            produceImage.gameObject.SetActive(true);
            produceTip.gameObject.SetActive(false);
            PlayerPrefs.SetString("isProduce", "True");
        }
    }
    //引导语言
    public void GuideInfo(string messg)
    {
        guideText.gameObject.SetActive(false);
        if (messg == "")
        {
            guideMask.SetActive(false);
            guide_Armature.animation.Play("pause_3",1);
            guide_Armature.AddEventListener(DragonBones.EventObject.COMPLETE, CloseGuideInfo);
        }
        else
        {
            guideMask.SetActive(true);
            guideInfo.SetActive(true);
            guideText.text = messg;
            guide_Armature.animation.Play("pause_1",1);
            guide_Armature.AddEventListener(DragonBones.EventObject.COMPLETE, OpenGuideInfo);
        }
    }
    private void CloseGuideInfo(string type, DragonBones.EventObject eventObject)
    {
        guideInfo.SetActive(false);
        guide_Armature.RemoveEventListener("complete", CloseGuideInfo);
    }
    private void OpenGuideInfo(string type, DragonBones.EventObject eventObject)
    {
        guideInfo.SetActive(true);
        guide_Armature.animation.Play("pause_2");
        guideText.gameObject.SetActive(true);
        guide_Armature.RemoveEventListener("complete", OpenGuideInfo);
    }
    public void OpenGuideMask(bool isOpen)
    {
        guideMask.SetActive(isOpen);
    }
    public void FreeDiamond(int index)
    {
        Vector3 endPoint = Vector3.zero;
        switch (index)
        {
            case 1:
                endPoint = diamNumText.transform.parent.position;
                break;
            case 2:
                endPoint = turretPanel.starText.transform.parent.position;
                break;
            case 3:
                endPoint = skillPanel.diamText.transform.parent.position;
                break;
            case 4:
                endPoint = diamondPanel.diamText.transform.parent.position;
                break;
            default:
                break;
        }
        bool isReward = false;
        freeTip.SetActive(false);
        if (Application.platform == RuntimePlatform.Android)
        {
            GameManager.Instance.UserChoseToWatchAd(AdsType.reset);
            GameManager.Instance.AdmobRewardCB = delegate
            {
                isTime = true;
            };
            GameManager.Instance.AdmobRewardClose = delegate
            {
                isReward = true;
            };
            GameManager.Instance.AdmobClose = delegate
            {
                if (isReward)
                {
                    SetStar(1);
                    OpenDiam(endPoint);
                    GameManager.Instance.ClonePrompt(1, 1);
                }
                DetectionPanel();
                GameManager.Instance.RequestRewardBasedVideo(AdsType.reset);
            };
        }
        else
        {
            SetStar(1);
            OpenDiam(endPoint);
            GameManager.Instance.ClonePrompt(1, 1);
        }
    }

    public void SetAttack()
    {
        if (atkTime > 0)
        {
            tipAttk.SetActive(false);
            if (PlayerPrefs.GetString("attckDou") == "")
            {
                atkDouble = 5f;
                plusText.text = "+5";
            }
            else
            {
                atkDouble = 10f;
                plusText.text = "+10";
            }
        }
        else
        {
            tipAttk.SetActive(true);
            atkDouble = 0;
            plusText.text = "";
            if (PlayerPrefs.GetString("attckDou") == "")
            {
                atkText.text = string.Format("{0}+5", ExcelTool.lang["attack"]);
            }
            else
            {
                atkText.text = string.Format("{0}+10", ExcelTool.lang["attack"]);
            }
        }
    }
    public void VideoAttak()
    {
        if (((int)atkTime / 3600) >= 4)
        {
            isTime = true;
            doublePanel.SetPanel(AdsType.attack, 0,atkTime);
            doublePanel.gameObject.SetActive(true);
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                GameManager.Instance.UserChoseToWatchAd(AdsType.attack);
                GameManager.Instance.AdmobRewardCB = delegate
                {
                    isTime = true;
                };
                GameManager.Instance.AdmobRewardClose = delegate
                {
                    isAdReward = true;
                    AttakReward(1);
                };
                GameManager.Instance.AdmobClose = delegate
                {
                    attackCool.OnClick();
                    InforAds(AdsType.attack,ExcelTool.lang["atkgrow"]);
                };
            }
            else
            {
                attackCool.OnClick();
                AttakReward(1);
            }
        }
    }
    public void AttakReward(int index)
    {
        if (PlayerPrefs.GetString("attckDou") == "")
        {
            atkDouble = 5f;
            plusText.text = "5";
        }
        else
        {
            atkDouble = 10f;
            plusText.text = "10";
        }
        isTime = true;
        if (4 >= atkTime / 3600)
            atkTime += 3600;
        doublePanel.SetPanel(AdsType.attack,index,atkTime);
        doublePanel.gameObject.SetActive(true);
        tipAttk.SetActive(false);
        TimeFormat(atkTime, atkText);
        if (!isAdReward)
            GameManager.Instance.CloneTip(ExcelTool.lang["atkgrow"]);
    }
    //收益双倍
    void EarningsDouble()
    {
        AudioManager.Instance.PlayTouch("other_1");
        earPanel.SetActive(true);
    }
    public void SetIncome()
    {
        if (earTime > 0)
        {
            tipEar.SetActive(false);
            if (PlayerPrefs.GetString("incomeDou") == "")
            {
                earDouble = 2;
            }
            else
            {
                earDouble = 3;
            }
        }
        else
        {
            tipEar.SetActive(true);
            earDouble = 1;
            if (PlayerPrefs.GetString("incomeDou") == "")
            {
                earText.text = string.Format("{0}X2", ExcelTool.lang["income"]);
            }
            else
            {
                earText.text = string.Format("{0}X3", ExcelTool.lang["income"]);
            }
        }
    }
    public void VideoEarnings()
    {
        if (((int)earTime / 3600) >= 4)
        {
            isTime = true;
            doublePanel.SetPanel(AdsType.earnings,0,earTime);
            doublePanel.gameObject.SetActive(true);
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                GameManager.Instance.UserChoseToWatchAd(AdsType.earnings);
                GameManager.Instance.AdmobRewardCB = delegate
                {
                    isTime = true;
                };
                GameManager.Instance.AdmobRewardClose = delegate
                {
                    isAdReward = true;
                    EarningsReward(1);
                };
                GameManager.Instance.AdmobClose = delegate
                {
                    earCool.OnClick();
                    InforAds(AdsType.earnings, ExcelTool.lang["eardouble"]);
                };
            }
            else
            {
                earCool.OnClick();
                EarningsReward(1);
            }
        }
    }
    public void EarningsReward(int index)
    {
        if (PlayerPrefs.GetString("incomeDou") == "")
        {
            earDouble = 2;
        }
        else
        {
            earDouble = 3;
        }
        if (4 >= earTime / 3600)
            earTime += 3600;
        isTime = true;
        tipEar.SetActive(false);
        doublePanel.SetPanel(AdsType.earnings,index,earTime);
        doublePanel.gameObject.SetActive(true);
        TimeFormat(earTime, earText);
        if(!isAdReward)
            GameManager.Instance.CloneTip(ExcelTool.lang["eardouble"]);
    }
    //自动合并
    public void ClickAuto()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (autoIndex >= 6)
        {
            autoIndex = -1;
            FitReward(0);
        }
        else
        {
            autoPanel.SetActive(true);
        }
    }
    public void VideoAuto()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GameManager.Instance.UserChoseToWatchAd(AdsType.auto);
            GameManager.Instance.AdmobRewardCB = delegate
            {
                isTime = true;
            };
            GameManager.Instance.AdmobRewardClose = delegate
            {
                isAdReward = true;
                FitReward(1);
            };
            GameManager.Instance.AdmobClose = delegate
            {
                InforAds(AdsType.auto, ExcelTool.lang["tip14"]);
            };
        }
        else
        {
            FitReward(1);
        }
    }

    public void FitReward(int index)
    {
        if (PlayerPrefs.GetString("mergeDelay") == "")
        {
            doublePanel.SetPanel(AdsType.auto, index, autoTime);
            autoTime = 600;
            autoSumTime = 600;
        }
        else
        {
            if (index == 1)
            {
                doublePanel.SetPanel(AdsType.auto, 5, autoTime);
            }
            else
            {
                doublePanel.SetPanel(AdsType.auto, 0, autoTime);
            }
            autoTime = 3600;
            autoSumTime = 3600;
        }
        autoBtn.enabled = false;
        autoTip.SetActive(false);
        isTime = true;
        TimeFormat(autoTime, autoText);
        autoCool.gameObject.SetActive(true);
        doublePanel.gameObject.SetActive(true);
        autoIndex += 1;
        PlayerPrefs.SetInt("autofrequency", autoIndex);
        if (!isAdReward)
            GameManager.Instance.CloneTip(ExcelTool.lang["tip14"]);
    }
    public void AutoTip()
    {
        if (autoTime <= 0 && autoIndex >= 6)
        {
            autoTip.SetActive(true);
        }
    }
    //奖励糖果
    void ClickAdsSweet()
    {
        sweetPanel.SetActive(true);
        AudioManager.Instance.PlayTouch("other_1");
    }
    public void VideoSweet()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GameManager.Instance.UserChoseToWatchAd(AdsType.sweets);
            GameManager.Instance.AdmobRewardCB = delegate
            {
                isTime = true;
            };
            GameManager.Instance.AdmobRewardClose = delegate
            {
                isAdReward = true;
               
            };
            GameManager.Instance.AdmobClose = delegate
            {
                if(isAdReward)
                {
                    CandyReward(1);
                    float candynum = 100 + turretGold * 20;
                    if (isRoute)
                    {
                        candynum = Mathf.Clamp(candynum, 0, 500);
                    }
                    InforAds(AdsType.sweets, "", candynum);
                }
            };
        }
        else
        {
            CandyReward(1);
            DetectionPanel();
        }
    }
    public void CandyReward(int index)
    {
        isTime = true;
        float candynum = 100 + turretGold * 20;
        if (isRoute)
        {
            candynum = Mathf.Clamp(candynum, 0, 500);
        }
        SetGold(candynum);
        sweetTime = 900;
        doublePanel.SetPanel(AdsType.sweets, index, sweetTime);
        doublePanel.gameObject.SetActive(true);
        sweetCool.gameObject.SetActive(true);
        sweetBtn.enabled = false;
        tipSweet.SetActive(false);
        failurePanel.RewardTiming(false);
        if(!isAdReward)
            GameManager.Instance.ClonePrompt((100 + turretGold * 10),0);
    }
    //看广告信息提示
    void InforAds(AdsType type,string messg,float num = -2)
    {
        if(isAdReward)
        {
            if(num != -2)
            {
                GameManager.Instance.ClonePrompt(num, 0);
            }
            else
            {
                GameManager.Instance.CloneTip(messg);
            }
        }
        else
        {
            DetectionPanel();
        }
        isAdReward = false;
        GameManager.Instance.RequestRewardBasedVideo(type);
    }
    //储蓄罐
    private void ClickMonery()
    {
        AudioManager.Instance.PlayTouch("bank_1");
        if (PlayerPrefs.GetString("PiggyReward")!="")
        {
            if(boxGold > 0)
            {
                float num = boxGold * 0.2f;
                for (int i = 0; i < 5; i++)
                {
                    StartCoroutine(CreateGold(num, i * 0.2f));
                }
                boxGold = 0;
                savingText.text = boxGold.ToString();
                PlayerPrefs.SetFloat("piggyBank", boxGold);
            }
        }
        else
        {
            isTime = true;
            storePanel.OpenPanel(820);
        }
    }
    IEnumerator CreateGold(float num,float day)
    {
        Transform go;
        yield return new WaitForSeconds(day);
        go = ObjectPool.Instance.CreateObject(glodPrefab.name, glodPrefab).transform;
        go.transform.SetParent(snowingPrent.transform);
        Vector3 point = moneyBtn.transform.localPosition;
        point.z = 300;
        go.transform.localPosition = point;
        go.localScale = Vector3.one * 500;
        go.DOMove(goldText.transform.parent.position, 1f);
        yield return new WaitForSeconds(1);
        goldImage.sprite = goldHigh;
        SetGold(num,false,false);
        ObjectPool.Instance.CollectObject(go.gameObject);
        yield return new WaitForSeconds(0.2f);
        goldImage.sprite = goldNor;
    }
  
    //销毁高级塔提示面板
    public void ConfirmPanel()
    {
        isTime = true;
        confirmPanel.SetActive(true);
    }  
    //是否销毁炮台
    public void ConfirmTurret(bool isDes)
    {
        isTime = false;
        TurretDrag.Instance.SetTurretDes(isDes);
        confirmPanel.SetActive(false);
    }
    //飘血效果
    public void WindBlood(Vector3 pos,float damage,Color color)
    {
        if (settingsPanel.isInjury) return;
        var go = ObjectPool.Instance.CreateObject(snowingPrefab.name, snowingPrefab).GetComponent<Text>();
        go.transform.SetParent(snowingPrent.transform);
        go.transform.localPosition = WordTOScreer(pos);
        go.transform.localScale = Vector3.zero;
        go.transform.DOScale(Vector3.one, 0.2f);
        go.transform.DOLocalMoveY(go.transform.localPosition.y + 70, 0.3f);
        go.color = color;
        if (damage <= 1)
        {
            go.text = string.Format("-{0}",1);
        }
        else
        {
            go.text = string.Format("-{0}", Mathf.Ceil(damage));
        }
        ObjectPool.Instance.CollectObject(go.gameObject,0.5f);
    }
    //金币
    public void ColeGold(Vector3 pos, float num)
    {
        var go = ObjectPool.Instance.CreateObject(glodPrefab.name, glodPrefab).transform;
        go.transform.SetParent(snowingPrent.transform);
        Vector3 point = WordTOScreer(pos);
        go.transform.localPosition = point;
        go.localScale = Vector3.one;
        go.DOScale(Vector3.one * 350,1f);
        go.DOMove(goldText.transform.parent.position, 1f);
        StartCoroutine(DelyGold(go.gameObject, num*earDouble));
    }
    IEnumerator DelyGold(GameObject go, float dale)
    {
        yield return new WaitForSeconds(1);
        AudioManager.Instance.PlaySource("money_1",source);
        goldImage.sprite = goldHigh;
        SetGold(dale);
        ObjectPool.Instance.CollectObject(go);
        yield return new WaitForSeconds(0.2f);
        goldImage.sprite = goldNor;
    }
    //时间格式化
    void TimeFormat(float time,Text text)
    {
        int hour = (int)time / 3600;
        int minute = (int)(time - hour * 3600) / 60;
        int second = (int)(time - hour * 3600 - minute * 60);
        text.text = string.Format("{0:D1}:{1:D2}:{2:D2}", hour, minute, second);
    }
    //指引销毁
    public void LeadDestroy(bool ishide, Vector3 t1, Vector3 t2)
    {
        if (ishide)
        {
            t1.y += 0.2f;
            t2.y += 0.2f;
            leaddestroy.SetActive(true);
            leadPoint3 = WordTOScreer(t1);
            leadPoint4 = WordTOScreer(t2);
            leaddestroy.transform.localPosition = leadPoint3;
            lead_index = 1;
            StartCoroutine(DestroyAnim());
        }
        else
        {
            lead_index = 5;
            leaddestroy.SetActive(false);
            StopCoroutine("DestroyAnim");
        }
    }
    IEnumerator DestroyAnim()
    {
        if(lead_index < 5)
        {
            leaddestroy.transform.DOLocalMove(leadPoint4, 1);
            yield return new WaitForSeconds(1);
            leaddestroy.transform.DOLocalMove(leadPoint3, 1);
            yield return new WaitForSeconds(1);
            lead_index++;
            StartCoroutine(DestroyAnim());
        }
        else
        {
            leaddestroy.SetActive(false);
        }
    }
    //指引合并
    public void LeadAnim(bool ishide, Vector3 t1, Vector3 t2)
    {
        StopCoroutine("MergerOpen");
        if (ishide)
        {
            t1.y += 0.2f;
            t2.y += 0.2f;
            leadTime = 1;
            leadObject.SetActive(true);
            leadPoint1 = WordTOScreer(t1);
            leadPoint2 = WordTOScreer(t2);
            leadObject.transform.localPosition = leadPoint1;
            StartCoroutine(MergerAnim());
        }
        else
        {
            leadTime = 5;
            leadObject.SetActive(false);
            StopCoroutine("MergerAnim");
        }
    }
    IEnumerator MergerAnim()
    {
        if (leadTime <= 5)
        {
            leadObject.transform.DOLocalMove(leadPoint2, 1);
            yield return new WaitForSeconds(1);
            leadObject.transform.DOLocalMove(leadPoint1, 1);
            yield return new WaitForSeconds(1);
            leadTime++;
            StartCoroutine(MergerAnim());
        }
        else
        {
            leadObject.SetActive(false);
        }
    }
    IEnumerator MergerOpen()
    {
        yield return new WaitForSeconds(30);
        if (!(leadPoint1 == Vector3.one && leadPoint2 == Vector3.one))
        {
            leadTime = 1;
            leadObject.SetActive(true);
            leadObject.transform.localPosition = leadPoint1;
            StartCoroutine(MergerAnim());
        }
    }
    //坐标转换
    private Vector3 WordTOScreer(Vector3 pos)
    {
        Vector2 aimScreenPos = UICamera.WorldToScreenPoint(pos);
        //在将屏幕坐标转换为UGUI坐标：
        Vector2 aimLocalPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, aimScreenPos, UICamera, out aimLocalPos))
        {
            return aimLocalPos;
        }
        return Vector3.zero;
    }
    public void BackgroundCut()
    {
        if (everydayPanel.gameObject.activeInHierarchy)
        {
            everydayPanel.Everyday(); return;
        }
        if (PlayerPrefs.GetFloat("DiamondAds") <= 0 || (PlayerPrefs.GetString("vipReward") != "" && PlayerPrefs.GetFloat("vipCandy") <= 0))
        {
            everydayPanel.Everyday();
        }
    }
    //检测面板是否关闭
    public void DetectionPanel()
    {
        if (failurePanel.gameObject.activeInHierarchy)
        {
            failurePanel.BackPanel();
        }
        for (int i = starPanel_index; i < endPanel_index; i++)
        {
            if(transform.GetChild(i).gameObject.activeInHierarchy)
            {
                isTime = true;
                return;
            }
        }
        isTime = false;
        GameManager.Instance.HideBanner();
    }
    //钻石动画
    public void OpenDiam(Vector3 endPoint)
    {
        diamAnimal.SetActive(true);
        diamAnimal.transform.localPosition = Vector3.zero;
        diamAnimal.transform.DOMove(endPoint, 1);
        StartCoroutine(HideDiam());
    }
    private IEnumerator HideDiam()
    {
        yield return new WaitForSeconds(1);
        diamAnimal.SetActive(false);
    }
    //龙骨贴图生成
    public DragonBones.UnityArmatureComponent SetArmature(DragonBones.UnityArmatureComponent model_Armature,Transform parent, string messgName, Vector3 scale,Vector3 point,bool isAnimal=false,string animal = "",bool isUI=true)
    {
        model_Armature.gameObject.SetActive(true);
        model_Armature.armature.Dispose();
        model_Armature = DragonBones.UnityFactory.factory.BuildArmatureComponent(messgName, "", "", "", null, isUI);
        model_Armature.transform.SetParent(parent);
        model_Armature.transform.SetAsFirstSibling();
        model_Armature.transform.localEulerAngles = Vector3.zero;
        model_Armature.transform.localPosition = point;
        model_Armature.transform.localScale = scale;
        if(isAnimal)
        {
            model_Armature.animation.Play(animal);
        }
        return model_Armature;
    }
    //龙骨动画索引
    public int ArmatureKeel(int index)
    {
        for (int i = 0; i < keelAnim.Length; i++)
        {
            if(keelAnim[i] == index)
            {
                return i;
            }
        }
        return 10;
    }
    //提示解锁面板
    public void UnlockPanel(int id,float level,int index,bool isTurret,string messg, Sprite sprite = null)
    {
        if (isTurret && !unlockturret.activeInHierarchy)
        {
            isTime = true;
            unlockturret.SetActive(true);
            //PlayerPrefs.SetString(messg, "unlock");
            unlockturret.GetComponent<UnlockPanel>().Init(id, level, isTurret, index, messg, sprite);
        }
        else if (!isTurret && !unlockSkill.activeInHierarchy)
        {
            isTime = true;
            unlockSkill.SetActive(true);
            //PlayerPrefs.SetString(messg, "unlock");
            unlockSkill.GetComponent<UnlockPanel>().Init(id, level, isTurret, index, messg, sprite);
        }
    }
    //随机不重复数字
    public int[] GetRandoms(int sum, int min, int max)
    {
        int[] arr = new int[sum];
        int j = 0;
        //表示键和值对的集合。
        Hashtable hashtable = new Hashtable();
        System.Random rm = new System.Random();
        while (hashtable.Count < sum)
        {
            //返回一个min到max之间的随机数
            int nValue = rm.Next(min, max);
            // 是否包含特定值
            if (!hashtable.ContainsValue(nValue))
            {
                //把键和值添加到hashtable
                hashtable.Add(nValue, nValue);
                arr[j] = nValue;

                j++;
            }
        }
        return arr;
    }
    // K M B T
    public string GoldDisplay(float num)
    {
        if (num >= 1000 && num < 1000000)
        {
            return string.Format("{0:F1}k", num * 0.001);
        }
        else if(num >= 1000000 && num < 1000000000)
        {
            return string.Format("{0:F1}m", num * 0.000001);
        }
        else if (num >= 1000000000)
        {
            return string.Format("{0:F1}b", num * 0.000000001f);
        }
        return string.Format("{0:F0}", Mathf.Floor(num));
    }
    #region//关卡UI
    bool islevel = true;
    public void InitLevel(float level)
    {
        customs = level;
        //Debug.Log("我执行了" + level);
        for (int i = 0; i < leverObject.Count; i++)
        {
            leverObject[i].SetActive(false);
            leverObject[i].GetComponent<SpriteLevel>().BossChallenge(false);
        }
        leverObject.Clear();
        int index = 0;
        if (level <= 1)
        {
            index = 0;
        }
        else
        {
            index = -1;
        }
        for (int i = index; i < 2; i++)
        {
            if(int.Parse(CreateModel.Instance.ReturnLevel(level + i).mapId) == CreateModel.Instance.sceneIndex)
            {
                var leverSprite = ObjectPool.Instance.CreateObject(leverPrefab.name, leverPrefab);
                leverSprite.transform.SetParent(leverPrent.transform);
                leverSprite.transform.localPosition = new Vector3(i * 40, 0f, 0);
                leverSprite.transform.localEulerAngles = new Vector3(0, 0, 90);
                leverSprite.GetComponent<SpriteLevel>().SetLevelText(level + i);
                leverObject.Add(leverSprite);
                if (i == 0)
                {
                    leverSprite.transform.localScale = Vector3.one * 1.5f;
                }
                else
                {
                    leverSprite.transform.localScale = Vector3.one;
                }
                if ((level + i) % 5 == 0 && CreateModel.Instance.maxLevel+1 >= level + i)
                {
                    if (leverSprite.transform.localPosition.x > 0)
                    {
                        leverSprite.GetComponent<SpriteLevel>().BossChallenge(true);
                    }
                }
            }
        }
    }
    public void LevelAddDesign(float grade)
    {
        if (grade > customs && islevel)
        {
            islevel = false;
            Invoke("DelyLevel", 1f);
            customs = grade;
            for (int i = 0; i < leverObject.Count; i++)
            {
                if (leverObject[i].transform.localPosition.x <= -40)
                {
                    leverObject[i].transform.DOScale(Vector3.zero, 0.5f);
                    StartCoroutine(Destruction(leverObject[i]));
                }
            }
            for (int i = 0; i < leverObject.Count; i++)
            {
                if (leverObject[i].transform.localPosition.x == 40f)
                {
                    leverObject[i].transform.DOScale(Vector3.one * 1.5f, 0.5f);
                    leverObject[i].GetComponent<SpriteLevel>().BossChallenge(false);
                    leverObject[i].transform.DOLocalMoveX(0, 0.5f);
                }
                else if (leverObject[i].transform.localPosition.x == 0)
                {
                    leverObject[i].transform.DOScale(Vector3.one, 0.5f);
                    leverObject[i].transform.DOLocalMoveX(-40, 0.5f);
                }
            }
            grade += 1;
            if (int.Parse(CreateModel.Instance.ReturnLevel(grade).mapId) == CreateModel.Instance.sceneIndex)
            {
                var leverSprite = ObjectPool.Instance.CreateObject(leverPrefab.name, leverPrefab);
                leverSprite.transform.SetParent(leverPrent.transform);
                leverSprite.transform.localPosition = new Vector3(40, 0f, 0);
                leverSprite.transform.localEulerAngles = new Vector3(0, 0, 90);
                leverSprite.GetComponent<SpriteLevel>().SetLevelText(grade);
                leverSprite.transform.localScale = Vector3.zero;
                leverSprite.transform.DOScale(Vector3.one, 0.5f);
                leverObject.Add(leverSprite);
                if (grade % 5 == 0 && CreateModel.Instance.maxLevel + 1 >= grade)
                {
                    leverSprite.GetComponent<SpriteLevel>().BossChallenge(true);
                }
            }
        }
    }
    public void LevelSubDesign(float grade)
    {
        if (!islevel) return;
        islevel = false;
        Invoke("DelyLevel", 1f);
        customs = grade;
        if (customs < 1)
        {
            return;
        }
        for (int i = 0; i < leverObject.Count; i++)
        {
            if (leverObject[i].transform.localPosition.x >= 40)
            {
                leverObject[i].transform.DOScale(Vector3.zero, 0.5f);
                StartCoroutine(Destruction(leverObject[i]));
            }
        }
        for (int i = 0; i < leverObject.Count; i++)
        {
            if (leverObject[i].transform.localPosition.x == -40)
            {
                leverObject[i].transform.DOScale(Vector3.one * 1.5f, 0.5f);
                leverObject[i].transform.DOLocalMoveX(0, 0.5f);
            }
            else if (leverObject[i].transform.localPosition.x == 0)
            {
                leverObject[i].transform.DOScale(Vector3.one, 0.5f);
                leverObject[i].transform.DOLocalMoveX(40, 0.5f);
                if (float.Parse(leverObject[i].GetComponent<SpriteLevel>().levelText.text) % 5 == 0)
                {
                    leverObject[i].GetComponent<SpriteLevel>().BossChallenge(true);
                }
            }
        }
        if (grade >= 2)
        {
            grade -= 1;
            if (int.Parse(CreateModel.Instance.ReturnLevel(grade).mapId) == CreateModel.Instance.sceneIndex)
            {
                var leverSprite = ObjectPool.Instance.CreateObject(leverPrefab.name, leverPrefab);
                leverSprite.transform.SetParent(leverPrent.transform);
                leverSprite.transform.localPosition = new Vector3(-40, 0f, 0);
                leverSprite.transform.localEulerAngles = new Vector3(0, 0, 90);
                leverSprite.GetComponent<SpriteLevel>().SetLevelText(grade);
                leverSprite.transform.localScale = Vector3.zero;
                leverSprite.transform.DOScale(Vector3.one, 0.5f);
                leverObject.Add(leverSprite);
            }
        }
    }
    void DelyLevel()
    {
        islevel = true;
    }
    IEnumerator Destruction(GameObject temp)
    {
        temp.GetComponent<SpriteLevel>().BossChallenge(false);
        leverObject.Remove(temp);
        yield return new WaitForSeconds(0.5f);
        ObjectPool.Instance.CollectObject(temp);
    }
    #endregion
}
