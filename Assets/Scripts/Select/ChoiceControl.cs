using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoiceControl : MonoBehaviour
{
    private static ChoiceControl instance;
    public static ChoiceControl Instance { get => instance; }

    public GameObject[] weather;
    public PlacardPanel placard;
    public StorePanel storeSelect;
    public LangePanel langePanel;
    public SettingsPanel settPanel;

    public Text goldText;
    public Text diamondText;
    public Text adsText;
    public Text levelText;
    public Text recordText;

    public Text wordName;
    public Text fitText;

    public GameObject lockTime;
    public GameObject tipTime;

    public GameObject lockHook;
    public GameObject tipHook;

    public GameObject wordLock;
    public GameObject wordTip;
    public CameraControl cameraControl;

    private float hangIndex;
    private float roudeIndex;

    private float goldNumber;
    private float diamNumber;

    private bool isUI;
    private void Awake()
    {
        instance = this;
        goldNumber = PlayerPrefs.GetFloat("GoldNumber", 200);
        diamNumber = PlayerPrefs.GetFloat("DiamondNumber");
        hangIndex = PlayerPrefs.GetFloat("MaxLevel");
        //roudeIndex = PlayerPrefs.GetFloat("RouteMaxLevel");
        int lineCount = ExcelTool.Instance.lines.Count;
        for (int i = 1; i < lineCount; i++)
        {
            if (PlayerPrefs.GetString("Simple" + (i)) != "")
            {
                roudeIndex += 1;
            }
            if (PlayerPrefs.GetString("Ordinary" + (i)) != "")
            {
                roudeIndex += 1;
            }
            if (PlayerPrefs.GetString("Difficult" + (i)) != "")
            {
                roudeIndex += 1;
            }
        }
        levelText.text = (roudeIndex + hangIndex).ToString("F0");
        GameManager.Instance.AdmobAdsCount = null;
        GameManager.Instance.AdmobAdsCount = AdsCount;
        GameManager.Instance.skipPanel.ClosePanel();
        placard.Init();
        storeSelect.Init();
        settPanel.Init();
    }
    private void Start()
    {
        SetGold(0);
        SetDiamond(0);
        GameManager.Instance.SetAdsNumber(0);
        AudioManager.Instance.CutBgMusic("waterweather_1");
        int ran = Random.Range(0, weather.Length);
        weather[ran].SetActive(true);
        weather[ran].GetComponent<WeatherMaager>().Weather(true, false);
        if (hangIndex >= 2)
        {
            lockTime.gameObject.SetActive(false);
            tipTime.gameObject.SetActive(true);
        }
        else
        {
            tipTime.gameObject.SetActive(false);
            lockTime.gameObject.SetActive(true);
        }

        if (hangIndex >= 1)
        {
            wordLock.gameObject.SetActive(false);
            wordTip.gameObject.SetActive(true);
        }
        else
        {
            wordTip.gameObject.SetActive(false);
            wordLock.gameObject.SetActive(true);
        }

        if (roudeIndex >= 2)
        {
            lockHook.gameObject.SetActive(false);
            tipHook.gameObject.SetActive(true);
        }
        else
        {
            tipHook.gameObject.SetActive(false);
            lockHook.gameObject.SetActive(true);
        }
        if(PlayerPrefs.GetString("HangModel") == "true")
        {
            PlayerPrefs.SetString("HangModel", "model");
            GameManager.Instance.CloneTip(ExcelTool.lang["tip33"]);
        }
        ExcelTool.LanguageEvent += CutLang;
        langePanel.Init();
    }
    private void CutLang()
    {
        wordName.text = ExcelTool.lang["wordta"];
        fitText.text = ExcelTool.lang["fitfire"];
        recordText.text = string.Format("{0}：{1:F1}M", ExcelTool.lang["recording"], PlayerPrefs.GetFloat("MaxScore"));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isUI = IsPointerOverGameObject(Input.mousePosition);
        }
           
        if(Input.GetMouseButtonUp(0))
        {
            isUI = false;
        }

        if(!isUI)
        {
            cameraControl.UpdataData();
        }
    }
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
    //关卡模式
    public void LevelPattern()
    {
        if (roudeIndex >= 2)
        {
            GameManager.Instance.SwitchScene("main", "hook");
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }

    //路线模式
    public void RoutePattern()
    {
        GameManager.Instance.SwitchScene("main", "roude");
    }

    //时间模式
    public void TimePattern()
    {
        if (hangIndex >= 2)
        {
            GameManager.Instance.SwitchScene("main", "time");
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }

    //寻宝模式
    public void TreasurePattern()
    {
        if (hangIndex >= 1)
        {
            GameManager.Instance.SwitchScene("Build", "");
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
    //语言
    public void OpenLang()
    {
        AudioManager.Instance.PlayTouch("open_1");
        langePanel.gameObject.SetActive(true);
    }
    //商店
    public void OpenPanel()
    {
        AudioManager.Instance.PlayTouch("open_1");
        storeSelect.OpenPanel(0);
    }
    ////排行榜
    //public void ClickRanking()
    //{
    //    AudioManager.Instance.PlayTouch("other_1");
    //    GameManager.Instance.GetComponent<GameCenterManager>().ShowLeaderboard();
    //}
    //设置
    public void OpenSettings()
    {
        AudioManager.Instance.PlayTouch("open_1");
        settPanel.gameObject.SetActive(true);
    }
    //金币
    public void SetGold(float num)
    {
        goldNumber += num;
        goldText.text = GoldDisplay(goldNumber);
        storeSelect.goldText.text = goldText.text;
        PlayerPrefs.SetFloat("GoldNumber", goldNumber);
    }
    //钻石
    public void SetDiamond(float num)
    {
        diamNumber += num;
        diamondText.text = GoldDisplay(diamNumber);
        storeSelect.diamText.text = diamondText.text;
        PlayerPrefs.SetFloat("DiamondNumber", diamNumber);
    }
    //广告
    private void AdsCount(int adsCount)
    {
        adsText.text = adsCount.ToString("F0");
        storeSelect.adsText.text = adsText.text;
    }
    // K M B T
    public string GoldDisplay(float num)
    {
        if (num >= 1000 && num < 1000000)
        {
            return string.Format("{0:F1}k", num * 0.001);
        }
        else if (num >= 1000000 && num < 1000000000)
        {
            return string.Format("{0:F1}m", num * 0.000001);
        }
        else if (num >= 1000000000)
        {
            return string.Format("{0:F1}b", num * 0.000000001f);
        }
        return string.Format("{0:F0}", Mathf.Floor(num));
    }
}
