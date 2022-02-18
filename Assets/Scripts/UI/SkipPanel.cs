using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipPanel : MonoBehaviour
{
    private int builIndex;
    private int turretIndex;

    private GameObject tipGame;
    private Transform spinBar;
    private Transform turretParent;
    private Transform builParent;
    private Text barText;
    private Button closeBtn;

    private AsyncOperation sceneAsync = null;
    private string modeMessg;
    private string modeScene;
    private float latTime = 0;
    private bool isLoad;
    public void Init()
    {
        barText = transform.Find("Text").GetComponent<Text>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        tipGame = transform.Find("TipText").gameObject;
        turretIndex = PlayerPrefs.GetInt("TurretIndex");
        builIndex = PlayerPrefs.GetInt("buildingIndex");
        turretParent = transform.Find("Turret");
        builParent = transform.Find("Building");
        spinBar = transform.Find("Spinbar");
        closeBtn.onClick.AddListener(BackPanel);
    }
    void BackPanel()
    {
        GameManager.Instance.CloseHelp(modeMessg);
        sceneAsync.allowSceneActivation = true;
        closeBtn.enabled = false;
    }
    public void ClosePanel()
    {
        if(gameObject.activeInHierarchy)
        {
            if (modeScene == "Build")
            {
                builParent.GetChild(builIndex).gameObject.SetActive(false);
                builIndex += 1;
                if (builIndex >= builParent.childCount)
                {
                    builIndex = 0;
                }
                PlayerPrefs.SetInt("buildingIndex", builIndex);
            }
            else
            {
                turretParent.GetChild(turretIndex).gameObject.SetActive(false);
                turretIndex += 1;
                if (turretIndex >= turretParent.childCount)
                {
                    turretIndex = 0;
                }
                PlayerPrefs.SetInt("TurretIndex", turretIndex);
            }
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 场景切换过度
    /// </summary>
    public void OpenPnael(string name,string mode)
    {
        latTime = 0;
        isLoad = true;
        modeScene = name;
        closeBtn.enabled = false;
        tipGame.SetActive(false);
        gameObject.SetActive(true);
        if(modeScene == "Build")
        {
            builParent.GetChild(builIndex).gameObject.SetActive(true);
        }
        else
        {
            turretParent.GetChild(turretIndex).gameObject.SetActive(true);
        }
        ObjectPool.Instance.ClearAll();
        ExcelTool.Instance.RemoveEvent();
        modeMessg = mode;
        StartCoroutine(LoadScenes(name));
    }

    IEnumerator LoadScenes(string sceneName)
    {
        yield return new WaitForEndOfFrame();
        sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        sceneAsync.allowSceneActivation = false;
        yield return sceneAsync;
    }

    void Update()
    {
        if(isLoad)
        {
            if (latTime < 100)
            {
                latTime += 5;
                latTime = Mathf.Clamp(latTime, 0, 100);
                spinBar.Rotate(Vector3.back * 5);
                barText.text = latTime + "%";
            }
            else
            {
                isLoad = false;
                closeBtn.enabled = true;
                tipGame.SetActive(true);
            }
        }
        
    }
}
