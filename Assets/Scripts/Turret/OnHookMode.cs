using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHookMode : MonoBehaviour
{
    public Animator trashAnimal;

    private Transform spanPoint;
    private Transform sendPoint;
    private Transform spanUnlck;
    private Transform sendUnlck;
    private Transform conePoint;
    private Transform fullPoint;
    private Transform enemyPoint;
    private GameObject embellishPrefab;

    private string sendMsg = "";
    private string spanMsg = "";

    //关卡
    private LevelItem levelData;
    private float rounds = 1;
    private float sumAnimal;
    private float suol_curve;
    private float suol_max;
    private float suol_sum;
    private bool isAddAnimal;
    private float minTime;
    private float maxTime;
    //炮塔
    private float turretRan;
    private int turretIndex;
    private List<Sprite> armsSprites = new List<Sprite>();

    //敌人
    private Dictionary<string, GameObject> allEnemy = new Dictionary<string, GameObject>();
    private EnemyItem enemyData;
    private GameObject enemyPrefab;
    private float enemyScale;
    private int enemyCount = 0;
    private int enemyId;
    private float enemyHp;
    private float enemyLevel;
    private float enemySum;
    private string[] boosId;
    private string[] enemyLever;
    private string[] enemyName;
    private string boosName;
    private string AttachName;
    private List<string> enemyRan = new List<string>();
    private float cloneTime = 2;
    private float lastTime = 0;

    private bool isTroops;
    private bool isPattern;
    private float fortyTime;
    private float extraTime;
    private int max_Enemy;
    private int index_Level = 33;
    private int extraLevel = 0;
    private WaitForSeconds waitFor = new WaitForSeconds(2);

    private CreateModel createModel;
    public void InitData(bool isMode)
    {
        createModel = transform.parent.GetComponent<CreateModel>();
        enemyPoint = transform.Find("StartPoint");
        spanPoint = transform.Find("SpanPoint");
        sendPoint = transform.Find("SendPoint");
        spanUnlck = transform.Find("SpanLock");
        sendUnlck = transform.Find("SendLock");
        conePoint = transform.Find("SendCone");
        fullPoint = transform.Find("Full");

        createModel.turretPoint = new Vector3(0, 1f, 0);
        createModel.turretAngle = new Vector3(-15, 0,0);
        createModel.cakePoint = new Vector3[1];
        createModel.cakePoint[0] = new Vector3(0, 0, 2);
        embellishPrefab = Resources.Load<GameObject>("Effects/EffCube");
        var enemy = ExcelTool.Instance.enemyAnimal;
        for (int i = 0; i < enemy.Length; i++)
        {
            allEnemy.Add(enemy[i].name, enemy[i].gameObject);
        }
        isPattern = isMode;
        InitialData();
    }

    private void Start()
    {
        InitialTurret();
        for (int i = 0; i < createModel.spanCount; i++)
        {
            if (spanPoint.GetChild(i).childCount >= 2)
            {
                for (int j = 0; j < createModel.sendCount; j++)
                {
                    if (j < sendPoint.childCount && sendPoint.GetChild(j).childCount <= 2)
                    {
                        conePoint.GetChild(j).gameObject.SetActive(true);
                    }
                }
                break;
            }
        }
    }
    private void InitialData()
    {
        max_Enemy = 30;
        if (isPattern)
        {
            createModel.level = PlayerPrefs.GetFloat("Level", 1);
        }
        createModel.passLevel = PlayerPrefs.GetFloat("PassLevel");
        sendMsg = PlayerPrefs.GetString("SendMessg");
        spanMsg = PlayerPrefs.GetString("SpanMessg");
        index_Level = PlayerPrefs.GetInt("EvaluateLevel", 33);
        createModel.sceneIndex = int.Parse(createModel.ReturnLevel(createModel.level).mapId);
        if (sendMsg != "")
        {
            string[] send = sendMsg.Split(',');
            int index = 0;
            for (int i = 0; i < send.Length - 1; i++)
            {
                string[] data = send[i].Split('-');
                index = int.Parse(data[1]);
                var go = ObjectPool.Instance.CreateObject(createModel.turretPrefab[index].name, createModel.turretPrefab[index]);
                go.transform.SetParent(sendPoint.GetChild(int.Parse(data[0])), true);
                go.transform.localPosition = createModel.turretPoint;
                go.transform.localEulerAngles = createModel.turretAngle;
                go.GetComponent<TurretControl>().SetNumerical(float.Parse(data[2]));
                go.GetComponent<TurretControl>().isMerge = true;
                createModel.turrets.Add(go.transform);
            }
        }
        if (spanMsg != "")
        {
            string[] span = spanMsg.Split(',');
            int index = 0;
            for (int i = 0; i < span.Length - 1; i++)
            {
                string[] data = span[i].Split('-');
                index = int.Parse(data[1]);
                var go = ObjectPool.Instance.CreateObject(createModel.turretPrefab[index].name, createModel.turretPrefab[index]);
                go.transform.SetParent(spanPoint.GetChild(int.Parse(data[0])), true);
                go.transform.localPosition = createModel.turretPoint;
                go.transform.localEulerAngles = createModel.turretAngle;
                go.GetComponent<TurretControl>().SetNumerical(float.Parse(data[2]));
                go.GetComponent<TurretControl>().isMerge = true;
                createModel.turrets.Add(go.transform);
            }
        }
    }
    public void TroopsInit()
    {
        if (createModel.enemys.Count <= max_Enemy)
        {
            if (suol_curve < sumAnimal * 0.5f && !BaseHealth.Instance.isOver && isTroops)
            {
                lastTime += Time.deltaTime;
                if (lastTime >= cloneTime)
                {
                    cloneTime = Random.Range(minTime, maxTime) * 0.1f;
                    lastTime = 0;
                    CloneEnemy();
                }
            }
        }
    }
    //开始游戏
    public void StarGame()
    {
        if (!isPattern)
        {
            createModel.ChangeScene((int)Random.Range(1, createModel.maxLevel));
            createModel.sendTroops = true;
            BaseHealth.Instance.SetHp(100);
            KeelAnimation.Instance.StartArmature();
        }
        else
        {
            createModel.ChangeScene(createModel.level);
        }
        StartCoroutine(RefreshLevel());
    }
    //敌兵生成
    public void GenerateEnemy()
    {
        if (!isPattern)
        {
            extraTime += Time.deltaTime;
            if(extraTime >= 60)
            {
                extraTime = 0;
                extraLevel += 1;
            }
            fortyTime += Time.deltaTime;
            if (fortyTime >= 30)
            {
                fortyTime = 0;
                int ran = Random.Range(1, 4);
                for (int i = 0; i < ran; i++)
                {
                    CreateForty();
                }
            }
        }
        
        if (createModel.enemys.Count <= max_Enemy && !createModel.sendTroops)
        {
            if (suol_curve < suol_max && !BaseHealth.Instance.isOver && isTroops)
            {
                lastTime += Time.deltaTime;
                if (lastTime >= cloneTime)
                {
                    if (isPattern)
                    {
                        cloneTime = Random.Range(minTime, maxTime);
                    }
                    else
                    {
                        cloneTime = Random.Range(minTime, maxTime) * 0.25f;
                    }
                    lastTime = 0;
                    CloneEnemy();
                }
            }
        }
    }

    //关卡灵魂数量
    public void ChooseLevel(float lv)
    {
        createModel.level = lv;
        StartCoroutine(RefreshLevel());
    }
    //判断是否通过
    public void SoulControl(float suol, bool isEnemy)
    {
        if (isPattern)
        {
            if (isEnemy)
            {
                for (int i = 0; i < createModel.enemys.Count; i++)
                {
                    TurretDrag.Instance.EenemyPixel(createModel.enemys[i].gameObject,
                        createModel.enemys[i].GetComponent<EnemyControl>().enemyID, false);
                    ObjectPool.Instance.CollectObject(createModel.enemys[i].gameObject);
                }
                createModel.enemys.Clear();
                StartCoroutine(LevelUp(4));
            }
            else
            {
                suol_sum += suol;
                if (suol_sum >= suol_max && suol_sum >= suol_curve)
                {
                    suol_sum = 0;
                    StartCoroutine(LevelUp(1f));
                }
                //else if (createModel.enemys.Count <= 0 && suol_curve >= suol_max)
                //{
                //    suol_sum = 0;
                //    Debug.Log("suol_curve");
                //    StartCoroutine(LevelUp(1f));
                //}
            }
        }
    }
    IEnumerator LevelUp(float dely)
    {
        yield return new WaitForSeconds(dely);
        KeelAnimation.Instance.WinArmature();
        yield return new WaitForSeconds(1.5f);
        MaxLevel();
        createModel.GlobalDestruction();
        GameManager.Instance.PassNumber();
    }
    public void HookNextLevel()
    {
        if (createModel.level == 1 && PlayerPrefs.GetString("GOBUILD") == "")
        {
            createModel.level += 1;
            UIManager.Instance.SwitchScene("Build");
            PlayerPrefs.SetString("GOBUILD", "gobuild");
        }
        else if (createModel.level == 2 && PlayerPrefs.GetString("SWITCHTIME") == "")
        {
            createModel.level += 1;
            UIManager.Instance.SwitchScene("time");
            PlayerPrefs.SetString("SWITCHTIME", "time");
        }
        else if (createModel.level == 900)
        {
            UIManager.Instance.SwitchScene("Select");
        }
        else
        {
            if (createModel.level % 5 == 0)
            {
                UIManager.Instance.IsTask(true);
            }
            else
            {
                UIManager.Instance.IsTask(false);
            }
        }
    }
    public void CheckLevel(bool isLever)
    {
        if (isLever)
        {
            if (createModel.level >= index_Level)
            {
                index_Level += 100;
                PlayerPrefs.GetInt("EvaluateLevel", index_Level);
                UIManager.Instance.evaluatePanel.OpenPanel();
            }
            if(createModel.level < 900)
            {
                createModel.level += 1;
            }
            UIManager.Instance.LevelAddDesign(createModel.level);
            UIManager.Instance.AutoTip();
        }
        else
        {
            if (createModel.level > 1)
            {
                if (int.Parse(createModel.ReturnLevel(createModel.level - 1).mapId) == createModel.sceneIndex)
                {
                    createModel.level -= 1;
                    UIManager.Instance.LevelSubDesign(createModel.level);
                }
            }
            BaseHealth.Instance.WaterAnim(false);
            UIManager.Instance.HideTask();
        }
        SaveHook();
        StartCoroutine(RefreshLevel());
    }
    void MaxLevel()
    {
        if (createModel.level > createModel.maxLevel)
        {
            createModel.maxLevel = createModel.level;
            createModel.sumLevel = createModel.maxLevel + PlayerPrefs.GetFloat("RouteMaxLevel");
            createModel.turretGrade = 1 + (int)Mathf.Floor(createModel.sumLevel / 10);
            createModel.UpTurretGrade();
            UIManager.Instance.SetGoldNeed(createModel.turretGrade);
            UIManager.Instance.SetStar(levelData.minssion_diamond);
            GameManager.Instance.ClonePrompt(levelData.minssion_diamond, 1);
            PlayerPrefs.SetFloat("MaxLevel", createModel.maxLevel);
        }
        if (BaseHealth.Instance.PerfectPass() && PlayerPrefs.GetFloat("PerfectPass" + createModel.level) != createModel.level)
        {
            createModel.passLevel += 1;
            PlayerPrefs.SetFloat("PerfectPass" + createModel.level, createModel.level);
            PlayerPrefs.SetFloat("PassLevel", createModel.passLevel);
        }
    }
    //初始关卡数据
    public IEnumerator RefreshLevel()
    {
        isTroops = false;
        yield return waitFor;
        isAddAnimal = true;
        suol_max = 0;  //总灵魂
        suol_curve = 0; //记录灵魂数
        suol_sum = 0;//小兵
        sumAnimal = 0;//小兵总灵魂   战场灵魂数=基础数+基础数*（轮数-1）*0.5
        levelData = createModel.ReturnLevel(createModel.level);
        rounds = Mathf.Floor(((createModel.level - 1) / 300)) + 1;
        if (createModel.level % 22 == 0)
        {
            AttachName = string.Format("10{0}", 40);
        }
        else
        {
            AttachName = string.Format("10{0}", (createModel.level % 22) + 18);
        }
        for (int i = 0; i < rounds; i++)
        {
            sumAnimal += ExcelTool.Instance.enemys[AttachName].soul;
        }
        if (isPattern)
        {
            sumAnimal += levelData.suol_max;
            enemyLever = levelData.soldier_lv.Split('|');
        }
        else
        {
            sumAnimal += levelData.suol_max_timemode;
            enemyLever = levelData.soldier_lv_timemode.Split('|');
        }
        sumAnimal = sumAnimal + sumAnimal * (rounds - 1) * 0.5f;
        suol_max = sumAnimal;
        if (levelData.boos != "-1")
        {
            boosId = levelData.boos.Split('|');
            suol_max += ExcelTool.Instance.enemys[boosId[0]].soul;
        }
        boosName = "";
        enemyName = levelData.soldier_id.Split('|');
        minTime = float.Parse(levelData.interval.Split('|')[0]);
        maxTime = float.Parse(levelData.interval.Split('|')[1]);
        ChanceSum();
        cloneTime = 0;
        createModel.CutWeather(levelData.weatherID);
        isTroops = true;

        createModel.AngleSwitch(levelData.timeAngle);
        createModel.ColorSwitch(levelData.lightcolor);
        createModel.colorTime = 0;
        createModel.starColor = RenderSettings.skybox.GetColor("_Tint");
        ColorUtility.TryParseHtmlString("#" + levelData.skycolor, out createModel.skyColor);
        if (createModel.starColor == createModel.skyColor)
        {
            createModel.colorTime = 35;
        }
        CancelInvoke("DelyArms");
        if (isPattern)
        {
            AllArms();
            createModel.sendTroops = true;
            BaseHealth.Instance.SetHp(100);
            KeelAnimation.Instance.StartArmature();
            Invoke("DelyArms", 6);
        }
        else
        {
            Invoke("DelyArms", 3);
        }
    }
    void DelyArms()
    {
        createModel.sendTroops = false;
        cloneTime = 1;
    }
    private void AllArms()
    {
        int index = 0;
        armsSprites.Clear();
        for (int i = 0; i < enemyName.Length; i++)
        {
            index = int.Parse(enemyName[i].Substring(2)) - 1;
            armsSprites.Add(ExcelTool.Instance.animalSprite[index]);
        }
        index = int.Parse(AttachName.Substring(2)) - 1;
        armsSprites.Add(ExcelTool.Instance.animalSprite[index]);
        UIManager.Instance.armsPanel.SetArmsInfo(armsSprites, createModel.level, int.Parse(enemyName[enemyName.Length - 1]) >= 2000);
    }

    //炮塔初始
    private void InitialTurret()
    {
        turretRan = 0; createModel.UpTurretGrade();
        for (int i = 0; i < createModel.productions.Count; i++)
        {
            turretRan += ExcelTool.Instance.shooters[createModel.productions[i]].drop_shooter;
        }
        for (int i = 0; i < spanPoint.childCount; i++)
        {
            if (i < createModel.spanCount)
            {
                spanPoint.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = true;
                spanPoint.GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                Vector3 point = spanPoint.GetChild(i).localPosition;
                point.y += 0.5f;
                spanPoint.GetChild(i).localPosition = point;
                Destroy(spanUnlck.GetChild(i).gameObject);
            }
            else
            {
                spanPoint.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = false;
                spanPoint.GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = false;
            }
        }
        for (int i = 0; i < createModel.conCount; i++)
        {
            spanPoint.GetChild(i).name = "fire";
            spanPoint.GetChild(i).GetChild(0).GetComponent<Renderer>().material = createModel.gunMat;
            //spanPoint.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = true;
            if (spanPoint.GetChild(i).childCount >= 3)
            {
                spanPoint.GetChild(i).GetChild(2).GetComponent<TurretControl>().isFire = true;
            }
        }
        for (int i = 0; i < sendPoint.childCount; i++)
        {
            if(createModel.sendCount > i)
            {
                sendPoint.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = true;
                sendPoint.GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                Vector3 point = sendPoint.GetChild(i).localPosition;
                point.y += 0.5f;
                sendPoint.GetChild(i).localPosition = point;
                Destroy(sendUnlck.GetChild(i).gameObject);
            }
            else
            {
                sendPoint.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = false;
                sendPoint.GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
    //提示框
    public IEnumerator ShowFitTip(int index)
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < createModel.spanCount; i++)
        {
            if (i < spanPoint.childCount && spanPoint.GetChild(i).childCount >= 3)
            {
                for (int j = 0; j < createModel.sendCount; j++)
                {
                    if (j < sendPoint.childCount && sendPoint.GetChild(j).childCount < 3)
                    {
                        conePoint.GetChild(j).gameObject.SetActive(true);
                    }
                }
                break;
            }
        }
    }
    public void HideFitTip(int index)
    {
        if (index < conePoint.childCount)
        {
            conePoint.GetChild(index).gameObject.SetActive(false);
        }
    }
    public void TurretProduc(bool isRemo, int index)
    {
        if (isRemo)
        {
            createModel.productions.Remove(index);
        }
        else
        {
            createModel.productions.Add(index);
        }
        turretRan = 0;
        for (int i = 0; i < createModel.productions.Count; i++)
        {
            turretRan += ExcelTool.Instance.shooters[createModel.productions[i]].drop_shooter;
        }
    }
    //解锁合成台
    public void UnlockSpan_Hook()
    {
        if (createModel.spanCount <= spanPoint.childCount)
        {
            spanPoint.GetChild(createModel.spanCount - 1).GetChild(1).GetComponent<BoxCollider>().enabled = true;
            spanPoint.GetChild(createModel.spanCount - 1).GetChild(0).GetComponent<BoxCollider>().enabled = true;
            Vector3 point = spanPoint.GetChild(createModel.spanCount - 1).localPosition;
            point.y += 0.5f;
            spanPoint.GetChild(createModel.spanCount - 1).localPosition = point;
            Destroy(spanUnlck.GetChild(0).gameObject);
        } 
    }
    //转化炮台
    public void ConvertGun_Hook()
    {
        spanPoint.GetChild(createModel.conCount).name = "fire";
        spanPoint.GetChild(createModel.conCount).GetChild(0).GetComponent<Renderer>().material = createModel.gunMat;
        //spanPoint.GetChild(createModel.conCount).GetChild(1).GetComponent<BoxCollider>().enabled = true;
        if (spanPoint.GetChild(createModel.conCount).childCount >= 3)
        {
            spanPoint.GetChild(createModel.conCount).GetChild(2).GetComponent<TurretControl>().isFire = true;
        }
    }
    //解锁炮台
    public void UnlockSend_Hook()
    {
        if(createModel.sendCount <= sendPoint.childCount)
        {
            sendPoint.GetChild(createModel.sendCount - 1).GetChild(1).GetComponent<BoxCollider>().enabled = true;
            sendPoint.GetChild(createModel.sendCount - 1).GetChild(0).GetComponent<BoxCollider>().enabled = true;
            Vector3 point = sendPoint.GetChild(createModel.sendCount - 1).localPosition;
            point.y += 0.5f;
            sendPoint.GetChild(createModel.sendCount - 1).localPosition = point;
            Destroy(sendUnlck.GetChild(0).gameObject);
        }
    }
    //生成炮塔
    public bool CloneTurret_Hook()
    {
        float num = 0;
        float ran = Random.Range(0, turretRan);
        for (int i = 0; i < createModel.productions.Count; i++)
        {
            num += ExcelTool.Instance.shooters[createModel.productions[i]].drop_shooter;
            if (ran <= num)
            {
                turretIndex = createModel.productions[i];
                break;
            }
        }
        Transform tempParent = null;
        for (int i = 0; i < createModel.sendCount; i++)
        {
            if (i < sendPoint.childCount && sendPoint.GetChild(i).childCount < 3)
            {
                tempParent = sendPoint.GetChild(i);
                HideFitTip(i);
                break;
            }
        }
        if (tempParent == null)
        {
            for (int j = 0; j < createModel.spanCount; j++)
            {
                if (j < spanPoint.childCount && spanPoint.GetChild(j).childCount < 3)
                {
                    tempParent = spanPoint.GetChild(j);
                    break;
                }
            }
        }
        if (tempParent != null)
        {
            GenerateTurret(tempParent);
            tempParent = null;
            return true;
        }
        return false;
    }
    void GenerateTurret(Transform tempParent)
    {
        var go = ObjectPool.Instance.CreateObject(createModel.turretPrefab[turretIndex].name, createModel.turretPrefab[turretIndex]).GetComponent<TurretControl>();
        go.transform.SetParent(tempParent, true);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = new Vector3(0, 15, 0);
        go.transform.DOLocalMove(createModel.turretPoint, 1f);
        go.transform.localEulerAngles = createModel.turretAngle;
        go.SetNumerical(createModel.turretGrade);
        go.PlayDragon("born");
        createModel.turrets.Add(go.transform);
        if (createModel.isSnake)
            go.CreateSnake(createModel.snakePrefab);
        StartCoroutine(ShowMerge(go));
    }
    IEnumerator ShowMerge(TurretControl turr)
    {
        yield return new WaitForSeconds(1);
        turr.isMerge = true;
    }
    //判断是否可以克隆炮塔
    public bool HintTurret_Hook()
    {
        CancelInvoke("TrashAnimation");
        for (int j = 0; j < createModel.spanCount; j++)
        {
            fullPoint.GetChild(j).gameObject.SetActive(false);
        }
        for (int i = 0; i < createModel.spanCount; i++)
        {
            if (i<spanPoint.childCount && spanPoint.GetChild(i).childCount < 3)
            {
                return true;
            }
        }
        for (int i = 0; i < createModel.sendCount; i++)
        {
            if (i < sendPoint.childCount && sendPoint.GetChild(i).childCount < 3)
            {
                return true;
            }
        }
        for (int i = 0; i < createModel.spanCount; i++)
        {
            fullPoint.GetChild(i).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetString("Guide7") == "")
        {
            if (createModel.JudeMerge())
                UIManager.Instance.guidePanel.OpenGuide(7, false);
        }
        InvokeRepeating("TrashAnimation", 0.5f, 1);
        return false;
    }
    //自动合并
    public void MergeTurret_Hook()
    {
        TurretControl turretObject1 = null;
        TurretControl turretObject2 = null;
        for (int i = 0; i < spanPoint.childCount; i++)
        {
            if (spanPoint.GetChild(i).childCount >= 3)
            {
                turretObject1 = spanPoint.GetChild(i).GetChild(2).GetComponent<TurretControl>();
                for (int j = i + 1; j < spanPoint.childCount; j++)
                {
                    if (spanPoint.GetChild(j).childCount >= 3)
                    {
                        turretObject2 = spanPoint.GetChild(j).GetChild(2).GetComponent<TurretControl>();
                        if (turretObject1.isMerge && turretObject2.isMerge && 
                            turretObject1.state == turretObject2.state && 
                            turretObject1.grade == turretObject2.grade)
                        {
                            StartCoroutine(DelyMergeTurret(turretObject1, turretObject2));
                            return;
                        }
                    }
                }
            }
        }
    }
    IEnumerator DelyMergeTurret(TurretControl turret1, TurretControl turret2)
    {
        createModel.HideCueCone();
        turret2.transform.parent = transform;
        turret1.GetComponent<Collider>().enabled = false;
        turret2.GetComponent<Collider>().enabled = false;
        turret2.PlayDragon("walk");
        turret2.isFire = false; ;
        turret2.transform.DOJump(turret1.transform.position, 3, 1, 0.5f);
        UIManager.Instance.IsCreateTureet();
        createModel.turrets.Remove(turret2.transform);
        yield return new WaitForSeconds(0.5f);
        turret1.GetComponent<Collider>().enabled = true;
        turret2.GetComponent<Collider>().enabled = true;
        turret1.UpGrade();
        TurretDrag.Instance.Composite(turret1.transform.parent.position, (int)turret1.state);
        ObjectPool.Instance.CollectObject(turret2.gameObject);
    }

    //敌兵克隆
    private void CloneEnemy()
    {
        if (suol_curve >= sumAnimal)
        {
            if (levelData.boos != "-1")
            {
                enemyData = ExcelTool.Instance.enemys[boosId[0]];
                CloneBoos();
                suol_curve += enemyData.soul;
                //StartCoroutine(CloneBoos());
            }
            return;
        }
        ArrConversion();
        if (enemyCount != 0)
        {
            enemyPrefab = allEnemy[enemyName[enemyId]];
            enemyData = ExcelTool.Instance.enemys[enemyName[enemyId]];
            enemyLevel = float.Parse(enemyLever[enemyId]) + 300 * (rounds - 1) + extraLevel;
            enemyHp = enemyData.hp + enemyData.hp * enemyData.hp_growth * 0.01f * (enemyLevel - 1);
            if (rounds >= 2)
            {
                float tempHp = enemyData.hp + enemyData.hp * enemyData.hp_growth * 0.01f * (float.Parse(enemyLever[enemyId]) + 300 * (rounds - 1));
                enemyScale = tempHp * 0.02f + tempHp / (9 + rounds * 40);
            }
            else
            {
                enemyScale = enemyHp * 0.02f + enemyHp / (9 + rounds * 40);
            }
            for (int i = 0; i < enemyCount; i++)
            {
                DealOne(false);
                suol_curve += enemyData.soul;
            }
        }
        if (isAddAnimal && suol_curve >= sumAnimal * 0.75f)
        {
            isAddAnimal = false;
            CreateForty();
            suol_curve += enemyData.soul;
        }
        if (enemyName.Length >= 1)
        {
            enemyId++;
            if (enemyId >= enemyName.Length)
            {
                enemyId = 0;
            }
        }

        if (suol_curve >= sumAnimal && levelData.boos != "-1")
        {
            cloneTime = 3;
            KeelAnimation.Instance.WarningArmature();
        }

        if (suol_curve >= suol_max)
        {
            if (PlayerPrefs.GetString("FirstSkill" + 1) == "")
            {
                UIManager.Instance.rewardsPanel.OpenPanel();
            };
        }
        if (!isPattern)
        {
            if (suol_curve >= suol_max && levelData.boos == "-1")
            {
                float num = 5 * (rounds);
                for (int i = 0; i < num; i++)
                {
                    CreateForty();
                }
                cloneTime = 10;
                createModel.level += 1;
                StartCoroutine(RefreshLevel());
            }
        }
    }
    //enemyScale = enemyHp * 0.02f + enemyHp * 0.5f;
    private void CreateForty()
    {
        enemyPrefab = allEnemy[AttachName];
        enemyData = ExcelTool.Instance.enemys[AttachName];
        enemyLevel = float.Parse(enemyLever[0]) + 5 + 300 * (rounds - 1) + extraLevel;
        enemyHp = enemyData.hp + enemyData.hp * enemyData.hp_growth * 0.01f * (enemyLevel - 1);
        if (rounds >= 2)
        {
            float tempHp = enemyData.hp + enemyData.hp * enemyData.hp_growth * 0.01f * (float.Parse(enemyLever[0]) + 5 + 300 * (rounds - 1));
            enemyScale = enemyHp * 0.02f + enemyHp * 0.5f;
        }
        else
        {
            enemyScale = enemyHp * 0.02f + enemyHp * 0.5f;
        }
        for (int i = 0; i < rounds; i++)
        {
            DealOne(true);
        }
    }

    private void DealOne(bool isTroo)
    {
        enemyScale = Mathf.Clamp(enemyScale, 80, 200);
        var enemy = ObjectPool.Instance.CreateObject(enemyPrefab.name, enemyPrefab).transform;
        enemy.SetParent(enemyPoint.GetChild(0).GetChild(Random.Range(0, 6)), true);
        if (createModel.sendTroops)
        {
            enemy.localPosition = new Vector3(0, Random.Range(35, 40), -5);
        }
        else if (isTroo)
        {
            enemy.localPosition = new Vector3(0, Random.Range(35, 40), 0);
        }
        else
        {
            enemy.localPosition = new Vector3(0, Random.Range(35, 40), Random.Range(-85, 10));
        }
        //if (isPattern)
        //{
        //    if (createModel.sendTroops)
        //    {
        //        enemy.localPosition = new Vector3(0, Random.Range(35, 40), -5);
        //    }
        //    else if (isTroo)
        //    {
        //        enemy.localPosition = new Vector3(0, Random.Range(35, 40), 0);
        //    }
        //    else
        //    {
        //        enemy.localPosition = new Vector3(0, Random.Range(35, 40), Random.Range(-80, 10));
        //    }
        //}
        //else
        //{
        //    if (createModel.sendTroops)
        //    {
        //        enemy.localPosition = new Vector3(0, Random.Range(35, 40), -5);
        //    }
        //    else
        //    {
        //        enemy.localPosition = new Vector3(Random.Range(-3.6f, 3.6f), Random.Range(35, 40), Random.Range(-5, 1));
        //    }
        //}
        enemy.localEulerAngles = enemyPrefab.transform.localEulerAngles;
        enemy.localScale = Vector3.one * enemyScale;
        enemy.GetComponent<EnemyControl>().SetNumerical(enemyData, enemyLevel, enemyHp,rounds);
        createModel.enemys.Add(enemy);
        StartCoroutine(CloningSpecial(Random.Range(0.5f, 2.5f)));
    }
    //体型大小=敌人血量/50+敌人血量/(1+轮数）
    private void CloneBoos()
    {
        UIManager.Instance.guidePanel.OpenGuide(12, false);
        for (int i = 0; i < Random.Range(7, 15); i++)
        {
            StartCoroutine(CloningSpecial(Random.Range(0, 1.5f)));
        }
        enemyLevel = float.Parse(boosId[1]) + (rounds - 1) * 70 + extraLevel;
        boosName = (float.Parse(boosId[0]) - 1000).ToString();
        enemyPrefab = allEnemy[boosName];
        enemyHp = enemyData.hp + enemyData.hp * enemyData.hp_growth * 0.01f * (enemyLevel - 1);
        var enemy = ObjectPool.Instance.CreateObject(enemyPrefab.name, enemyPrefab).transform;
        enemy.SetParent(enemyPoint, true);
        enemy.localPosition = new Vector3(0, 40, 0);
        enemy.localEulerAngles = enemyPrefab.transform.localEulerAngles;
        enemyScale = enemyHp * 0.02f + enemyHp * 0.5f;
        enemyScale = Mathf.Clamp(enemyScale, 250, 420);
        enemy.localScale = Vector3.one * enemyScale;
        enemy.GetComponent<EnemyControl>().SetNumerical(enemyData, enemyLevel, enemyHp,rounds, true);
        createModel.enemys.Add(enemy);
        if (!isPattern)
        {
            cloneTime = 10;
            createModel.level += 1;
            StartCoroutine(RefreshLevel());
        }
    }
    IEnumerator CloningSpecial(float day)
    {
        yield return new WaitForSeconds(day);
        var effCube = ObjectPool.Instance.CreateObject(embellishPrefab.name, embellishPrefab).transform;
        effCube.SetParent(enemyPoint, true);
        effCube.localPosition = new Vector3(Random.Range(-3.6f, 3.6f), 30, -2);
    }
    void ArrConversion()
    {
        float num = 0;
        enemyCount = 0;
        enemyId = 0;
        float ran = Random.Range(0f, 1f);
        for (int i = 0; i < enemyRan.Count; i++)
        {
            string[] mess = enemyRan[i].Split('|');
            for (int j = 0; j < mess.Length; j++)
            {
                num += float.Parse(mess[j]) / enemySum;
                if (num >= ran)
                {
                    enemyId = i;
                    enemyCount = j;
                    return;
                }
            }
        }
    }
    void ChanceSum()
    {
        enemySum = 0;
        enemyRan.Clear();
        if (levelData.soldier_1 != "-1")
        {
            enemyRan.Add(levelData.soldier_1);
            string[] sp = levelData.soldier_1.Split('|');
            for (int i = 0; i < sp.Length; i++)
            {
                enemySum += float.Parse(sp[i]) + (rounds - 1) * 5;
            }
        }
        if (levelData.soldier_2 != "-1")
        {
            enemyRan.Add(levelData.soldier_2);
            string[] sp = levelData.soldier_2.Split('|');
            for (int i = 0; i < sp.Length; i++)
            {
                enemySum += float.Parse(sp[i]) + (rounds - 1) * 5;
            }
        }
        if (levelData.soldier_3 != "-1")
        {
            enemyRan.Add(levelData.soldier_3);
            string[] sp = levelData.soldier_3.Split('|');
            for (int i = 0; i < sp.Length; i++)
            {
                enemySum += float.Parse(sp[i]) + (rounds - 1) * 5;
            }
        }
        if (levelData.soldier_4 != "-1")
        {
            enemyRan.Add(levelData.soldier_4);
            string[] sp = levelData.soldier_4.Split('|');
            for (int i = 0; i < sp.Length; i++)
            {
                enemySum += float.Parse(sp[i]) + (rounds - 1) * 5;
            }
        }
        if (levelData.soldier_5 != "-1")
        {
            enemyRan.Add(levelData.soldier_5);
            string[] sp = levelData.soldier_5.Split('|');
            for (int i = 0; i < sp.Length; i++)
            {
                enemySum += float.Parse(sp[i]) + (rounds - 1) * 5;
            }
        }
        if (levelData.soldier_6 != "-1")
        {
            enemyRan.Add(levelData.soldier_6);
            string[] sp = levelData.soldier_6.Split('|');
            for (int i = 0; i < sp.Length; i++)
            {
                enemySum += float.Parse(sp[i]) + (rounds - 1) * 5;
            }
        }
        if (levelData.soldier_7 != "-1")
        {
            enemyRan.Add(levelData.soldier_7);
            string[] sp = levelData.soldier_7.Split('|');
            for (int i = 0; i < sp.Length; i++)
            {
                enemySum += float.Parse(sp[i]) + (rounds - 1) * 5;
            }
        }
    }
    public void ClearGlodal()
    {
        isTroops = false;
        if(enemyName != null)
        {
            for (int i = 0; i < enemyName.Length; i++)
            {
                ObjectPool.Instance.Clear(enemyName[i]);
            }
        }
        if(AttachName != null)
        {
            ObjectPool.Instance.Clear(AttachName);
        }
        if (levelData.boos!="-1" && boosName != null)
        {
            ObjectPool.Instance.Clear(boosName);
        }
    }

    //数据保存
    public void SaveHook()
    {
        if (isPattern)
        {
            PlayerPrefs.SetFloat("Level", createModel.level);
        }
        spanMsg = ""; sendMsg = "";
        TurretControl turret = null;
        for (int i = 0; i < sendPoint.childCount; i++)
        {
            if (sendPoint.GetChild(i).childCount >= 3)
            {
                turret = sendPoint.GetChild(i).GetChild(2).GetComponent<TurretControl>();
                sendMsg += string.Format("{0}-{1}-{2},", i, (int)turret.state, turret.grade);
            }
        }
        for (int i = 0; i < spanPoint.childCount; i++)
        {
            if (spanPoint.GetChild(i).childCount >= 3)
            {
                turret = spanPoint.GetChild(i).GetChild(2).GetComponent<TurretControl>();
                spanMsg += string.Format("{0}-{1}-{2},", i, (int)turret.state, turret.grade);
            }
        }
        turret = null;
        PlayerPrefs.SetString("SpanMessg", spanMsg);
        PlayerPrefs.SetString("SendMessg", sendMsg);
    }

    void TrashAnimation()
    {
        trashAnimal.Play("rubbishbin", 0, 0);
    }
}
