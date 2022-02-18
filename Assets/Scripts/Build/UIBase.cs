using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIBase : MonoBehaviour
{
    private static UIBase instance;
    public static UIBase Instance { get => instance; }
    public GameObject treasurePrefab;
    public Sprite move_scene;
    public Sprite rotate_scene;
    public Sprite move_animal;
    public Sprite rotate_animal;
    public GameObject qrCode;
    public List<Transform> bornAnimal = new List<Transform>();
    public List<int> deathAnimal = new List<int>();
    public bool isAnimal;
    public bool isMove;
    public float sumAnimal;
    public float speedRotate;
    public float speedSlide;
    public float diamond;
    public float maxScore;
    public float height = 3;

    Button patternBtn;
    Button shareBtn;
    Button scenesBtn;
    Button regretBtn;
    Button freedBtn;
    Button switchBtn;
    Button resetBtn;
    Button restoreBtn;
    Button giftBtn;
    Button buyBtn;
    Button freeBtn;
    Button centerBtn;

    public Text buyDiamText;
    Text adsText;
    Text diamondText;
    Text fractionText;
    Text deathsText;
    Text animalText;
    Text buildText;
    Text maxText;
    Text midText;
    Text minText;
    Text treasureText;

    Image tipFreed;
    //Image battImage;
    Image scenesImage;
    Image switchImage;
    Image titleImage;

    private DragonBones.UnityArmatureComponent win;
    public CreateAnimalHead animalHead;
    private BuyAnimalsPanel animalsPanel;
    private GiftPanel giftPanel;
    public GuidePanel guidePanel;
    private AdvertiseButton advertise;
    private CameraControl cameraControl;
    private CanvasGroup totleGroup;
    private Camera m_Camera;
    private Transform initAnimal;
    private Transform direction;
    private Transform BasePlane;
    private Transform tempAnimal;
    private Transform currentAnimal;
    private Transform[] animalPrefab;

    private GameObject tipGift;
    private GameObject maskPanel;
    private GameObject animalImage;
    private GameObject shareTip;
    //private GameObject battleTip;
    private GameObject buyTip;
    private GameObject freeTip;
    private GameObject lockTime;
    private GameObject freedTip;

    private bool isRecovery;
    private bool isRotate;
    private bool isClickUI;
    private int tempIndex = -10;
    private float tempScale;
    private float laveNumber;
    //private float battTime = 5;
    private float lastTime;
    private float currScore;

    private float animalHigh;
    private float freedTime;
    private int treasureCount;
    private int turnTreasure;
    private int diffCount;
    private int layerMask;

    private Vector3 directionPoint = new Vector3(0, 0.01f, 0);
    private Vector3 tempPoint;
    private Vector3 tempAngles;
    private Vector3 initPoint;
    private List<Transform> treasures = new List<Transform>();
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        tempAnimal = null;
        Application.quitting += SaveData;
        m_Camera = Camera.main;
        initAnimal = m_Camera.transform.parent.Find("Cube");
        AudioManager.Instance.CutBgMusic("waterweather_1");
        animalPrefab = ExcelTool.Instance.buildAnimal;
        FindNode();
    }

    private void FindNode()
    {
        totleGroup = gameObject.GetComponent<CanvasGroup>();
        win = transform.Find("win").GetComponent<DragonBones.UnityArmatureComponent>();
        BasePlane = GameObject.Find("BaseParent").transform;
        direction = BasePlane.Find("Direction");
        maskPanel = transform.Find("MaskPanel").gameObject;
        animalImage = transform.Find("AnimalBtn").gameObject;
        tipGift = transform.Find("Distance/GiftBtn/TipImage").gameObject;
        shareTip = transform.Find("ShareBtn/TipImage").gameObject;
        //battleTip = transform.Find("BattBtn/TipImage").gameObject;
        buyTip = transform.Find("BuyBtn/TipImage").gameObject;
        freeTip = transform.Find("LeftBg/FreeBtn/TipImage").gameObject;
        lockTime = transform.Find("SelectBtn/TimeBtn/lock").gameObject;
        freedTip = transform.Find("FreedBtn/tipImage").gameObject;

        cameraControl = m_Camera.transform.parent.GetComponent<CameraControl>();
        advertise = transform.Find("BGBtn").GetComponent<AdvertiseButton>();
        animalHead = transform.Find("Scroll View").GetComponent<CreateAnimalHead>();
        giftPanel = transform.Find("GiftPanel").GetComponent<GiftPanel>();
        guidePanel = transform.Find("GuidePanel").GetComponent<GuidePanel>();
        animalsPanel = transform.Find("BuyAnimalsPanel").GetComponent<BuyAnimalsPanel>();

        titleImage = transform.Find("TitleImage").GetComponent<Image>();
        adsText = transform.Find("Ads/Text").GetComponent<Text>();
        diamondText = transform.Find("Diamond/Text").GetComponent<Text>();
        fractionText = transform.Find("FractionText").GetComponent<Text>();
        deathsText = transform.Find("Deaths/Text").GetComponent<Text>();
        animalText = transform.Find("Animal/Text").GetComponent<Text>();
        buildText = transform.Find("Build/Text").GetComponent<Text>();
        maxText = transform.Find("Distance/Max/Text").GetComponent<Text>();
        midText = transform.Find("Distance/Mid/Text").GetComponent<Text>();
        minText = transform.Find("Distance/Min/Text").GetComponent<Text>();
        treasureText = transform.Find("TreasureText").GetComponent<Text>();

        tipFreed = transform.Find("FreedBtn/Image").GetComponent<Image>();
        //battImage = transform.Find("BattBtn/Image").GetComponent<Image>();
        scenesImage = transform.Find("ScenesBtn/Image").GetComponent<Image>();
        switchImage = transform.Find("SwitchBtn/Image").GetComponent<Image>();

        patternBtn = transform.Find("PatternBtn").GetComponent<Button>();
        shareBtn = transform.Find("ShareBtn").GetComponent<Button>();
        scenesBtn = transform.Find("ScenesBtn").GetComponent<Button>();
        regretBtn = transform.Find("RegretBtn").GetComponent<Button>();
        freedBtn = transform.Find("FreedBtn").GetComponent<Button>();
        switchBtn = transform.Find("SwitchBtn").GetComponent<Button>();
        resetBtn = transform.Find("LeftBg/ResetBtn").GetComponent<Button>();
        restoreBtn = transform.Find("LeftBg/RestoreBtn").GetComponent<Button>();
        giftBtn = transform.Find("Distance/GiftBtn").GetComponent<Button>();
        buyBtn = transform.Find("BuyBtn").GetComponent<Button>();
        freeBtn = transform.Find("LeftBg/FreeBtn").GetComponent<Button>();
        centerBtn = transform.Find("CenterBtn").GetComponent<Button>();
        centerBtn.gameObject.SetActive(false);
        //battBtn.enabled = false;
        resetBtn.onClick.AddListener(() => { advertise.OpenBtn(true); });
        restoreBtn.onClick.AddListener(() => { advertise.OpenBtn(false); });
        giftBtn.onClick.AddListener(() => { giftPanel.SetData(); });
        patternBtn.onClick.AddListener(OpenPattern);
        scenesBtn.onClick.AddListener(SwitchOperation);
        freedBtn.onClick.AddListener(FreedEvent);
        switchBtn.onClick.AddListener(SwitchMode);
        regretBtn.onClick.AddListener(BackStep);
        shareBtn.onClick.AddListener(ShareEvent);
        buyBtn.onClick.AddListener(BuyAnimal);
        freeBtn.onClick.AddListener(FreeDiamond);
        centerBtn.onClick.AddListener(BackCenterBtn);

        maxScore = PlayerPrefs.GetFloat("MaxScore");
        diamond = PlayerPrefs.GetFloat("DiamondNumber");
        totleGroup.alpha = 0;
    }
    private void Start()
    {
        animalHead.Init();
        giftPanel.Init();
        animalsPanel.Init();
        guidePanel.Init("BuildGuide");
        deathAnimal = Modification.GetDeathData();
        diamondText.text = diamond.ToString("F0");
        buyDiamText.text = diamond.ToString("F0");
        fractionText.text = ExcelTool.lang["recording"] + maxScore.ToString("F1") + "M";
        maxText.text = giftPanel.GetMaxHight(maxScore);
        deathsText.text = deathAnimal.Count.ToString();
        animalText.text = sumAnimal.ToString("F0");
        StartCoroutine(InitScene());
        tipFreed.gameObject.SetActive(false);
        titleImage.sprite = ExcelTool.Instance.tempSprite;
        titleImage.SetNativeSize();
        GameManager.Instance.skipPanel.ClosePanel();
        adsText.text = GameManager.Instance.adsCount.ToString();
        GameManager.Instance.AdmobAdsCount = null;
        GameManager.Instance.AdmobAdsCount = AdsCount;
        layerMask = LayerMask.GetMask("Enemy");
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000, layerMask))
            {
                if (isRecovery)
                {
                    hit.transform.GetComponent<AnimalControl>().DestraeGame();
                }
                else
                {
                    isRecovery = true;
                    Invoke("DelayedRecovery", 0.2f);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isAnimal = animalImage == EventSystem.current.currentSelectedGameObject;
            if (isAnimal)
            {
                isClickUI = true;
                guidePanel.OpenGuide(4, false);
            }
            else
            {
                isClickUI = IsPointerOverGameObject(Input.mousePosition);
                if (!isClickUI)
                {
                    guidePanel.OpenGuide(5, false);
                }
            }
            if (lastTime >= 30 && tempAnimal)
            {
                tempAnimal.localScale = tempScale * Vector3.one;
            }
            lastTime = 0;
        }
        if (Input.GetMouseButton(0))
        {
            cameraControl.ScrollMouse();
            if (isAnimal)
            {
                if (isRotate)
                {
                    if (Input.touchCount == 1)
                    {
                        float mouse_X = Input.GetTouch(0).deltaPosition.x;
                        if (mouse_X != 0)
                        {
                            tempAnimal.Rotate(-Vector3.up * mouse_X * speedRotate, Space.World);
                        }
                    }
                    else if (Input.touchCount < 1)
                    {
                        float mouse_X = Input.GetAxis("Mouse X");
                        if (mouse_X != 0)
                        {
                            tempAnimal.Rotate(-Vector3.up * mouse_X * speedRotate, Space.World);
                        }
                    }
                }
                else
                {
                    Vector3 _vec3TargetScreenSpace = Camera.main.WorldToScreenPoint(tempAnimal.position);
                    Vector3 _vec3MouseScreenSpace = Input.mousePosition;
                    _vec3MouseScreenSpace.z = _vec3TargetScreenSpace.z - 0.8f;
                    Vector3 _vec3TargetWorldSpace = Camera.main.ScreenToWorldPoint(_vec3MouseScreenSpace);
                    tempAnimal.position = new Vector3(Mathf.Clamp(_vec3TargetWorldSpace.x, -40, 40), tempAnimal.position.y, Mathf.Clamp(_vec3TargetWorldSpace.z, -40, 40));
                    ImpactDetection();
                }
            }
        }
        else
        {
            lastTime += Time.deltaTime;
        }
        if (!isClickUI)
        {
            cameraControl.UpdataData();
            UpdataPoint();
            Vector3 point = cameraControl.transform.position;
            if (point.x > 50 || point.x < -50 || point.z > 25 || point.z < -65)
            {
                centerBtn.gameObject.SetActive(true);
            }
            else if (point.x < 50 && point.x > -50 && point.z < 25 && point.z > -65)
            {
                centerBtn.gameObject.SetActive(false);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isAnimal)
            {
                isAnimal = false;
                isMove = true;
                tempPoint = tempAnimal.localPosition;
                tempAngles = tempAnimal.localEulerAngles;
            }
        }

        if (freedTime > 0)
        {
            freedTime -= Time.deltaTime;
            tipFreed.fillAmount = freedTime;
            if (freedTime <= 0)
            {
                freedBtn.enabled = true;
                tipFreed.gameObject.SetActive(false);
            }
        }

        //if (battTime >= 0)
        //{
        //    battTime -= Time.deltaTime;
        //    battImage.fillAmount = battTime * 0.2f;
        //    if (battTime <= 0)
        //    {
        //        battBtn.enabled = true;
        //        battImage.gameObject.SetActive(false);
        //    }
        //}

        if (lastTime >= 30)
        {
            lastTime = 0;
            StartCoroutine(TipAnimator());
        }
    }
    void DelayedRecovery()
    {
        isRecovery = false;
    }
    void AdsCount(int count)
    {
        adsText.text = count.ToString();
    }
    public void SaveData()
    {
        string messg = "";
        for (int i = 0; i < treasures.Count; i++)
        {
            messg += treasures[i].position.y + ",";
        }
        PlayerPrefs.SetString("DiamTreas", messg);
        Modification.ServerData(bornAnimal, deathAnimal);
    }
    //初始进入
    private IEnumerator InitScene()
    {
        yield return new WaitForSeconds(0.5f);
        maskPanel.SetActive(true);
        cameraControl.transform.DOLocalMove(new Vector3(0, 10, -20), 4);
        yield return new WaitForSeconds(3.5f);
        m_Camera.transform.DOLocalRotate(new Vector3(30, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.8f);
        Modification.GetPoints(animalPrefab, BasePlane);
        SetPossess();
        animalHead.InitCreate();
        totleGroup.DOFade(1, 0.5f);
        tipGift.SetActive(maxScore >= giftPanel.highDemand);
        yield return new WaitForSeconds(0.3f);
        maskPanel.SetActive(false);
        SetDistance();
        if (laveNumber > 0)
        {
            guidePanel.OpenGuide(1, false);
        }
        treasureCount = PlayerPrefs.GetInt("DiamondsTreasure");
        turnTreasure = PlayerPrefs.GetInt("TurnUpTreasure");
        diffCount = treasureCount - turnTreasure;
        string mess = PlayerPrefs.GetString("DiamTreas");
        string[] messg = null;
        if (mess != "")
        {
            messg = mess.Split(',');
        }
        for (int i = 0; i < diffCount; i++)
        {
            var go = ObjectPool.Instance.CreateObject(treasurePrefab.name, treasurePrefab);
            go.transform.SetParent(treasurePrefab.transform.parent);
            if (messg!=null && messg.Length-1 > i)
            {
                go.GetComponent<TreasureDiamonds>().SetInit(float.Parse(messg[i]));
            }
            else
            {
                go.GetComponent<TreasureDiamonds>().SetInit(tempAnimal.localPosition.y);
            }
            treasures.Add(go.transform);
        }
        if (PlayerPrefs.GetString("TreasureInit") == "")
        {
            var go = Instantiate(treasurePrefab);
            go.SetActive(true);
            go.transform.SetParent(treasurePrefab.transform.parent);
            go.GetComponent<TreasureDiamonds>().SetTreasure(2);
            treasureCount += 1;
            PlayerPrefs.SetInt("DiamondsTreasure", treasureCount);
        }
        treasureText.text = string.Format("{0} {1}/{2}",ExcelTool.lang["Treasure"], turnTreasure, treasureCount);
        initPoint = new Vector3(0,0,-20);
    }
    //胜利
    private void WinArmature()
    {
        if (!win.gameObject.activeInHierarchy)
        {
            win.gameObject.SetActive(true);
            win.animation.Play("win", 1);
            if (!win.HasEventListener(DragonBones.EventObject.COMPLETE))
            {
                win.AddEventListener(DragonBones.EventObject.COMPLETE, HideWin);
            };
        }
    }
    private void HideWin(string type, DragonBones.EventObject eventObject)
    {
        win.gameObject.SetActive(false);
    }
    //提示动画
    private IEnumerator TipAnimator()
    {
        tempAnimal.DOScale(tempScale * 1.5f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        tempAnimal.DOScale(tempScale, 0.6f);
    }
    private void SetDistance()
    {
        if (currScore < 0) currScore = 0;
        midText.text = currScore.ToString("F1");
        minText.text = giftPanel.GetMinHight(currScore);
    }

    public void CreateTreasure()
    {
        if(diffCount < 100)
        {
            treasureCount += 5;
            diffCount = treasureCount - turnTreasure;
            PlayerPrefs.SetInt("DiamondsTreasure", treasureCount);
            for (int i = 0; i < 5; i++)
            {
                var go = ObjectPool.Instance.CreateObject(treasurePrefab.name, treasurePrefab.gameObject);
                go.transform.SetParent(treasurePrefab.transform.parent);
                go.GetComponent<TreasureDiamonds>().SetInit(tempAnimal.localPosition.y);
                treasures.Add(go.transform);
            }
            treasureText.text = string.Format("{0} {1}/{2}", ExcelTool.lang["Treasure"], turnTreasure, treasureCount);
            GameManager.Instance.CloneTip(ExcelTool.lang["Treasure"] +"+5");
            guidePanel.OpenGuide(8,false);
        }
    }
    public void TurnUpTreasure(Transform go,int number)
    {
        SetDiamond(number);
        turnTreasure += 1;
        diffCount = treasureCount - turnTreasure;
        treasureText.text = string.Format("{0} {1}/{2}", ExcelTool.lang["Treasure"], turnTreasure, treasureCount);
        PlayerPrefs.SetInt("TurnUpTreasure", turnTreasure);
        EffectGenerator.Instance.Meage(tempAnimal);
        GameManager.Instance.ClonePrompt(number, 1);
        if (treasures.Contains(go))
        {
            treasures.Add(go.transform);
        }
    }

    public void SetAnimals(int count)
    {
        sumAnimal += count;
        animalText.text = sumAnimal.ToString("F0");
        laveNumber = sumAnimal - deathAnimal.Count - bornAnimal.Count;
        buildText.text = laveNumber.ToString("F0");
        animalHead.IsTip(tempIndex);
        freedTip.SetActive(laveNumber > 0);
    }
   
    public void FreeDiamond()
    {
        bool isReward = false;
        if (Application.platform == RuntimePlatform.Android)
        {
            GameManager.Instance.UserChoseToWatchAd(AdsType.sweets);
            GameManager.Instance.AdmobRewardCB = delegate
            {
                
            };
            GameManager.Instance.AdmobRewardClose = delegate
            {
                isReward = true;
            };
            GameManager.Instance.AdmobClose = delegate
            {
                if (isReward)
                {
                    SetDiamond(1);
                    GameManager.Instance.ClonePrompt(1, 1);
                }
                GameManager.Instance.RequestRewardBasedVideo(AdsType.sweets);
            };
        }
        else
        {
            SetDiamond(1);
            GameManager.Instance.ClonePrompt(1, 1);
        }
    }
    private void BackCenterBtn()
    {
        centerBtn.gameObject.SetActive(false);
        initPoint.y = cameraControl.transform.position.y;
        cameraControl.transform.position = initPoint;
        tempAnimal.localPosition = initAnimal.position;
        tempAnimal.localEulerAngles = initAnimal.eulerAngles;
        cameraControl.Rotion_Transform = initAnimal.position;
    }
    //购买动物
    private void BuyAnimal()
    {
        AudioManager.Instance.PlayTouch("other_1");
        buyTip.SetActive(false);
        animalsPanel.gameObject.SetActive(true);
    }
    //分享事件
    private void ShareEvent()
    {
        AudioManager.Instance.PlayTouch("other_1");
        shareTip.SetActive(false);
        titleImage.gameObject.SetActive(true);
        qrCode.SetActive(true);
        GJCNativeShare.Instance.ShareWithNative(titleImage.gameObject, qrCode);
        if (PlayerPrefs.GetInt("ShareEvent") < 20)
        {
            GameManager.Instance.ShareEvent();
        }
    }
    private void OpenPattern()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (PlayerPrefs.GetFloat("MaxLevel") < 2)
        {
            lockTime.SetActive(true);
        }
        else
        {
            lockTime.SetActive(false);
        }
    }

    public void PlayerSound(string sound)
    {
        AudioManager.Instance.PlayTouch(sound);
    }
    //切换场景
    public void SwitchScene(string scene)
    {
        if (scene == "time")
        {
            if (PlayerPrefs.GetFloat("MaxLevel") < 2)
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
                return;
            }
        }
        StartCoroutine(DelySwitch(scene));
    }
    IEnumerator DelySwitch(string isTime)
    {
        totleGroup.DOFade(0, 1);
        m_Camera.transform.DORotate(new Vector3(85, 0, 0), 0.5f);
        cameraControl.transform.DOLocalMove(new Vector3(0, 5000, 200), 2f);
        yield return new WaitForSeconds(2.2f);
        //battleTip.SetActive(false);
        Modification.ServerData(bornAnimal, deathAnimal);
        Application.quitting -= SaveData;
        ObjectPool.Instance.ClearAll();
        //SceneManager.LoadScene("Loading");
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        GameManager.Instance.SwitchScene("main", isTime);
    }

    //切换相机操作
    private void SwitchOperation()
    {
        AudioManager.Instance.PlayTouch("other_1");
        cameraControl.SetCenter(initAnimal.position);
        if (cameraControl.isRotate)
        {
            scenesImage.sprite = rotate_scene;
        }
        else
        {
            scenesImage.sprite = move_scene;
        }
    }
    //后悔按钮
    private void BackStep()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (currentAnimal)
        {
            EffectGenerator.Instance.Meage(currentAnimal);
            int index = currentAnimal.GetComponent<AnimalControl>().indexAnimal;
            animalHead.SetHead(index, 1);
            if (bornAnimal.Contains(currentAnimal))
                bornAnimal.Remove(currentAnimal);
            laveNumber += 1;
            buildText.text = laveNumber.ToString("F0");
            freedTip.SetActive(laveNumber > 0);
            Arrivals();
            CreateShadow(index, currentAnimal.localScale.x);
            currentAnimal.gameObject.SetActive(false);
            currentAnimal = null;
        }
    }
    //释放按钮
    private void FreedEvent()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (animalHead.IsCreate(tempIndex))
        {
            if (!isMove)
            {
                tempPoint = initAnimal.position;
                tempAngles = initAnimal.eulerAngles;
                tempAnimal.localPosition = tempPoint;
                tempAnimal.localEulerAngles = tempAngles;
                animalImage.transform.position = m_Camera.WorldToScreenPoint(tempAnimal.position);
            }
            animalHead.SetHead(tempIndex, -1);
            currentAnimal = ObjectPool.Instance.CreateObject(animalPrefab[tempIndex].name, animalPrefab[tempIndex].gameObject).transform;
            currentAnimal.SetParent(BasePlane, false);
            currentAnimal.localScale = Vector3.zero;
            currentAnimal.DOScale(Vector3.one * tempScale, 0.5f);
            currentAnimal.localPosition = tempAnimal.localPosition;
            currentAnimal.localEulerAngles = tempAnimal.localEulerAngles;
            currentAnimal.GetComponent<AnimalControl>().FreedObject(true);
            currentAnimal.GetComponent<AnimalControl>().SetData(tempIndex);
            bornAnimal.Add(currentAnimal);
            animalHead.FindAnimal(tempIndex);
            freedBtn.enabled = false;
            tipFreed.gameObject.SetActive(true);
            tipFreed.fillAmount = 1;
            freedTime = 1;
            if (laveNumber <= 0)
            {
                //battleTip.SetActive(true);
                if (diamond >= 1)
                {
                    if (PlayerPrefs.GetString("InitBuy") == "")
                    {
                        PlayerPrefs.SetString("InitBuy", "InitBuy");
                        BuyAnimal();
                    }
                    else
                    {
                        buyTip.SetActive(true);
                    }
                }
            }
        }
        else
        {
            animalHead.FindAnimal(tempIndex);
        }
    }
    //模式切换
    private void SwitchMode()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isRotate = !isRotate;
        if (tempIndex != -10)
        {
            if (isRotate)
            {
                switchImage.sprite = rotate_animal;
                direction.GetChild(0).gameObject.SetActive(true);
                direction.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                switchImage.sprite = move_animal;
                direction.GetChild(0).gameObject.SetActive(false);
                direction.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    //判断是否落地
    public void JudeDetermine(Transform animal)
    {
        //if (!bornAnimal.Contains(animal))
        //    bornAnimal.Add(animal);
        Arrivals();
        SetPossess();
        if (maxScore < currScore)
        {
            maxScore = currScore;
            fractionText.text = ExcelTool.lang["recording"] + maxScore.ToString("F1") + "M";
            PlayerPrefs.SetFloat("MaxScore", maxScore);
            JudgHight();
            maxText.text = giftPanel.GetMaxHight(maxScore);
        }
        SetDistance();
        if (PlayerPrefs.GetString("BuildGuide4") == "")
        {
            int count = 0;
            for (int i = 0; i < bornAnimal.Count; i++)
            {
                if (bornAnimal[i].localPosition.y <= 0.5f)
                {
                    count++;
                }
            }
            if (count >= 4)
            {
                guidePanel.OpenGuide(4, false);
            }
        }
    }
    //方向判断
    private void DirectionSwitch()
    {
        direction.SetParent(tempAnimal, true);
        direction.localPosition = directionPoint;
        direction.localEulerAngles = Vector3.zero;
        direction.localScale = Vector3.one * 0.013f;
        direction.GetChild(2).gameObject.SetActive(true);
        if (isRotate)
        {
            direction.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            direction.GetChild(1).gameObject.SetActive(true);
        }
    }
    //雾效
    void FogHigh()
    {
        if (height >= 500)
        {
            RenderSettings.fog = false;
        }
        if (height < 100)
        {
            RenderSettings.fogDensity = 0.0005f;
        }
        else if (height >= 100 && height < 200)
        {
            RenderSettings.fogDensity = 0.0008f;
        }
        else if (height >= 200 && height < 300)
        {
            RenderSettings.fogDensity = 0.001f;
        }
        else if (height >= 300 && height < 400)
        {
            RenderSettings.fogDensity = 0.0015f;
        }
        else if (height >= 400 && height < 500)
        {
            RenderSettings.fogDensity = 0.002f;
        }
    }
    //获取最高高度
    float GetHighPoint()
    {
        float scaleSum = -10;
        float scale = 0;
        currScore = -10;
        AnimalControl animal;
        for (int i = 0; i < bornAnimal.Count; i++)
        {
            animal = bornAnimal[i].GetComponent<AnimalControl>();
            //scale = animal.maximum.y + bornAnimal[i].localPosition.y;
            scale = animal.sizeScale.GetComponent<Renderer>().bounds.size.y + bornAnimal[i].localPosition.y;
            if (animal.isDeath)
            {
                if (currScore <= scale)
                {
                    currScore = scale;
                }
            }
            if(scaleSum < scale)
            {
                scaleSum = scale;
            }
        }
        if (scaleSum < 0)
        {
            scaleSum = 0;
        }
        return scaleSum;
    }
    public void JudgHight()
    {
        if (maxScore >= giftPanel.maxDemand)
        {
            shareTip.SetActive(true);
            tipGift.SetActive(true);
            WinArmature();
            CreateTreasure();
        }
        else
        {
            if (currScore <= giftPanel.highDemand)
                tipGift.SetActive(false);
        }
    }
    //设置钻石
    public void SetDiamond(float num)
    {
        diamond += num;
        diamondText.text = diamond.ToString("F0");
        buyDiamText.text = diamondText.text;
        animalsPanel.HeadAnimals();
        PlayerPrefs.SetFloat("DiamondNumber", diamond);
        if(num >= 1)
        {
            if (diamond >= 1)
            {
                buyTip.SetActive(true);
            }
            else
            {
                buyTip.SetActive(false);
            }
            float sumDiam= PlayerPrefs.GetFloat("sumDiamond");
            sumDiam += num;
            PlayerPrefs.SetFloat("sumDiamond", sumDiam);
        }
    }
    //UI位置更新
    public void UpdataPoint()
    {
        if (tempAnimal)
        {
            if (cameraControl.isRotate)
            {
                tempAnimal.localEulerAngles = initAnimal.eulerAngles;
                animalImage.transform.position = m_Camera.WorldToScreenPoint(tempAnimal.position);
            }
            else
            {
                isMove = false;
                tempAnimal.localPosition = initAnimal.position;
                tempAnimal.localEulerAngles = initAnimal.eulerAngles;
                ImpactDetection();
            }
        }
    }
    //生成动物影像
    public void CreateShadow(int index, float scale)
    {
        if (tempIndex != index)
        {
            if (tempIndex != -10)
            {
                tempAnimal.gameObject.SetActive(false);
                tempAnimal = null;
            }
            animalHead.IsTip(index);
            animalHigh = (scale * 0.015f);
            animalHead.SetBgImage(index);
            tempIndex = index; tempScale = scale;
            tempAnimal = ObjectPool.Instance.CreateObject(animalPrefab[index].name, animalPrefab[index].gameObject).transform;
            tempAnimal.GetComponent<AnimalControl>().FreedObject(false);
            tempAnimal.SetParent(BasePlane, false);
            tempAnimal.localScale = Vector3.one * scale;
            if (!isMove)
            {
                tempPoint = initAnimal.position;
                tempAngles = initAnimal.eulerAngles;
            }
            tempPoint.y = height;
            tempAnimal.localPosition = tempPoint;
            tempAnimal.localEulerAngles = tempAngles;
            DirectionSwitch();
            Arrivals();
        }
        else
        {
            tempAnimal.localPosition = initAnimal.position;
            tempAnimal.localEulerAngles = initAnimal.eulerAngles;
        }

    }
    //判断是否到达
    public void Arrivals()
    {
        height = GetHighPoint() + animalHigh * 2;
        cameraControl.transform.DOLocalMoveY(height + 7, 0.3f);
        direction.GetChild(2).DOScaleY(height + 10, 0.3f);
        tempAnimal.DOLocalMoveY(height, 0.3f);
        Invoke("ImpactDetection", 0.4f);
        SetDistance();
        FogHigh();
    }
    //点击回收
    public void RecycleAnimal(int index, Transform animal)
    {
        animalHead.SetHead(index, 1);
        bornAnimal.Remove(animal);
        laveNumber += 1;
        buildText.text = laveNumber.ToString("F0");
        freedTip.SetActive(laveNumber > 0);
        Arrivals();
        if (animal == currentAnimal)
        {
            currentAnimal = null;
        }
    }
    //复活回收
    public void Resurrection()
    {
        int ran = 10;
        if (ran > deathAnimal.Count)
        {
            ran = deathAnimal.Count;
        }
        if (ran > 0)
        {
            for (int i = ran - 1; i >= 0; i--)
            {
                animalHead.SetHead(deathAnimal[i], 1);
                deathAnimal.Remove(deathAnimal[i]);
            }
            animalHead.IsTip(tempIndex);
            SetPossess();
        }
    }
    void SetPossess()
    {
        laveNumber = sumAnimal - deathAnimal.Count - bornAnimal.Count;
        deathsText.text = deathAnimal.Count.ToString();
        buildText.text = laveNumber.ToString("F0");
        freedTip.SetActive(laveNumber > 0);
    }
    //死亡移除
    public void DeathRecovery(Transform anim, int index)
    {
        if (bornAnimal.Contains(anim))
        {
            bornAnimal.Remove(anim);
        }
        deathAnimal.Add(index);
        SetPossess();
        if (anim == currentAnimal)
        {
            currentAnimal = null;
        }
        if (deathAnimal.Count >= 5)
        {
            guidePanel.OpenGuide(6, false);
        }
        if (deathAnimal.Count >= 10 && laveNumber <= 0)
        {
            guidePanel.OpenGuide(7, false);
        }
    }
    //清零所有
    public void ResetAnimal()
    {
        for (int i = 0; i < bornAnimal.Count; i++)
        {
            int index = bornAnimal[i].GetComponent<AnimalControl>().indexAnimal;
            bornAnimal[i].gameObject.SetActive(false);
            animalHead.SetHead(index, 1);
            EffectGenerator.Instance.EenemyPixel(bornAnimal[i], index, 10);
        }
        for (int i = 0; i < deathAnimal.Count; i++)
        {
            animalHead.SetHead(deathAnimal[i], 1);
        }
        deathAnimal.Clear();
        bornAnimal.Clear();
        if (tempIndex != -10)
        {
            tempAnimal.DOLocalMoveY(3, 0.5f);
        }
        ImpactDetection();
        cameraControl.transform.DOLocalMoveY(10, 0.5f);
        SetPossess();
        midText.text = 0.ToString();
        minText.text = 0.ToString();
        animalHead.IsTip(tempIndex);
    }
    //获取点击对象
    public void ImpactDetection()
    {
        if (tempAnimal)
        {
            Ray ray = new Ray(tempAnimal.position, Vector3.down);
            RaycastHit hit;
            int mask = LayerMask.GetMask("Turret","Enemy");
            if (Physics.Raycast(ray, out hit,1000, mask))
            {
                //print("击中目标");
                Vector3 point = hit.point;
                point.y += 0.2f;
                direction.GetChild(3).position = point;
            }
            animalImage.transform.position = m_Camera.WorldToScreenPoint(tempAnimal.position);
        }
    }
    //点击UI
    public bool IsPointerOverGameObject(Vector2 screenPosition)
    {
        if (Input.GetMouseButtonDown(0))
        {
            //实例化点击事件
            PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            //将点击位置的屏幕坐标赋值给点击事件
            eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            //向点击处发射射线
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            return results.Count > 0;
        }
        return false;
    }
}
