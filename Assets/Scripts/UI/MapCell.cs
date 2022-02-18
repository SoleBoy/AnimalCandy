using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCell : MonoBehaviour
{
    public Sprite blueSprite;
    public Sprite redSprite;
    private GameObject unlck;
    private GameObject tipArraw;
    private GameObject starObject;
    private GameObject passImage;
    private Text gradeText;
    private Button button;
    private Image sp;
    private int leveIndex;
    private void Awake()
    {
        unlck = transform.Find("Image").gameObject;
        tipArraw = transform.Find("TipArraw").gameObject;
        starObject = transform.Find("Star").gameObject;
        gradeText = transform.Find("Grade").GetComponent<Text>();
        passImage = transform.Find("PassImage").gameObject;
        button = GetComponent<Button>();
        sp = GetComponent<Image>();
        button.onClick.AddListener(Click);
    }
    
    private void Click()
    {
        AudioManager.Instance.PlayTouch("other_1");
        UIManager.Instance.mapPanel.NextLevel(leveIndex);
    }
    
    void ScrollCellIndex(int index)
    {
        leveIndex = index+1;
        LevelJudge();
    }

    private void LevelJudge()
    {
        gradeText.text = leveIndex.ToString();
        tipArraw.SetActive(false);
        starObject.SetActive(false);
        if (CreateModel.Instance.level + 1 == leveIndex && CreateModel.Instance.isScene)
        {
            tipArraw.SetActive(true);
            unlck.SetActive(false);
            button.enabled = true;
            sp.sprite = redSprite;
            passImage.SetActive(PlayerPrefs.GetFloat("PerfectPass" + leveIndex) == leveIndex);
            return;
        }
        if (CreateModel.Instance.maxLevel+1 >= leveIndex)
        {
            unlck.SetActive(false);
            passImage.SetActive(PlayerPrefs.GetFloat("PerfectPass" + leveIndex) == leveIndex);
            if (CreateModel.Instance.level == leveIndex && (int.Parse(CreateModel.Instance.ReturnLevel(leveIndex).mapId) == CreateModel.Instance.sceneIndex))
            {
                if (CreateModel.Instance.isScene)
                {
                    button.enabled = true;
                    sp.sprite = blueSprite;
                }
                else
                {
                    button.enabled = false;
                    sp.sprite = redSprite;
                }
                if (CreateModel.Instance.maxLevel >= leveIndex)
                {
                    starObject.SetActive(true);
                }
            }
            else
            {
                button.enabled = true;
                sp.sprite = blueSprite;
                if (CreateModel.Instance.maxLevel >= leveIndex)
                {
                    starObject.SetActive(true);
                }
            }
        }
        else if(CreateModel.Instance.maxLevel < leveIndex)
        {
            unlck.SetActive(true);
            button.enabled = false;
        }
    }
    private void OnEnable()
    {
        if (leveIndex != 0)
        {
            LevelJudge();
           // Debug.Log("开启调用");
        }
    }
}
