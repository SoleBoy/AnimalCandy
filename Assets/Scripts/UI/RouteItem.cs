using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteItem : MonoBehaviour
{
    private Image[] hearts = new Image[3];
    private Button selectBtn;
    private Text gradeText;

    private GameObject starObject;
    private GameObject perfect;
    private GameObject normaleState;
    private GameObject highState;
    private GameObject lockState;
    private GameObject tipArraw;

    private int gradeIndex;
    private int heartIndex;
    private LineItem lineItem;
    private void Awake()
    {
        selectBtn = GetComponent<Button>();
        gradeText = transform.Find("grade").GetComponent<Text>();
        hearts[0] = transform.Find("Heard/heart1").GetComponent<Image>();
        hearts[1] = transform.Find("Heard/heart2").GetComponent<Image>();
        hearts[2] = transform.Find("Heard/heart3").GetComponent<Image>();

        starObject = transform.Find("star").gameObject;
        perfect = transform.Find("perfect").gameObject;
        normaleState = transform.Find("normale").gameObject;
        highState = transform.Find("high").gameObject;
        lockState = transform.Find("lock").gameObject;
        tipArraw = transform.Find("TipArraw").gameObject;

        selectBtn.onClick.AddListener(SelectClick);
    }

    //private void OnEnable()
    //{
    //    if (gradeIndex != 0)
    //    {
    //        LevelJudge();
    //    }
    //}

    private void SelectClick()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.routeMapPanel.SelectLevel(gradeIndex);
    }
   
    public void SetGrade(int index)
    {
        lineItem = ExcelTool.Instance.lines[index.ToString()];
        gradeIndex = index;
        if(index == 0)
        {
            gradeText.text = ExcelTool.lang["teacpass"];
            hearts[0].transform.parent.gameObject.SetActive(false);
            perfect.SetActive(false);
        }
        else
        {
            gradeText.text = index.ToString();
        }
        //LevelJudge();
    }

    public void SetLanguage()
    {
        gradeText.text = ExcelTool.lang["teacpass"];
    }

    public bool GetPerfect()
    {
        return heartIndex >= 3;
    }

    public void LevelJudge(string messg,float maxLevel)
    {
        starObject.SetActive(false);
        if (maxLevel + 1 >= gradeIndex)
        {
            JudeTask(messg);
            selectBtn.enabled = true;
            normaleState.SetActive(true);
            lockState.SetActive(false);
           
            if (maxLevel >= gradeIndex)
            {
                tipArraw.gameObject.SetActive(false);
                starObject.SetActive(true);
            }
            else if(maxLevel + 1 == gradeIndex)
            {
                tipArraw.gameObject.SetActive(true);
            }
        }
        else
        {
            lockState.SetActive(true);
            selectBtn.enabled = false;
            tipArraw.gameObject.SetActive(false);
        }
    }
    private void JudeTask(string messg)
    {
        //if(perfect.activeInHierarchy) return;
        heartIndex = 0;
        perfect.SetActive(false);
        string[] taskID = lineItem.battlefield_mission_type.Split('|');
        string taskName = "";
        for (int i = 0; i < taskID.Length; i++)
        {
            taskName = string.Format("Model{0}Pass{1}Mission{2}", messg, gradeIndex, taskID[i]);
            hearts[i].gameObject.SetActive(true);
            if (PlayerPrefs.GetString(taskName) == "true")
            {
                heartIndex++;
                hearts[i].color = Color.white;
            }
            else
            {
                hearts[i].color = Color.black;
            }
        }
        if (heartIndex >= 3)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].gameObject.SetActive(false);
            }
            perfect.SetActive(true);
        }
    }
}
