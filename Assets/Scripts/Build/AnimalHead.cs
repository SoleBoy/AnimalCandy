using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHead : MonoBehaviour
{
    public Sprite headBright;
    public Sprite headNormal;
    public Sprite headDark;

    private Button clickBtn;
    private Image bgSprite;
    private Image headSprite;
    private Text numText;
    private GameObject Shade;
    private GameObject tipCone;

    private int headIndex;
    private int number;
    public float headScale;
    public bool IsCreate
    {
        get => number > 0;
    }
    public void InIt(Sprite sp, int index)
    {
        bgSprite = transform.GetComponent<Image>();
        headSprite = transform.Find("Image").GetComponent<Image>();
        clickBtn = transform.GetComponent<Button>();
        numText = transform.Find("Text").GetComponent<Text>();
        Shade = transform.Find("Shade").gameObject;
        tipCone = transform.Find("Tip").gameObject;
        clickBtn.onClick.AddListener(ClickAnimal);
        headSprite.sprite = sp;
        headSprite.SetNativeSize();
        number = PlayerPrefs.GetInt("Construction"+index);
        UIBase.Instance.sumAnimal += number;
        numText.text = number.ToString();
        this.headIndex = index;
        string bodyName = "1001";
        if (index < 9)
        {
            bodyName = string.Format("100{0}", index+1);
        }
        else
        {
            bodyName = string.Format("10{0}", index+1);
        }
        headScale = ExcelTool.Instance.enemys[bodyName].body_type;
        if (IsCreate)
        {
            if(index == 0)
            {
                bgSprite.sprite = headBright;
            }
            else
            {
                bgSprite.sprite = headNormal;
            }
            clickBtn.enabled = true;
            Shade.SetActive(false);
        }
        else
        {
            clickBtn.enabled = false;
            Shade.SetActive(true);
            bgSprite.sprite = headDark;
        }
        tipCone.SetActive(false);
    }

    public void ChooseLargest(bool isHide)
    {
        tipCone.SetActive(isHide);
    }

    public void SetNumber(int add)
    {
        number += add;
        if (number > 0)
        {
            clickBtn.enabled = true;
            Shade.SetActive(false);
            bgSprite.sprite = headNormal;
        }
        else
        {
            number = 0;
            Shade.SetActive(true);
            clickBtn.enabled = false;
            bgSprite.sprite = headDark;
        }
        numText.text = number.ToString();
    }
    public void SetBgSprite()
    {
        if (number > 0)
        {
            bgSprite.sprite = headNormal;
        }
        else
        {
            bgSprite.sprite = headDark;
        }
    }
    //选中
    public void ClickAnimal()
    {
        AudioManager.Instance.PlayTouch("other_1");
        bgSprite.sprite = headBright;
        UIBase.Instance.CreateShadow(headIndex, headScale);
    }
}
