using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarryOut : MonoBehaviour
{
    private GameObject goldObject;
    private GameObject addObject;
    private Transform candyParent;
    private Text goldText;
    private Text nameText;
    private Button carryBtn;
    private DragonBones.UnityArmatureComponent model_Armature;
    private int carryIndex;
    private bool isTurret;
    private void Awake()
    {
        goldObject = transform.Find("Gold").gameObject;
        addObject = transform.Find("Image").gameObject;
        carryBtn = GetComponent<Button>();
        goldText = transform.Find("Gold/Text").GetComponent<Text>();
        nameText = transform.Find("Text").GetComponent<Text>();
        candyParent = transform.Find("Mask");
        carryBtn.onClick.AddListener(UnloadState);
        //candyParent.gameObject.SetActive(true);
        model_Armature = candyParent.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>();
        carryIndex = -10;
    }

    private void UnloadState()
    {
        if(carryIndex >= 0)
        {
            if(isTurret)
            {
                UIManager.Instance.turretPanel.UnloadCarry(carryIndex);
            }
            else
            {
                UIManager.Instance.skillPanel.UnloadCarry(carryIndex-1);
            }
            carryIndex = -10;
        }
    }

    public bool Unselected()
    {
        return addObject.activeInHierarchy;
    }

    public void NormalState()
    {
        goldObject.SetActive(false);
        addObject.SetActive(true);
        nameText.text = "";
        candyParent.gameObject.SetActive(false);
    }
    //技能
    public void SelectedState(string name,string gold,int index,string keelName)
    {
        isTurret = false;
        addObject.SetActive(false);
        goldObject.SetActive(true);
        nameText.text = name;
        goldText.text = gold;
        carryIndex = index;
        candyParent.gameObject.SetActive(true);
        model_Armature = UIManager.Instance.SetArmature(model_Armature,candyParent, keelName, Vector3.one * 18, Vector3.up*-8);
    }
    //炮台
    public void SelectedState(string name,int index, string keelName)
    {
        isTurret = true;
        addObject.SetActive(false);
        nameText.text = name;
        carryIndex = index;
        candyParent.gameObject.SetActive(true);
        model_Armature = UIManager.Instance.SetArmature(model_Armature,candyParent, keelName, Vector3.one * 30, Vector3.up * -5);
    }
}
