using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyControl : MonoBehaviour
{
    public Transform sizeScale;
    public EnemyItem enemyData;
    public float baneTime;
    public float iceTime;
    public float maxHp;
    public float speed;
    public float min_attack;
    public bool isBoos;
    public int enemyID;

    private float max_attack;
    private float maxSpeed;
    float Hp;
    float def;
    float maxDef;
    float def_spe;
    float laserTime;
    float bane_dam;
    float bane_Efftime;
    float bane_Damtime;
    float fireTime;
    float eyeTime;
    float jump_inve;
    float jumpTime;
    float skillTime;
    float sou_Damage;
    float jumpSpeed;
    float maxJumpSpeed;
    float shoot_Time;
    float shoot_interval;
    float shoot_distance;
    float shoot_speed;
    float shoot_Hight;
    float shoot_buffTime;
    float attak_time;
    bool isSource;

    Text hpText;
    Color baneColor;
    Color bulletColor;
    Rigidbody body;
    Collider boxCollider;

    BuffItem buffData;
    SkillData skilldata;
    AnimalAnimation animalAnim;
    AudioSource source;

    private Transform enemyType;
    private GameObject crown_1;
    private GameObject crown_2;
    private GameObject eyeEff;
    private GameObject slowObject;
    private GameObject purpleFire;
    private GameObject dizzObject;
    private GameObject shootObject;
    private GameObject sttackObject;
    private GameObject poisoEffect;
    private GameObject enemyBullet;
    private GameObject cloudPrefab;


    public List<Transform> roudePoint=new List<Transform>();
    private int roudeIndex;
    private bool isRoude;
    private bool isShoot;
    private Quaternion targetlook;

    private void Awake()
    {
        if (!source) source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        body = GetComponent<Rigidbody>();
        boxCollider = GetComponent<Collider>();
        animalAnim = GetComponent<AnimalAnimation>();
        crown_1 = transform.Find("crown_1").gameObject;
        crown_2 = transform.Find("crown_2").gameObject;
        eyeEff = transform.Find("eye").gameObject;
        hpText = transform.Find("Canvas/TextHp").GetComponent<Text>();
        transform.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
        sizeScale = transform.Find("Body");
        enemyID = int.Parse(transform.name.Substring(2, 2));
        bulletColor = ExcelTool.Instance.animalColor[enemyID - 1];
        if(GameManager.Instance.modeSelection == "roude")
        {
            isRoude = true;
            body.constraints = ~RigidbodyConstraints.FreezePositionY;
        }
    }
    public void SetNumerical(EnemyItem data,float lever,float hp,float rounds,bool isboos=false,List<Transform> roudes = null)
    {
        enemyData = data;
        Hp = hp;
        maxHp = Hp;
        if (isRoude)
        {
            roudeIndex = 0;
            roudePoint = roudes;
            speed = float.Parse(data.speed.Split('|')[1]) * CreateModel.Instance.enemySpeed;
            maxJumpSpeed = float.Parse(data.jump_speed.Split('|')[1]) * CreateModel.Instance.enemySpeed;
            isBoos = int.Parse(data.id) > 2000;
            CreateCloud();
        }
        else
        {
            isBoos = isboos;
            maxJumpSpeed = (float.Parse(data.jump_speed.Split('|')[0]) + (rounds - 1) * 5) * 0.02f;
            speed = float.Parse(data.speed.Split('|')[0]) + (rounds - 1) * 20;
            speed = Mathf.Clamp(speed, 1, data.speed_add_max);
            speed = speed * 0.02f;
        }
        jumpSpeed = maxJumpSpeed;
        sou_Damage = (enemyData.soul + (lever - 1) * 0.1f) * 3;
        max_attack = enemyData.soul * CreateModel.Instance.enemySoul;
        min_attack = max_attack;
        def = Mathf.Round(data.def + data.def * (lever - 1) * (data.def_growth * 0.01f));
        maxDef = def;
        maxSpeed = speed;
        if (data.def_spe != -1)
        {
            def_spe = Mathf.Round(data.def_spe + data.def_spe * (lever - 1) * data.def_spe_growth * 0.01f);
        }
        boxCollider.enabled = true;
        hpText.text = Hp.ToString("f0");
        HideEff();
        ShootingAttributes();
        animalAnim.SetState();
    }
    void HideEff()
    {
        skillTime = 1;
        eyeTime = 0;
        bane_dam = 0;
        bane_Damtime = 0;
        jump_inve = 0;
        iceTime = -1;
        baneTime = -1;
        fireTime = -1;
        laserTime = -1;
        shoot_buffTime = -1;
        attak_time = -1;
        isSource = false;
        eyeEff.SetActive(false);
        if (isBoos)
        {
            if (CreateModel.Instance.level % 30 == 0)
            {
                crown_2.SetActive(true);
            }
            else
            {
                crown_1.SetActive(true);
            }
            jumpTime = float.Parse(enemyData.jump_interval);
        }
        else
        {
            crown_1.SetActive(false);
            crown_2.SetActive(false);
            string[] jump_time = enemyData.jump_interval.Split('|');
            if(jump_time.Length >= 2)
            {
                jumpTime = Random.Range(float.Parse(jump_time[0]), float.Parse(jump_time[1]));
            }
            else
            {
                jumpTime = float.Parse(jump_time[0]);
            }
        }
        GenerateType();
        if (slowObject)
            slowObject.SetActive(false);
        if (purpleFire)
            purpleFire.SetActive(false);
        if (dizzObject)
            dizzObject.SetActive(false);
        if (shootObject)
            shootObject.SetActive(false);
        if (sttackObject)
            sttackObject.SetActive(false);
    }
    void ShootingAttributes()
    {
        isShoot = enemyData.shoot_if == 2;
        if(isRoude && isShoot)
        {
            isShoot = CreateModel.Instance.isBullet;
        }
        if (isShoot)
        {
            string bulletName = "";
            if(enemyData.def_spe_type == -1)
            {
                bulletName = string.Format("Bullet/EnemyBullet_{0}", 0);
            }
            else
            {
                bulletName = string.Format("Bullet/EnemyBullet_{0}", enemyData.def_spe_type);
            }
            enemyBullet = Resources.Load<GameObject>(bulletName);
            string[] interval =  enemyData.shoot_interval.Split('|');
            string[] distance; 
            if (isRoude)
            {
                distance= enemyData.shoot_distance_line.Split('|');
                shoot_speed = float.Parse(enemyData.shoot_speed.Split('|')[1]);
            }
            else
            {
                distance = enemyData.shoot_distance.Split('|');
                shoot_speed = float.Parse(enemyData.shoot_speed.Split('|')[0]) * 0.02f;
            }
            shoot_interval = Random.Range(float.Parse(interval[0]), float.Parse(interval[1]));
            shoot_distance = Random.Range(float.Parse(distance[0]), float.Parse(distance[1]));
            shoot_Hight =sizeScale.GetComponent<Renderer>().bounds.size.y;
            shoot_Time = shoot_interval;
        }
    }

    void Update()
    {
        if (UIManager.Instance.isTime || CreateModel.Instance.sendTroops) return;
        if(skillTime <= 1)
            skillTime += Time.deltaTime;
        if (gameObject.activeInHierarchy && boxCollider.enabled)
        {
            FireBullet();
            AnimatorMove();
            if (iceTime >= 0)
            {
                iceTime -= Time.deltaTime;
                if (iceTime <= 0)
                {
                    if (slowObject)
                    {
                        slowObject.SetActive(false);
                    }
                    if(laserTime <= 0)
                    {
                        speed = maxSpeed;
                        jumpSpeed = maxJumpSpeed;
                    }
                }
            }
            if (laserTime >= 0)
            {
                laserTime -= Time.deltaTime;
                if (laserTime <= 0)
                {
                    if (dizzObject)
                    {
                        dizzObject.SetActive(false);
                    }
                    if (iceTime <= 0)
                    {
                        speed = maxSpeed;
                        jumpSpeed = maxJumpSpeed;
                    }
                }
                if (dizzObject)
                {
                    if (dizzObject.activeInHierarchy)
                        dizzObject.transform.Rotate(Vector3.up * 30 * Time.deltaTime);
                }
            }
            if (baneTime >= 0)
            {
                bane_Damtime += Time.deltaTime;
                bane_Efftime += Time.deltaTime;
                if (bane_Damtime >= 1)
                {
                    OnHurt(bane_dam,"", baneColor);
                    baneTime -= bane_Damtime;
                    bane_Damtime = 0;
                }
                if (bane_Efftime >= 0.2f)
                {
                    var poiso = ObjectPool.Instance.CreateObject("poisoEffect", poisoEffect);
                    poiso.transform.SetParent(transform.parent);
                    poiso.GetComponent<PoisoEffect>().SetPoint(transform, baneColor);
                    bane_Efftime = 0;
                }
            }
            if (fireTime >= 0)
            {
                fireTime -= Time.deltaTime;
                if (fireTime <= 0)
                {
                    this.def = maxDef;
                    if(purpleFire)
                    {
                        purpleFire.SetActive(false);
                    }
                }
            }
            
            if(shoot_buffTime > 0)
            {
                shoot_buffTime -= Time.deltaTime;
                if(shoot_buffTime <= 0)
                {
                    isShoot = true;
                }
            }

            if(attak_time > 0)
            {
                attak_time -= Time.deltaTime;
                if(attak_time <= 0)
                {
                    min_attack = max_attack;
                } 
            }
        }
    }
    //是否是正常速度
    public bool JuredSpeed()
    {
        return speed == maxSpeed;
    }

    public void InjuredType(string buffID,float type,string param,Color color,float hurt)
    {
        InjuryType(buffID,color,hurt);
        if (type != -1 && type == enemyData.def_spe_type)
        {
            hurt -= def_spe;
        }
        hurt -= def;
        hurt = Mathf.Clamp(hurt, 1, hurt);
        animalAnim.InitStatus(AnimalState.Hit);
        OnHurt(hurt, param, color);
    }
    private void OnHurt(float hurt, string parm, Color color)
    {
        if (Hp >= 0)
        {
            if(!source.isPlaying && CreateModel.Instance.PlaySound())
            {
                AudioManager.Instance.PlaySource(ExcelTool.Instance.sources[enemyID - 1], source);
            }
            if(hurt < Hp)
            {
                UIManager.Instance.WindBlood(transform.position, hurt, color);
            }
            else
            {
                UIManager.Instance.WindBlood(transform.position, Hp, color);
            }
            Hp -= hurt;
            if (Hp <= 0)
            {
                Hp = -10;
                GetComponent<Collider>().enabled = false;
                int ran = Random.Range(1,101);
                if(ran <= enemyData.dropG)
                {
                    UIManager.Instance.ColeGold(transform.position, sou_Damage);
                }
                if(isBoos)
                {
                    TurretDrag.Instance.BoosPixel(transform.position);
                }
                PlayerPrefs.SetFloat("enemyHp" + enemyID, PlayerPrefs.GetFloat("enemyHp" + enemyID) + maxHp);
                CreateModel.Instance.LevelControl(enemyData.soul,isBoos);
                CreateModel.Instance.RemoveEnemy(transform,isBoos);
            }
            else
            {
                if(eyeTime <= 0)
                {
                    eyeTime = 1;
                    StartCoroutine(HideEye());
                }
                hpText.text = Mathf.Ceil(Hp).ToString("f0");
                ScaleJudge(parm);
            }
        }
        if (!isBoos)
        {
            int ran = Random.Range(0, 100);
            if (ran <= 70)
            {
                TurretDrag.Instance.EenemyPixel(gameObject, enemyID, enemyID > 18);
                return;
            }
        }
        TurretDrag.Instance.EenemyPixel(gameObject, enemyID, isBoos);
    }
    private IEnumerator HideEye()
    {
        eyeEff.SetActive(true);
        yield return new WaitForSeconds(1f);
        eyeTime = 0;
        eyeEff.SetActive(false);
    }
    //移动
    private void AnimatorMove()
    {
        if (isRoude)
        {
            Vector3 point = transform.position;
            point.y = 0;
            if (Vector3.Distance(roudePoint[roudePoint.Count - 1].position, point) < 0.2f)
            {
                transform.position = roudePoint[roudePoint.Count - 1].position;
                return;
            }
            if (Vector3.Distance(roudePoint[roudeIndex].position, point) < 0.2f)
            {
                if (roudeIndex != roudePoint.Count - 1)
                {
                    roudeIndex++;
                }
            }
            else
            {
                //transform.forward = roudePoint[roudeIndex].position - point;
                targetlook = Quaternion.LookRotation(roudePoint[roudeIndex].position - point);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetlook, 0.5f); // 角色朝向目标点
                if (transform.localPosition.y <= 0.5f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * speed);
                }
                else
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * jumpSpeed);
                }
            }
        }
        else
        {
            if (transform.position.x < -4.3f || transform.position.x > 4.3f)
            {
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            }
            if (transform.localPosition.y <= 0.5f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime * jumpSpeed);
            }
        }
        if (transform.localPosition.y <= 0.5f)
        {
            jump_inve += Time.deltaTime;
            if (jump_inve >= jumpTime)
            {
                jump_inve = 0;
                body.AddForce(Vector3.up * enemyData.jump_power, ForceMode.Impulse);//向上
            }
        }
    }
    //发射子弹
    private void FireBullet()
    {
        if (isShoot)
        {
            for (int i = 0; i < CreateModel.Instance.cakePoint.Length; i++)
            {
                Vector3 cakePoint = CreateModel.Instance.cakePoint[i];
                if (Vector3.Distance(transform.position, cakePoint) <= shoot_distance)
                {
                    shoot_Time += Time.deltaTime;
                    if (shoot_Time >= shoot_interval)
                    {
                        shoot_Time = 0;
                        var bullet = ObjectPool.Instance.CreateObject(enemyBullet.name, enemyBullet);
                        Vector3 point = transform.position;
                        point.y += shoot_Hight;
                        bullet.transform.localPosition = point;
                        bullet.transform.localRotation = transform.localRotation;
                        bullet.transform.localScale = new Vector3(1.5f, 2f, 1.5f);
                        float hurt = enemyData.shoot_number;
                        if (isRoude)
                        {
                            hurt *= CreateModel.Instance.enemyAttack;
                        }
                        else
                        {
                            cakePoint.x = Random.Range(-4, 4);
                        }
                        bullet.GetComponent<EnemyBullet>().SetInit(cakePoint, shoot_speed, enemyData.shoot_type, hurt, bulletColor);
                        CreateModel.Instance.enemyBullets.Add(bullet.transform);
                        animalAnim.InitStatus(AnimalState.Attack);
                    }
                    break;
                }
            }
        }
    }
    //buff类型
    private void InjuryType(string buffID,Color color, float hurt)
    {
        float odds = 0;
        float ran = 0;
        if (buffID != "-1")
        {
            buffData = ExcelTool.Instance.buffs[buffID];
            if (isBoos)
            {
                odds = buffData.boosOdds;
            }
            else
            {
                odds = buffData.probability;
            }
            ran = Random.Range(1, 101);
            if (ran <= odds)
            {
                switch (buffData.type)
                {
                    case 1:
                    case 12:
                    case 14:
                        Sustained(color, hurt);
                        break;
                    case 4:
                    case 11:
                    case 16:
                        Freezing();
                        break;
                    case 5:
                    case 9:
                        FireDamage();
                        break;
                    case 6:
                        Stun();
                        break;
                    case 7:
                        CantShoot();
                        break;
                    case 8:
                    case 10:
                        AttackSum();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    //持续伤害
    private void Sustained(Color color,float hurt)
    {
        if (!poisoEffect)
        {
            poisoEffect = Resources.Load<GameObject>("Effects/PoisoEffect");
        }
        baneColor = color;
        if(buffData.type == 12 || buffData.type ==14)
        {
            if (buffData.number != "-1")
            {
                if (isBoos)
                {
                    bane_dam = Hp - Hp * float.Parse(buffData.number.Split('|')[1]);
                }
                else
                {
                    bane_dam = Hp - Hp * float.Parse(buffData.number.Split('|')[0]);
                }
            }
        }
        else
        {
            bane_dam = hurt;
        }
        baneTime = buffData.time;
    }
    //冰冻效果
    private void Freezing()
    {
        if(buffData.type == 16)
        {
            speed = 0;
            jumpSpeed = 0;
            iceTime = buffData.time;
        }
        else if(speed > 0)
        {
            float iceNum = 0.5f;
            if (buffData.number != "-1")
            {
                iceNum = float.Parse(buffData.number);
            }
            speed = maxSpeed - maxSpeed * iceNum;
            jumpSpeed = maxJumpSpeed - maxJumpSpeed * iceNum;
            iceTime = buffData.time;
        }
        if (slowObject)
        {
            slowObject.SetActive(true);
        }
        else
        {
            slowObject = Instantiate(Resources.Load<GameObject>("Effects/SlowEffect"));
            slowObject.transform.SetParent(transform, true);
            slowObject.transform.localScale = Vector3.one;
            slowObject.transform.localPosition = Vector3.zero;
            slowObject.SetActive(true);
        }
    }
    //火伤效果
    private void FireDamage()
    {
        if (buffData.type == 9)
        {
            def = 0;
            fireTime = buffData.time;
        }
        else
        {
            float defNume = 0.5f;
            if (buffData.number != "-1")
            {
                defNume = float.Parse(buffData.number);
            }
            def = maxDef - maxDef * defNume;
            fireTime = buffData.time;
        }
        if (purpleFire)
        {
            purpleFire.SetActive(true);
        }
        else
        {
            purpleFire = Instantiate(Resources.Load<GameObject>("Effects/PurpleFire"));
            purpleFire.transform.SetParent(transform, true);
            purpleFire.transform.localScale = Vector3.one;
            purpleFire.transform.localPosition = Vector3.zero;
            purpleFire.SetActive(true);
        }
        purpleFire.transform.localEulerAngles = new Vector3(Random.Range(-20, 25), 180, Random.Range(-10, 30));
    }
    //眩晕效果
    private void Stun()
    {
        speed = 0;
        jumpSpeed = 0;
        laserTime = buffData.time;
        if (dizzObject)
        {
            dizzObject.SetActive(true);
        }
        else
        {
            dizzObject = Instantiate(Resources.Load<GameObject>("Effects/Dizziness"));
            dizzObject.transform.SetParent(transform, true);
            dizzObject.transform.localPosition = Vector3.zero;
            dizzObject.transform.localScale = Vector3.one;
            dizzObject.SetActive(true);
        }
    }
    //减少伤害
    private void AttackSum()
    {
        if(buffData.type == 10)
        {
            if (buffData.number != "-1")
            {
                if(isBoos)
                {
                    min_attack = int.Parse(buffData.number.Split('|')[1]);
                }
                else
                {
                    min_attack = int.Parse(buffData.number.Split('|')[0]);
                }
            }
        }
        else
        {
            float atkNum = 0.5f;
            if (buffData.number != "-1")
            {
                atkNum = float.Parse(buffData.number);
            }
            min_attack = Mathf.CeilToInt(enemyData.soul * atkNum);
        }
        attak_time = buffData.time;
        if (sttackObject)
        {
            sttackObject.SetActive(true);
        }
        else
        {
            sttackObject = Instantiate(Resources.Load<GameObject>("Effects/AttackObject"));
            sttackObject.transform.SetParent(transform, true);
            sttackObject.transform.localPosition = Vector3.zero;
            sttackObject.transform.localScale = Vector3.one;
            sttackObject.SetActive(true);
        }
    }
    //不能射击
    private void CantShoot()
    {
        if (enemyData.shoot_if == 2)
        {
            isShoot = false;
            shoot_buffTime = buffData.time;
            if (shootObject)
            {
                shootObject.SetActive(true);
            }
            else
            {
                shootObject = Instantiate(Resources.Load<GameObject>("Effects/CantShoot"));
                shootObject.transform.SetParent(transform, true);
                shootObject.transform.localPosition = Vector3.zero;
                shootObject.transform.localScale = Vector3.one;
                shootObject.transform.localEulerAngles = Vector3.zero;
                shootObject.SetActive(true);
            }
        }
    }
    //特殊防御标识
    private void GenerateType()
    {
        if (enemyData.def_spe_type == -1) return;
        if(enemyType == null)
        {
            enemyType = Instantiate(Resources.Load<Transform>("Effects/Defensive"));
            enemyType.SetParent(transform, true);
            enemyType.localEulerAngles = Vector3.zero;
            enemyType.localScale = Vector3.one;
        }
        if (isBoos)
        {
            enemyType.localPosition = Vector3.up*0.015f;
        }
        else
        {
            enemyType.localPosition = Vector3.zero;
        }
        if(enemyData.def_spe_type == 1)
        {
            enemyType.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
        }
        else if (enemyData.def_spe_type == 2)
        {
            enemyType.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (enemyData.def_spe_type == 3)
        {
            enemyType.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        }
        else if (enemyData.def_spe_type == 4)
        {
            enemyType.GetChild(0).GetComponent<Renderer>().material.color = Color.magenta;
        }
    }
    //击退效果
    private void ScaleJudge(string parm)
    {
        if (parm == "") return;
        if (isRoude)
        {
            if (transform.localPosition.y < 0.5f)
            {
                body.AddForce(Vector3.up * float.Parse(parm), ForceMode.Impulse);
                animalAnim.InitStatus(AnimalState.Knock);
            }
        }
        else
        {
            string[] msg = parm.Split('-');
            if (isBoos)
            {
                if (msg[0] == "up" && transform.localPosition.y < 5)
                {
                    body.AddForce(Vector3.up * float.Parse(msg[2]), ForceMode.Impulse);//向上
                    animalAnim.InitStatus(AnimalState.Knock);
                }
                else if (msg[0] == "back")
                {
                    body.AddForce(Vector3.forward * float.Parse(msg[2]), ForceMode.Impulse);//向后
                    animalAnim.InitStatus(AnimalState.Repel);
                }
            }
            else
            {
                if (msg[0] == "up" && transform.localPosition.y < 10)
                {
                    body.AddForce(Vector3.up * float.Parse(msg[1]), ForceMode.Impulse);
                    animalAnim.InitStatus(AnimalState.Knock);
                }
                else if (msg[0] == "back")
                {
                    body.AddForce(Vector3.forward * float.Parse(msg[1]), ForceMode.Impulse);
                    animalAnim.InitStatus(AnimalState.Repel);
                }
            }
        }
    }
    //云底
    private void CreateCloud()
    {
        if(transform.localPosition.y > 3)
        {
            if(cloudPrefab)
            {
                cloudPrefab.SetActive(true);
            }
            else
            {
                cloudPrefab = Instantiate(Resources.Load<GameObject>("Effects/CloudBottom"));
                cloudPrefab.SetActive(true);
                cloudPrefab.transform.SetParent(transform);
                cloudPrefab.transform.localScale = Vector3.one;
                cloudPrefab.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            if (cloudPrefab)
            {
                cloudPrefab.SetActive(false);
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Roote") && !isSource)
        {
            isSource = true;
            AudioManager.Instance.PlaySource("down_1", source);
        }
        //else if (other.gameObject.CompareTag("Enemy"))
        //{
        //    EnemyControl tempAnimal = other.gameObject.GetComponent<EnemyControl>();
        //    if (laserTime > 0 && tempAnimal.laserTime <= 0)
        //    {
        //        tempAnimal.laserTime = laserTime;
        //    }
        //    else if (laserTime <= 0 && tempAnimal.laserTime > 0)
        //    {
        //        laserTime = tempAnimal.laserTime;
        //    }
        //    else if (iceTime > 0 && tempAnimal.iceTime <= 0)
        //    {
        //        tempAnimal.speed = speed;
        //        tempAnimal.iceTime = iceTime;
        //    }
        //    else if (iceTime <= 0 && tempAnimal.iceTime > 0)
        //    {
        //        speed = tempAnimal.speed;
        //        iceTime = tempAnimal.iceTime;
        //    }
        //}
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Skill") && skillTime >= 1)
        {
            skilldata = other.GetComponent<SkillData>();
            skilldata.CreateEffeats(transform);
            skillTime = 0;
            if(isRoude)
            {
                InjuredType(skilldata.item.buff_id, skilldata.item.type, skilldata.item.param_line, skilldata.color, skilldata.MaxHurt(isBoos, maxHp));
            }
            else
            {
                InjuredType(skilldata.item.buff_id, skilldata.item.type, skilldata.item.param, skilldata.color, skilldata.MaxHurt(isBoos, maxHp));
            }
        }
    }
}
