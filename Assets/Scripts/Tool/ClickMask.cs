using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickMask : MonoBehaviour
{
    private GameObject maskObject;
    private Text mask_text;
    private Button clickBtn;

    private float maskTime;
    private void Awake()
    {
        clickBtn = GetComponent<Button>();
        maskObject = transform.Find("TimeMask").gameObject;
        mask_text = transform.Find("TimeMask/Text").GetComponent<Text>();
        clickBtn.onClick.AddListener(OpenMask);
    }

    private void OnEnable()
    {
        maskTime = -1;
        clickBtn.interactable = true;
        maskObject.SetActive(false);
    }

    private void OpenMask()
    {
        maskTime = 3;
        mask_text.text = "3";
        clickBtn.interactable = false;
        maskObject.SetActive(true);
    }

    private void Update()
    {
        if(maskTime >= 0)
        {
            maskTime -= Time.deltaTime;
            mask_text.text = maskTime.ToString("F0");
            if (maskTime <= 0)
            {
                clickBtn.interactable = true;
                maskObject.SetActive(false);
            }
        }
    }
}
