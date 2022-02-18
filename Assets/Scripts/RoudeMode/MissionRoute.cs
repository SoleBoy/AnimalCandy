using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionRoute : MonoBehaviour
{
    private Text[] mission_text=new Text[3];

    private Image[] mission_image = new Image[3];

    private Button confirmBtn;

    private int skillindex;
    private int turretIndex;
    private int taskIndex;

    private string[] taskID ;
    private string[] taskNum ;
    private string taskMessg;
    private string taskId;
    private bool isFinish;
    private void Awake()
    {
        mission_text[0] = transform.Find("plate/mission1/Text").GetComponent<Text>();
        mission_text[1] = transform.Find("plate/mission2/Text").GetComponent<Text>();
        mission_text[2] = transform.Find("plate/mission3/Text").GetComponent<Text>();
        mission_image[0]= transform.Find("plate/mission1").GetComponent<Image>();
        mission_image[1] = transform.Find("plate/mission2").GetComponent<Image>();
        mission_image[2] = transform.Find("plate/mission3").GetComponent<Image>();
        confirmBtn = transform.Find("Button").GetComponent<Button>();
        confirmBtn.onClick.AddListener(NextConfirm);
    }

    private void NextConfirm()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        if (isFinish)
        {
            UIManager.Instance.settlePanel.OpenSettle(true,taskIndex);
        }
        else
        {
            UIManager.Instance.lockSkill.Clear();
            CreateModel.Instance.productions.Clear();
            UIManager.Instance.skillPanel.OpenSelected(skillindex);
            UIManager.Instance.turretPanel.OpenSelected(turretIndex);
        }
    }

    public void SetMission(LineItem item,string messg)
    {
        taskMessg = messg;
        gameObject.SetActive(true);
        isFinish = false;
        taskId = item.id;
        skillindex = int.Parse(item.line_skill);
        turretIndex = int.Parse(item.line_shooter);
        taskID = item.battlefield_mission_type.Split('|');
        taskNum = item.battlefield_mission_id_num.Split('|');
        string taskName = "";
        for (int i = 0; i < mission_image.Length; i++)
        {
            taskName = string.Format("Model{0}Pass{1}Mission{2}", taskMessg,taskId, taskID[i]);
            if (PlayerPrefs.GetString(taskName) != "true")
            {
                mission_image[i].color = Color.black;
            }
            else
            {
                mission_image[i].color = Color.white;
            }
        }
        TaskIntroduction();
    }

    private void TaskIntroduction()
    {
        for (int i = 0; i < mission_image.Length; i++)
        {
            mission_text[i].text = TaskType(taskID[i], taskNum[i]);
        }
    }

    //过关任务判断
    public void TaskCompletion()
    {
        string taskName = "";
        isFinish = true; taskIndex = 0;
        gameObject.SetActive(true);
        for (int i = 0; i < mission_image.Length; i++)
        {
            taskName = string.Format("Model{0}Pass{1}Mission{2}", taskMessg,taskId, taskID[i]);
            if (PlayerPrefs.GetString(taskName) != "true")
            {
                if(TaskSchedule(taskID[i], taskNum[i]))
                {
                    taskIndex += 1;
                    mission_image[i].color = Color.white;
                    PlayerPrefs.SetString(taskName, "true");
                }
                else
                {
                    mission_image[i].color = Color.black;
                }
            }
            else
            {
                mission_image[i].color = Color.white;
            }
        }
        TaskIntroduction();
    }
    //任务类型
    private string TaskType(string Id, string num)
    {
        switch (Id)
        {
            case "1":
                return string.Format("{0}:{1}",ExcelTool.lang["routetask1"] ,num);
            case "2":
                return string.Format("{0}:{1}", ExcelTool.lang["routetask2"], num);
            case "3":
                string[] skills = num.Split(',');
                string messgskill = "";
                for (int i = 0; i < skills.Length; i++)
                {
                    messgskill += ExcelTool.lang["skillrealname" + skills[i]];
                    if (i < skills.Length - 1)
                    {
                        messgskill += ",";
                    }
                }
                return string.Format("{0}:{1}", ExcelTool.lang["routetask3"], messgskill);
            case "4"://turretrealname6
                string[] turrets = num.Split(',');
                string messg = "";
                for (int i = 0; i < turrets.Length; i++)
                {
                    messg += ExcelTool.lang["turretrealname"+int.Parse(turrets[i].Substring(2))];
                    if(i < turrets.Length-1)
                    {
                        messg += ",";
                    }
                }
                return string.Format("{0}:{1}", ExcelTool.lang["routetask4"], messg);
            case "5":
                return string.Format("{0}:{1}s", ExcelTool.lang["routetask5"], num);
            case "6":
                return ExcelTool.lang["routetask6"];
            case "7":
                return ExcelTool.lang["routetask7"];
            case "8":
                return string.Format("{0}:{1}", ExcelTool.lang["routetask8"], num);
            case "9":
                return ExcelTool.lang["routetask9"];
            default:
                return "";
        }
    }

    public bool TaskSchedule(string Id, string num)
    {
        switch (Id)
        {
            case "1":
                return float.Parse(num) <= UIManager.Instance.routeGold;
            case "2":
                return float.Parse(num) >= UIManager.Instance.skillNumber;
            case "3":
                string[] skill = num.Split(',');//技能种类
                if (skill.Length >= UIManager.Instance.lockSkill.Count)
                {
                    for (int i = 0; i < UIManager.Instance.lockSkill.Count; i++)
                    {
                        if (!GetContain(skill, UIManager.Instance.lockSkill[i].ToString()))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            case "4":
                string[] turret = num.Split(',');//炮塔种类
                if (turret.Length >= CreateModel.Instance.productions.Count)
                {
                    for (int i = 0; i < CreateModel.Instance.productions.Count; i++)
                    {
                        string index = "30";
                        if (CreateModel.Instance.productions[i] <= 10)
                        {
                            index += "0" + (CreateModel.Instance.productions[i] + 1).ToString();
                        }
                        else
                        {
                            index += (CreateModel.Instance.productions[i] + 1).ToString();
                        }
                        if (!GetContain(turret, index))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            case "5":
                return float.Parse(num) >= UIManager.Instance.timeCurret;
            case "6":
                return 0 >= UIManager.Instance.skillNumber;
            case "7":
                return 1 >= CreateModel.Instance.productions.Count;
            case "8":
                return float.Parse(num) >= CreateModel.Instance.routeMode.terretMax;
            case "9":
                return CreateModel.Instance.cakeCon.FullBlood();
            default:
                return false;
        }
    }

    bool GetContain(string[] messg, string index)
    {
        for (int i = 0; i < messg.Length; i++)
        {
            if (messg[i] == index)
            {
                return true;
            }
        }
        return false;
    }
}
