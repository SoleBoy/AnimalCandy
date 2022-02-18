using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsPanel : MonoBehaviour
{
    public SkillRewards rewards;

    private Transform candyMask;
    Transform conter;
    Text nameText;
    Text infoText;
    Button closeBtn;
    Button rewardBtn;
    Button diamBtn;
    Button certainBtn;

    private int index;
    private bool isAds;
    private DragonBones.UnityArmatureComponent model_Armature;
    public void Init()
    {
        candyMask = transform.Find("CandyMask");
        conter = transform.parent.Find("GamePanel/Scroll View").GetComponent<ScrollRect>().content;
        nameText = transform.Find("NameText").GetComponent<Text>();
        infoText = transform.Find("Image/InfoText").GetComponent<Text>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        rewardBtn = transform.Find("RewardBtn").GetComponent<Button>();
        diamBtn = transform.Find("DiamondBtn").GetComponent<Button>();
        certainBtn = transform.Find("CertainBtn").GetComponent<Button>();
        model_Armature = candyMask.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>();
        closeBtn.onClick.AddListener(ClosePanel);
        diamBtn.onClick.AddListener(DiamondRewards);
        rewardBtn.onClick.AddListener(SkillRewards);
        certainBtn.onClick.AddListener(Certain);
    }
    public void RandomSkills()
    {
        HideButton(true);
        isAds = false;
        UIManager.Instance.isTime = true;
        gameObject.SetActive(true);
        infoText.text = ExcelTool.lang["adsakill"];
        Calculate();
    }
    private void SkillLock()
    {
        if (GameManager.Instance.modeSelection != "roude")
        {
            UIManager.Instance.lockSkill.Clear();
            for (int i = 1; i < 21; i++)
            {
                if (i >= 8)
                {
                    UIManager.Instance.lockSkill.Add(i);
                }
                else if (PlayerPrefs.GetInt("Skill" + i) == 0)
                {
                    UIManager.Instance.lockSkill.Add(i);
                }
            }
        }
    }

    private void Calculate()
    {
        SkillLock();
        float oddsSum = 0;
        float sum = 0;
        for (int i = 0; i < UIManager.Instance.lockSkill.Count; i++)
        {
            oddsSum += ExcelTool.Instance.skills[(UIManager.Instance.lockSkill[i]).ToString()].drop_skill;
        }
        float ran = Random.Range(0, oddsSum);
        for (int i = 0; i < UIManager.Instance.lockSkill.Count; i++)
        {
            sum += ExcelTool.Instance.skills[(UIManager.Instance.lockSkill[i]).ToString()].drop_skill;
            if (ran < sum)
            {
                index = UIManager.Instance.lockSkill[i]-1;
                nameText.text = ExcelTool.Instance.skills[(index+1).ToString()].realname;
                KeelAnimation();
                break;
            }
        }
        
    }
    private void KeelAnimation()
    {
        string keelName;
        if (index < 10)
            keelName = string.Format("candy_10{0}_1", index + 1);
        else
            keelName = string.Format("candy_1{0}_1", index + 1);
        model_Armature = UIManager.Instance.SetArmature(model_Armature, candyMask, keelName, Vector3.one * 60, Vector3.zero, true, "rest");
    }
    private void DiamondRewards()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if(UIManager.Instance.starNumber >= 1)
        {
            UIManager.Instance.skillPanel.freedSkill[index].DemoSkill(1);
            infoText.text = ExcelTool.lang["addakill"];
            HideButton(false);
        }
        else
        {
            UIManager.Instance.diamondPanel.OpenPanel();
        }
    }
    private void SkillRewards()
    {
        AudioManager.Instance.PlayTouch("ads_1");
        if (Application.platform == RuntimePlatform.Android)
        {
            GameManager.Instance.UserChoseToWatchAd(AdsType.skill);
            GameManager.Instance.AdmobRewardCB = delegate
            {
                UIManager.Instance.isTime = true;
            };
            GameManager.Instance.AdmobRewardClose = delegate
            {
                isAds = true;
                HideButton(false);
                infoText.text = ExcelTool.lang["addakill"];
            };
            GameManager.Instance.AdmobClose = delegate
            {
                if(!isAds)
                {
                    ClosePanel();
                }
                GameManager.Instance.RequestRewardBasedVideo(AdsType.skill);
            };
        }
        else
        {
            isAds = true;
            HideButton(false);
            infoText.text = ExcelTool.lang["addakill"];
        }
    }

    private void Certain()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (isAds)
        {
            UIManager.Instance.skillPanel.freedSkill[index].DemoSkill(1);
            UIManager.Instance.doublePanel.SetPanel(AdsType.skill,1, 600);
            UIManager.Instance.doublePanel.gameObject.SetActive(true);
            rewards.FlyOut();
            gameObject.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.GetString("Guide" + 6) == "")
            {
                gameObject.SetActive(false);
                UIManager.Instance.guidePanel.OpenGuide(6,false);
            }
            else
            {
                rewards.FlyOut();
                gameObject.SetActive(false);
                UIManager.Instance.DetectionPanel();
            }
            GameManager.Instance.HideBanner();
        }
        GameManager.Instance.CloneTip(ExcelTool.lang["tip3"]);
    }
    public void OpenPanel()
    {
        HideButton(false);
        isAds = false;
        gameObject.SetActive(true);
        nameText.text = ExcelTool.lang["skillrealname1"];
        infoText.text = ExcelTool.lang["addakill"];
        index = 0;
        model_Armature = UIManager.Instance.SetArmature(model_Armature, candyMask, "candy_101_1",Vector3.one*60,Vector3.zero, true, "rest");
        UIManager.Instance.skillPanel.freedSkill[index].DemoSkill(1);
        PlayerPrefs.SetString("FirstSkill" + 1,"1");
        UIManager.Instance.isTime = true;
    }

    private void HideButton(bool isHide)
    {
        closeBtn.gameObject.SetActive(isHide);
        rewardBtn.gameObject.SetActive(isHide);
        diamBtn.gameObject.SetActive(isHide);
        certainBtn.gameObject.SetActive(!isHide);
    }
    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        rewards.FlyOut();
        gameObject.SetActive(false);
        GameManager.Instance.HideBanner();
        UIManager.Instance.DetectionPanel();
    }
    private void OnEnable()
    {
        GameManager.Instance.ShowBanner();
    }
}
