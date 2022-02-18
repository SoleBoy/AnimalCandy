using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyAnimalsPanel : MonoBehaviour
{
    private Transform animalPrefab;
    private Transform animalParent;

    private Button backBtn;
    private BuyAnimals[] buyAnimals;
    public void Init()
    {
        int count = ExcelTool.Instance.animalSprite.Length;
        animalPrefab = transform.Find("Image/Animal");
        animalParent = transform.Find("Image/Scroll View").GetComponent<ScrollRect>().content;
        buyAnimals = new BuyAnimals[count];
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(ClosePanel);
        int index = -2;
        for (int i = 0; i < count; i++)
        {
            BuyAnimals head = new BuyAnimals();
            var animal = Instantiate(animalPrefab);
            animal.gameObject.SetActive(true);
            animal.SetParent(animalParent);
            animal.localPosition=Vector3.zero;
            animal.localScale=Vector3.one;
            index += 3;
            head.InitAnimal(animal, ExcelTool.Instance.animalSprite[i], i, index);
            buyAnimals[i] = head;
        }
    }

    public void HeadAnimals()
    {
        for (int i = 0; i < buyAnimals.Length; i++)
        {
            buyAnimals[i].SetHead();
        }
    }

    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
    }
}


public class BuyAnimals
{
    private Button buyBtn;
    private Text numberText;
    private Image headImage;
    private int indexAnimal;
    private int numberAnimal;
    private int demandAnimal;
    private string messgAniaml;
    private string messgTip;
    private GameObject tipGame;
    private GameObject shadeGame;
    public void InitAnimal(Transform parent,Sprite head,int index,int Demand)
    {
        demandAnimal = Demand;
        indexAnimal = index;
        messgAniaml = "Construction" + indexAnimal;
        messgTip= ExcelTool.lang[string.Format("taskname{0}", index + 6)]+"+"+1;
        parent.Find("Demand").GetComponent<Text>().text = demandAnimal.ToString();
        numberText =parent.Find("Number").GetComponent<Text>();
        headImage = parent.Find("Head").GetComponent<Image>();
        buyBtn = parent.GetComponent<Button>();
        tipGame = parent.Find("TipCone").gameObject;
        shadeGame = parent.Find("Shade").gameObject;
        headImage.sprite = head;
        headImage.SetNativeSize();
        numberAnimal = PlayerPrefs.GetInt(messgAniaml);
        numberText.text = numberAnimal.ToString();
        buyBtn.onClick.AddListener(BuyHead);
        tipGame.SetActive(false);
        SetHead();
        if (indexAnimal < 1 && UIBase.Instance.diamond >= demandAnimal)
        {
            tipGame.SetActive(true);
        }
    }

    public void SetHead()
    {
        if (UIBase.Instance.diamond >= demandAnimal)
        {
            shadeGame.SetActive(false);
            buyBtn.enabled = true;
            if (indexAnimal == 0)
            {
                tipGame.SetActive(true);
            }
        }
        else
        {
            buyBtn.enabled = false;
            shadeGame.SetActive(true);
            if (indexAnimal == 0)
            {
                tipGame.SetActive(false);
            }
        }
    }

    private void BuyHead()
    {
        if(UIBase.Instance.diamond >= demandAnimal)
        {
            numberAnimal += 1;
            numberText.text = numberAnimal.ToString();
            PlayerPrefs.SetInt(messgAniaml, numberAnimal);
            UIBase.Instance.SetDiamond(-demandAnimal);
            UIBase.Instance.animalHead.SetHead(indexAnimal, 1);
            UIBase.Instance.SetAnimals(1);
            GameManager.Instance.CloneTip(messgTip);
            if(tipGame.activeInHierarchy)
                tipGame.SetActive(false);
        }
        else
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
}
