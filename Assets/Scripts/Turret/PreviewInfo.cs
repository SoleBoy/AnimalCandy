using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewInfo : MonoBehaviour
{
    private Text nameText;
    private Text soulText;
    private Text candyText;
    private Text hpText;
    private Text numText;
    private Text resisText;

    private Image headSpite;
    private Image resisSprite;

    private string messgName;
    private string messgResis;
    private void Awake()
    {
        headSpite = transform.Find("Image").GetComponent<Image>();
        resisSprite = transform.Find("Resis").GetComponent<Image>();

        resisText = transform.Find("Resis/Text").GetComponent<Text>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        candyText = transform.Find("CandyType/Text").GetComponent<Text>();
        soulText = transform.Find("SoulType/Text").GetComponent<Text>();
        hpText = transform.Find("HpType/Text").GetComponent<Text>();
        numText = transform.Find("NumText").GetComponent<Text>();
        ExcelTool.LanguageEvent += CutLang;
    }

    private void CutLang()
    {
        if (messgName == null || messgResis == null) return;
        nameText.text = ExcelTool.lang[messgName];
        resisText.text = ExcelTool.lang[messgResis];
    }

    public void SetInfo(EnemyItem enemyItem,float level,string num)
    {
        gameObject.SetActive(true);
        int index = int.Parse(enemyItem.id.Substring(2)) - 1;
        headSpite.sprite = ExcelTool.Instance.animalSprite[index];
        headSpite.SetNativeSize();
        numText.text = num;
        messgName = string.Format("taskname{0}", (index + 6));
        nameText.text = ExcelTool.lang["taskname" + (index+6)];
        if(enemyItem.shoot_number == -1)
        {
            candyText.text = "0";
        }
        else
        {
            candyText.text = (enemyItem.shoot_number * CreateModel.Instance.enemyAttack).ToString("F0");
        }
        soulText.text = enemyItem.soul.ToString("F0");
        hpText.text=(enemyItem.hp + enemyItem.hp * enemyItem.hp_growth * 0.01f * (level - 1)).ToString("F0");
        SetResistance(enemyItem.def_spe_type);
    }

    private void SetResistance(float index)
    {
        if (index == -1)
        {
            resisSprite.color = Color.white;
            messgResis = "resis1";
            resisText.text = ExcelTool.lang["resis1"];
        }
        else if (index == 1)
        {
            resisSprite.color = Color.green;
            messgResis = "resis2";
            resisText.text = ExcelTool.lang["resis2"];
        }
        else if (index == 2)
        {
            resisSprite.color = Color.blue;
            messgResis = "resis3";
            resisText.text = ExcelTool.lang["resis3"];
        }
        else if (index == 3)
        {
            resisSprite.color = Color.red;
            messgResis = "resis4";
            resisText.text = ExcelTool.lang["resis4"];
        }
        else if (index == 4)
        {
            resisSprite.color = Color.magenta;
            messgResis = "resis5";
            resisText.text = ExcelTool.lang["resis5"];
        }
    }
}
