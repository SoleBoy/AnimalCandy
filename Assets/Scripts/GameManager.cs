using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    public static int multiple = 10000;
    private DateTime today;//今日日期
    private DateTime signData;//上次签到日期
    private string lastData;//最终签到时间

    private GameObject tipPanel;
    private GameObject promptPanel;
    private Transform parent;
    public SkipPanel skipPanel;

    public int adsCount;
    public int sceneIndex;
    private int adsIndex;
    private int tableIndex;
    public string modeSelection;
    public bool isCansign;
    public bool isDay;
    private bool isNet;
    private bool isAds;
    private bool isShare;
    private bool isOpenad;
    
    private Action<int> admobAdsCount;
    public Action<int> AdmobAdsCount { get => admobAdsCount; set => admobAdsCount = value; }
    private void Awake()
    {
        if (instance == null)
        {
            //PlayerPrefs.DeleteAll();
            GetAttAuth();
            instance = this;
            Application.targetFrameRate = 30;
            tipPanel = Resources.Load<GameObject>("UI/TipPanel");
            promptPanel = Resources.Load<GameObject>("UI/PromptPanel");
            GetComponent<AudioManager>().Init();
            GetComponent<ExcelTool>().Init();
            GetComponent<GameCenterManager>().Init();
            GetComponent<Purchaser>().Init();
            skipPanel = transform.Find("Canvas/SkipPanel").GetComponent<SkipPanel>();
            adsCount = PlayerPrefs.GetInt("NumberAds");
            parent = transform.Find("Canvas");
            skipPanel.Init();
            ScreenSize();
            PlayerPrefs.SetString("OpenPlacard", "true");
            DontDestroyOnLoad(gameObject);
            if(PlayerPrefs.GetString("InitLogin2") == "")
            {
                PlayerPrefs.SetString("InitLogin2", "InitLogin");
                PlayerPrefs.SetInt("Construction0", PlayerPrefs.GetInt("Construction0")+5);//小黄鸭
                PlayerPrefs.SetInt("Construction1", PlayerPrefs.GetInt("Construction1")+3);//小黑狗
                PlayerPrefs.SetInt("Construction2", PlayerPrefs.GetInt("Construction2")+2);
                PlayerPrefs.SetInt("Construction3", PlayerPrefs.GetInt("Construction3")+1);//松鼠
                PlayerPrefs.SetInt("Construction4", PlayerPrefs.GetInt("Construction4")+1);//蝙蝠
                PlayerPrefs.SetInt("Construction5", PlayerPrefs.GetInt("Construction5")+1);//老鹰
                PlayerPrefs.SetInt("Construction6", PlayerPrefs.GetInt("Construction6")+1);//树袋熊
                PlayerPrefs.SetFloat("DiamondNumber", PlayerPrefs.GetFloat("DiamondNumber") + 5);
            }
        }
        else
        {
            Destroy(this);
        }
    }
    //切换场景
    public void SwitchScene(string scene,string mode)
    {
        HideBanner();
        skipPanel.OpenPnael(scene,mode);
        AudioManager.Instance.CutBgMusic("loading");
    }
    public void CloseHelp(string messg)
    {
        modeSelection = messg;
    }
    //提示面板
    public void CloneTip(string mes)
    {
        if (!isAds)
        {
            var tip = ObjectPool.Instance.CreateObject(tipPanel.name, tipPanel);
            tip.transform.SetParent(parent, false);
            tip.GetComponent<TipPanel>().TipMessage(mes);
            isAds = true; Invoke("CloseTip", 0.1f);
        }
        else
        {
            StartCoroutine(DelyTip(mes,0, 0,false));
        }
    }
    //奖励次数
    public void ClonePrompt(float mes,int index)
    {
        if (!isAds)
        {
            var tip = ObjectPool.Instance.CreateObject(promptPanel.name, promptPanel);
            tip.transform.SetParent(parent, false);
            tip.GetComponent<SignPrompt>().TipMessage(mes, index);
            isAds = true; Invoke("CloseTip", 0.1f);
        }
        else
        {
            StartCoroutine(DelyTip("",mes,index, true));
        }
    }
    private IEnumerator DelyTip(string messg,float mes, int index,bool isTip)
    {
        yield return new WaitForSeconds(0.2f);
        if(isTip)
        {
            if(!isAds)
            {
                var tip = ObjectPool.Instance.CreateObject(promptPanel.name, promptPanel);
                tip.transform.SetParent(parent, false);
                tip.GetComponent<SignPrompt>().TipMessage(mes, index);
                isAds = true; Invoke("CloseTip", 0.1f);
            }
            else
            {
                StartCoroutine(DelyTip("", mes, index, true));
            }
        }
        else
        {
            if (!isAds)
            {
                var tip = ObjectPool.Instance.CreateObject(tipPanel.name, tipPanel);
                tip.transform.SetParent(parent, false);
                tip.GetComponent<TipPanel>().TipMessage(messg);
                isAds = true; Invoke("CloseTip", 0.1f);
            }
            else
            {
                StartCoroutine(DelyTip(messg, 0, 0, false));
            }
        }
    }
    private void CloseTip()
    {
        isAds = false;
    }
    //广告次数
    public void SetAdsNumber(int count)
    {
        adsCount += count;
        PlayerPrefs.SetInt("NumberAds", adsCount);
        if(AdmobAdsCount != null)
        {
            AdmobAdsCount.Invoke(adsCount);
        }
    }
    //分享次数
    public void ShareEvent()
    {
        if(!isShare)
        {
            isShare = true;
            Invoke("ShareCallback",5);
        }
    }
    private void ShareCallback()
    {
        isShare = false;
        int number = PlayerPrefs.GetInt("ShareEvent");
        number += 1;
        ClonePrompt(1, 1);
        PlayerPrefs.SetInt("ShareEvent", number);
        if (UIManager.Instance)
            UIManager.Instance.SetStar(1);
        if (UIBase.Instance)
            UIBase.Instance.SetDiamond(1);
    }
    //过关次数
    public void PassNumber()
    {
        adsIndex += 1;
        if(adsIndex >= 6)
        {
            adsIndex = 0;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                bool isReward = false;
                if (RequestRewardedAd(AdsType.restore).IsLoaded())
                {
                    UserChoseToWatchAd(AdsType.restore);
                    AdmobRewardCB = delegate
                    {
                        
                    };
                    AdmobRewardClose = delegate
                    {
                        isReward = true;
                    };
                    AdmobClose = delegate
                    {
                        if(isReward)
                        {
                            isReward = false;
                            ClonePrompt(1, 1);
                            UIManager.Instance.SetStar(1);
                        }
                        CreateModel.Instance.AddLevel();
                        RequestRewardBasedVideo(AdsType.restore);
                    };
                    return;
                }
            }
        }
        CreateModel.Instance.AddLevel();
    }

    private void Update()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable && isNet == false)
        {
            isNet = true;
            InitialAds();
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable && isNet)
        {
            isNet = false;
        }
    }
    //后台切换
    private void OnApplicationPause(bool focus)
    {
        bool isHook = UIManager.Instance;
        if (focus)
        {
            if(isHook)
            {
                CreateModel.Instance.SaveData();
            }
            else if(UIBase.Instance)
            {
                UIBase.Instance.SaveData();
            }
        }
        else
        {
            if (isOpenad)
            {
                isOpenad = false;
                return;
            }
            if (isHook)
            {
                if (PlayerPrefs.GetString("GoEvaluate") == "true")
                {
                    PlayerPrefs.SetString("GoEvaluate", "fals");
                    ClonePrompt(50, 1);
                    UIManager.Instance.SetStar(50);
                }
            }
            tableIndex += 1;
            if(tableIndex >= 4)
            {
                tableIndex = 0;
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    bool isReward = false;
                    UserChoseToWatchAd(AdsType.restore);
                    AdmobRewardCB = delegate
                    {
                        if (isHook)
                            UIManager.Instance.isTime = true;
                    };
                    AdmobRewardClose = delegate
                    {
                        isReward = true;
                    };
                    AdmobClose = delegate
                    {
                        if (isHook)
                        {
                            UIManager.Instance.BackgroundCut();
                            UIManager.Instance.DetectionPanel();
                        }
                        if(isReward)
                        {
                            isReward = false;
                            if (UIManager.Instance)
                            {
                                UIManager.Instance.SetStar(1);
                            }
                            else if (UIBase.Instance)
                            {
                                UIBase.Instance.SetDiamond(1);
                            }
                            else
                            {
                                ChoiceControl.Instance.SetDiamond(1);
                            }
                            ClonePrompt(1, 1);
                        }
                        RequestRewardBasedVideo(AdsType.restore);
                    };
                }
                else
                {
                    if (isHook)
                    {
                        UIManager.Instance.BackgroundCut();
                    }
                    if (UIManager.Instance)
                    {
                        UIManager.Instance.SetStar(1);
                    }
                    else if (UIBase.Instance)
                    {
                        UIBase.Instance.SetDiamond(1);
                    }
                    else
                    {
                        ChoiceControl.Instance.SetDiamond(1);
                    }
                    ClonePrompt(1, 1);
                }
            }
        }
    }
    //分辨率
    private void ScreenSize()
    {
        float beliel = (float)Screen.height / Screen.width;
        if(beliel >= 1.3f && beliel < 1.5f)
        {
            sceneIndex = 0;
        }
        else if(beliel >= 1.5f && beliel < 1.7f)
        {
            sceneIndex = 1;
        }
        else if (beliel >= 1.7f && beliel < 1.9f)
        {
            sceneIndex = 2;
        }
        else if (beliel >= 1.9f)
        {
            sceneIndex = 3;
        }
    }
    //Unity 调用IOS ATT授权弹窗
    private void GetAttAuth()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            int curStatus = ATTAuth.GetAppTrackingAuthorizationStatus();
            if (curStatus == 0)
            {
                ATTAuth.RequestTrackingAuthorizationWithCompletionHandler((status) =>
                {
                    Debug.Log("ATT status :" + status);
                });
            }
        }
    }
    #region 签到时间判断
    public void InitTimeData()
    {
        lastData = PlayerPrefs.GetString("AbortTime");
        if (lastData == "")
        {
            lastData= DateTime.Now.AddYears(3).ToString();
            PlayerPrefs.SetString("AbortTime", lastData);
        }
        signData = DateTime.Parse(PlayerPrefs.GetString("SignData", DateTime.MinValue.ToString()));
        isCansign = Convert.ToDateTime(lastData) >= DateTime.Now;
        if (isCansign)
        {
            today = DateTime.Now.AddDays(1);
            isDay = IsOneDay();
        }
        else
        {
            isDay = false;
        }
    }
    //刷新日期
    public void RefreshDate()
    {
        signData = today;
        //更新存档
        PlayerPrefs.SetString("SignData", today.ToString());
    }
    private bool IsOneDay()
    {
        if (signData.Year == today.Year && signData.Month == today.Month && signData.Day == today.Day)
        {
            return false;
        }
        if (DateTime.Compare(signData, today) < 0)
        {
            return true;
        }
        return false;
    }
    #endregion
    #region 广告
    Action _admobRewardCB;
    Action _admobRewardClose;
    Action _admobClose;
    public System.Action AdmobRewardCB
    {
        set
        {
            _admobRewardCB = value;
        }
    }
    public System.Action AdmobRewardClose
    {
        set
        {
            _admobRewardClose = value;
        }
    }
    public System.Action AdmobClose
    {
        set
        {
            _admobClose = value;
        }
    }
    public RewardedAd attackRewardedAd;
    public RewardedAd earRewardedAd;
    public RewardedAd autoRewardedAd;
    public RewardedAd sweetsRewardedAd;
    public RewardedAd dimRewardedAd;
    public RewardedAd skillRewardedAd;
    public RewardedAd resetRewardedAd;
    public RewardedAd restorRewardedAd;
    public BannerView bannerView;
    private bool isRewarded;
    private bool isBanner;
    private bool isFailed;
    void InitialAds()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("初始加载广告");
            isFailed = true;
            //ca-app-pub-3940256099942544~1458002511
            MobileAds.Initialize(AdsConfigure.ADMOB_APPID);

            attackRewardedAd=CreateAndLoadRewardedAd(AdsConfigure.ADMOB_ATKREWARD);
            earRewardedAd=CreateAndLoadRewardedAd(AdsConfigure.ADMOB_EARREWARD);
            autoRewardedAd=CreateAndLoadRewardedAd(AdsConfigure.ADMOB_AUTOREWARD);
            sweetsRewardedAd=CreateAndLoadRewardedAd(AdsConfigure.ADMOB_SWEETSREWARD);
            dimRewardedAd=CreateAndLoadRewardedAd(AdsConfigure.ADMOB_DIAMREWARD);
            skillRewardedAd=CreateAndLoadRewardedAd(AdsConfigure.ADMOB_Skill);
            resetRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_RESET);
            restorRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_RESTORE);
            attackRewardedAd.adsFailed = 0;
            earRewardedAd.adsFailed = 0;
            autoRewardedAd.adsFailed = 0;
            sweetsRewardedAd.adsFailed = 0;
            dimRewardedAd.adsFailed = 0;
            skillRewardedAd.adsFailed = 0;
            resetRewardedAd.adsFailed = 0;
            restorRewardedAd.adsFailed = 0;
            //RequestBanner();
        }
    }
    
    public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);
        //成功加载ad请求时调用。
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        //当ad请求加载失败时调用。
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        //在显示广告时调用。
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        //当广告请求未能显示时调用。
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        //当用户因与广告互动而被奖励时调用。
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        //当广告关闭时就会调用。
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        //请求广告次数
        //rewardedAd.adsFailed = 0;
        return rewardedAd;
    }
    public void RequestRewardBasedVideo(AdsType type)
    {
        AdRequest request = new AdRequest.Builder().Build();
        switch (type)
        {
            case AdsType.attack:
                if (this.attackRewardedAd != null)
                {
                    ClearEvent(attackRewardedAd);
                    this.attackRewardedAd = null;
                }
                attackRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_ATKREWARD);
                break;
            case AdsType.earnings:
                if (earRewardedAd != null)
                {
                    ClearEvent(earRewardedAd);
                    earRewardedAd = null;
                }
                this.earRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_EARREWARD);
                break;
            case AdsType.auto:
                if (autoRewardedAd != null)
                {
                    ClearEvent(autoRewardedAd);
                    autoRewardedAd = null;
                }
                this.autoRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_AUTOREWARD);
                break;
            case AdsType.sweets:
                if (sweetsRewardedAd != null)
                {
                    ClearEvent(sweetsRewardedAd);
                    sweetsRewardedAd = null;
                }
                this.sweetsRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_SWEETSREWARD);
                break;
            case AdsType.diamond:
                if (dimRewardedAd != null)
                {
                    ClearEvent(dimRewardedAd);
                    dimRewardedAd = null;
                }
                this.dimRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_DIAMREWARD);
                break;
            case AdsType.skill:
                if (skillRewardedAd != null)
                {
                    ClearEvent(skillRewardedAd);
                    skillRewardedAd = null;
                }
                this.skillRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_Skill);
                break;
            case AdsType.reset:
                if (resetRewardedAd != null)
                {
                    ClearEvent(resetRewardedAd);
                    resetRewardedAd = null;
                }
                this.resetRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_RESET);
                break;
            case AdsType.restore:
                if (restorRewardedAd != null)
                {
                    ClearEvent(restorRewardedAd);
                    restorRewardedAd = null;
                }
                this.restorRewardedAd = CreateAndLoadRewardedAd(AdsConfigure.ADMOB_RESTORE);
                break;
            default:
                break;
        }
    }
    void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        StartCoroutine(WaitThreadLoaded());
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        StartCoroutine(RequestAgain(sender));
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        StartCoroutine(WaitThreadOpening());
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        StartCoroutine(WaitThreadClosed());
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    void HandleUserEarnedReward(object sender, Reward args)
    {
        StartCoroutine(WaitThreadReward());
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }
    public bool IsAdvertisement(AdsType type)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            CloneTip(ExcelTool.lang["tip4"]);
            return false;
        }
        if(RequestRewardedAd(type).IsLoaded())
        {
            return true;
        }
        CloneTip(ExcelTool.lang["tip6"]);
        return false;
    }
    private IEnumerator WaitThreadLoaded()
    {
        yield return new WaitForEndOfFrame();
        isFailed = false;
    }
    private IEnumerator WaitThreadOpening()
    {
        //等待一帧 
        yield return new WaitForEndOfFrame();
        isOpenad = true;
        AudioManager.Instance.PauseSource();
        if (_admobRewardCB != null)
        {
            _admobRewardCB.Invoke();
            _admobRewardCB = null;
        }
    }
    private IEnumerator WaitThreadClosed()
    {
        //等待一帧 
        yield return new WaitForEndOfFrame();
        AudioManager.Instance.RestoreSource();
        if (isRewarded)
        {
            isRewarded = false;
            SetAdsNumber(1);
            ClonePrompt(1, 2);
        }
        if (_admobClose != null)
        {
            _admobClose.Invoke();
            _admobClose = null;
        }
    }
    private IEnumerator WaitThreadReward()
    {
        //等待一帧 
        yield return new WaitForEndOfFrame();
        if (_admobRewardClose != null)
        {
            isRewarded = true;
            _admobRewardClose.Invoke();
            _admobRewardClose = null;
        }
    }
    private IEnumerator RequestAgain(object sender)
    {
        yield return new WaitForEndOfFrame();
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            CloneTip(ExcelTool.lang["tip4"]);
        }
        else
        {
            yield return new WaitForSeconds(2);
            RewardedAd ad = (RewardedAd)sender;
            ad.adsIndex = 0;
            if (ad.adsFailed <= 3)
            {
                ad.adsFailed += 1;
                if (isFailed && ad.adsFailed >= 3)
                {
                    isFailed = false;
                    InitialAds();
                }
                else
                {
                    //Debug.Log("请求广告次数");
                    AdRequest request = new AdRequest.Builder().Build();
                    ad.LoadAd(request);
                }
            }
        }
    }

    public void UserChoseToWatchAd(AdsType type)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            CloneTip(ExcelTool.lang["tip4"]);
            return;
        }
        if (RequestRewardedAd(type).IsLoaded())
        {
            RequestRewardedAd(type).Show();
        }
        else
        {
            CloneTip(ExcelTool.lang["tip6"]);
            RewardedAd ad = RequestRewardedAd(type);
            if(ad.adsIndex < 1)
            {
                AdRequest request = new AdRequest.Builder().Build();
                ad.LoadAd(request);
                ad.adsIndex += 1;
            }
           
        }
    }
    public RewardedAd RequestRewardedAd(AdsType type)
    {
        switch (type)
        {
            case AdsType.attack:
                return attackRewardedAd;
            case AdsType.earnings:
                return earRewardedAd;
            case AdsType.auto:
                return autoRewardedAd;
            case AdsType.sweets:
                return sweetsRewardedAd;
            case AdsType.diamond:
                return dimRewardedAd;
            case AdsType.skill:
                return skillRewardedAd;
            case AdsType.reset:
                return resetRewardedAd;
            case AdsType.restore:
                return restorRewardedAd;
            default:
                break;
        }
        return null;
    }
    void ClearEvent(RewardedAd rewarded)
    {
        rewarded.OnAdLoaded -= HandleRewardedAdLoaded;
        rewarded.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        rewarded.OnAdOpening -= HandleRewardedAdOpening;
        rewarded.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        rewarded.OnUserEarnedReward -= HandleUserEarnedReward;
        rewarded.OnAdClosed -= HandleRewardedAdClosed;
    }
    //横幅广告
    private void RequestBanner()
    {
        Debug.Log("加载Banner广告");
        if (isBanner) return;

        //AdSize adSize = new AdSize(Screen.width,65);
        bannerView = new BannerView(AdsConfigure.ADMOB_Banners, AdSize.SmartBanner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        StartCoroutine(WaitThreadBanner());
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    private IEnumerator WaitThreadBanner()
    {
        //等待一帧 
        yield return new WaitForEndOfFrame();
        isBanner = true;
        bannerView.Hide();
    }

    public void ShowBanner()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }
        if (isBanner)
        {
            //UIManager.Instance.bottomImage.SetActive(true);
            //bannerView.Show();
        }
    }
    public void HideBanner()
    {
        if (isBanner)
        {
            //UIManager.Instance.bottomImage.SetActive(true);
            //bannerView.Hide();
        }
    }
    #endregion
}



public interface WeatherMaager
{
    void Weather(bool iswather, bool ishide);
}