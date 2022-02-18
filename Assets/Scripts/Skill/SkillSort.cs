using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 技能释放按钮
/// </summary>
public class SkillSort : MonoBehaviour
{
    public Sprite spriteUnlock;
    public Sprite spriteNot;
    private Transform candy;
    private Transform levelSet;
    private Transform lockSet;
    private Transform candySet;
    private Transform armatureParent;

    private Text nameText;
    private Text candyText;
    private Text numText;
    private Text leveltext;
    private Text timingText;
    private Image shadeImage;
    private Image sprite;
    private Button skillBtn;

    private GameObject skillPrefab;
    private GameObject trashTip;
    private GameObject mask_Object;
    private DragonBones.UnityArmatureComponent armature;
    private DragonBones.UnityArmatureComponent model_Armature;
    private SkillItem skillItem;
    private LaserCandy laserCandy;
    private AudioSource source_candy;

    private int ID;
    private int tipCount;
    private float lastTime = -1;
    private float skill_num;
    private float att_interval;
    private float max_z;
    private float min_z;
    public float skill_hurt;
    public float candy_num;

    private bool isUnlock;
    private bool isRoude;

    private Vector3 candyPoint;
    private string keelName;
    public void InitData(SkillItem item,Transform candy,int ID)
    {
        skillItem = item;
        this.ID = ID;
        this.candy = candy;
        levelSet = transform.Find("Num");
        lockSet = transform.Find("Lock");
        candySet = transform.Find("Candy");
        armatureParent = transform.Find("CandyMask");
        trashTip = transform.Find("Trash").gameObject;
        mask_Object = transform.Find("Mask").gameObject;
        skillPrefab = Resources.Load<GameObject>("Skill/Skill" + ID);
        leveltext = lockSet.Find("Level").GetComponent<Text>();
        numText = levelSet.Find("Text").GetComponent<Text>();
        candyText = candySet.Find("Text").GetComponent<Text>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        timingText = transform.Find("Timing").GetComponent<Text>();
        shadeImage = transform.Find("Shade").GetComponent<Image>();
        model_Armature = armatureParent.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>();
        sprite = transform.GetComponent<Image>();
        skillBtn = GetComponent<Button>();
        source_candy = candy.GetComponent<AudioSource>();
        skillBtn.onClick.AddListener(ClickSkill);
        ExcelTool.LanguageEvent += CutLang;
        string[] interval = skillItem.att_interval.Split('|');
        if (GameManager.Instance.modeSelection == "roude")
        {
            isRoude = true;
            skill_num = 0;
            att_interval = float.Parse(interval[1]);
            candy_num = float.Parse(skillItem.cost.Split('|')[1]);
        }
        else
        {
            isRoude = false;
            skill_num = PlayerPrefs.GetFloat("SkillNum" + ID);
            att_interval = float.Parse(interval[0]);
            candy_num = float.Parse(skillItem.cost.Split('|')[0]);
            lastTime = PlayerPrefs.GetFloat("SkillTime" + ID);
            if (lastTime > 0)
            {
                shadeImage.gameObject.SetActive(true);
                timingText.gameObject.SetActive(true);
            }
            else
            {
                shadeImage.gameObject.SetActive(false);
            }
        }
        candyText.text = candy_num.ToString();
        FindArmature();
    }
    private void FindArmature()
    {
        if (isRoude)
        {
            min_z = 37; max_z = 55;
            if (ID == 2 || ID == 3 || ID == 4 || ID == 9 || ID == 12 || ID == 14 || ID == 18)
            {
                candyPoint = new Vector3(0, 5, 30);
            }
            else
            {
                candyPoint = new Vector3(0, 30, 37);
            }
            candy.transform.localEulerAngles = new Vector3(65,0,0);
        }
        else
        {
            min_z = 0; max_z = 90;
            if (ID == 2 || ID == 3 || ID == 4 || ID == 9 || ID == 12 || ID == 14 || ID == 18)
            {
                candyPoint = new Vector3(0, 5, -15);
            }
            else if (ID == 5)
            {
                candyPoint = new Vector3(0, 5, -1);
            }
            else
            {
                candyPoint = new Vector3(0, 30, 0);
            }
        }
        candy.transform.localPosition = candyPoint;
        if (ID == 2 || ID == 4)
        {
            laserCandy = candy.Find("box").GetComponent<LaserCandy>();
        }
        armature = candy.GetComponent<DragonBones.UnityArmatureComponent>();
        candy.gameObject.SetActive(false);
    }
    private void PlayDragon(string messg)
    {
        armature.animation.Reset();
        armature.animation.Play(messg);
        //if (!armature.HasEventListener(DragonBones.EventObject.COMPLETE))
        //{
        //    armature.AddEventListener(DragonBones.EventObject.COMPLETE, HideCandy);
        //}
    }
    private void HideCandy(string type, DragonBones.EventObject eventObject)
    {
        armature.animation.Play("rest");
    }
    private void CutLang()
    {
        nameText.text = skillItem.realname;//ExcelTool.lang["skillrealname" + ID];
    }
    //candy_101_1 龙骨动画
    public void SetKeel(string keelName)
    {
        model_Armature = UIManager.Instance.SetArmature(model_Armature, armatureParent, keelName, Vector3.one * 20, Vector3.zero);
    }
    //冷却
    public void Cooling(bool isColi)
    {
        OpenAnimal(false);
        if(skill_num > 0 || isUnlock)
        {
            if (isColi)
            {
                sprite.sprite = spriteNot;
            }
            else
            {
                sprite.sprite = spriteUnlock;
                if (isRoude)
                {
                    IsJude(UIManager.Instance.routeGold);
                }
                else
                {
                    IsJude(UIManager.Instance.goldNumber);
                }
            }
        }
    }
    //未解锁
    public void NotUnlock()
    {
        isUnlock = false;
        if (skill_num > 0)
        {
            DemoSkill(0);
        }
        else
        {
            sprite.sprite = spriteNot;
            mask_Object.SetActive(true);
            leveltext.text = skillItem.unlock_level.ToString();
            timingText.gameObject.SetActive(false);
            candySet.gameObject.SetActive(false);
            levelSet.gameObject.SetActive(false);
            lockSet.gameObject.SetActive(true);
        }
    }
    //解锁
    public void UnlockSkill()
    {
        isUnlock = true;
        if (skill_num > 0)
        {
            DemoSkill(0);
        }
        else
        {
            candySet.gameObject.SetActive(true);
            levelSet.gameObject.SetActive(false);
            lockSet.gameObject.SetActive(false);
            if (isRoude)
            {
                IsJude(UIManager.Instance.routeGold);
            }
            else
            {
                IsJude(UIManager.Instance.goldNumber);
            }
        }
    }

    public void SkillCarry(bool isCarry)
    {
        lastTime = 0;
        if (isCarry)
        {
            isUnlock = true;
            DemoSkill(1);
            gameObject.SetActive(true);
        }
        else
        {
            skill_num = 0;
            isUnlock = false;
            sprite.sprite = spriteNot;
            mask_Object.SetActive(true);
            leveltext.text = skillItem.unlock_level.ToString();
            candySet.gameObject.SetActive(false);
            levelSet.gameObject.SetActive(false);
            lockSet.gameObject.SetActive(true);
            trashTip.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    //奖励次数
    public void DemoSkill(float num)
    { 
        skill_num += num;
        sprite.sprite = spriteUnlock;
        mask_Object.SetActive(false);
        numText.text = skill_num.ToString();
        candySet.gameObject.SetActive(false);
        levelSet.gameObject.SetActive(true);
        lockSet.gameObject.SetActive(false);
        trashTip.gameObject.SetActive(false);
        if (!isRoude)
        {
            PlayerPrefs.SetFloat("SkillNum" + ID, skill_num);
        }
    }
    //判断糖果是否足够
    public void IsJude(float candy)
    {
        if(isUnlock && skill_num <= 0)
        {
            if(candy >= candy_num)
            {
                sprite.sprite = spriteUnlock;
                mask_Object.SetActive(false);
                if (lastTime <= 0)
                {
                    trashTip.gameObject.SetActive(true);
                }
            }
            else
            {
                mask_Object.SetActive(true);
                sprite.sprite = spriteNot;
                trashTip.gameObject.SetActive(false);
            }
        }
    }
    public void HideTip()
    {
        if(isUnlock)
        {
            trashTip.gameObject.SetActive(false);
        }
    }
    //提示动画
    public bool JudeTip()
    {
        return trashTip.activeInHierarchy || levelSet.gameObject.activeInHierarchy;
    }
    public void OpenAnimal(bool isOpen)
    {
        if(isOpen)
        {
            tipCount = 1;
            if(trashTip.activeInHierarchy)
            {
                StartCoroutine(PromptAnimal());
            }
            else if (levelSet.gameObject.activeInHierarchy)
            {
                StartCoroutine(NumAnimal());
            }
        }
        else
        {
            tipCount = 5;
            StopCoroutine("NumAnimal");
            StopCoroutine("PromptAnimal");
            trashTip.transform.localScale = Vector3.one;
            levelSet.transform.localScale = Vector3.one;
        }
    }
    private IEnumerator PromptAnimal()
    {
        if(tipCount < 5)
        {
            trashTip.transform.DOScale(Vector3.one * 1.5f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            trashTip.transform.DOScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(0.5f);
            tipCount++;
            StartCoroutine(PromptAnimal());
        }
    }
    private IEnumerator NumAnimal()
    {
        if (tipCount < 5)
        {
            levelSet.transform.DOScale(Vector3.one * 1.5f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            levelSet.transform.DOScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(0.5f);
            tipCount++;
            StartCoroutine(NumAnimal());
        }
    }
    private void ClickSkill()
    {
        AudioManager.Instance.PlayTouch("other_1");
        if (skill_num <= 0 && !isUnlock)
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip15"]);
            return;
        }
        if(lastTime >= 0 || UIManager.Instance.skillTime > 0)
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip12"]);
        }
        else
        {
            if(skill_num > 0)
            {
                skill_num-=1;
                numText.text = skill_num.ToString();
                PlayerPrefs.SetFloat("SkillNum" + ID, skill_num);
                if (skill_num <= 0)
                {
                    if (isUnlock)
                        UnlockSkill();
                    else
                        NotUnlock();
                }
                FreedSkill();
                return;
            }
            bool isAmple = false;
            if(isRoude)
            {
                isAmple = UIManager.Instance.routeGold >= candy_num;
            }
            else
            {
                isAmple = UIManager.Instance.goldNumber >= candy_num;
            }
            if(isAmple)
            {
                UIManager.Instance.SetGold(-candy_num);
                FreedSkill();
            }
            else
            {
                GameManager.Instance.CloneTip(ExcelTool.lang["tip11"]);
            }
        }
    }

    private void FreedSkill()
    {
        PlayDragon("born");
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<SkillSort>().HideTip();
        }
        if (!isRoude)
        {
            if (ID < 10 && PlayerPrefs.GetString("FirstSkill" + ID) != "")
            {
                if (PlayerPrefs.GetString("FirstSkill" + (ID + 1)) == "")
                {
                    transform.parent.GetChild(ID).GetComponent<SkillSort>().DemoSkill(1);
                    PlayerPrefs.SetString("FirstSkill" + (ID + 1), (ID + 1).ToString());
                }
            }
        }
        lastTime = att_interval;
        shadeImage.gameObject.SetActive(true);
        timingText.text = lastTime.ToString();
        timingText.gameObject.SetActive(true);
        skill_hurt += UIManager.Instance.atkDouble;
        UIManager.Instance.SetSkill();
        UIManager.Instance.skillPanel.TailingEffect(candy,ID-1);
        AudioManager.Instance.PlaySource("warlock_" + ID, source_candy);
        switch (ID)
        {
            case 1:
                StartCoroutine(EggHide());
                break;
            case 2:
            case 4:
                StartCoroutine(LaserHide());
                break;
            case 3:
                StartCoroutine(WaterHide());
                break;
            case 5:
                StartCoroutine(StoneHide());
                break;
            case 6:
                StartCoroutine(HideMagma());
                break;
            case 7:
                StartCoroutine(HideQuake());
                break;
            case 8:
                StartCoroutine(HideQuantum());
                break;
            case 9:
                StartCoroutine(HideBall());
                break;
            case 10:
                StartCoroutine(HideMoon());
                break;
            case 11:
                StartCoroutine(HideRedWin());
                break;
            case 12:
                StartCoroutine(HideIceWord()); 
                break;
            case 13:
                StartCoroutine(HideSeafire());
                break;
            case 14:
                StartCoroutine(HideLandslide());
                break;
            case 15:
                StartCoroutine(HideGoldleaf());
                break;
            case 16:
                StartCoroutine(HideSnake());
                break;
            case 17:
                StartCoroutine(HideCookie());
                break;
            case 18:
                StartCoroutine(HideRolling());
                break;
            case 19:
                StartCoroutine(HideLittle());
                break;
            case 20:
                StartCoroutine(HideSnowman());
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (UIManager.Instance.isTime) return;
        if(lastTime >= 0)
        {
            lastTime-=Time.deltaTime;
            shadeImage.fillAmount = lastTime / att_interval;
            if(!isRoude)
            {
                PlayerPrefs.SetFloat("SkillTime" + ID, lastTime);
            }
            timingText.text = lastTime.ToString("F1");
            if (lastTime <= 0)
            {
                shadeImage.gameObject.SetActive(false);
                timingText.gameObject.SetActive(false);
                if (UIManager.Instance.skillTime <= 0)
                {
                    if (isUnlock && skill_num <= 0)
                    {
                        if (isRoude)
                        {
                            trashTip.gameObject.SetActive(UIManager.Instance.routeGold >= candy_num);
                        }
                        else
                        {
                            trashTip.gameObject.SetActive(UIManager.Instance.goldNumber >= candy_num);
                        }
                    }
                }
            }
        }
    }
    //鸡蛋技能 1
    IEnumerator EggHide()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(0, 1f);
        yield return new WaitForSeconds(1);
        PlayDragon("attack_1");
        candy.DOLocalMoveY(5, 0.5f);
        for (int i = 0; i < skillItem.num; i++)
        {
            var egg = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
            egg.gameObject.SetActive(true);
            egg.transform.SetParent(candy.parent);
            if (i < CreateModel.Instance.enemys.Count)
            {
                Vector3 point = CreateModel.Instance.enemys[i].position;
                point.y = 30;
                egg.transform.localPosition = point;
                
            }
            else
            {
                egg.transform.localPosition = new Vector3(Random.Range(-4, 4), 30, Random.Range(min_z, max_z));
            }
            egg.GetComponent<SkillEgg>().SetInit(i * 0.1f, skillItem, skill_hurt);
        }
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
    //激光技能2  雪崩 4
    IEnumerator LaserHide()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveZ(min_z, 0.5f);
        yield return new WaitForSeconds(1);
        candy.DOLocalMoveZ(90, 3f);
        PlayDragon("walk");
        laserCandy.Init(skillItem,skillPrefab, skill_hurt);
        yield return new WaitForSeconds(3);
        laserCandy.PlayAnim();
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        PlayDragon("disappear");
        candy.localPosition = candyPoint;
        candy.gameObject.SetActive(false);
    }
    //水世界
    IEnumerator WaterHide()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveZ(min_z, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        candy.DOScale(Vector3.one * 1.2f,0.5f);
        var water = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        water.gameObject.SetActive(true);
        water.transform.SetParent(candy.parent);
        water.GetComponent<SkillWater>().SetInit(skillItem,skill_hurt);
        if (isRoude)
        {
            water.transform.localPosition = Vector3.forward * 33;
        }
        else
        {
            water.transform.localPosition = Vector3.zero;
        }
        yield return new WaitForSeconds(1);
        candy.DOScale(Vector3.one, 1f);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
        candy.localPosition = candyPoint;
    }
    //石壁
    IEnumerator StoneHide()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        candy.DOLocalMoveY(7.5f, 0.5f);
        var stone = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        stone.gameObject.SetActive(true);
        stone.transform.SetParent(candy.parent);
        if(isRoude)
        {
            stone.transform.localPosition = Vector3.forward * 35;
        }
        else
        {
            stone.transform.localPosition = Vector3.zero;
        }
        stone.GetComponent<SkillStone>().SetInit(skillItem, skill_hurt);
        yield return new WaitForSeconds(1.5f);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1f);
        candy.gameObject.SetActive(false);
        candy.localPosition = candyPoint;
    }
    //岩浆
    IEnumerator HideMagma()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(0, 1f);
        yield return new WaitForSeconds(1);
        candy.DOLocalMoveY(5, 1f);
        PlayDragon("attack_1");
        var Magma = Instantiate(skillPrefab);//ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        Magma.gameObject.SetActive(true);
        Magma.transform.SetParent(candy.parent);
        if (isRoude)
        {
            Magma.transform.localPosition = Vector3.forward * 33;
        }
        else
        {
            Magma.transform.localPosition = Vector3.zero;
        }
        Magma.GetComponent<SkillMagma>().SetInit(skillItem,skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 2f);
        yield return new WaitForSeconds(2);
        candy.gameObject.SetActive(false);
    }
    //地震
    IEnumerator HideQuake()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(0, 1f);
        yield return new WaitForSeconds(1);
        PlayDragon("attack_1");
        candy.DOLocalMoveY(5, 1f);
        for (int i = 1; i <= skillItem.num; i++)
        {
            var quake = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
            quake.gameObject.SetActive(true);
            quake.transform.SetParent(candy.parent);
            if(isRoude)
            {
                quake.transform.localPosition = new Vector3(0, -12, 132 - i * 9);
            }
            else
            {
                quake.transform.localPosition = new Vector3(0, -12, 98 - i * 9);
            }
            quake.GetComponent<SkillQuake>().SetInit(skillItem,skill_hurt,i*0.1f, skillItem.num-i);
        }
        yield return new WaitForSeconds(1.5f);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
    //电磁炮
    IEnumerator HideQuantum()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(0, 0.2f);
        yield return new WaitForSeconds(0.2f);
        PlayDragon("attack_1");
        candy.DOLocalMoveY(6, 0.5f);
        yield return new WaitForSeconds(0.5f);
        var quantum = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        quantum.gameObject.SetActive(true);
        quantum.transform.SetParent(candy.parent);
        if (isRoude)
        {
            quantum.transform.localPosition = Vector3.forward * 45;
        }
        else
        {
            quantum.transform.localPosition = Vector3.zero;
        }
        quantum.GetComponent<SkillQuantum>().SetInit(skillItem,skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(0.5f);
        candy.gameObject.SetActive(false);
    }
    //铁球滚动
    IEnumerator HideBall()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveZ(min_z, 0.5f);
        PlayDragon("attack_1");
        var ball = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        ball.gameObject.SetActive(true);
        ball.transform.SetParent(candy.parent);
        if (isRoude)
        {
            ball.transform.localPosition = Vector3.forward * 36;
        }
        else
        {
            ball.transform.localPosition = Vector3.zero;
        }
        ball.GetComponent<SkillIronBall>().SetInit(skillItem, skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.localPosition = candyPoint;
        candy.gameObject.SetActive(false);
    }
    //月球坠落
    IEnumerator HideMoon()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        candy.DOScale(Vector3.one*1.2f,0.2f);
        var Moon = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        Moon.gameObject.SetActive(true);
        Moon.transform.SetParent(candy.parent);
        if (isRoude)
        {
            Moon.transform.localPosition = Vector3.forward * 30;
        }
        else
        {
            Moon.transform.localPosition = Vector3.zero;
        }
        Moon.GetComponent<SkillMoonFalling>().SetInit(skillItem, skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.localScale = Vector3.one;
        candy.gameObject.SetActive(false);
    }
    //红色龙卷风
    IEnumerator HideRedWin()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        candy.DOScale(Vector3.one * 1.2f, 0.2f);
        var win = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        win.gameObject.SetActive(true);
        win.transform.SetParent(candy.parent);
        if (isRoude)
        {
            win.transform.localPosition = Vector3.forward * 25;
        }
        else
        {
            win.transform.localPosition = Vector3.forward * -10;
        }
        win.GetComponent<SkillRedwind>().SetInit(skillItem, skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.localScale = Vector3.one;
        candy.gameObject.SetActive(false);
    }
    //冰界
    IEnumerator HideIceWord()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveZ(min_z, 0.3f);
        yield return new WaitForSeconds(0.3f);
        PlayDragon("attack_1");
        yield return new WaitForSeconds(0.2f);
        candy.DOLocalMoveZ(90, 3f);
        var win = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        win.gameObject.SetActive(true);
        win.transform.SetParent(candy.parent);
        if (isRoude)
        {
            win.transform.localPosition = Vector3.forward * 35;
        }
        else
        {
            win.transform.localPosition = Vector3.zero;
        }
        win.GetComponent<SkillIceWord>().SetInit(skillItem, skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.localScale = Vector3.one;
        candy.localPosition = candyPoint;
        candy.gameObject.SetActive(false);
    }
    //熔岩火海
    IEnumerator HideSeafire()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        var win = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        win.gameObject.SetActive(true);
        win.transform.SetParent(candy.parent);
        if (isRoude)
        {
            win.transform.localPosition = Vector3.forward * 34;
        }
        else
        {
            win.transform.localPosition = Vector3.zero;
        }
        win.GetComponent<SkillSeafire>().SetInit(skillItem, skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
    //山崩
    IEnumerator HideLandslide()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveZ(min_z, 0.3f);
        yield return new WaitForSeconds(0.3f);
        PlayDragon("attack_1");
        yield return new WaitForSeconds(0.2f);
        candy.DOLocalMoveZ(90, 3f);
        candy.DOScale(Vector3.one * 1.2f, 0.2f);
        for (int i = 0; i < 5; i++)
        {
            var win = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
            win.gameObject.SetActive(true);
            win.transform.SetParent(candy.parent);
            win.GetComponent<SkillLandslide>().SetInit(skillItem, skill_hurt,i);
        }
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.localPosition = candyPoint;
        candy.localScale = Vector3.one;
        candy.gameObject.SetActive(false);
    }
    //金叶子
    IEnumerator HideGoldleaf()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        for (int i = 0; i < skillItem.num; i++)
        {
            var egg = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
            egg.gameObject.SetActive(true);
            egg.transform.SetParent(candy.parent);
            if (i < CreateModel.Instance.enemys.Count)
            {
                egg.transform.localPosition = CreateModel.Instance.enemys[i].position;
            }
            else
            {
                egg.transform.localPosition = new Vector3(Random.Range(-4, 4), 0, Random.Range(min_z, max_z));
            }
            egg.GetComponent<SkillGoldleaf>().SetInit(skillItem, skill_hurt,i * 0.2f);
        }
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
    //战力蛇
    IEnumerator HideSnake()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        var egg = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        egg.gameObject.SetActive(true);
        egg.transform.SetParent(candy.parent);
        egg.transform.localPosition = Vector3.forward * 45;
        egg.GetComponent<SkillSnake>().SetInit(skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
    //饼干人
    IEnumerator HideCookie()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        var egg = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        egg.transform.SetParent(candy.parent);
        egg.transform.localPosition = Vector3.forward * 45;
        egg.GetComponent<SkillCookie>().SetInit(skillItem, skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
    //六边形滚动糖果
    IEnumerator HideRolling()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveZ(min_z, 0.3f);
        yield return new WaitForSeconds(0.3f);
        PlayDragon("attack_1");
        yield return new WaitForSeconds(0.2f);
        candy.DOLocalMoveZ(90, 3f);
        for (int i = 0; i < 3; i++)
        {
            var egg = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
            egg.gameObject.SetActive(true);
            egg.transform.SetParent(candy.parent);
            //egg.transform.localScale = new Vector3(100,500,150);
           // egg.transform.localEulerAngles = new Vector3(90,90,0);
            egg.transform.localPosition = new Vector3(0, 35, min_z);
            egg.GetComponent<SkillRolling>().SetInit(skillItem, skill_hurt, i);
        }
        yield return new WaitForSeconds(3);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
    //诅咒小熊
    IEnumerator HideLittle()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        var egg = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        egg.gameObject.SetActive(true);
        egg.transform.SetParent(candy.parent);
        egg.transform.localScale = Vector3.one;
        egg.transform.localPosition = Vector3.forward * min_z;
        egg.GetComponent<SkillLittle>().SetInit(skillItem, skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
    //雪人
    IEnumerator HideSnowman()
    {
        candy.gameObject.SetActive(true);
        candy.DOLocalMoveY(5, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PlayDragon("attack_1");
        var egg = Instantiate(skillPrefab); //ObjectPool.Instance.CreateObject(skillPrefab.name, skillPrefab);
        egg.gameObject.SetActive(true);
        egg.transform.SetParent(candy.parent);
        egg.transform.localScale = Vector3.one;
        egg.transform.localPosition = Vector3.forward * 36;
        egg.GetComponent<SkillSnowman>().SetInit(skill_hurt);
        yield return new WaitForSeconds(2);
        PlayDragon("disappear");
        candy.DOLocalMoveY(40, 1f);
        yield return new WaitForSeconds(1);
        candy.gameObject.SetActive(false);
    }
}
