using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacardPanel : MonoBehaviour
{
    public GameObject helpPanel;
    private Text infoText_1;
    private Text infoText_2;
    private Text infoText_3;
    private Text infoText_4;
    private Text infoText_5;

    private Text versionText_1;
    private Text versionText_2;
    private Text versionText_3;
    private Text versionText_4;
    private Text versionText_5;

    private Button closeBtn;
    public void Init()
    {
        Transform parent = transform.Find("Scroll View/Viewport/Content");
        infoText_1 = parent.Find("Item1/Info").GetComponent<Text>();
        infoText_2 = parent.Find("Item2/Info").GetComponent<Text>();
        infoText_3 = parent.Find("Item3/Info").GetComponent<Text>();
        infoText_4 = parent.Find("Item4/Info").GetComponent<Text>();
        infoText_5 = parent.Find("Item5/Info").GetComponent<Text>();
        versionText_1 = parent.Find("Item1/version").GetComponent<Text>();
        versionText_2 = parent.Find("Item2/version").GetComponent<Text>();
        versionText_3 = parent.Find("Item3/version").GetComponent<Text>();
        versionText_4 = parent.Find("Item4/version").GetComponent<Text>();
        versionText_5 = parent.Find("Item5/version").GetComponent<Text>();

        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(ClosePanel);
        ExcelTool.LanguageEvent += CutLang;
        if(PlayerPrefs.GetString("OpenPlacard") == "true")
        {
            PlayerPrefs.SetString("OpenPlacard","");
            helpPanel.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void CutLang()
    {
        versionText_1.text = "1.1.6" + ExcelTool.lang["version"];
        versionText_2.text = "1.1.5" + ExcelTool.lang["version"];
        versionText_3.text = "1.1" + ExcelTool.lang["version"];
        versionText_4.text = "1.0" + ExcelTool.lang["version"];
        versionText_5.text = "1.1.7" + ExcelTool.lang["version"];
        infoText_1.text = ExcelTool.lang["versioninfo1"];
        infoText_2.text = ExcelTool.lang["versioninfo2"];
        infoText_3.text = ExcelTool.lang["versioninfo3"];
        infoText_4.text = ExcelTool.lang["versioninfo4"];
        infoText_5.text = ExcelTool.lang["versioninfo5"];
    }

    public void OpenPanel()
    {
        AudioManager.Instance.PlayTouch("open_1");
        gameObject.SetActive(true);
    }

    private void ClosePanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        gameObject.SetActive(false);
    }

    public void HelpPanel()
    {
        AudioManager.Instance.PlayTouch("close_1");
        helpPanel.SetActive(false);
    }
}
