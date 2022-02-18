using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPanel : MonoBehaviour
{
    Button closeBtn;
    Text titleText;
    Text realText;
    Text levelText;
    Text timeText;
    private Image modelImage;
    private Transform MaskCandy;
    private DragonBones.UnityArmatureComponent model_Armature;
    private string messgInfo;
    private bool isTurret;
    private int turretindex;
    private void Awake()
    {
        MaskCandy = transform.Find("MaskCandy");
        closeBtn = transform.Find("BGImage").GetComponent<Button>();
        titleText = transform.Find("Title").GetComponent<Text>();
        realText = transform.Find("Real").GetComponent<Text>();
        levelText = transform.Find("Level").GetComponent<Text>();
        timeText = transform.Find("Time").GetComponent<Text>();
        modelImage = transform.Find("Sprite").GetComponent<Image>();
        model_Armature = MaskCandy.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>();
        closeBtn.onClick.AddListener(ClosePanel);
    }
    public void Init(int id,float level,bool isTurret,int index,string messg,Sprite sprite)
    {
        this.turretindex = index;
        this.isTurret = isTurret;
        this.messgInfo = messg;
        if (isTurret)
        {
            if(sprite)
            {
                realText.text = ExcelTool.lang["cakerealname"];
            }
            else
            {
                realText.text = ExcelTool.lang["turretrealname" + id];
            }
            titleText.text = ExcelTool.lang["turret"];
        }
        else
        {
            realText.text = ExcelTool.lang["skillrealname" + id];
            titleText.text = ExcelTool.lang["skill"];
        }
        if(sprite)
        {
            model_Armature.gameObject.SetActive(false);
            modelImage.gameObject.SetActive(true);
            modelImage.sprite = sprite;
            modelImage.SetNativeSize();
        }
        else
        {
            model_Armature.gameObject.SetActive(true);
            modelImage.gameObject.SetActive(false);
            SetKeel(id, isTurret);
        }
        timeText.text = ExcelTool.lang["click"];
        for (int i = 0; i < ExcelTool.Instance.lockLevel.Count; i++)
        {
            if (level < ExcelTool.Instance.lockLevel[i])
            {
                levelText.text = ExcelTool.Instance.lockLevel[i].ToString();
                break;
            }
            else
            {
                levelText.text = "999";
            }
        }
    }

    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        if(isTurret)
        {
            UIManager.Instance.turretPanel.OpenPanel(turretindex);
        }
        else
        {
            UIManager.Instance.skillPanel.OpenPanel(turretindex);
        }
        PlayerPrefs.SetString(messgInfo, "unlock");
        gameObject.SetActive(false);
    }
    //candy_1_1 龙骨动画
    public void SetKeel(int id, bool isTurret)
    {
        string keelName;
        if (isTurret)
        {
            keelName = string.Format("candy_{0}_1", id);
        }
        else
        {
            if (id < 10)
            {
                keelName = string.Format("candy_10{0}_1", id);
            }
            else
            {
                keelName = string.Format("candy_1{0}_1", id);
            }
        }
        model_Armature = UIManager.Instance.SetArmature(model_Armature,MaskCandy, keelName, Vector3.one * 70, Vector3.zero, true, "rest");
    }
}
