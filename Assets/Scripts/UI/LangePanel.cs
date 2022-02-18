using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangePanel : MonoBehaviour
{
    public Sprite noChecked; 
    public Sprite unChecked;
    public Button[] langeBtns;
    private int count;
    string[] lang = { "package_en" , "package_jp" , "package_cn" , "package_big", "package_kor" , "package_rus" };
    public void Init()
    {
        JudeLang();
        langeBtns[count].GetComponent<Image>().sprite = noChecked;
        langeBtns[count].enabled = false;
        AffirmClick();
    }

    public void UpdataBtn(int index)
    {
        AudioManager.Instance.PlayTouch("other_1");
        langeBtns[count].GetComponent<Image>().sprite = unChecked;
        langeBtns[count].enabled=true;
        this.count = index;
        langeBtns[index].GetComponent<Image>().sprite = noChecked;
        langeBtns[index].enabled = false;
        AffirmClick();
    }
    void JudeLang()
    {
        string name = PlayerPrefs.GetString("PackageLanguage", "package_en");
        for (int i = 0; i < lang.Length; i++)
        {
            if(lang[i] == name)
            {
                count = i;
                break;
            }
        }
    }
    public void AffirmEvent()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        if(UIManager.Instance)
        {
            UIManager.Instance.DetectionPanel();
        }
        AffirmClick();
    }
    private void AffirmClick()
    {
        ExcelTool.Instance.CutLanguage(lang[count]);
        PlayerPrefs.SetString("PackageLanguage", lang[count]);
    }

    private void OnEnable()
    {
        GameManager.Instance.ShowBanner();
    }

    private void OnDisable()
    {
        GameManager.Instance.HideBanner();
    }
}
