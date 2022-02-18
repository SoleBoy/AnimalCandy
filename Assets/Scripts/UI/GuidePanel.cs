using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    Text titleText;
    Button closeBtn;
    int guideIndex = 1;
    int oneIndex=0;
    float lastTime;
    List<int> count = new List<int>();
    string nameT;
    public void Init(string messg)
    {
        titleText = transform.Find("Text").GetComponent<Text>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        if(messg=="Guide")
        {
            closeBtn.onClick.AddListener(ClosePanel);
        }
        else
        {
            closeBtn.onClick.AddListener(BrackPanel);
        }
        nameT = messg;
    }
    void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        if (lastTime <= 0)
        {
            if(guideIndex == 1)
            {
                transform.GetChild(guideIndex).GetChild(oneIndex).gameObject.SetActive(false);
                oneIndex++;
                if (oneIndex <= 2)
                {
                    lastTime = 1;
                    transform.GetChild(guideIndex).GetChild(oneIndex).gameObject.SetActive(true);
                    return;
                }
                PlayerPrefs.SetString("LangePanel", "AffirmEvent");
            }
            count.Remove(guideIndex);
            if(guideIndex != 2 && guideIndex != 21 && guideIndex != 4)
            {
                PlayerPrefs.SetString(nameT + guideIndex,"index");
            }
            transform.GetChild(guideIndex).gameObject.SetActive(false);
            if (count.Count > 0)
            {
                lastTime = 1;
                guideIndex = count[0];
                transform.GetChild(guideIndex).gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
                if (guideIndex == 16)
                {
                    UIManager.Instance.mapPanel.OpenPanel(false);
                }
                else if(guideIndex == 14)
                {
                    UIManager.Instance.taskPanel.SetPanel(false,false);
                }
                else if(guideIndex == 10)
                {
                    UIManager.Instance.failurePanel.gameObject.SetActive(true);
                }
                else if (guideIndex == 18)
                {
                    UIManager.Instance.SwitchScene("Build");
                }
                else
                {
                    UIManager.Instance.DetectionPanel();
                }
                if (guideIndex == 2)
                {
                    CreateModel.Instance.sendTroops = true;
                    UIManager.Instance.GuideInfo(ExcelTool.lang["guideinfo2_1"]+":"+ ExcelTool.lang["guideinfo2_2"]);
                }
            }
        }
    }

    void BrackPanel()
    {
        if(lastTime <= 0)
        {
            AudioManager.Instance.PlayTouch("close_1");
            count.Remove(guideIndex);
            PlayerPrefs.SetString(nameT + guideIndex, guideIndex.ToString());
            transform.GetChild(guideIndex).gameObject.SetActive(false);
            if (count.Count > 0)
            {
                lastTime = 1;
                guideIndex = count[0];
                transform.GetChild(guideIndex).gameObject.SetActive(true);
            }
            else
            {
                if (guideIndex == 5)
                {
                    OpenGuide(9, false);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void OpenGuide(int index,bool isPause)
    {
        if (PlayerPrefs.GetString(nameT + index) == "" && !count.Contains(index))
        {
            if(count.Count <= 0)
            {
                lastTime = 1;
                guideIndex = index;
                gameObject.SetActive(true);
                transform.GetChild(guideIndex).gameObject.SetActive(true);
                if (index == 1)
                {
                    transform.GetChild(guideIndex).GetChild(oneIndex).gameObject.SetActive(true);
                }
                if (UIManager.Instance)
                    UIManager.Instance.isTime = true;
            }
            count.Add(index);
        }
    }

    private void Update()
    {
        if (lastTime >= 0)
        {
            lastTime -= Time.deltaTime;
            if (lastTime <= 0)
            {
                titleText.text = ExcelTool.lang["click"];
            }
            else
            {
                titleText.text = lastTime.ToString("F0");
            }
        }
    }
}
