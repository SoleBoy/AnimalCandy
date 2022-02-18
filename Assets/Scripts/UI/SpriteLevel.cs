using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteLevel : MonoBehaviour
{
    public Text levelText;
    public Button boosBtn;
    public Sprite boosSp;
    public Sprite enemySp;

    void Awake()
    {
        levelText = transform.Find("Text").GetComponent<Text>();
        boosBtn = transform.Find("BossBtn").GetComponent<Button>();
        boosBtn.onClick.AddListener(ClickBoos);
    }
    public void SetLevelText(float level)
    {
        levelText.text = level.ToString();
        if (float.Parse(levelText.text)%5 == 0)
        {
            GetComponent<Image>().sprite = boosSp;
        }
        else
        {
            GetComponent<Image>().sprite = enemySp;
        }
    }
    void ClickBoos()
    {
        KeelAnimation.Instance.Stop();
        AudioManager.Instance.PlayTouch("bossicon_1");
        CreateModel.Instance.GlobalDestruction();
        CreateModel.Instance.LevelJudge(true);
    }
    public void BossChallenge(bool isHide)
    {
        boosBtn.enabled = true;
        boosBtn.gameObject.SetActive(isHide);
    }
}
