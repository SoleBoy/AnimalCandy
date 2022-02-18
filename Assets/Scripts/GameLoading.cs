using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
/// <summary>
/// 初始加载loading
/// </summary>
public class GameLoading : MonoBehaviour
{
    Text barText;
    Transform titleSpite;
    Transform spinBar;
    AsyncOperation sceneAsync = null;
    float latTime = 0;
    private bool isLoad;
    void Start()
    {
        titleSpite = transform.Find("head");
        spinBar = transform.Find("Spinbar");
        barText = transform.Find("Text").GetComponent<Text>();
        barText.text = "0%";
        isLoad = true;
        for (int i = 0; i < titleSpite.childCount; i++)
        {
            if(titleSpite.GetChild(i).GetComponent<Image>().sprite.name== ExcelTool.Instance.tempSprite.name)
            {
                titleSpite.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                titleSpite.GetChild(i).gameObject.SetActive(false);
            }
        }
        if(PlayerPrefs.GetString("RouteFirst") == "")
        {
            StartCoroutine(LoadScenes("main"));
            GameManager.Instance.modeSelection = "roude";
        }
        else
        {
            StartCoroutine(LoadScenes("Select"));
        }

        AudioManager.Instance.CutBgMusic("loading");
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
                barText.text = string.Format("{0}%",latTime);
            }
            else
            {
                sceneAsync.allowSceneActivation = true;
            }
        }
    }
}
