using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum RACEIMG
{
    Rem_0,
    Bane_1,
    Ice_2,
    Fire_3,
    Laser_4,
    Egg_5,
    Frozen_6,
    Cameo_7,
    Electric_8,
    Snow_9,
    Leaf_10,
    Magnet_11,
    Leeway_12,
    Sunfire_13,
    Icecone_14,
    Cluster_15,
    Moon_16,
    rod_17,
    Flakes_18,
    Gasoline_19,
    Rockets_20
}

public class CreateModel : MonoBehaviour
{
    private static CreateModel instance;
    public static CreateModel Instance { get => instance; }
    public Vector3[] cakePoint;
    public Vector3 turretPoint;
    public Vector3 turretAngle;
    public Material gunMat;
    public Transform directional;
    public Light globalLight;

    //场景
    public Transform weather_tran;
    public Material[] skyBoxs;
    public GameObject[] scenes;
    public GameObject[] weather;
    public Transform skillBarrel;
    public Transform trashCan;
    private Transform cueCone1;
    private Transform cueCone2;
    private Color nowColor;
    public Color skyColor;
    public Color starColor;
    public float colorTime = 35;
    private float cueTime;
    private float ranSound;
    public int sceneIndex;
    public bool isScene;
    private bool isCue = true;

    //数据
    public float level = 1;//关卡数
    public float sumLevel;
    public float maxLevel;
    public float passLevel;
    public float snakeHurt;
    public int turretGrade;
    //public int turretCount = 1;
    public int spanCount = 1;
    public int conCount;
    public int sendCount = 5;
    //炮塔
    public bool sendTroops;
    public bool isSnake;
    public GameObject snakePrefab;
    public GameObject[] turretPrefab;
    public List<Transform> turrets = new List<Transform>();
    public List<int> productions = new List<int>();
    //敌人
    public List<Transform> enemys = new List<Transform>();
    public List<Transform> enemyBullets = new List<Transform>();
    private GameObject ghostPrefab;
    public float enemyMultiple;
    public float enemySpeed;
    public float enemyAttack;
    public float enemySoul;
    public bool isBullet;

    public OnHookMode hookMode;//时间 挂机 模式
    public RouteMode routeMode;//路线模式
    public CakeController cakeCon;
    private bool isMode;
    private bool isStar = true;
    private bool isGuide;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        InitialData();
        instance = this;
        Application.quitting += SaveData;
        turretPrefab = Resources.LoadAll<GameObject>("Turret");
        snakePrefab = Resources.Load<GameObject>("Effects/snake");
        ghostPrefab = Resources.Load<GameObject>("Effects/GhostEnemy");
        skillBarrel = transform.Find("OtherObject/SkillRewards");
        trashCan = transform.Find("OtherObject/Rubbish");
        cueCone1 = transform.Find("OtherObject/CueCone1");
        cueCone2 = transform.Find("OtherObject/CueCone2");
        hookMode = transform.Find("HookMode").GetComponent<OnHookMode>();
        routeMode = transform.Find("RouteMode").GetComponent<RouteMode>();
        isMode = GameManager.Instance.modeSelection != "roude";
        if (isMode)
        {
            ranSound = 25;
            routeMode.gameObject.SetActive(false);
            hookMode.gameObject.SetActive(true);
            maxLevel = PlayerPrefs.GetFloat("MaxLevel");
            sumLevel = maxLevel + PlayerPrefs.GetFloat("RouteMaxLevel");
            turretGrade = (int)Mathf.Floor(1 + sumLevel / 10);
            hookMode.InitData(GameManager.Instance.modeSelection == "hook");
            skillBarrel.localPosition = new Vector3(-5.2f, -0.3f,-7.3f);
            trashCan.localPosition = new Vector3(5.15f, 0.9f, -7.2f);
        }
        else
        {
            hookMode.gameObject.SetActive(false);
            routeMode.gameObject.SetActive(true);
            cakeCon = transform.Find("RouteMode/PointParent").GetComponent<CakeController>();
            maxLevel = PlayerPrefs.GetFloat("RouteMaxLevel");
            turretGrade = 1;
            routeMode.InitData();
            weather_tran.localPosition = new Vector3(0,0,40);
            skillBarrel.localPosition = new Vector3(-4.6f, 0.2f, 35.2f);
            trashCan.localPosition = new Vector3(4.6f, 1.3f, 35.2f);
        }
        UIManager.Instance.SetGoldNeed(turretGrade);
    }
    void InitialData()
    {
        //turretCount = PlayerPrefs.GetInt("TurretCount", 1);
        sendCount = PlayerPrefs.GetInt("SendCount", 8);
        spanCount = PlayerPrefs.GetInt("SpanCount", 6);
        conCount = PlayerPrefs.GetInt("ConCount");
    }
    private void FixedUpdate()
    {
        if (isMode && sendTroops)
        {
            hookMode.TroopsInit();
        }
        Changecolor();
        if (UIManager.Instance.isTime) return;
        float num = RenderSettings.skybox.GetFloat("_Rotation");
        RenderSettings.skybox.SetFloat("_Rotation", num + 0.05f);
        if (isMode)
        {
            if(isStar)
            {
                isStar = false;
                hookMode.StarGame();
            }
            hookMode.GenerateEnemy();
        }
        else
        {
            if (!sendTroops)
            {
                routeMode.UpdateWave();
            }
        }

        if (isCue)
        {
            cueTime += Time.deltaTime;
            if (cueTime >= 10)
            {
                JudeMerge();
                cueTime = 0;
                isCue = false;
            }
        }
    }
    //设置路线模式
    public void ModelDifficulty(string messg,float level)
    {
        this.level = level;
        if (messg=="Simple")
        {
            isBullet = false;
            enemyMultiple = 1;
            enemySpeed = 1;
            enemyAttack = 1;
            enemySoul = 1;
            ranSound = 50;
            routeMode.levelmultiple = 1;
            routeMode.WaveInterval(8);
        }
        else if (messg == "Ordinary")
        {
            isBullet = true;
            enemyMultiple = 1.25f;
            enemySpeed = 1.2f;
            enemyAttack = 1;
            enemySoul = 1;
            ranSound = 40;
            routeMode.levelmultiple = 2;
            routeMode.WaveInterval(5);
        }
        else if (messg == "Difficult")
        {
            isBullet = true;
            enemyMultiple = 2;
            enemySpeed = 1.5f;
            enemyAttack = 2;
            enemySoul = 2;
            ranSound = 30;
            routeMode.levelmultiple = 3;
            routeMode.WaveInterval(3);
        }
    }
    //选择关卡
    public void SetLevel(float lv)
    {
        TurretDrag.Instance.HideEffect();
        GlobalDestruction();
        if (isMode)
        {
            hookMode.ChooseLevel(lv);
        }
        else
        {
            routeMode.ReadTheLevel();
            productions.Sort();
            if(level != 0)
            {
                string messgCarry = "";
                for (int i = 0; i < productions.Count; i++)
                {
                    messgCarry += productions[i];
                    if (i < productions.Count - 1)
                    {
                        messgCarry += "|";
                    }
                }
                PlayerPrefs.SetString("AnnalCarry", messgCarry);
            }
        }
    }
    //收集灵魂数
    public void LevelControl(float soul,bool isBoos)
    {
        if (isMode)
        {
            hookMode.SoulControl(soul,isBoos);
        }
        else
        {
            routeMode.SoulControl();
        }
    }
    //是否过关
    public void LevelJudge(bool isNext)
    {
        if(isMode)
        {
            hookMode.CheckLevel(isNext);
        }
    }
    //过关
    public void AddLevel()
    {
        if (isMode)
        {
            hookMode.HookNextLevel();
        }
    }
    //敌人声音
    public bool PlaySound()
    {
        int ran = Random.Range(1,101);
        return ran < ranSound;
    }
    //路线过关
    public void MaxPass(float max,bool isMax)
    {
        if(isMax)
        {
            maxLevel += 1;
            sumLevel += 1;
            PlayerPrefs.SetFloat("RouteMaxLevel",maxLevel);
        }
        else
        {
            maxLevel = max;
            sumLevel = maxLevel + PlayerPrefs.GetFloat("MaxLevel");
        }
    }
    //判断关卡是否切换
    public bool SceneChange()
    {
        if (int.Parse(ReturnLevel((level + 1)).mapId) != sceneIndex)
        {
            isScene = true;
            return true;
        }
        return false;
    }
    //判断大于300关
    public LevelItem ReturnLevel(float grade)
    {
        grade = grade % 300;
        if (grade == 0)
        {
            grade = 300;
        }
        return ExcelTool.Instance.levels[grade.ToString()];
    }
    //角度切换
    public void AngleSwitch(string messg)
    {
        string[] angle = messg.Split('|');
        if(angle.Length >= 3)
        {
            Vector3 point = new Vector3(float.Parse(angle[0]), float.Parse(angle[1]), float.Parse(angle[2]));
            if(point != directional.localEulerAngles)
                 directional.DOLocalRotate(point,30);
        }
    }
    //颜色切换
    public void ColorSwitch(string messg)
    {
        ColorUtility.TryParseHtmlString("#"+ messg, out nowColor);
        if(nowColor != globalLight.color)
        { 
            globalLight.DOColor(nowColor, 30);
        }
    }
    public void Changecolor()
    {
        if (colorTime <= 30)
        {
            colorTime += Time.deltaTime;
            float lerp = Mathf.PingPong(colorTime, 30) / 30;
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(starColor, skyColor, lerp));
        }
    }
    //切换场景
    public void ChangeScene(float level)
    {
        if(isMode)
        {
            sceneIndex = int.Parse(ReturnLevel(level).mapId);
        }
        else
        {
            sceneIndex = int.Parse(ExcelTool.Instance.lines[level.ToString()].mapID);
        }
        AudioManager.Instance.CutBgMusic("battle" + sceneIndex);
        RenderSettings.skybox = skyBoxs[sceneIndex - 1];
        RenderSettings.skybox.SetFloat("_Rotation", 0);
        for (int i = 0; i < scenes.Length; i++)
        {
            if (i == sceneIndex - 1)
            {
                scenes[i].gameObject.SetActive(true);
            }
            else
            {
                scenes[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < weather.Length; i++)
        {
            weather[i].GetComponent<WeatherMaager>().Weather(false, true);
        }
    }
    //切换天气
    public void CutWeather(string weatMess)
    {
        string[] weat = weatMess.Split('|');
        for (int i = 0; i < weather.Length; i++)
        {
            for (int j = 0; j < weat.Length; j++)
            {
                if (int.Parse(weat[j].ToString()) - 1 == i)
                {
                    weather[i].SetActive(true);
                    weather[i].GetComponent<WeatherMaager>().Weather(true, false);
                    break;
                }
                else
                {
                    weather[i].GetComponent<WeatherMaager>().Weather(false, false);
                }
            }
        }
    }

    //提示框
    public void ShowTipCone(int index)
    {
        if(isMode)
        {
            StartCoroutine(hookMode.ShowFitTip(index));
        }
        else
        {
            routeMode.ShowCueCone();
        }
    }
    public void HideTipCone(int index)
    {
        if (isMode)
        {
            hookMode.HideFitTip(index);
        }
        else
        {
            if (PlayerPrefs.GetString("Guide21") == "")
            {
                PlayerPrefs.SetString("Guide21", "index");
                UIManager.Instance.GuideInfo("");
                sendTroops = false;
            }
        }
    }
    public void ControlProduc(bool isRemo,int index)
    {
        if (isMode)
        {
            hookMode.TurretProduc(isRemo, index);
        }
        else
        {
            routeMode.TurretProduc(isRemo, index);
        }
    }
    //炮塔升星
    public void StarTurret()
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].GetComponent<TurretControl>().CreateKeel();
        }
    }
    //解锁合成台
    public void UnlockSpan()
    {
        spanCount += 1;
        PlayerPrefs.SetInt("SpanCount", spanCount);
        if (isMode)
        {
            hookMode.UnlockSpan_Hook();
        }
        else
        {
            routeMode.UnlockFittable_Roude();
        }
    }
    //转化炮台
    public void ConvertGun()
    {
        if (isMode)
        {
            hookMode.ConvertGun_Hook();
        }
        conCount += 1;
        PlayerPrefs.SetInt("ConCount", conCount);
    }
    //解锁炮台
    public void UnlockSend()
    {
        sendCount += 1;
        PlayerPrefs.SetInt("SendCount", sendCount);
        if (isMode)
        {
            hookMode.UnlockSend_Hook();
        }
        else
        {
            routeMode.UnlockTurret_Roude();
        }
    }
    //生成炮塔
    public bool CloneTurret()
    {
        if (isMode)
        {
            return hookMode.CloneTurret_Hook();
        }
        else
        {
            return routeMode.CreateTurret_Roude();
        }
    }
    //战力蛇
    public void CreateSnake()
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].GetComponent<TurretControl>().CreateSnake(snakePrefab);
        }
        isSnake = true;
        Invoke("HideSnake", 30);
    }
    private void HideSnake()
    {
        isSnake = false;
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].GetComponent<TurretControl>().HideSnake();
        }
    }
    //隐藏合体提示
    public void HideCueCone()
    {
        isCue = true;
        cueTime = 0;
        cueCone1.gameObject.SetActive(false);
        cueCone2.gameObject.SetActive(false);
        UIManager.Instance.LeadAnim(false, Vector3.one, Vector3.one);
        if (!isMode)
        {
            UIManager.Instance.LeadDestroy(false,Vector3.one,Vector3.one);
        }
    }
    //判断是否可以合并
    public bool JudeMerge()
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            for (int j = i + 1; j < turrets.Count; j++)
            {
                if (turrets[i].GetComponent<TurretControl>().grade == turrets[j].GetComponent<TurretControl>().grade &&
                    turrets[i].GetComponent<TurretControl>().state == turrets[j].GetComponent<TurretControl>().state)
                {
                    if (isCue && cueTime >= 10)
                    {
                        isCue = false;
                        Vector3 point = turrets[i].parent.position;
                        point.y += 1.5f;
                        cueCone1.GetComponent<TipCone>().SetPoint(point);
                        cueCone1.gameObject.SetActive(true);
                        point = turrets[j].parent.position;
                        cueCone2.gameObject.SetActive(true);
                        point.y += 1.5f;
                        cueCone2.GetComponent<TipCone>().SetPoint(point);
                        UIManager.Instance.LeadAnim(true, turrets[i].parent.position, turrets[j].parent.position);
                    }
                    return false;
                }
            }
        }
        return true;
    }
    //判断是否可以克隆炮塔
    public bool HintTurret()
    {
        if (isMode)
        {
            return hookMode.HintTurret_Hook();
        }
        else
        {
            return routeMode.HintTurret_Roude();
        }
    }
    //是否最后一个炮塔
    public bool LastTurret()
    {
        if (turrets.Count > 1)
        {
            return true;
        }
        return false;
    }
    //炮台 合体台等级判断
    public void RankJudgment()
    {
        if (!isMode)
        {
            routeMode.UnlockTurret_Roude();
            if (PlayerPrefs.GetString("Guide4") == "")
            {
                if (isGuide)
                {
                    isGuide = false; sendTroops = false;
                    UIManager.Instance.GuideInfo("");
                    PlayerPrefs.SetString("Guide4", "index");
                }
                else
                {
                    for (int i = 0; i < turrets.Count; i++)
                    {
                        for (int j = i + 1; j < turrets.Count; j++)
                        {
                            if (turrets[i].GetComponent<TurretControl>().isFire && !turrets[j].GetComponent<TurretControl>().isFire)
                            {
                                if (turrets[i].GetComponent<TurretControl>().grade <
                                turrets[j].GetComponent<TurretControl>().grade)
                                {
                                    sendTroops = true; isGuide = true;
                                    UIManager.Instance.GuideInfo(ExcelTool.lang["guideinfo4"]);
                                    UIManager.Instance.guidePanel.OpenGuide(4, false);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    //过关等级判断
    public void UpTurretGrade()
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            if (turrets[i].GetComponent<TurretControl>().grade < turretGrade)
            {
                turrets[i].GetComponent<TurretControl>().SetLevel(turretGrade);
            }
        }
    }
    //路线模式 炮台判断
    public void HideUnlock()
    {
        if (!isMode)
        {
            routeMode.UnlockHide_Roude();
        }
    }

    //自动合并
    public void MergeTurret()
    {
        if (isMode)
        {
            hookMode.MergeTurret_Hook();
        }
        else
        {
            routeMode.MergeTurret_Roude();
        }
    }
    //糖果合体等级之和
    public float HighestFit()
    {
        float num = 0;
        for (int i = 0; i < ExcelTool.Instance.shooters.Count; i++)
        {
            num += PlayerPrefs.GetFloat("FitLevel" + (RACEIMG)i);
        }
        return num;
    }
    //糖果 and 技能 升星等级之和
    public float HighestStar()
    {
        float num = 0;
        for (int i = 0; i < ExcelTool.Instance.shooters.Count; i++)
        {
            num += ExcelTool.Instance.shooters[i].starLevel;
        }
        for (int i = 1; i <= 10; i++)
        {
            num += PlayerPrefs.GetInt("SkillGrade" + i);
        }
        return num;
    }

    //移除死亡特效
    public void RemoveEnemy(Transform enemy, bool isBoos)
    {
        enemys.Remove(enemy);
        if (isBoos)
        {
            for (int i = 0; i < 10; i++)
            {
                StartCoroutine(BoosGhost(i * 0.2f, enemy.position));
            }
        }
        else
        {
            var go = ObjectPool.Instance.CreateObject(ghostPrefab.name, ghostPrefab);
            go.transform.position = enemy.position;
        }
        ObjectPool.Instance.CollectObject(enemy.gameObject);
    }
    IEnumerator BoosGhost(float dely, Vector3 point)
    {
        yield return new WaitForSeconds(dely);
        for (int i = 0; i < 2; i++)
        {
            var go = ObjectPool.Instance.CreateObject(ghostPrefab.name, ghostPrefab);
            point.x += (Random.Range(-2f, 2f));
            point.y += 1;
            go.transform.position = point;
        }
    }
    //清除物体
    public void GlobalDestruction()
    {
        if(isMode)
        {
            hookMode.ClearGlodal();
        }
        else
        {
            //cakePoint = null;
            //cakeCon.cakeHealths = null;
            routeMode.ClearGlodal();
        }
        for (int i = 0; i < 5; i++)
        {
            ObjectPool.Instance.Clear("EnemyBullet" + i);
        }
        enemyBullets.Clear();
        enemys.Clear();
        ObjectPool.Instance.Clear(ghostPrefab.name);
        ObjectPool.Instance.Clear("EffCube");
        ObjectPool.Instance.Clear("PixelBlock");
        ObjectPool.Instance.Clear("MixEff");
        ObjectPool.Instance.Clear("TipPanel");
        ObjectPool.Instance.Clear("Snowflakes");
        ObjectPool.Instance.Clear("xin");
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    //数据保存
    public void SaveData()
    {
        if (isMode)
        {
            hookMode.SaveHook();
        }
        UIManager.Instance.SaveData();
    }
    
    private void OnDestroy()
    {
        Application.quitting -= SaveData;
    }
}
