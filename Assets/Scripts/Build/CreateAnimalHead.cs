using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAnimalHead : MonoBehaviour
{
    private GameObject headPrefab;
    private Transform content;
    private int currentIndex;
    Dictionary<int, AnimalHead> animalHead = new Dictionary<int, AnimalHead>();
    public void Init()
    {
        int count = ExcelTool.Instance.animalSprite.Length;
        headPrefab = Resources.Load<GameObject>("UI/AnimalHead");
        content = GetComponent<ScrollRect>().content;
        currentIndex = 0;
        for (int i = 0; i < count; i++)
        {
            var animal = Instantiate(headPrefab).GetComponent<AnimalHead>();
            animal.transform.SetParent(content);
            animal.transform.localScale = Vector3.one * 0.9f;
            animal.InIt(ExcelTool.Instance.animalSprite[i],i);
            animalHead.Add(i, animal);
        }

        
    }

    public void InitCreate()
    {
        for (int i = animalHead.Count - 1; i >= 0; i--)
        {
            if (animalHead[i].IsCreate)
            {
                UIBase.Instance.CreateShadow(i, animalHead[i].headScale);
                return;
            }
        }
        UIBase.Instance.CreateShadow(0,60);
    }

    public bool IsCreate(int index)
    {
        return animalHead[index].IsCreate;
    }
    public float IsScale(int index)
    {
        return animalHead[index].headScale;
    }

    public void SetHead(int index,int num)
    {
        if(animalHead.ContainsKey(index))
        {
            animalHead[index].SetNumber(num);
        }
        else
        {
            Debug.Log(index+"值错误不包含");
        }
    }

    public void IsTip(int index)
    {
        for (int i = 0; i < animalHead.Count; i++)
        {
            animalHead[i].ChooseLargest(false);
        }
        for (int i = animalHead.Count-1; i >= 0; i--)
        {
            if(animalHead[i].IsCreate && index < i)
            {
                animalHead[i].ChooseLargest(true);
                break;
            }
        }
    }

    public void SetBgImage(int index)
    {
        if(currentIndex!=index)
        {
            if (animalHead.ContainsKey(index))
            {
                animalHead[currentIndex].SetBgSprite();
                currentIndex = index;
            }
            else
            {
                Debug.Log(index + "值错误不包含");
            }
        }
    }

    public void FindAnimal(int index)
    {
        if(!animalHead[index].IsCreate)
        {
            for (int i = animalHead.Count - 1; i >= 0; i--)
            {
                if (animalHead[i].IsCreate)
                {
                    UIBase.Instance.CreateShadow(i, animalHead[i].headScale);
                    return;
                }
            }
            if(index != 0)
            {
                UIBase.Instance.CreateShadow(0, animalHead[0].headScale);
            }
            GameManager.Instance.CloneTip(ExcelTool.lang["tip1"]);
        }
    }
}