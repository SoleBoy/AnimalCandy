using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoPanel : MonoBehaviour
{
    private PreviewInfo[] enemyInfo;
    private Text levelText;
    private float hight;
    private float timeCount;
    private Transform parent;
    private Image hideMask;
    private GameObject closeText;
    private Button closeBtn;
    public void Init(int index)
    {
        timeCount = index;
        hight = Screen.height;
        parent = transform.Find("Prent");
        hideMask = GetComponent<Image>();
        closeBtn = GetComponent<Button>();
        closeText = transform.Find("Prent/CloseText").gameObject;
        levelText = transform.Find("Prent/LevelText").GetComponent<Text>();

        closeBtn.onClick.AddListener(ClosePanel);
        Transform headPrent = transform.Find("Prent/Head");
        enemyInfo = new PreviewInfo[headPrent.childCount];
        for (int i = 0; i < headPrent.childCount; i++)
        {
            enemyInfo[i] = headPrent.GetChild(i).GetComponent<PreviewInfo>();
        }
        headPrent = null;
    }
    public void SetArmsInfo(LineItem item,float enemyNum,float enemyLvel)
    {
        gameObject.SetActive(true);
        parent.localPosition = Vector3.up * hight;
        for (int i = 0; i < enemyInfo.Length; i++)
        {
            enemyInfo[i].gameObject.SetActive(false);
        }
        levelText.text = item.id;
        string[] grade = item.line_enemy_level.Split('|');
        string[] number = item.line_enemy_num.Split('|');
        string[] enemy = item.line_enemy_type.Split('|');
        for (int i = 0; i < enemy.Length; i++)
        {
            if(i < enemyInfo.Length)
            {
                if(i >= enemy.Length - 1)
                {
                    //  enemyLevel = int.Parse(level_troops[wave_curret]) * enemymultiple;
                    //enemyCount = Mathf.CeilToInt(int.Parse(number_troops[wave_curret]) * createModel.enemyMultiple);
                    enemyInfo[enemyInfo.Length - 1].SetInfo(ExcelTool.Instance.enemys[enemy[i]],float.Parse(grade[i])*enemyLvel, Mathf.CeilToInt(float.Parse(number[i]) * enemyNum).ToString());
                }
                else
                {
                    enemyInfo[i].SetInfo(ExcelTool.Instance.enemys[enemy[i]], float.Parse(grade[i]) * enemyLvel, Mathf.CeilToInt(float.Parse(number[i]) * enemyNum).ToString());
                }
            }
        }
        closeBtn.enabled = false;
        hideMask.DOFade(0, 0.5f);
        closeText.SetActive(false);
        UIManager.Instance.isTime = true;
        StartCoroutine(Animator());
    }

    IEnumerator Animator()
    {
        yield return new WaitForSeconds(timeCount);
        hideMask.DOFade(0.6f, 0.5f);
        parent.DOLocalMoveY(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        closeBtn.enabled = true;
        closeText.SetActive(true);
    }

    private void ClosePanel()
    {
        StartCoroutine(CloseAnimal());
    }

    private IEnumerator CloseAnimal()
    {
        parent.DOLocalMoveY(-hight, 0.5f);
        hideMask.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        UIManager.Instance.DetectionPanel();
        UIManager.Instance.guidePanel.OpenGuide(2, true);
    }
}
