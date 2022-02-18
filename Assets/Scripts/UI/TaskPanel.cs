using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    public Text goldText;
    public Text diamText;
    public Sprite[] typeSprite;
    
    Text nameText;
    Text gradeText;
    Text requireText;
    Text awardText;
    Text defText;
    Text timeText;
    Image sprite;

    Button awardBtn;
    Button drawBtn;
    Button closBtn;
    Button backBtn;

    GameObject rewardPanel;
    GameObject maskPanel;
    Transform animalPrefab;
    Transform content;
    List<int> headCounts = new List<int>();
    Dictionary<int, Attributes> keys = new Dictionary<int, Attributes>();
    int taskIndex;
    float lastTime;
    float timeDown;
    bool isClick;
    bool isLevel;
    public void Init()
    {
        maskPanel = transform.Find("MaskPanel").gameObject;
        rewardPanel = transform.Find("Reward").gameObject;
        timeText = transform.Find("TimeText").GetComponent<Text>();
        nameText = rewardPanel.transform.Find("Image/Text").GetComponent<Text>();
        defText = rewardPanel.transform.Find("DefText").GetComponent<Text>();
        gradeText = rewardPanel.transform.Find("depict/Grade").GetComponent<Text>();
        requireText = rewardPanel.transform.Find("depict/Require").GetComponent<Text>();
        awardText = rewardPanel.transform.Find("depict/Award").GetComponent<Text>();
        sprite = rewardPanel.transform.Find("Sprite").GetComponent<Image>();
        awardBtn = rewardPanel.transform.Find("DrawBtn").GetComponent<Button>();
        backBtn = rewardPanel.transform.Find("BackBtn").GetComponent<Button>();
        awardBtn.onClick.AddListener(ClickClaim);
        backBtn.onClick.AddListener(Backreward);
        
        animalPrefab = transform.Find("Prefab");
        content = transform.Find("Scroll View").GetComponent<ScrollRect>().content;
        goldText = transform.Find("Candy/Text").GetComponent<Text>();
        diamText = transform.Find("Diamond/Text").GetComponent<Text>();
        closBtn = transform.Find("CloseBtn").GetComponent<Button>();
        drawBtn = transform.Find("DrawBtn").GetComponent<Button>();
        drawBtn.onClick.AddListener(ClickAccess);
        closBtn.onClick.AddListener(ClosePanel);

        int count = ExcelTool.Instance.animalSprite.Length + 6;
        for (int i = 0; i < count; i++)
        {
            if(i > 5)
            {
                Attributes task = new Attributes();
                var animal = Instantiate(animalPrefab);
                animal.gameObject.SetActive(true);
                animal.SetParent(content);
                animal.localPosition=Vector3.zero;
                animal.localEulerAngles=Vector3.zero;
                animal.localScale=Vector3.one;
                task.Init(animal, ExcelTool.Instance.tasks[i.ToString()], typeSprite,ExcelTool.Instance.animalSprite[i-6]);
                keys.Add(i, task);
            }
            else
            {
                Attributes task = new Attributes();
                task.Init(content.GetChild(i), ExcelTool.Instance.tasks[i.ToString()], typeSprite);
                keys.Add(i, task);
            }
        }
        maskPanel.SetActive(false);
    }
    //打开
    public void SetPanel(bool isOn,bool islevel)
    {
        if (islevel)
        {
            this.isLevel = islevel;
        }
        if (PlayerPrefs.GetString("Guide" + 14) == "")
        {
            UIManager.Instance.guidePanel.OpenGuide(14, true);
        }
        else
        {
            isClick = isOn;
            if (isClick)
            {
                timeDown = 15;
                lastTime = 0;
                timeText.text = timeDown.ToString("F0") + "s";
            }
            else
            {
                timeText.text = "";
            }
            gameObject.SetActive(true);
            GameManager.Instance.ShowBanner();
        }
        headCounts.Clear();
    }
    private void Update()
    {
        if (isClick)
        {
            if(Input.GetMouseButtonDown(0))
            {
                isClick = false;
                timeText.text = "";
            }
            if (timeDown >= 0)
            {
                lastTime += Time.deltaTime;
                if (lastTime >= 1)
                {
                    timeDown -= lastTime;
                    timeText.text = timeDown.ToString("F0") + "s";
                    if (timeDown <= 0)
                    {
                        ClosePanel();
                    }
                    lastTime = 0;
                }
            }
        }
    }
    void Backreward()
    {
        rewardPanel.SetActive(false);
        AudioManager.Instance.PlayTouch("close_1");
    }
    //关闭
    void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        if(headCounts.Count > 0)
        {
            float sum = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                sum += keys[i].buildNum;
                keys[i].SetBuilid();
            }
            UIManager.Instance.headPanel.CreateHead(sum, headCounts);
        }
        else
        {
            JudeLevel();
        }
    }
    public float GetSum
    {
        get
        {
            float sum = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                sum += keys[i].buildNum;
            }
            return sum;
        }
    }
    public void JudeLevel()
    {
        if (isLevel)
        {
            isLevel = false;
            if (CreateModel.Instance.SceneChange())
            {
                UIManager.Instance.mapPanel.OpenPanel(true);
                return;
            }
            else
            {
                CreateModel.Instance.LevelJudge(true);
            }
        }
        GameManager.Instance.HideBanner();
        UIManager.Instance.DetectionPanel();
        isClick = false;
    }

    public void SetProperty(int index,string name)
    {
        rewardPanel.SetActive(true);
        taskIndex = index;
        nameText.text = name;
        if (keys[index].item.resistance == "")
        {
            defText.text = "";
        }
        else if(keys[index].item.resistance == "-1")
        {
            defText.text =ExcelTool.lang["resis1"];
        }
        else
        {
            defText.text = ExcelTool.lang["resis" + (int.Parse(keys[index].item.resistance) + 1)];
        }
        sprite.GetComponent<RectTransform>().sizeDelta = keys[index].sprite.GetComponent<RectTransform>().sizeDelta;
        sprite.GetComponent<RectTransform>().rotation = keys[index].sprite.GetComponent<RectTransform>().rotation;
        sprite.sprite = keys[index].sprite.sprite;
        SetTaskInfo(taskIndex);
    }
    //领取奖励
    void ClickClaim()
    {
        if (keys[taskIndex].Receive())
        {
            SetTaskInfo(taskIndex);
            if (taskIndex >= 6)
            {
                headCounts.Add(taskIndex - 6);
            }
            UIManager.Instance.HideTask();
        }
    }
    void SetTaskInfo(int taskIndex)
    {
        requireText.text = ExcelTool.lang["require"] + ":" + Scientific(keys[taskIndex].demand);
        switch (taskIndex)
        {
            case 0:
                gradeText.text = ExcelTool.lang["taskname0"] + ":" + CreateModel.Instance.passLevel;
                awardText.text = ExcelTool.lang["rewarddiam"] + ":" + keys[taskIndex].draw;
                break;
            case 1:
                gradeText.text = ExcelTool.lang["passlevel"] + ":" + CreateModel.Instance.sumLevel;
                awardText.text = ExcelTool.lang["rewarddiam"] + ":" + keys[taskIndex].draw;
                break;
            case 2:
                gradeText.text = ExcelTool.lang["totlestar"] + ":" + CreateModel.Instance.HighestStar();
                awardText.text = ExcelTool.lang["rewardcandy"] + ":" + keys[taskIndex].draw;
                break;
            case 3:
                gradeText.text = ExcelTool.lang["totlefirt"] + ":" + CreateModel.Instance.HighestFit();
                awardText.text = ExcelTool.lang["rewarddiam"] + ":" + keys[taskIndex].draw;
                break;
            case 4:
                gradeText.text = ExcelTool.lang["totlediam"] + ":" + UIManager.Instance.sunDiam;
                awardText.text = ExcelTool.lang["rewardcandy"] + ":" + keys[taskIndex].draw;
                break;
            case 5:
                gradeText.text = ExcelTool.lang["totletime"] + ":" + UIManager.Instance.timeRecord.ToString("F1");
                awardText.text = ExcelTool.lang["rewarddiam"] + ":" + keys[taskIndex].draw;
                break;
            default:
                gradeText.text = ExcelTool.lang["extlife"] + ":" + Scientific(PlayerPrefs.GetFloat("enemyHp" + (taskIndex - 5)));
                awardText.text = ExcelTool.lang["rewardcandy"] + ":" + keys[taskIndex].draw;
                break;
        }
    }
    string Scientific(float num)
    {
        if(num >= 10000000)
        {
            return num.ToString("F1");
        }
        else
        {
            return num.ToString("F0");
        }
    }
    float index = 0;
    bool isLoad;
    void ClickAccess()
    {
        isLoad = false;
        if (!maskPanel.activeInHierarchy)
        {
            AudioManager.Instance.PlayTouch("other_1");
            maskPanel.SetActive(true);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            switch (i)
            {
                case 0:
                    if (keys[i].JudeDraw(CreateModel.Instance.passLevel))
                    {
                        StartCoroutine(Delayed(i, index));
                        index += 0.5f;
                        isLoad = true;
                    };
                    break;
                case 1:
                    if (keys[i].JudeDraw(CreateModel.Instance.sumLevel)) {
                        StartCoroutine(Delayed(i,index));
                        index += 0.5f;
                        isLoad = true;
                    };
                    break;
                case 2:
                    if (keys[i].JudeDraw(CreateModel.Instance.HighestStar())) {
                        StartCoroutine(Delayed(i, index));
                        index += 0.5f;
                        isLoad = true;
                    };
                    break;
                case 3:
                    if (keys[i].JudeDraw(CreateModel.Instance.HighestFit())) {
                        StartCoroutine(Delayed(i, index));
                        index += 0.5f;
                        isLoad = true;
                    };
                    break;
                case 4:
                    if (keys[i].JudeDraw(UIManager.Instance.sunDiam)) {
                        StartCoroutine(Delayed(i, index));
                        index += 0.5f;
                        isLoad = true;
                    };
                    break;
                case 5:
                    if (keys[i].JudeDraw(UIManager.Instance.timeRecord))
                    {
                        StartCoroutine(Delayed(i, index));
                        index += 0.5f;
                        isLoad = true;
                    };
                    break;
                default:
                    if (keys[i].JudeDraw(PlayerPrefs.GetFloat("enemyHp" + (i - 5)))) {
                        StartCoroutine(Delayed(i, index));
                        headCounts.Add(i - 6);
                        index += 0.5f;
                        isLoad = true;
                    };
                    break;
            }
        }
        StartCoroutine(HideMask(index));
    }
    IEnumerator HideMask(float gap)
    {
        yield return new WaitForSeconds(gap);
        index = 0;
        if (Judge())
        {
            ClickAccess();
        }
        else
        {
            if(!isLoad)
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
            }
            maskPanel.SetActive(false);
            isLoad = false;
            UIManager.Instance.HideTask(false);
        }
    }
    IEnumerator Delayed(int id,float gap)
    {
        yield return new WaitForSeconds(gap);
        keys[id].Receive();
    }
    //刷新
    public bool Judge()
    {
        bool isDraw = false;
        for (int i = 0; i < keys.Count; i++)
        {
            switch (i)
            {
                case 0:
                    if (keys[i].JudeDraw(CreateModel.Instance.passLevel)) isDraw = true;
                    break;
                case 1:
                    if (keys[i].JudeDraw(CreateModel.Instance.sumLevel))isDraw = true;
                    break;
                case 2:
                    if(keys[i].JudeDraw(CreateModel.Instance.HighestStar()))isDraw=true;
                    break;
                case 3:
                    if(keys[i].JudeDraw(CreateModel.Instance.HighestFit()))isDraw = true;
                    break;
                case 4:
                    if(keys[i].JudeDraw(UIManager.Instance.sunDiam))isDraw = true;
                    break;
                case 5:
                    if (keys[i].JudeDraw(UIManager.Instance.timeRecord)) isDraw = true;
                    break;
                default:
                    if(keys[i].JudeDraw(PlayerPrefs.GetFloat("enemyHp" + (i-5))))isDraw = true;
                    break;
            }
        }
        return isDraw;
    }
}

public class Attributes
{
    public MissionItem item;
    
    public float demand;
    public float draw;
    public float taskGrade;
    public float temp;
    public Image sprite;
    public int buildNum;
    private string taskName;
    Text nameText;
    Button openBtn;
    GameObject tip;
    Image typeSprite;
    Image resisSprite;
    Text buildText;
    int IndexItem;
    int idItem;
    Color purple = new Color(148, 0, 255, 255);
    public void Init(Transform tan,MissionItem item,Sprite[] sp,Sprite head=null)
    {
        this.item = item;
        idItem = int.Parse(item.id);
        IndexItem = idItem - 5;
        nameText = tan.Find("NameText").GetComponent<Text>();
        typeSprite = tan.Find("type").GetComponent<Image>();
        openBtn = tan.GetComponent<Button>();
        ExcelTool.LanguageEvent += CutLang;
        tip = tan.Find("Tip").gameObject;
        openBtn.onClick.AddListener(OpenPlane);
        typeSprite.sprite = sp[(int)item.type_reward-1];
        taskName = "Task" + item.icon_name;
        taskGrade = PlayerPrefs.GetFloat(taskName, 1);
        //if (idItem >= 5)
        //{
        //    if(idItem == 5)
        //    {
        //        taskGrade = PlayerPrefs.GetFloat(item.icon_name, 1);
        //    }
        //    else
        //    {
        //        taskGrade = PlayerPrefs.GetFloat("Task" + (idItem-1), 1);
        //    }
        //}
        //else
        //{
        //    taskGrade = PlayerPrefs.GetFloat("Task" + idItem, 1);
        //}
        sprite = tan.Find("Image").GetComponent<Image>();
        if (IndexItem > 0)
        {
            resisSprite = tan.Find("Resis").GetComponent<Image>();
            buildText = tan.Find("Text").GetComponent<Text>();
            buildNum = PlayerPrefs.GetInt("Construction" + (IndexItem-1));
            sprite.transform.localScale = Vector3.one * 0.7f;
            sprite.sprite = head;
            sprite.SetNativeSize();
            buildText.text = buildNum.ToString();
            SetResistance(int.Parse(item.resistance));
        }
        draw = item.reward + item.reward_growth * (taskGrade - 1);
        demand = item.need + item.need_growth * (taskGrade - 1);
    }
    void SetResistance(int index)
    {
        if(index == -1)
        {
            resisSprite.gameObject.SetActive(false);
            return;
        }
        Color color = Color.white;
        if (index == 1)
        {
            color = Color.green;
        }
        else if (index == 2)
        {
            color = Color.blue;
        }
        else if (index == 3)
        {
            color = Color.red;
        }
        else if (index == 4)
        {
            color = purple;
        }
        resisSprite.color = color;
    }

    public void CutLang()
    {
        nameText.text = ExcelTool.lang["taskname"+item.id];
    }
    public void SetBuilid()
    {
        if (IndexItem > 0)
            buildText.text = buildNum.ToString();
    }
    public bool JudeDraw(float value)
    {
        temp = value;
        if (value >= demand)
        {
            tip.SetActive(true);
            return true;
        }
        tip.SetActive(false);
        return false;
    }
    private void OpenPlane()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.taskPanel.SetProperty(int.Parse(item.id), nameText.text);
    }
    public bool Receive()
    {
        if (tip.activeInHierarchy)
        {
            drawSum = 0;
            DrawTotal();
            if (item.type_reward == 2)
            {
                drawSum = Mathf.Clamp(drawSum,0,30);
                UIManager.Instance.SetStar(drawSum);
                GameManager.Instance.ClonePrompt(drawSum, 1);
            }
            else
            {
                UIManager.Instance.SetGold(drawSum,true);
                GameManager.Instance.ClonePrompt(drawSum, 0);
            }
            return true;
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
            return false;
        }
    }
    float drawSum;
    void DrawTotal()
    {
        if(temp >= demand)
        {
            drawSum += draw;
            if (item.type_mission == 1)
            {
                temp = 0;
                switch (item.id)
                {
                    case "4":
                        UIManager.Instance.sunDiam = 0;
                        PlayerPrefs.SetFloat("sumDiamond", 0);
                        break;
                    default:
                        buildNum += 1;
                        PlayerPrefs.SetFloat("enemyHp"+ IndexItem, 0);
                        PlayerPrefs.SetInt("Construction"+ (IndexItem - 1), buildNum);
                        break;
                }
            }
            taskGrade += 1;
            draw = item.reward + item.reward_growth * (taskGrade - 1);
            demand = item.need + item.need_growth * (taskGrade - 1);
            PlayerPrefs.SetFloat(taskName, taskGrade);
            DrawTotal();
            //if (idItem >= 5)
            //{
            //    if (idItem == 5)
            //    {
            //        PlayerPrefs.SetFloat(item.icon_name, taskGrade);
            //    }
            //    else
            //    {
            //        PlayerPrefs.SetFloat("Task" + (idItem - 1), taskGrade);
            //    }
            //}
            //else
            //{
            //    PlayerPrefs.SetFloat("Task" + idItem, taskGrade);
            //}
        }
    }
    
}

