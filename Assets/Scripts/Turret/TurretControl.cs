using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurretControl : MonoBehaviour
{
    public RACEIMG state;
    public bool isMerge;
    public bool isTrigger;
    public bool isFire;
    public float grade;
    public Transform otherObj;
    public SpriteRenderer attackTips;

    public float distance;
    private float attackRatetime;
    private float nor_damage;
    private int keelIndex;
    private Text levelText;
    private Animator haloSpot;
    private ShooterItem shooterData;
    private GameObject bulletPrefab;
    private Transform firePos;
    private Transform tarack;
   
    private GameObject snakeObject;
    private AudioSource source;
    private DragonBones.UnityArmatureComponent candy;
    private float lastTime;
    private float minDic = 100000;
    private bool isRoude;
    private Vector3 candyAngle;
    //private Color tipColor;
    private List<Transform> enemyAll = new List<Transform>();
    private void Awake()
    {
        bulletPrefab = Resources.Load<GameObject>("Bullet/CandyBullet_" + (int)state);
        haloSpot = transform.Find("Halo").GetComponent<Animator>();
        levelText = transform.Find("Canvas/Text").GetComponent<Text>();
        transform.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
        candy = transform.Find("candy").GetComponent<DragonBones.UnityArmatureComponent>();

        if (!source) source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        //source.volume = 0.2f;
        firePos = transform.Find("FirePos");
        shooterData = ExcelTool.Instance.shooters[(int)state];
        attackRatetime = shooterData.att_interval;
        string[] dis = shooterData.atk_distance.Split('|');
        if (GameManager.Instance.modeSelection == "roude")
        {
            isRoude = true;
            distance = float.Parse(dis[1]);
            candyAngle = new Vector3(55,0,0);
            transform.Find("Canvas").localEulerAngles = candyAngle;
        }
        else
        {
            isRoude = false;
            distance = float.Parse(dis[0]);
            candyAngle = new Vector3(50, 0, 0);
            transform.Find("Canvas").localEulerAngles = candyAngle;
        }
        CreateRingTip();
    }
    public void SetNumerical(float lever)
    {
        isMerge = false;
        SetLevel(lever);
        candy.sortingOrder = -5;
        if (transform.parent.name.Contains("fire"))
        {
            isFire = true;
        }
        else
        {
            isFire = false;
        }
        firePos.gameObject.SetActive(false);
        if (state == RACEIMG.Magnet_11 || state == RACEIMG.Leeway_12 || state == RACEIMG.Sunfire_13
            || state == RACEIMG.Moon_16 || state == RACEIMG.Flakes_18)
        {
            firePos.localEulerAngles = -transform.localEulerAngles;
        }
        CreateKeel();
        attackTips.gameObject.SetActive(false);
        AudioManager.Instance.PlaySource(ExcelTool.Instance.sources[(int)(state + 61)], source,0.3f);
    }
    public void PlayDragon(string messg)
    {
        candy.animation.Reset();
        candy.animation.Play(messg, 1);
        if(messg != "rest" && messg != "walk")
        {
            if(!candy.HasEventListener(DragonBones.EventObject.COMPLETE))
            {
                candy.AddEventListener(DragonBones.EventObject.COMPLETE, HideCandy);
            }
        }
    }
    void HideCandy(string type, DragonBones.EventObject eventObject)
    {
        candy.RemoveEventListener(DragonBones.EventObject.COMPLETE, HideCandy);
        candy.animation.Play("rest");
    }
    public void PlayDeath()
    {
        attackTips.gameObject.SetActive(false);
        transform.GetComponent<Collider>().enabled = false;
        candy.animation.Reset();
        candy.animation.Play("dead", 1);
        transform.DOMoveY(10, 2);
        Invoke("HideDeath",2);
    }
    private void HideDeath()
    {
        transform.GetComponent<Collider>().enabled = true;
        ObjectPool.Instance.CollectObject(gameObject);
    }
    
    private void FixedUpdate()
    {
        if (UIManager.Instance.isTime || CreateModel.Instance.sendTroops) return;
        if (transform.parent == null)
        {
            gameObject.SetActive(false); return;
        }
        enemyAll.Clear();
        for (int i = 0; i < CreateModel.Instance.enemys.Count; i++)
        {
            enemyAll.Add(CreateModel.Instance.enemys[i]);
        }
        for (int i = 0; i < CreateModel.Instance.enemyBullets.Count; i++)
        {
            enemyAll.Add(CreateModel.Instance.enemyBullets[i]);
        }
        if (isFire && enemyAll.Count > 0)
        {
            //distance = shooterData.atk_distance /*+ CreateModel.Instance.enemys[0].GetComponent<Renderer>().bounds.size.x*/;
            for (int i = 0; i < enemyAll.Count; i++)
            {
                if (Vector3.Distance(transform.position, enemyAll[i].position) <= distance)
                {
                    lastTime += Time.deltaTime;
                    if (lastTime >= attackRatetime)
                    {
                        Attack();
                        tarack = enemyAll[i];
                        lastTime = 0;
                    }
                    break;
                }
            }
        }
    }
    private void Attack()
    {
        if (state == RACEIMG.Bane_1)
        {
            for (int i = 0; i < enemyAll.Count; i++)
            {
                if(enemyAll[i].GetComponent<EnemyControl>())
                {
                    if (Vector3.Distance(transform.position, enemyAll[i].position) < minDic
                                        && enemyAll[i].GetComponent<EnemyControl>().baneTime <= 0)
                    {
                        tarack = enemyAll[i];
                        minDic = Vector3.Distance(transform.position, tarack.position);
                    }
                }
                else
                {
                    break;
                }
            }
           
        }
        else if(state == RACEIMG.Ice_2)
        {
            for (int i = 0; i < enemyAll.Count; i++)
            {
                if (enemyAll[i].GetComponent<EnemyControl>())
                {
                    if (Vector3.Distance(transform.position, enemyAll[i].position) < minDic
                   && enemyAll[i].GetComponent<EnemyControl>().JuredSpeed())
                    {
                        tarack = CreateModel.Instance.enemys[i];
                        minDic = Vector3.Distance(transform.position, tarack.position);
                    }
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < enemyAll.Count; i++)
            {
                if (Vector3.Distance(transform.position, enemyAll[i].position) < minDic)
                {
                    tarack = enemyAll[i];
                    minDic = Vector3.Distance(transform.position, tarack.position);
                }
            }
        }
        AddDamage();
        if (!source.isPlaying)
        {
            AudioManager.Instance.PlaySource(ExcelTool.Instance.sources[(int)(state + 40)], source, 0.3f);
        }
        firePos.gameObject.SetActive(true);
        if (state== RACEIMG.Laser_4)
        {
            PlayDragon("attack_"+ Random.Range(1,3));
            Vector3 targetPos = tarack.position;
            targetPos.y = targetPos.y + 0.5f;
            firePos.LookAt(targetPos);
            firePos.GetComponent<BulletType>().SetTarget(state, shooterData,nor_damage, distance);
        }
        else if(state == RACEIMG.Magnet_11 || state == RACEIMG.Leeway_12 || state == RACEIMG.Sunfire_13
             || state == RACEIMG.Moon_16 || state == RACEIMG.rod_17 || state == RACEIMG.Flakes_18 || state == RACEIMG.Gasoline_19)
        {
            firePos.GetComponent<BulletType>().SetTarget(state, shooterData, nor_damage, distance);
        }
        else
        {
            if(tarack != null && tarack.gameObject.activeInHierarchy)
            {
                PlayDragon("attack_" + Random.Range(1, 3));
                haloSpot.gameObject.SetActive(true);
                haloSpot.Play("halo",0,0);
                StartCoroutine(HideHalo());
                Vector3 targetPos = tarack.position;
                targetPos.y = targetPos.y + 0.5f;
                firePos.LookAt(targetPos);
                for (int i = 0; i < firePos.childCount; i++)
                {
                    var bullet = ObjectPool.Instance.CreateObject(bulletPrefab.name, bulletPrefab);
                    bullet.transform.localPosition = firePos.GetChild(i).position;
                    bullet.transform.localRotation = firePos.GetChild(i).rotation;
                    if (state == RACEIMG.Electric_8)
                    {
                        bullet.GetComponent<Electric_Bullet>().OpenAnimal(state, shooterData, nor_damage,distance);
                    }
                    else
                    {
                        bullet.GetComponent<Bullet>().SetTarget(state, targetPos, shooterData, nor_damage);
                    }    
                }
            }
        }
        minDic = 100000;
    }
    IEnumerator HideHalo()
    {
        yield return new WaitForSeconds(0.3f);
        haloSpot.gameObject.SetActive(false);
    }
    //每排角度计算
    public void TurretLaunch(Transform parent)
    {
        this.isFire = parent.name.Contains("fire");
        transform.SetParent(parent);
        transform.localPosition = CreateModel.Instance.turretPoint;
        if (isFire)
        {
            CreateModel.Instance.HideTipCone(parent.GetSiblingIndex());
        }
        else
        {
            CreateModel.Instance.ShowTipCone(parent.GetSiblingIndex());
        }
        //if (isRoude && isFire)
        //{
        //    //Vector3 angle = Vector3.zero;
        //    //int array = Mathf.CeilToInt(int.Parse(transform.parent.name.Substring(5)) / 7);
        //    //angle.x = 10 - array * 2;
        //    //transform.localEulerAngles = angle;
        //    //attackTips.transform.localEulerAngles = Vector3.right * 90 - angle;
        //}
    }
    //合并升级
    public void UpGrade()
    {
        if(transform.parent)
        {
            this.grade++;
            levelText.text = grade.ToString();
            TurretDrag.Instance.Meage(transform.position);
            PlayDragon("born");
            if (PlayerPrefs.GetFloat("FitLevel" + state) < grade)
            {
                PlayerPrefs.SetFloat("FitLevel" + state, grade);
            }
        }
    }
    //设置等级
    public void SetLevel(float level)
    {
        grade = level;
        levelText.text = grade.ToString();
    }
    //判断是合并还是升级
    public void JudeGrade(Transform parent)
    {
        TurretControl turret;
        turret = otherObj.GetChild(2).GetComponent<TurretControl>();
        if (turret)
        {
            transform.SetParent(otherObj, true);
            transform.localPosition = CreateModel.Instance.turretPoint;
            if (turret.grade == this.grade && turret.state==state)
            {
                TurretDrag.Instance.Composite(otherObj.position,(int)state);
                UpGrade();
                ObjectPool.Instance.CollectObject(turret.gameObject);
                turret.transform.SetParent(null);
                CreateModel.Instance.turrets.Remove(turret.transform);
                CreateModel.Instance.ShowTipCone(parent.GetSiblingIndex());
            }
            else
            {
                turret.transform.SetParent(parent, true);
                turret.transform.localPosition = CreateModel.Instance.turretPoint;
                turret.isFire = parent.name.Contains("fire");
            }
        }
    }
    //candy_101_1 龙骨动画
    public void CreateKeel()
    {
        if (keelIndex == 3 && keelIndex != (int)shooterData.starLevel) return;
        keelIndex = (int)shooterData.starLevel;
        if (keelIndex > 3)
        {
            keelIndex = 3;
        }
        if (keelIndex <= 0)
        {
            keelIndex = 1;
        }
        string keelName = string.Format("candy_{0}_{1}", ((int)state + 1), keelIndex);
        candy = UIManager.Instance.SetArmature(candy, transform, keelName, Vector3.one * 0.8f, Vector3.zero, true, "rest",false);
        candy.transform.localEulerAngles = candyAngle;
        candy.sortingMode = DragonBones.SortingMode.SortByOrder;
        candy.sortingOrder = -5;
    }
    //战力蛇
    public void CreateSnake(GameObject game)
    {
        if(snakeObject)
        {
            snakeObject.SetActive(true);
        }
        else
        {
            var snake = ObjectPool.Instance.CreateObject("snakeCombat", game);
            snake.transform.SetParent(transform);
            snake.transform.localPosition = new Vector3(0,-0.5f,0);
            snake.transform.localScale = Vector3.one * 45;
            snakeObject = snake;
        }
    }
    public void HideSnake()
    {
        if (snakeObject)
        {
            snakeObject.SetActive(false);
            snakeObject = null;
        }
    }
    //计算伤害
    private void AddDamage()
    {
        if(isRoude)
        {
            nor_damage = Mathf.Round(shooterData.atk_attach + (grade - 1) * shooterData.atk_growth_attach) * 2;
        }
        else
        {
            nor_damage = Mathf.Round(shooterData.atk_attach + (grade - 1) * shooterData.atk_growth_attach);
        }
        nor_damage = nor_damage + UIManager.Instance.turretPanel.attackGrade + UIManager.Instance.atkDouble;
        if (CreateModel.Instance.isSnake)
            nor_damage = nor_damage+CreateModel.Instance.snakeHurt * nor_damage;
        nor_damage += Mathf.Round(shooterData.atk_type + (shooterData.starLevel) * shooterData.atk_growth_type);
    }
    //点击
    public void HideFire()
    {
        if (isFire)
        {
            CreateModel.Instance.HideUnlock();
        }
        isTrigger = false;
        isFire = false;
        isMerge = false;
        if(isRoude)
        {
            attackTips.DOPause();
            attackTips.DOFade(0.7f, 0.5f);
            attackTips.gameObject.SetActive(true);
        }
        firePos.gameObject.SetActive(false);
    }
    //攻击范围
    private void CreateRingTip()
    {
        var go = Instantiate(Resources.Load<Transform>("Effects/atk_tips"));
        go.SetParent(transform);
        go.localPosition = Vector3.zero;
        go.localScale = Vector3.one * distance * 1.8f;
        go.localEulerAngles = Vector3.right * 90 - CreateModel.Instance.turretAngle;
        attackTips = go.GetComponent<SpriteRenderer>();
        //tipColor = attackTips.color;
        //tipColor.a = 0.7f;
    }
    public void OpenRingTip()
    {
        attackTips.DOFade(0, 2);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Side") && !isTrigger)
        {
            isTrigger = true;
            otherObj = other.transform.parent;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Side") && isTrigger)
        {
            isTrigger = false;
        }
    }
}
