using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSwitch : MonoBehaviour
{
    public string[] msg;
    Text itemText;
    string mess = "";
    private void Awake()
    {
        itemText = GetComponent<Text>();
    }
    private void OnEnable()
    {
        OnItemText();
    }
    private void OnItemText()
    {
        mess = "";
        for (int i = 0; i < msg.Length; i++)
        {
            if (i < msg.Length - 1)
            {
                mess += ExcelTool.lang[msg[i]] + "\n";
            }
            else
            {
                mess += ExcelTool.lang[msg[i]];
            }
        }
        itemText.text = mess;
    }
}
