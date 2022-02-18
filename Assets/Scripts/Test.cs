
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public Text messgText;
    public Image goldtext;
    public List<Sprite> sprites = new List<Sprite>();
    public Transform timemode1;
    public Transform timemode2;
    public Animator animator;
    public AudioSource clip;

    public DragonBones.UnityArmatureComponent game;
    string messg;
    void Start()
    {
        messg = "*合理选择出战种类，更容易过关。*根据糖果射手的射击距离，优化布阵方法。*根据动物的抗性属性，合理放置不同属性的射手。" +
            "*特殊功能的糖果射手，效果不一样。*合理使用糖果术士，有助于完成满血任务。*不同路线，需要思考不同的布阵。" +
            "*观察战前敌人信息：子弹攻击值，身体攻击值，血量。*看战场广告，可以获得更多糖果资源。";
        //for (int i = 0; i < point.Length; i++)
        //{
        //    Debug.Log(point[i].GetComponent<Renderer>().bounds.size.y);
        //}
        //StopCoroutine("starTest");
        //StartCoroutine(starTest());
        //StartCoroutine(GetTokenData("http://wfcms.mayi-tuan.com/?action=getElectric"));
        //StartCoroutine(GetUrlData(""));
        game.animation.Play("pause_1", 1);
        game.AddEventListener(DragonBones.EventObject.COMPLETE, HideWin1);

        
    }

    public static int[] getRandoms(int sum, int min, int max)
    {
        int[] arr = new int[sum];
        int j = 0;
        //表示键和值对的集合。
        Hashtable hashtable = new Hashtable();
        System.Random rm = new System.Random();
        while (hashtable.Count < sum)
        {
            //返回一个min到max之间的随机数
            int nValue = rm.Next(min, max);
            // 是否包含特定值
            if (!hashtable.ContainsValue(nValue))
            {
                //把键和值添加到hashtable
                hashtable.Add(nValue, nValue);
                arr[j] = nValue;

                j++;
            }
        }
        return arr;
    }
    public int index;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            int[] numbers = getRandoms(6, 0, 100);
            for (int i = 0; i < numbers.Length; i++)
            {
                Debug.Log(numbers[i]);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            timemode1.localPosition = Vector3.zero;
            timemode1.DOMove(timemode2.position,3);
            messgText.DOText(messg,100);
            ////goldtext.DOText(index.ToString(),3);
            ////声明
            //Sequence mScoreSequence;
            ////函数内初始化
            //mScoreSequence = DOTween.Sequence();
            ////函数内设置属性
            //mScoreSequence.SetAutoKill(false);
            //mScoreSequence.Append(DOTween.To(delegate (float value) {
            //    //向下取整
            //    var temp = Math.Floor(value);
            //    //向Text组件赋值
            //    goldtext.text = temp + "";
            //}, 0, index, 3f));
            //将更新后的值记录下来, 用于下一次滚动动画
            //mOldScore = newScore;
            //Debug.Log(Vector3.Distance(timemode1.localPosition, timemode2.localPosition));
        }
    }

    private void HideWin1(string type, DragonBones.EventObject eventObject)
    {
        game.animation.Play("pause_2", 1);
    }

    //龙骨贴图生成
    public void SetArmature(Transform model_Armature, string messgName, Vector3 scale, Vector3 point, bool isAnimal = false, string animal = "")
    {
        UnityEngine.Transform parent = model_Armature.parent;
        model_Armature.gameObject.SetActive(true);
        DragonBones.UnityArmatureComponent unityArmature = model_Armature.GetComponent<DragonBones.UnityArmatureComponent>();
        unityArmature.armature.Dispose();
        unityArmature = DragonBones.UnityFactory.factory.BuildArmatureComponent(messgName, "", "", "", null, true);
        model_Armature.transform.SetParent(parent);
        model_Armature.transform.SetAsFirstSibling();
        model_Armature.transform.localEulerAngles = Vector3.zero;
        model_Armature.transform.localPosition = point;
        model_Armature.transform.localScale = scale;
    }

    IEnumerator GetUrlData(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("tolietNo", 001);//UnityWebRequest request = new UnityWebRequest(url, "POST");
        form.AddField("satisfaction", 75);
        form.AddField("waterCost", 18);
        form.AddField("powerCost", 9);
        form.AddField("datetime", "2021-03-25 17:22:45");
        UnityWebRequest webRequest = UnityWebRequest.Post("http://yfs15.ynssx.com/api/Toliets/addtoliet", form);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求Token错误,网络错误,请检查网络:" + webRequest.error);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            try
            {
                
            }
            catch (System.Exception)
            {
                Debug.Log("数据解析错误:");
            }
        }
    }

    IEnumerator GetTokenData(string url)
    {
        //Dictionary<string, object> tokenData = null;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求Token错误,网络错误,请检查网络:" + webRequest.error);
        }
        else
        {
            try
            {
                Debug.Log(webRequest.downloadHandler.text);
                Dictionary<string, object> tokenData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
               
            }
            catch (System.Exception)
            {
                Debug.Log("数据解析错误:");
            }
        }
    }
}
