using UnityEngine;
using UnityEngine.UI;

public class LanguageCut : MonoBehaviour
{
    public string messg;
    Text text;
    private void Start()
    {
        if(messg != "")
        {
            text = GetComponent<Text>();
            text.text = ExcelTool.lang[messg];
        }
    }
}
