using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteMode : MonoBehaviour
{
    public Button next_wave;
    public Image waveImage;
    public Transform[] routePrefab;

    private GameObject embellishPrefab;
    private GameObject cakePrefab;
    private GameObject bronPrefab;
    private GameObject lockPrefab;
    private GameObject cueConePrefab;
    private Transform linebgPrent;
    private Transform enemyParent;
    private Transform lockParent;
    private Transform currentMap;
    private Transform turretAll;
    private Transform routePath;
    private Transform fittable;
    private Transform fittable_lock;
    private Transform sighTip;
    private LineRenderer lineRenderer;

    //路线模式数据
    private Dictionary<string, List<Transform>> routePoints = new Dictionary<string, List<Transform>>();
    private List<Transform> turretPoints = new List<Transform>();
    private List<GameObject> turretCone = new List<GameObject>();
    private string[] type_troops;
    private string[] level_troops;
    private string[] number_troops;
    private string[] line_order;
    private Vector3 startPoints;
    private Vector3 endPoints;
    private LineItem roudeItem;
    private float wave_interval;
    private float wave_time;
    private float wave_mintime;
    private float wave_maxtime;
    private int wave_max;
    private int wave_curret;
    private int fly_wave;
    private float enemy_time;
    private float enemy_interval;
    private float soul_max;
    private float soul_curr;
    private float soul_sum;
    private float soul_wave;
    //敌人
    private Dictionary<string, GameObject> allEnemy = new Dictionary<string, GameObject>();
    public List<Sprite> armsSprites = new List<Sprite>();
    private EnemyItem enemyData;
    private GameObject enemyPrefab;
    private float enemyScale;
    private int enemyCount = 0;
    private int enemyNumber = 0;
    private float enemyHp;
    private int enemyLevel;
    private string enemyName;
    public int levelmultiple;

    //炮塔
    public int terretMax;
    private float turretRan;
    private int turretIndex;
    private string lineCurret;

    private Text wavetext_time;
    private CreateModel createModel;
    private bool isWave;
    private bool isTroops;
    private bool isSigh;

    public void InitData()
    {
        embellishPrefab = Resources.Load<GameObject>("Effects/EffCube");
        createModel = transform.parent.GetComponent<CreateModel>();
        wavetext_time = next_wave.transform.Find("TimeText").GetComponent<Text>();
        //waveImage = next_wave.transform.Find("TipImage").GetComponent<Image>();
        lockPrefab = transform.Find("Prefab/SuoPrefab").gameObject;
        cakePrefab = transform.Find("Prefab/Cake").gameObject;
        bronPrefab = transform.Find("Prefab/StarCube").gameObject;
        cueConePrefab = transform.Find("Prefab/CueCone").gameObject;
        sighTip = transform.Find("Prefab/Sigh");
        enemyParent = transform.Find("EnemyParent");
        lockParent = transform.Find("LockParent");
        linebgPrent = transform.Find("LineBg");
        lineRenderer = transform.Find("Prefab/LineRend").GetComponent<LineRenderer>();

        routePath = transform.Find("Path");
        fittable = transform.Find("Fort");
        fittable_lock = transform.Find("SpanLock");

        createModel.turretPoint = new Vector3(0,1,0);
        createModel.turretAngle = new Vector3(10,0,0);//Vector3.zero;
        next_wave.onClick.AddListener(NextWave);
        var enemy = ExcelTool.Instance.enemyAnimal;
        for (int i = 0; i < enemy.Length; i++)
        {
            allEnemy.Add(enemy[i].name, enemy[i].gameObject);
        }
        InitialData();
    }

    private void InitialData()
    {
        turretRan = 0;
        createModel.level = PlayerPrefs.GetFloat("RouteLevel",1);
        createModel.passLevel = PlayerPrefs.GetFloat("RoutePassLevel");
        for (int i = 0; i < createModel.productions.Count; i++)
        {
            turretRan += ExcelTool.Instance.shooters[createModel.productions[i]].drop_shooter;
        }
        //LevelTutorial(createModel.spanCount);
        //for (int i = 0; i < fittable.childCount; i++)
        //{
        //    if(createModel.spanCount > i)
        //    {
        //        fittable.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = true;
        //        fittable.GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = true;
        //        fittable_lock.GetChild(i).gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        fittable.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = false;
        //        fittable.GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = false;
        //        fittable_lock.GetChild(i).gameObject.SetActive(true);
        //    }
        //}
    }

    public void LevelTutorial(bool ishide)
    {
        if (ishide)
        {
            createModel.sendCount = 20;
            createModel.spanCount = fittable.childCount;
        }
        else
        {
            createModel.sendCount = PlayerPrefs.GetInt("SendCount", 8);
            createModel.spanCount = PlayerPrefs.GetInt("SpanCount", 6);
        }
        for (int i = 0; i < fittable.childCount; i++)
        {
            if (createModel.spanCount > i)
            {
                fittable.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = true;
                fittable.GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                fittable_lock.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                fittable.GetChild(i).GetChild(1).GetComponent<BoxCollider>().enabled = false;
                fittable.GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = false;
                fittable_lock.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    public void UpdateWave()
    {
        if (isTroops) return;
        if (createModel.enemys.Count <= 30 && soul_curr < soul_max)
        {
            if(wave_curret < wave_max && isWave)
            {
                wave_time += Time.deltaTime;
                wavetext_time.text = (wave_interval - wave_time).ToString("F0");
                if (wave_time > wave_interval)
                {
                    wave_time = 0;
                    InitEnemy();
                }
            }
            else if (enemyNumber < enemyCount)
            {
                enemy_time += Time.deltaTime;
                if (enemy_time >= enemy_interval)
                {
                    enemy_time = 0;
                    enemy_interval = Random.Range(wave_mintime, wave_maxtime);
                    CreateEnemy();
                }
            }
        }
       
    }
    //读取当前关卡
    public void ReadTheLevel()
    {
        wave_curret = 0;
        enemy_time = 0;
        soul_sum = 0;
        soul_max = 0;
        soul_curr = 0;
        soul_wave = 0;
        terretMax = 0;
        enemy_interval = 1;
        lineCurret = "";
        CancelInvoke("DelyArms");
        roudeItem = ExcelTool.Instance.lines[createModel.level.ToString()];

        line_order = roudeItem.line_order.Split('|');
        string[] time = roudeItem.enemy_interval.Split('|');
        wave_mintime = float.Parse(time[0]);
        wave_maxtime = float.Parse(time[1]);
        wave_max = line_order.Length;
        CreateMap();
        CreateRoute();
        CreateWave();
        AllArms();
        CreateSight();
        createModel.colorTime = 0;
        createModel.CutWeather(roudeItem.weatherID);
        createModel.AngleSwitch(roudeItem.time);
        createModel.ColorSwitch(roudeItem.lightcolor);
        createModel.HideCueCone();
        createModel.starColor = RenderSettings.skybox.GetColor("_Tint");
        ColorUtility.TryParseHtmlString("#" + roudeItem.skycolor, out createModel.skyColor);
        if (createModel.starColor == createModel.skyColor)
        {
            createModel.colorTime = 35;
        }
        for (int i = 0; i < lockParent.childCount; i++)
        {
            lockParent.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < linebgPrent.childCount; i++)
        {
            linebgPrent.GetChild(i).gameObject.SetActive(false);
        }
        isTroops = true;
        linebgPrent.GetChild(roudeItem.Line_Bg).gameObject.SetActive(true);
        UIManager.Instance.SetWave(line_order.Length, wave_curret);
        UIManager.Instance.SetRoudeLevel(createModel.level, roudeItem.start_gold,roudeItem.add_suger);//roudeItem.start_gold
    }
    public void WaveInterval(float interval)
    {
        wave_interval = interval;
    }
    //读取敌人
    private void AllArms()
    {
        int index = 0;
        armsSprites.Clear();
        for (int i = 0; i < type_troops.Length; i++)
        {
            index = int.Parse(type_troops[i].Substring(2)) - 1;
            armsSprites.Add(ExcelTool.Instance.animalSprite[index]);
        }
        UIManager.Instance.enemyInfoPanel.SetArmsInfo(roudeItem,createModel.enemyMultiple, levelmultiple);
        Invoke("DelyArms", 4);
    }
    private void DelyArms()
    {
        isTroops = false;
    }
    //生成地图
    private void CreateMap()
    {
        if (currentMap)
        {
            Destroy(currentMap.gameObject);
        }
        turretPoints.Clear();
        currentMap = Instantiate(routePrefab[(int.Parse(roudeItem.line_id) - 1)]);
        currentMap.SetParent(transform);
        currentMap.localPosition = Vector3.forward * 41f;
        currentMap.localScale = Vector3.one;
        currentMap.localEulerAngles = Vector3.zero;
        turretAll = currentMap.Find("fire");
        turretAll.gameObject.SetActive(true);
        string[] turretName = roudeItem.shooter_position.Split('|');
        for (int i = 0; i < turretAll.childCount; i++)
        {
            turretAll.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < turretName.Length; i++)
        {
            Transform turret = turretAll.Find("fire_" + turretName[i]);
            if(turret)
            {
                turretPoints.Add(turret);
                turret.gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < turretCone.Count; i++)
        {
            turretCone[i].SetActive(false);
        }
        turretCone.Clear();
        for (int i = 0; i < turretPoints.Count; i++)
        {
            Vector3 point = turretPoints[i].position;
            point.y += 1f;
            var cueCone = ObjectPool.Instance.CreateObject("ConneRoute", cueConePrefab);
            cueCone.transform.SetParent(transform);
            cueCone.transform.localPosition = point;
            turretCone.Add(cueCone);
        }
        for (int i = 0; i < turretCone.Count; i++)
        {
            turretCone[i].SetActive(false);
        }
        string[] bronName = roudeItem.start.Split('|');
        isSigh = bronName.Length >= 2;
        for (int i = 0; i < bronName.Length; i++)
        {
            var bron = ObjectPool.Instance.CreateObject(bronPrefab.name,bronPrefab);
            bron.transform.SetParent(createModel.cakeCon.transform);
            bron.transform.localPosition= routePath.GetChild(int.Parse(bronName[i])).position;
        }
        string[] cakeName = roudeItem.end.Split('|');
        createModel.cakeCon.cakeHealths = new CakeHealth[cakeName.Length];
        createModel.cakePoint = new Vector3[cakeName.Length];
        for (int i = 0; i < cakeName.Length; i++)
        {
            var cake = ObjectPool.Instance.CreateObject(cakePrefab.name, cakePrefab);
            cake.transform.SetParent(createModel.cakeCon.transform);
            cake.transform.localPosition = routePath.GetChild(int.Parse(cakeName[i])).position;
            createModel.cakeCon.cakeHealths[i] = cake.GetComponent<CakeHealth>();
            createModel.cakePoint[i] = cake.transform.position;
        }
        createModel.cakeCon.SetHealth();
    }
    //生成线路
    private void CreateRoute()
    {
        routePoints.Clear();
        Transform point = null;
        if (roudeItem.line_1 != "0")
        {
            string[] line_1 = roudeItem.line_1.Split('|');
            routePoints.Add("1", new List<Transform>());
            for (int i = 0; i < line_1.Length; i++)
            {
                point = routePath.GetChild(int.Parse(line_1[i]));
                if(point)
                {
                    routePoints["1"].Add(point);
                }
            }
        }
        if (roudeItem.line_2 != "0")
        {
            string[] line_2 = roudeItem.line_2.Split('|');
            routePoints.Add("2", new List<Transform>());
            for (int i = 0; i < line_2.Length; i++)
            {
                point = routePath.GetChild(int.Parse(line_2[i]));
                if (point)
                {
                    routePoints["2"].Add(point);
                }
            }
        }
        if (roudeItem.route_1 != "0")
        {
            string[] route_1 = roudeItem.route_1.Split('|');
            routePoints.Add("3", new List<Transform>());
            for (int i = 0; i < route_1.Length; i++)
            {
                point = routePath.GetChild(int.Parse(route_1[i]));
                if (point)
                {
                    routePoints["3"].Add(point);
                }
            }
        }
        if (roudeItem.route_2 != "0")
        {
            string[] route_2 = roudeItem.route_2.Split('|');
            routePoints.Add("4", new List<Transform>());
            for (int i = 0; i < route_2.Length; i++)
            {
                point = routePath.GetChild(int.Parse(route_2[i]));
                if (point)
                {
                    routePoints["4"].Add(point);
                }
            }
        }
        if (roudeItem.route_3 != "0")
        {
            string[] route_3 = roudeItem.route_3.Split('|');
            routePoints.Add("5", new List<Transform>());
            for (int i = 0; i < route_3.Length; i++)
            {
                point = routePath.GetChild(int.Parse(route_3[i]));
                if (point)
                {
                    routePoints["5"].Add(point);
                }
            }
        }
        if (roudeItem.route_4 != "0")
        {
            string[] route_4 = roudeItem.route_4.Split('|');
            routePoints.Add("6", new List<Transform>());
            for (int i = 0; i < route_4.Length; i++)
            {
                point = routePath.GetChild(int.Parse(route_4[i]));
                if (point)
                {
                    routePoints["6"].Add(point);
                }
            }
        }
        point = null;
    }
    //设置路线图
    private void SetLineRender(string index)
    {
        if(lineCurret != index)
        {
            lineCurret = index;
            lineRenderer.positionCount = routePoints[line_order[wave_curret]].Count;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, routePoints[line_order[wave_curret]][i].position);
            }
            //if(int.Parse(lineCurret) >= 3)
            //{
            //    lineRenderer.transform.localPosition = Vector3.up * 4;
            //}
            //else
            //{
            //    lineRenderer.transform.localPosition = Vector3.up * 0.1f;
            //}
        }
    }
    //生成出口提示
    private void CreateSight()
    {
        if(isSigh && wave_curret < wave_max)
        {
            Vector3 point= routePoints[line_order[wave_curret]][0].position;
            point.y += 3;
            sighTip.position = point;
            sighTip.gameObject.SetActive(true);
        }
        isWave = true;
        wave_time = 0;
        waveImage.sprite = armsSprites[wave_curret];
        waveImage.SetNativeSize();
        wavetext_time.text = wave_interval.ToString("F0");
        next_wave.gameObject.SetActive(true);
        SetLineRender(line_order[wave_curret]);
    }
    //生成波次
    private void CreateWave()
    {
        type_troops = roudeItem.line_enemy_type.Split('|');
        level_troops = roudeItem.line_enemy_level.Split('|');
        number_troops = roudeItem.line_enemy_num.Split('|');

        for (int i = 0; i < number_troops.Length; i++)
        {
            soul_max += Mathf.CeilToInt(int.Parse(number_troops[i]) * createModel.enemyMultiple);
        }
    }
    //生成炮台
    public bool CreateTurret_Roude()
    {
        //if (createModel.sendTroops) return false;
        Transform tempParent = null;
        for (int i = 0; i < createModel.spanCount; i++)
        {
            if(fittable.childCount > i)
            {
                if (fittable.GetChild(i).childCount < 3)
                {
                    tempParent = fittable.GetChild(i);
                    break;
                }
            }
        }
        if (tempParent != null)
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
            GenerateTurret(tempParent);
            tempParent = null;
            return true;
        }
        return false;
    }
    private void GenerateTurret(Transform tempParent)
    {
        var go = ObjectPool.Instance.CreateObject(createModel.turretPrefab[turretIndex].name, createModel.turretPrefab[turretIndex]).GetComponent<TurretControl>();
        go.transform.SetParent(tempParent, true);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = new Vector3(0, 15, 0);
        go.transform.DOLocalMove(createModel.turretPoint,1f);
        go.transform.localEulerAngles = createModel.turretAngle;
        go.SetNumerical(createModel.turretGrade);
        go.PlayDragon("born");
        createModel.turrets.Add(go.transform);
        if (createModel.isSnake)
            go.CreateSnake(createModel.snakePrefab);
        StartCoroutine(ShowMerge(go));
        if(PlayerPrefs.GetString("Guide21") == "")
        {
            createModel.sendTroops = true;
            PlayerPrefs.SetString("Guide2", "index");
            UIManager.Instance.GuideInfo(ExcelTool.lang["guideinfo1"]);
            UIManager.Instance.guidePanel.OpenGuide(21,true);
        }
        else if (PlayerPrefs.GetString("guideinfo2") == "")
        {
            createModel.sendTroops = true;
            UIManager.Instance.GuideInfo(ExcelTool.lang["guideinfo2"]);
        }
    }
    private IEnumerator ShowMerge(TurretControl turr)
    {
        yield return new WaitForSeconds(1);
        turr.isMerge = true;
        ShowCueCone();
    }
    public bool HintTurret_Roude()
    {
        for (int i = 0; i < createModel.spanCount; i++)
        {
            if (fittable.childCount > i)
            {
                if (fittable.GetChild(i).childCount < 3)
                {
                    return true;
                }
            }
        }
        if(createModel.JudeMerge())
        {
            if (PlayerPrefs.GetString("Guide7") == "")
            {
                UIManager.Instance.guidePanel.OpenGuide(7, false);
            }
            else if(!UIManager.Instance.leaddestroy.activeInHierarchy)
            {
                float min = 10000;
                Vector3 point = Vector3.zero;
                for (int i = 0; i < createModel.spanCount; i++)
                {
                    if (fittable.childCount > i && fittable.GetChild(i).childCount >= 3)
                    {
                        if (min >= fittable.GetChild(i).GetChild(2).GetComponent<TurretControl>().grade)
                        {
                            min = fittable.GetChild(i).GetChild(2).GetComponent<TurretControl>().grade;
                            point = fittable.GetChild(i).position;
                        }
                    }
                }
                if (point != Vector3.zero)
                {
                    UIManager.Instance.LeadDestroy(true, point, createModel.trashCan.position);
                }
            }
        }
        return false;
    }
    //解锁
    public void UnlockFittable_Roude()
    {
        if(fittable_lock.childCount >= createModel.spanCount)
        {
            fittable.GetChild(createModel.spanCount - 1).GetChild(1).GetComponent<BoxCollider>().enabled = true;
            fittable.GetChild(createModel.spanCount - 1).GetChild(0).GetComponent<BoxCollider>().enabled = true;
            fittable_lock.GetChild(createModel.spanCount - 1).gameObject.SetActive(false);
        }
    }
    //炮台位置锁
    public void UnlockTurret_Roude()
    {
        int index = 0;
        for (int i = 0; i < turretPoints.Count; i++)
        {
            if (turretPoints[i].childCount >= 3)
            {
                index++;
            }
        }
        if(terretMax < index)
        {
            terretMax = index;
        }
        ShowCueCone();
        UnlockHide_Roude();
        if (index >= createModel.sendCount)
        {
            for (int i = 0; i < turretPoints.Count; i++)
            {
                if (turretPoints[i].childCount < 3)
                {
                    //生成锁
                    var suo = ObjectPool.Instance.CreateObject(lockPrefab.name, lockPrefab);
                    suo.transform.SetParent(lockParent);
                    suo.transform.localScale = Vector3.one * 70;
                    Vector3 point = turretPoints[i].position;
                    point.y += 0.5f;
                    suo.transform.localPosition = point;
                    turretPoints[i].GetChild(1).GetComponent<Collider>().enabled = false;
                    turretPoints[i].GetChild(0).GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
    public void UnlockHide_Roude()
    {
        for (int i = 0; i < lockParent.childCount; i++)
        {
            lockParent.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < turretPoints.Count; i++)
        {
            turretPoints[i].GetChild(0).GetComponent<Collider>().enabled = true;
            turretPoints[i].GetChild(1).GetComponent<Collider>().enabled = true;
        }
    }
    //自动合并
    public void MergeTurret_Roude()
    {
        TurretControl turretObject1 = null;
        TurretControl turretObject2 = null;
        for (int i = 0; i < fittable.childCount; i++)
        {
            if (fittable.GetChild(i).childCount >= 3)
            {
                turretObject1 = fittable.GetChild(i).GetChild(2).GetComponent<TurretControl>();
                for (int j = i + 1; j < fittable.childCount; j++)
                {
                    if (fittable.GetChild(j).childCount >= 3)
                    {
                        turretObject2 = fittable.GetChild(j).GetChild(2).GetComponent<TurretControl>();
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
        turret2.isFire = false;
        turret2.transform.DOJump(turret1.transform.position, 3, 1, 0.5f);
        UIManager.Instance.IsCreateTureet();
        createModel.turrets.Remove(turret2.transform);
        yield return new WaitForSeconds(0.5f);
        if(turret1 && turret2)
        {
            turret1.GetComponent<Collider>().enabled = true;
            turret2.GetComponent<Collider>().enabled = true;
            turret1.UpGrade();
            TurretDrag.Instance.Composite(turret1.transform.parent.position, (int)turret1.state);
            ObjectPool.Instance.CollectObject(turret2.gameObject);
        }
    }
    //回收敌人 tiproude4
    public void SoulControl()
    {
        soul_sum++;
        if (soul_sum >= soul_max)
        {
            StartCoroutine(MaxPass());
        }
        else if(soul_curr >= soul_max && createModel.enemys.Count <= 0)
        {
            StartCoroutine(MaxPass());
        }
        else
        {
            if (soul_sum >= soul_wave && !isWave)
            {
                wave_curret += 1;
                CreateSight();
            }
        }
    }
    //初始敌人
    public void InitEnemy()
    {
        isWave = false;
        enemyNumber = 0;
        fly_wave = int.Parse(line_order[wave_curret]);
        enemyName = type_troops[wave_curret];
        enemyLevel = int.Parse(level_troops[wave_curret]) * levelmultiple;
        enemyCount = Mathf.CeilToInt(int.Parse(number_troops[wave_curret])*createModel.enemyMultiple);
        startPoints = routePoints[line_order[wave_curret]][0].position;
        enemyData = ExcelTool.Instance.enemys[enemyName];
        if (float.Parse(enemyName) > 2000)
        {
            enemy_interval = 3;
            enemyName = (float.Parse(enemyName) - 1000).ToString();
            enemyPrefab = allEnemy[enemyName];
            KeelAnimation.Instance.WarningArmature();
        }
        enemyPrefab = allEnemy[enemyName];
        soul_wave += enemyCount;
        enemyHp = enemyData.hp + enemyData.hp * enemyData.hp_growth * 0.01f * (enemyLevel - 1);
        enemyScale = enemyHp * 0.02f + enemyHp * 0.5f;
        enemyScale = Mathf.Clamp(enemyScale, 45, 55);
        next_wave.gameObject.SetActive(false);
        sighTip.gameObject.SetActive(false);
        UIManager.Instance.SetWave(line_order.Length,wave_curret+1);
    }
    //生成敌人
    private void CreateEnemy()
    {
        enemyNumber += 1;
        var enemy = ObjectPool.Instance.CreateObject(enemyPrefab.name, enemyPrefab).transform;
        enemy.SetParent(enemyParent, true);
        if (fly_wave >= 3)
        {
            startPoints.y = 4.2f;
        }
        enemy.localPosition = startPoints;
        enemy.localScale = Vector3.one * enemyScale;
        enemy.localEulerAngles = Vector3.zero;
        enemy.GetComponent<EnemyControl>().SetNumerical(enemyData, enemyLevel, enemyHp, 0, false, routePoints[line_order[wave_curret]]);
        createModel.enemys.Add(enemy);
        soul_curr++;
    }
    private void NextWave()
    {
        if(wave_time < wave_interval)
        {
            wave_time = wave_interval + 1;
        }
        next_wave.gameObject.SetActive(false);
        sighTip.gameObject.SetActive(false);
        AudioManager.Instance.PlayTouch("Production_1");
    }
    private IEnumerator MaxPass()
    {
        UIManager.Instance.isTime = true;
        yield return new WaitForSeconds(2);
        UIManager.Instance.routeMapPanel.mission.TaskCompletion();
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

    public void ClearGlodal()
    {
        StopAllCoroutines();
        if (type_troops != null)
        {
            for (int i = 0; i < type_troops.Length; i++)
            {
                ObjectPool.Instance.Clear(type_troops[i]);
            }
        }
        for (int i = 0; i < createModel.turretPrefab.Length; i++)
        {
            ObjectPool.Instance.Clear(createModel.turretPrefab[i].name);
            ObjectPool.Instance.Clear("CandyBullet_" + i);
        }
        createModel.turrets.Clear();
        ObjectPool.Instance.Clear(bronPrefab.name);
        ObjectPool.Instance.Clear(cakePrefab.name);
    }
    //提示框
    public void ShowCueCone()
    {
        int index = 0;bool isHide=false;
        for (int i = 0; i < turretPoints.Count; i++)
        {
            if (turretPoints[i].childCount >= 3)
            {
                index++;
            }
        }
        if (createModel.sendCount > index)
        {
            for (int i = 0; i < fittable.childCount; i++)
            {
                if (fittable.GetChild(i).childCount > 2)
                {
                    isHide = true;
                    break;
                }
            }
        }
        for (int j = 0; j < turretPoints.Count; j++)
        {
            if (turretPoints[j].childCount < 3)
            {
                turretCone[j].gameObject.SetActive(isHide);
            }
            else
            {
                turretCone[j].gameObject.SetActive(false);
            }
        }
    }
}
