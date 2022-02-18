using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailurePanel : MonoBehaviour
{
    Button closeBtn;
    Button atkBtn;
    Button earBtn;
    Button goldBtn;
    Button starBtn;
    Button upBtn;
    Button storeBtn;
    Button taskBtn;
    Button selectBtn;
    Text timeText;
    private float timeDown;
    private float lastTime;
    private bool isClick;
    private bool isModel;
    public void Init(bool isroude)
    {
        isModel = isroude;
        timeText = transform.Find("TimeText").GetComponent<Text>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        atkBtn = transform.Find("atk/Button").GetComponent<Button>();
        earBtn = transform.Find("earnings/Button").GetComponent<Button>();
        goldBtn = transform.Find("gold/Button").GetComponent<Button>();
        starBtn = transform.Find("star/Button").GetComponent<Button>();
        upBtn = transform.Find("up/Button").GetComponent<Button>();
        storeBtn = transform.Find("Store/Button").GetComponent<Button>();
        taskBtn = transform.Find("Task/Button").GetComponent<Button>();
        selectBtn = transform.Find("Select/Button").GetComponent<Button>();

        closeBtn.onClick.AddListener(CloseClick);
        atkBtn.onClick.AddListener(DoubleAttack);
        earBtn.onClick.AddListener(DoubleIncome);
        goldBtn.onClick.AddListener(RewardGold);
        starBtn.onClick.AddListener(RewardStar);
        upBtn.onClick.AddListener(UpTurret);
        storeBtn.onClick.AddListener(GoStore);
        taskBtn.onClick.AddListener(GoTask);
        selectBtn.onClick.AddListener(GoSelect);
    }
    private void OnEnable()
    {
        //Debug.Log("失败面板");
        isClick = true;
        timeDown = 15;
        lastTime = 0;
        timeText.text = timeDown.ToString("F0") + "s";
    }
    private void Update()
    {
        if(gameObject.activeInHierarchy && isClick)
        {
            if (timeDown >= 0)
            {
                lastTime += Time.deltaTime;
                if(lastTime >= 1)
                {
                    timeDown -= lastTime;
                    timeText.text = timeDown.ToString("F0")+"s";
                    if (timeDown <= 0)
                    {
                        CloseClick();
                    }
                    lastTime = 0;
                }
            }
        }
    }
    private void GoSelect()
    {
        isClick = false;
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.SwitchScene("Select");
    }

    private void GoStore()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isClick = false;
        UIManager.Instance.storePanel.OpenPanel(0);
    }
    private void GoTask()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isClick = false;
        UIManager.Instance.taskPanel.SetPanel(false,false);
    }

    private void RewardStar()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isClick = false;
        UIManager.Instance.everydayPanel.gameObject.SetActive(true);
    }

    private void UpTurret()
    {
        AudioManager.Instance.PlayTouch("other_1");
        isClick = false;
        UIManager.Instance.turretPanel.OpenPanel();
    }

    private void DoubleAttack()
    {
        AudioManager.Instance.PlayTouch("ads_1");
        isClick = false;
        UIManager.Instance.VideoAttak();
    }

    private void DoubleIncome()
    {
        AudioManager.Instance.PlayTouch("ads_1");
        isClick = false;
        UIManager.Instance.VideoEarnings();
    }

    private void RewardGold()
    {
        AudioManager.Instance.PlayTouch("ads_1");
        isClick = false;
        UIManager.Instance.VideoSweet();
    }
    public void RewardTiming(bool isBtn)
    {
        if (isBtn)
        {
            goldBtn.enabled = true;
            goldBtn.GetComponent<Image>().color = Color.white;
        }
        else
        {
            goldBtn.enabled = false;
            goldBtn.GetComponent<Image>().color = Color.gray;
        }
    }
    public void BackPanel()
    {
        gameObject.SetActive(false);
        if (isModel)
        {
            UIManager.Instance.routeMapPanel.OpenPanel();
        }
        else
        {
            CreateModel.Instance.LevelJudge(false);
        }
    }

    private void CloseClick()
    {
        AudioManager.Instance.PlayTouch("close_1");
        UIManager.Instance.DetectionPanel();
    }

}
