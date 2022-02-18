using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCool : MonoBehaviour
{
    private Button click;
    private Image shadeObject;
    private float lastTime=-10;

    void Start()
    {
        click = GetComponent<Button>();
        shadeObject = transform.Find("Shade").GetComponent<Image>();
        shadeObject.gameObject.SetActive(false);
        //click.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        lastTime = 15;
        shadeObject.fillAmount = 1;
        click.interactable = false;
        shadeObject.gameObject.SetActive(true);
    }

    void Update()
    {
        if(!click.interactable)
        {
            if (lastTime >= 0)
            {
                lastTime -= Time.deltaTime;
                shadeObject.fillAmount = lastTime / 15;
                if(lastTime <= 0)
                {
                    click.interactable = true;
                    shadeObject.gameObject.SetActive(false);
                }
            }
        }
       
    }
}
