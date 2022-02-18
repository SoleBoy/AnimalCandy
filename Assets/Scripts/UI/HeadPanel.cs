using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadPanel : MonoBehaviour
{
    private Text sumText;
    private Text currentText;

    private Button closeBtn;

    private Transform headPrefab;
    private Transform headPrent;

    private List<HeadAnimal> headAnimals=new List<HeadAnimal>();
    public void Init()
    {
        int count = ExcelTool.Instance.animalSprite.Length;
        sumText = transform.Find("Sum/Text").GetComponent<Text>();
        currentText = transform.Find("CurrentText").GetComponent<Text>();
        closeBtn = transform.Find("DrawBtn").GetComponent<Button>();

        headPrefab = transform.Find("HeadPrefab");
        headPrent = transform.Find("Scroll View").GetComponent<ScrollRect>().content;

        closeBtn.onClick.AddListener(ClosePanel);
        for (int i = 0; i < count; i++)
        {
            var head = Instantiate(headPrefab);
            head.SetParent(headPrent);
            head.localScale = Vector3.one;
            head.localPosition = Vector3.zero;
            HeadAnimal animal = new HeadAnimal();
            animal.Init(head, ExcelTool.Instance.animalSprite[i],i);
            headAnimals.Add(animal);
        }
    }
    
    public void CreateHead(float sum,List<int> counts)
    {
        gameObject.SetActive(true);
        sumText.text = sum.ToString("F0");
        currentText.text = ExcelTool.lang["thistime"]+"+" + counts.Count;
        for (int i = 0; i < counts.Count; i++)
        {
            headAnimals[counts[i]].SetNode(i);
        }
    }

    private void ClosePanel()
    {
        for (int i = 0; i < headAnimals.Count; i++)
        {
            headAnimals[i].HideNode();
        }
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
        UIManager.Instance.taskPanel.JudeLevel();
    }

}

class HeadAnimal
{
    private Image headSprite;
    private Text headName;
    private string indexName;

    Transform nodeObject;
    public void Init(Transform parent,Sprite sp,int count)
    {
        nodeObject = parent;
        parent.gameObject.SetActive(false);
        headSprite = parent.Find("Image").GetComponent<Image>();
        headName = parent.Find("NameText").GetComponent<Text>();
        headSprite.sprite = sp;
        headSprite.SetNativeSize();
        indexName = "taskname" + (count + 5);
    }

    public void SetNode(int count)
    {
        nodeObject.gameObject.SetActive(true);
        nodeObject.SetSiblingIndex(count);
        headName.text = ExcelTool.lang[indexName];
    }

    public void HideNode()
    {
        nodeObject.gameObject.SetActive(false);
    }
}
