using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteMapPanel : MonoBehaviour
{
    public MissionRoute mission;
    public Sprite normal;
    public Sprite highlight;
    public Sprite graylock;

    private Transform roudeParent;
    private GameObject roudePrefab;
    private GameObject ordinaryTip;
    private GameObject ordinaryLock;
    private GameObject difficultTip;
    private GameObject difficultLock;

    private Text passText;
    private Text starText;
    private Text ordinaryText;
    private Text difficultText;
    private Text tipRoudeText;
    private Button closeBtn;
    private Button simpleBtn;
    private Button ordinaryBtn;
    private Button difficultBtn;
    private Image simpleImage;
    private Image ordinaryImage;
    private Image difficultImage;
    private Image simpleLow;
    private Image ordinaryLow;
    private Image difficultLow;

    private float simpleCount;
    private float ordinaryCount;
    private float difficultCount;
    private string modelMessg = "Simple";
    private string passMessg;
    private Color imageColor;

    private List<RouteItem> routeItems = new List<RouteItem>();
    public void Init()
    {
        roudePrefab = Resources.Load<GameObject>("UI/RouteItem");
        ordinaryTip = transform.Find("Ordinary/Tip").gameObject;
        ordinaryLock = transform.Find("Ordinary/Lock").gameObject;
        difficultTip = transform.Find("Difficult/Tip").gameObject;
        difficultLock = transform.Find("Difficult/Lock").gameObject;
        roudeParent = transform.Find("Scroll View/Content");
        starText = transform.Find("Star/Text").GetComponent<Text>();
        passText = transform.Find("Pass/Text").GetComponent<Text>();
        tipRoudeText = transform.Find("InfoText").GetComponent<Text>();
        ordinaryText = transform.Find("InfoText2").GetComponent<Text>();
        difficultText = transform.Find("InfoText3").GetComponent<Text>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        simpleBtn = transform.Find("Simple/SimpleBtn").GetComponent<Button>();
        ordinaryBtn = transform.Find("Ordinary/OrdinaryBtn").GetComponent<Button>();
        difficultBtn = transform.Find("Difficult/DifficultBtn").GetComponent<Button>();
        simpleImage = simpleBtn.GetComponent<Image>();
        ordinaryImage = ordinaryBtn.GetComponent<Image>();
        difficultImage = difficultBtn.GetComponent<Image>();
        simpleLow = transform.Find("Simple").GetComponent<Image>();
        ordinaryLow = transform.Find("Ordinary").GetComponent<Image>();
        difficultLow = transform.Find("Difficult").GetComponent<Image>();
        closeBtn.onClick.AddListener(HideMap);
        simpleBtn.onClick.AddListener(SimpleModel);
        ordinaryBtn.onClick.AddListener(OrdinaryModel);
        difficultBtn.onClick.AddListener(DifficultModel);
        int high = ExcelTool.Instance.lines.Count;
        for (int i = 0; i < high; i++)
        {
            var item = Instantiate(roudePrefab);
            item.transform.SetParent(roudeParent);
            item.transform.localPosition = Vector3.zero;
            item.transform.localEulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.GetComponent<RouteItem>().SetGrade(i);
            routeItems.Add(item.GetComponent<RouteItem>());
        }
        high = (int)Mathf.Ceil(high *0.25f);
        Vector2 vector = roudeParent.GetComponent<RectTransform>().sizeDelta;
        vector.y = high * 100 + high * 20 + 15;
        roudeParent.GetComponent<RectTransform>().sizeDelta = vector;
        ExcelTool.LanguageEvent += CutLang;
        imageColor = Color.white;
        imageColor.a = 0.5f;
        if(PlayerPrefs.GetString("RouteFirst") == "")
        {
            Checkpoint();
            SetFirst();
        }
        else
        {
            OpenPanel();
        }
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        Checkpoint();
        UIManager.Instance.timeCurret = 0;
        UIManager.Instance.OpenMission();
        GameManager.Instance.ShowBanner();
    }
    public void SelectLevel(float level)
    {
        GameManager.Instance.HideBanner();
        CreateModel.Instance.ModelDifficulty(modelMessg, level);
        CreateModel.Instance.ChangeScene(level);
        mission.SetMission(ExcelTool.Instance.lines[level.ToString()], modelMessg);
        passMessg = string.Format("{0}{1}", modelMessg, level);
        if (level == 0)
        {
            mission.gameObject.SetActive(false);
            UIManager.Instance.turretPanel.TutorialsCarry();
            UIManager.Instance.skillPanel.TutorialsCarry();
            Camera.main.GetComponent<CameraView>().InitMove();
            UIManager.Instance.Tutorials(false);
            CreateModel.Instance.routeMode.LevelTutorial(true);
        }
        else
        {
            UIManager.Instance.Tutorials(true);
            CreateModel.Instance.routeMode.LevelTutorial(false);
        }
        gameObject.SetActive(false);
    }
    public void PassLevel()
    {
        if(PlayerPrefs.GetString(passMessg) == "")
        {
            PlayerPrefs.SetString(passMessg, "true");
            CreateModel.Instance.MaxPass(1,true);
        }
    }
    private void SetFirst()
    {
        mission.SetMission(ExcelTool.Instance.lines["0"], modelMessg);
        mission.gameObject.SetActive(false);
        CreateModel.Instance.ModelDifficulty(modelMessg, 0);
        CreateModel.Instance.routeMode.LevelTutorial(true);
        UIManager.Instance.Tutorials(false);
        UIManager.Instance.turretPanel.TutorialsCarry();
        UIManager.Instance.skillPanel.TutorialsCarry();
        PlayerPrefs.SetString("RouteFirst", "first");
        Camera.main.GetComponent<CameraView>().InitMove();
        passMessg = string.Format("{0}{1}", modelMessg,0);
        if (PlayerPrefs.GetString("LangePanel") == "")
        {
            PlayerPrefs.SetString("LangePanel", "lange");
            UIManager.Instance.langePanel.gameObject.SetActive(true);
        }
    }
    private void CutLang()
    {
        if (simpleCount < 5)
        {
            ordinaryText.text = ExcelTool.lang["lockroude1"];
        }
        if (ordinaryCount < 8)
        {
            difficultText.text = ExcelTool.lang["lockroude2"];
        }
        routeItems[0].SetLanguage();
    }
    private void Checkpoint()
    {
        int perfect = 0;
        ordinaryCount = 0;
        difficultCount = 0;
        simpleCount = 0;
        for (int i = 1; i < routeItems.Count; i++)
        {
            if (PlayerPrefs.GetString("Simple" + (i)) != "")
            {
                simpleCount += 1;
            }
            if (PlayerPrefs.GetString("Ordinary" + (i)) != "")
            {
                ordinaryCount += 1;
            }
            if (PlayerPrefs.GetString("Difficult" + (i)) != "")
            {
                difficultCount += 1;
            }
            if (JudeTask("Simple", i))
            {
                perfect += 1;
            }
            if (JudeTask("Ordinary", i))
            {
                perfect += 1;
            }
            if (JudeTask("Difficult", i))
            {
                perfect += 1;
            }
        }
        if (simpleCount >= 5)
        {
            ordinaryImage.sprite = normal;
            ordinaryLock.SetActive(false);
            ordinaryBtn.enabled = true;
            ordinaryTip.SetActive(ordinaryCount < 1);
            ordinaryText.gameObject.SetActive(false);
        }
        else
        {
            ordinaryLock.SetActive(true);
            ordinaryTip.SetActive(false);
            ordinaryBtn.enabled = false;
            ordinaryImage.sprite = graylock;
        }
        if (ordinaryCount >= 5)
        {
            difficultImage.sprite = normal;
            difficultLock.SetActive(false);
            difficultBtn.enabled = true;
            difficultTip.SetActive(difficultCount < 1);
            difficultText.gameObject.SetActive(false);
        }
        else
        {
            difficultLock.SetActive(true);
            difficultTip.SetActive(false);
            difficultBtn.enabled = false;
            difficultImage.sprite = graylock;
        }
        float sumLevel= simpleCount + ordinaryCount + difficultCount;
        CreateModel.Instance.MaxPass(sumLevel,false);
        starText.text = CreateModel.Instance.sumLevel.ToString();
        passText.text = perfect.ToString();
        UIManager.Instance.SetStar(0);
        if(modelMessg == "Simple")
        {
            SimpleModel();
        }
        else if (modelMessg == "Ordinary")
        {
            OrdinaryModel();
        }
        else if (modelMessg == "Difficult")
        {
            DifficultModel();
        }
    }

    private void HideMap()
    {
        AudioManager.Instance.PlayTouch("close_1");
        UIManager.Instance.SwitchScene("Select");
        GameManager.Instance.HideBanner();
        gameObject.SetActive(false);
    }

    private void DifficultModel()
    {
        modelMessg = "Difficult";
        simpleLow.color = imageColor;
        ordinaryLow.color = imageColor;
        difficultLow.color = Color.white;
        simpleLow.transform.localScale = Vector3.one;
        ordinaryLow.transform.localScale = Vector3.one;
        difficultLow.transform.localScale = Vector3.one * 1.2f;
        simpleImage.sprite = normal;
        ordinaryImage.sprite = normal;
        difficultImage.sprite = highlight;
        difficultTip.SetActive(false);
        float pointY = (int)Mathf.Ceil(difficultCount * 0.25f);
        pointY = pointY * 100 + pointY * 20 + 15;
        roudeParent.transform.localPosition = Vector3.up * pointY;
        tipRoudeText.text = ExcelTool.lang["tiproude3"];
        TabSettings(modelMessg, difficultCount);
    }

    private void OrdinaryModel()
    {
        modelMessg = "Ordinary";
        simpleLow.color = imageColor;
        ordinaryLow.color = Color.white;
        difficultLow.color = imageColor;
        simpleLow.transform.localScale = Vector3.one;
        ordinaryLow.transform.localScale = Vector3.one * 1.2f;
        difficultLow.transform.localScale = Vector3.one;
        simpleImage.sprite = normal;
        ordinaryImage.sprite = highlight;
        ordinaryTip.SetActive(false);
        if (ordinaryCount > 5)
        {
            difficultImage.sprite = normal;
        }
        float pointY = (int)Mathf.Ceil(ordinaryCount * 0.25f);
        pointY = pointY * 100 + pointY * 20 + 15;
        roudeParent.transform.localPosition = Vector3.up * pointY;
        tipRoudeText.text = ExcelTool.lang["tiproude2"];
        TabSettings(modelMessg, ordinaryCount);
    }

    private void SimpleModel()
    {
        modelMessg = "Simple";
        simpleLow.color = Color.white;
        ordinaryLow.color = imageColor;
        difficultLow.color = imageColor;
        simpleLow.transform.localScale = Vector3.one*1.2f;
        ordinaryLow.transform.localScale = Vector3.one;
        difficultLow.transform.localScale = Vector3.one;
        simpleImage.sprite = highlight;
        if (ordinaryCount > 5)
        {
            difficultImage.sprite = normal;
        }
        if (simpleCount > 5)
        {
            ordinaryImage.sprite = normal;
        }
        float pointY = (int)Mathf.Ceil(simpleCount * 0.25f);
        pointY = pointY * 100 + pointY * 20 + 15;
        roudeParent.transform.localPosition = Vector3.up * pointY;
        tipRoudeText.text= ExcelTool.lang["tiproude1"];
        TabSettings(modelMessg,simpleCount);
    }
    //tiproude1
    private void TabSettings(string messg,float length)
    {
        for (int i = 0; i < routeItems.Count; i++)
        {
            routeItems[i].LevelJudge(messg,length);
        }
    }

    private bool JudeTask(string messg,int index)
    {
        int heartIndex = 0;
        string[] taskID = ExcelTool.Instance.lines[index.ToString()].battlefield_mission_type.Split('|');
        string taskName = "";
        for (int i = 0; i < taskID.Length; i++)
        {
            taskName = string.Format("Model{0}Pass{1}Mission{2}", messg,index, taskID[i]);
            if (PlayerPrefs.GetString(taskName) == "true")
            {
                heartIndex++;
            }
        }
        if (heartIndex >= 3)
        {
            return true;
        }
        return false;
    }
}
