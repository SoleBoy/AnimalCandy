using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    public Transform[] candyParent;
    public Color[] colors;
    public CarryOut[] carryOuts;
    public TrailRenderer tailingObject;
    public Transform armatureParent;
    public DragonBones.UnityArmatureComponent model_Armature;
    public Transform contentParent;
    public Transform sortParent;
    private Transform skillPrefab;
    private Transform freedPrefab;
    private GameObject infoPanel;
    private GameObject selectPanel;
    private GameObject buyPanel;
    private GameObject selectTip;
    private GameObject goldtip_buy;
    private GameObject diamtip_buy;
    private DragonBones.UnityArmatureComponent info_Armature;

    private Text buyposse_diam;
    private Text buyposse_gold;
    private Text levelText;
    public Text diamText;
    public Text goldText;
    private Text infoText;
    private Text nameText;
    private Text carryText;
    private Text buyText_diam;
    private Text buyText_gold;
    private Image buyImage;
    private Button warBtn;
    private Button backBtn;
    private Button closeBtn;
    private Button infoBtn;
    private Button buyBtn_close;
    private bool isUp;

    private Action action_buy;

    private float diam_Consume;
    private float gold_Consume;
    private int indexCount;
    private int carry_max;
    private Dictionary<int, CarryOut> carryKey = new Dictionary<int, CarryOut>();
    private int[] indexSorts = {1,2,3,4,5,6,7,11,12,13,14,15,8,16,17,9,18,10,19,20};
    public List<SkillSort> freedSkill = new List<SkillSort>();
    private List<SkillAttributes> attributes = new List<SkillAttributes>();
    public void Init()
    {
        sortParent = transform.parent.Find("GamePanel/Scroll View/Viewport/Content");
        contentParent = transform.Find("Scroll View").GetComponent<ScrollRect>().content;
        freedPrefab = transform.Find("FreedPrefab");
        skillPrefab = transform.Find("SkillPrefab");
        infoPanel = transform.Find("InfoPanel").gameObject;
        buyPanel = transform.Find("BuyPanel").gameObject;
        goldtip_buy = transform.Find("BuyPanel/goldtip").gameObject;
        diamtip_buy = transform.Find("BuyPanel/diamtip").gameObject;
        buyposse_diam = transform.Find("BuyPanel/DiamText").GetComponent<Text>();
        buyposse_gold = transform.Find("BuyPanel/GoldText").GetComponent<Text>();
        levelText = transform.Find("Level/Text").GetComponent<Text>();
        diamText = transform.Find("Diamond/Text").GetComponent<Text>();
        goldText = transform.Find("Gold/Text").GetComponent<Text>();
        infoText = transform.Find("InfoPanel/InfoText").GetComponent<Text>();
        nameText = transform.Find("InfoPanel/NameText").GetComponent<Text>();
        buyText_diam = transform.Find("BuyPanel/Diamond/Text").GetComponent<Text>();
        buyText_gold = transform.Find("BuyPanel/Gold/Text").GetComponent<Text>();
        buyImage = transform.Find("BuyPanel/DiamBtn").GetComponent<Image>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        infoBtn = transform.Find("InfoPanel/Button").GetComponent<Button>();
        buyBtn_close = transform.Find("BuyPanel/Close").GetComponent<Button>();
        info_Armature = transform.Find("InfoPanel/candy").GetComponent<DragonBones.UnityArmatureComponent>();
        closeBtn.onClick.AddListener(ClosePanel);
        backBtn.onClick.AddListener(BackPanel);
        infoBtn.onClick.AddListener(CloseInfoPanel);
        buyBtn_close.onClick.AddListener(CloseBuyPanel);
        if (GameManager.Instance.modeSelection == "roude")
        {
            sortParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0,68);
            selectPanel = transform.Find("Select").gameObject;
            selectTip = transform.Find("Select/SelectTip").gameObject;
            carryText = transform.Find("Select/Text").GetComponent<Text>();
            warBtn = transform.Find("Select/WarBtn").GetComponent<Button>();
            warBtn.onClick.AddListener(CloseCarry);
        }
        else
        {
            sortParent.GetComponent<RectTransform>().sizeDelta = new Vector2(1465, 68);
        }
        InitData();
    }
    void InitData()
    {
        for (int i = 0; i < 20; i++)
        {
            var freed = Instantiate(freedPrefab);
            freed.gameObject.SetActive(true);
            freed.SetParent(sortParent);
            freed.localScale = Vector3.one * 1.1f;
            freed.localPosition = Vector3.zero;
            freed.localEulerAngles = Vector3.zero;
            freed.GetComponent<SkillSort>().InitData(ExcelTool.Instance.skills[(i+1).ToString()], candyParent[i], i+1);
            freedSkill.Add(freed.GetComponent<SkillSort>());
        }
      
        for (int i = 0; i < 20; i++)
        {
            var skill = Instantiate(skillPrefab);
            skill.gameObject.SetActive(true);
            skill.SetParent(contentParent);
            skill.localScale = Vector3.one;
            skill.localPosition = Vector3.zero;
            skill.localEulerAngles = Vector3.zero;
            skill.GetComponent<SkillAttributes>().InitData(sortParent.GetChild(i).GetComponent<SkillSort>(), i+1, colors[i]);
            attributes.Add(skill.GetComponent<SkillAttributes>());
        }
        for (int i = 0; i < indexSorts.Length; i++)
        {
            freedSkill[indexSorts[i] - 1].transform.SetAsLastSibling();
        }
        for (int i = 0; i < indexSorts.Length; i++)
        {
            attributes[indexSorts[i] - 1].transform.SetAsLastSibling();
        }
    }
    //public void FistSkill()
    //{
    //    for (int i = 0; i < freedSkill.Count; i++)
    //    {
    //        freedSkill[i].SkillCarry(false);
    //    }
    //    for (int i = 0; i < 2; i++)
    //    {
    //        freedSkill[i].SkillCarry(true);
    //        UIManager.Instance.lockSkill.Add(i+1);
    //    }
    //}
    public void TutorialsCarry()
    {
        for (int i = 0; i < freedSkill.Count; i++)
        {
            freedSkill[i].SkillCarry(false);
        }
        UIManager.Instance.lockSkill.Clear();
        int[] numbers = UIManager.Instance.GetRandoms(6,0,freedSkill.Count);
        for (int i = 0; i < numbers.Length; i++)
        {
            freedSkill[numbers[i]].SkillCarry(true);
            UIManager.Instance.lockSkill.Add(numbers[i] + 1);
        }
    }
    public void OpenSelected(int carryNum)
    {
        carry_max = carryNum;
        carryKey.Clear();
        backBtn.gameObject.SetActive(true);
        closeBtn.gameObject.SetActive(false);
        selectTip.SetActive(false);
        gameObject.SetActive(true);
        selectPanel.SetActive(true);
        carryText.text = string.Format("{0}:{1}/{2}", ExcelTool.lang["carry"], 0, carry_max);
        for (int i = 0; i < carryOuts.Length; i++)
        {
            if (i < carryNum)
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
            string messg = PlayerPrefs.GetString("AnnalSkill", "");
            if (messg != "")
            {
                string[] annal = messg.Split('|');
                int index = 0;
                for (int i = annal.Length - 1; i >= 0; i--)
                {
                    index++;
                    if (index > carryNum)
                    {
                        break;
                    }
                    attributes[int.Parse(annal[i]) - 1].SelectCarry();
                }
            }
        }
        if (PlayerPrefs.GetString("selectskill") == "")
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip28"]);
            PlayerPrefs.SetString("selectskill", "true");
        }
        if (PlayerPrefs.GetString("Guide" + 15) == "")
        {
            UIManager.Instance.guidePanel.OpenGuide(15, true);
        }
    }

    public bool SelectCarry(int id,string name,float gold,string keelname)
    {
        if(carryKey.Count <= carry_max)
        {
            for (int i = 0; i < carry_max; i++)
            {
                if(carryOuts[i].Unselected())
                {
                    carryOuts[i].SelectedState(name,gold.ToString(),id, keelname);
                    carryKey.Add(id, carryOuts[i]);
                    carryText.text = string.Format("{0}:{1}/{2}", ExcelTool.lang["carry"], carryKey.Count, carry_max);
                    return true;
                }
            }
        }
        return false;
    }

    public void CancelSelection(int id)
    {
        carryKey[id].NormalState();
        carryKey.Remove(id);
        CarryHide();
    }
    public void UnloadCarry(int id)
    {
        attributes[id].SelectCarry();
    }

    public void TailingEffect(Transform parent,int index)
    {
        tailingObject.transform.SetParent(parent);
        tailingObject.transform.localPosition = Vector3.zero;
        tailingObject.transform.localScale = Vector3.one;
        tailingObject.startColor = colors[index];
        tailingObject.endColor = colors[index];
    }
    public void CarryHide()
    {
        bool isHide = carryKey.Count < carry_max;
        for (int i = 0; i < attributes.Count; i++)
        {
            attributes[i].CarryHideTip(isHide);
        }
        selectTip.SetActive(carryKey.Count > 0);
        if (carryKey.Count >= carry_max && PlayerPrefs.GetString("Carryskill") == "")
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip27"]);
            PlayerPrefs.SetString("Carryskill", "true");
        }
    }

    public void JudeFreed(float candy)
    {
        for (int i = 0; i < sortParent.childCount; i++)
        {
            sortParent.GetChild(i).GetComponent<SkillSort>().IsJude(candy);
        }
    }

    public bool JudeUpGrade(float level, float diam)
    {
        levelText.text = level.ToString();
        diamText.text = diam.ToString();
        isUp = false; indexCount = -5;
        for (int i = attributes.Count-1; i >= 0; i--)
        {
            if (attributes[i].JudeUp(level, diam))
            {
                if(!isUp)
                {
                    indexCount = attributes[i].transform.GetSiblingIndex();
                    string keelName;
                    if (i < 10)
                        keelName = string.Format("candy_10{0}_1", i + 1);
                    else
                        keelName = string.Format("candy_1{0}_1", i);
                    model_Armature = UIManager.Instance.SetArmature(model_Armature, armatureParent, keelName, Vector3.one * 10, Vector3.up * -5);
                }
                isUp = true;
            }
        }
        return isUp;
    }

    //信息面板
    public void TurretInfo(string messg,string infoName,string armatureName)
    {
        AudioManager.Instance.PlayTouch("other_1");
        infoPanel.SetActive(true);
        infoText.text = messg;
        nameText.text = infoName;
        info_Armature = UIManager.Instance.SetArmature(info_Armature, infoPanel.transform, armatureName, Vector3.one * 50, Vector3.up * 200, true, "rest");
        info_Armature.transform.SetAsLastSibling();
    }

    public void OpenPanel(int index = -10)
    {
        if(index != -10)
        {
            if (index <= 0)
                index = indexCount;
            if(index >= 0)
            {
                Vector3 vector = contentParent.localPosition;
                vector.y = index * 270 + index * 120;
                contentParent.localPosition = vector;
            }
        }
        gameObject.SetActive(true);
    }
    //消耗面板
    public void OpenBuyPanel(Action action, float diam, float gold)
    {
        action_buy = action;
        if (diam <= UIManager.Instance.starNumber)
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
        buyposse_diam.text = string.Format("{0}:{1}", ExcelTool.lang["possesum"], UIManager.Instance.starNumber);
        buyposse_gold.text = string.Format("{0}:{1}", ExcelTool.lang["possesum"], UIManager.Instance.GoldDisplay(UIManager.Instance.goldNumber));
        buyPanel.SetActive(true);
    }

    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
    }
    private void BackPanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        UIManager.Instance.turretPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
        UIManager.Instance.routeMapPanel.OpenPanel();
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
    public void WaysPurchase(bool isGold)
    {
        AudioManager.Instance.PlayTouch("starup_1");
        if (isGold)
        {
            UIManager.Instance.SetGold(-gold_Consume, true);
        }
        else
        {
            if (diam_Consume > UIManager.Instance.starNumber)
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
                return;
            }
            UIManager.Instance.SetStar(-diam_Consume);
        }
        if (action_buy != null)
        {
            action_buy.Invoke();
        }
        buyPanel.SetActive(false);
    }
    //公共冷却
    public void PublicCooling(bool isCool)
    {
        for (int i = 0; i < freedSkill.Count; i++)
        {
            freedSkill[i].Cooling(isCool);
        }
    }

    private void CloseCarry()
    {
        AudioManager.Instance.PlayTouch("close_1");
        if (UIManager.Instance.lockSkill.Count <= 0)
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip29"]);
        }
        else
        {
            gameObject.SetActive(false);
            if (PlayerPrefs.GetString("Guide" + 13) == "")
            {
                UIManager.Instance.guidePanel.OpenGuide(13, true);
            }
            if (PlayerPrefs.GetString("selectturret") == "")
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip28"]);
                PlayerPrefs.SetString("selectturret", "true");
            }
        }
    }

    public void NextCarry()
    {
        closeBtn.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);
        selectPanel.SetActive(false);
        for (int i = 0; i < carryOuts.Length; i++)
        {
            carryOuts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < indexSorts.Length; i++)
        {
            contentParent.GetChild(i).GetComponent<SkillAttributes>().CarryHide();
        }
        UIManager.Instance.lockSkill.Sort();
        if(CreateModel.Instance.level != 0)
        {
            string messgCarry = "";
            for (int i = 0; i < UIManager.Instance.lockSkill.Count; i++)
            {
                messgCarry += UIManager.Instance.lockSkill[i];
                if (i < UIManager.Instance.lockSkill.Count - 1)
                {
                    messgCarry += "|";
                }
            }
            PlayerPrefs.SetString("AnnalSkill", messgCarry);
        }
    }
    //提示动画
    public void TipAnimal(List<int> tipIndex)
    {
        List<int> animal = new List<int>();
        for (int i = 0; i < tipIndex.Count; i++)
        {
            freedSkill[tipIndex[i]-1].OpenAnimal(false);
            if(freedSkill[tipIndex[i] - 1].JudeTip())
            {
                animal.Add(tipIndex[i] - 1);
            }  
        }
        if(animal.Count > 0)
        {
            int ran = UnityEngine.Random.Range(0, animal.Count);
            freedSkill[animal[ran]].OpenAnimal(true);
        }
    }
}


