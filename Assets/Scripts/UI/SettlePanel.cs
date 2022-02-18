using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DragonBones;
using System;

public class SettlePanel : MonoBehaviour
{
    private GameObject settleParent;
    private GameObject win_object;
    private GameObject lose_object;

    private Text curretText;
    private Text diamText;
    private Text goldText;
    private Text timeText;
    private Text hpText;
    private Button closeBtn;

    private UnityArmatureComponent new_timemode;
    private UnityArmatureComponent old_timemode;

    private int takeDiam;
    private int levelCount;
    public float recordTime = 0;
    private float timeReco;
    private float timeCurr;
    private float lastTime = -1;
    private bool isRoute;
    private bool isVictory;
    void Awake()
    {
        timeReco= PlayerPrefs.GetFloat("PatternTime");
        settleParent = transform.Find("Parent").gameObject;
        win_object = transform.Find("Parent/win").gameObject;
        lose_object = transform.Find("Parent/lose").gameObject;
        diamText = transform.Find("Parent/diamnum").GetComponent<Text>();
        timeText = transform.Find("Parent/keeptime").GetComponent<Text>();
        new_timemode = transform.Find("new_timemode").GetComponent<UnityArmatureComponent>();
        old_timemode = transform.Find("old_timemode").GetComponent<UnityArmatureComponent>();
        if(GameManager.Instance.modeSelection=="roude")
        {
            isRoute = true;
            levelCount = PlayerPrefs.GetInt("EvaluationTimes");
            closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
            goldText = transform.Find("Parent/goldnum").GetComponent<Text>();
            hpText = transform.Find("Parent/hptext").GetComponent<Text>();
            closeBtn.onClick.AddListener(BackSelect);
        }
        else
        {
            isRoute = false;
            curretText = transform.Find("Parent/time").GetComponent<Text>();
        }
    }
    //时间模式结束
    public void OpenSettle()
    {
        UIManager.Instance.isTime = true;
        gameObject.SetActive(true);
        settleParent.SetActive(false);
        timeCurr = UIManager.Instance.timeCurret;
        if (timeCurr > timeReco)
        {
            win_object.SetActive(true);
            lose_object.SetActive(false);
            old_timemode.gameObject.SetActive(false);
            new_timemode.gameObject.SetActive(true);
            new_timemode.animation.Play("new_timemode", 1);
            new_timemode.AddEventListener(EventObject.FRAME_EVENT, HideWin);
            PlayerPrefs.SetFloat("PatternTime", timeCurr);
            if (PlayerPrefs.GetFloat("TimeRecort") < CreateModel.Instance.level)
            {
                PlayerPrefs.SetFloat("TimeRecort", CreateModel.Instance.level);
            }
        }
        else
        {
            win_object.SetActive(false);
            lose_object.SetActive(true);
            new_timemode.gameObject.SetActive(false);
            old_timemode.gameObject.SetActive(true);
            old_timemode.animation.Play("old_timemode", 1);
            old_timemode.AddEventListener(EventObject.FRAME_EVENT, HideWarning);
        }
        if(timeCurr-recordTime >= 30)
        {
            int number = (int)((timeCurr - recordTime) / 30);
            UIManager.Instance.SetStar(number);
            GameManager.Instance.ClonePrompt(number, 1);
            diamText.text = number.ToString();
        }
        else
        {
            diamText.text = "0";
        }
        curretText.text = timeCurr.ToString("F1")+"s";
        UIManager.Instance.timeCurret = 0;
    }

    private void HideWarning(string type, EventObject eventObject)
    {
        lastTime = 3;
        settleParent.SetActive(true);
        old_timemode.animation.Stop();
    }

    private void HideWin(string type, EventObject eventObject)
    {
        lastTime = 3;
        settleParent.SetActive(true);
        new_timemode.animation.Stop();
    }
    //路线模式结束
    public void OpenSettle(bool isVictory,int task = 0)
    {
        if (gameObject.activeInHierarchy) return;
        this.isVictory = isVictory;
        gameObject.SetActive(true);
        closeBtn.enabled = false;
        settleParent.SetActive(false);
        if (isVictory)
        {
            takeDiam = task + 1;
            win_object.SetActive(true);
            lose_object.SetActive(false);
            old_timemode.gameObject.SetActive(false);
            new_timemode.gameObject.SetActive(true);
            new_timemode.animation.Play("win_1", 1);
            new_timemode.RemoveEventListener("complete", CloseWarning);
            new_timemode.AddEventListener(EventObject.COMPLETE, RoudeWin);
            hpText.text = CreateModel.Instance.cakeCon.Remaining();
            UIManager.Instance.routeMapPanel.PassLevel();
            GameManager.Instance.ClonePrompt(takeDiam, 1);
            levelCount += 1;
            if (levelCount >= 6)
            {
                levelCount = 0;
                UIManager.Instance.evaluatePanel.OpenPanel();
            }
            PlayerPrefs.SetInt("EvaluationTimes", levelCount);
        }
        else
        {
            win_object.SetActive(false);
            lose_object.SetActive(true);
            new_timemode.gameObject.SetActive(false);
            old_timemode.gameObject.SetActive(true);
            old_timemode.animation.Play("fail_1", 1);
            old_timemode.RemoveEventListener("complete", CloseWarning);
            old_timemode.AddEventListener(EventObject.COMPLETE, RoudeWarning);
            hpText.text = "0";
        }
        UIManager.Instance.GetRoudeGold(isVictory);
        timeText.text = ExcelTool.lang["click"];
        diamText.text = takeDiam.ToString();
        goldText.text = UIManager.Instance.routeGold.ToString("F0");
    }
    private void RoudeWin(string type, EventObject eventObject)
    {
        closeBtn.enabled = true;
        settleParent.SetActive(true);
        new_timemode.animation.Play("win_2", 1);
    }
    private void RoudeWarning(string type, EventObject eventObject)
    {
        closeBtn.enabled = true;
        settleParent.SetActive(true);
        old_timemode.animation.Play("fail_2", 1); ;
    }
    private void CloseWarning(string type, EventObject eventObject)
    {
        gameObject.SetActive(false);
        CreateModel.Instance.cakeCon.WaterAnim(false);
        if (isVictory)
        {
            UIManager.Instance.SetStar(takeDiam);
        }
        if (CreateModel.Instance.maxLevel >= 2 && PlayerPrefs.GetString("HangModel") == "")
        {
            PlayerPrefs.SetString("HangModel", "true");
            UIManager.Instance.SwitchScene("Select");
        }
        else
        {
            if (!isVictory)
            {
                if(PlayerPrefs.GetString("Guide10") == "")
                {
                    UIManager.Instance.guidePanel.OpenGuide(10, true);
                }
                else
                {
                    UIManager.Instance.failurePanel.gameObject.SetActive(true);
                }
            }
            else
            {
                UIManager.Instance.routeMapPanel.OpenPanel();
            }
        }
    }
    //返回选择大厅
    private void BackSelect()
    {
        if (isRoute)
        {
            if(isVictory)
            {
                new_timemode.animation.Play("win_3", 1);
                new_timemode.RemoveEventListener("complete", RoudeWin);
                new_timemode.AddEventListener(EventObject.COMPLETE, CloseWarning);
            }
            else
            {
                old_timemode.animation.Play("fail_3", 1);
                old_timemode.RemoveEventListener("complete", RoudeWarning);
                old_timemode.AddEventListener(EventObject.COMPLETE, CloseWarning);
            }
            closeBtn.enabled = false;
            settleParent.SetActive(false);
        }
        else
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    UIManager.Instance.SwitchScene("Select");
                    return;
                }
                if (GameManager.Instance.RequestRewardedAd(AdsType.restore).IsLoaded())
                {
                    bool isReward = false;
                    GameManager.Instance.UserChoseToWatchAd(AdsType.restore);
                    GameManager.Instance.AdmobRewardCB = delegate
                    {
                        UIManager.Instance.isTime = true;
                    };
                    GameManager.Instance.AdmobRewardClose = delegate
                    {
                        isReward = true;
                    };
                    GameManager.Instance.AdmobClose = delegate
                    {
                        if(isReward)
                        {
                            isReward = false;
                            UIManager.Instance.SetStar(1);
                            GameManager.Instance.ClonePrompt(1, 1);
                        }
                        UIManager.Instance.SwitchScene("Select");
                        GameManager.Instance.RequestRewardBasedVideo(AdsType.restore);
                    };
                }
                else
                {
                    UIManager.Instance.SwitchScene("Select");
                }
            }
            else
            {
                UIManager.Instance.SwitchScene("Select");
            }
        }
    }

    private void Update()
    {
        if(!isRoute)
        {
            if (settleParent.activeInHierarchy)
            {
                if (lastTime > 0)
                {
                    lastTime -= Time.deltaTime;
                    timeText.text = lastTime.ToString("F0");
                    if (lastTime <= 0)
                    {
                        BackSelect();
                        timeText.text = ExcelTool.lang["click"];
                    }
                }
            }
        }
    }
}
