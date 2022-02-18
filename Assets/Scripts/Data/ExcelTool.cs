using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
public delegate void SetLanguage();
public class ExcelTool : MonoBehaviour
{
    private static ExcelTool instance;
    public static ExcelTool Instance { get => instance;}
    public static event SetLanguage LanguageEvent;
    public static Dictionary<string, string> lang = new Dictionary<string, string>();
    public Color[] animalColor;
    public Transform[] enemyAnimal;
    public Transform[] buildAnimal;
    public Sprite[] animalSprite;
    public Sprite[] titleSprite;
    public Sprite tempSprite;

    public Dictionary<string, LevelItem> levels = new Dictionary<string, LevelItem>();
    public Dictionary<string, LineItem> lines = new Dictionary<string, LineItem>();
    public Dictionary<string, EnemyItem> enemys = new Dictionary<string, EnemyItem>();
    public Dictionary<string, BuffItem> buffs = new Dictionary<string, BuffItem>();
    public Dictionary<string, MissionItem> tasks = new Dictionary<string, MissionItem>();
    public Dictionary<string, SkillItem> skills = new Dictionary<string, SkillItem>();
    public List<ShooterItem> shooters = new List<ShooterItem>();
    public List<string> sources = new List<string>();
    public List<float> lockLevel = new List<float>();
    public void Init()
    {
        instance = this;
        PackageLevel level = Resources.Load<PackageLevel>("DataAssets/levelRegular");
        levels = level.GetItems();
        LinePackage route = Resources.Load<LinePackage>("DataAssets/line");
        lines = route.GetItems();
        PackageEnemy enemy = Resources.Load<PackageEnemy>("DataAssets/Enemy");
        enemys = enemy.GetItems();
        PackageBuff buff = Resources.Load<PackageBuff>("DataAssets/Buff");
        buffs = buff.GetItems();
        PackageShooter shooter = Resources.Load<PackageShooter>("DataAssets/Turret");
        shooters = shooter.items;
        PackageTask task = Resources.Load<PackageTask>("DataAssets/Task");
        tasks = task.GetItems();
        PackageSource source= Resources.Load<PackageSource>("DataAssets/Source");
        sources = source.GetItems();
        PackageSkill skill = Resources.Load<PackageSkill>("DataAssets/Skill");
        skills = skill.GetItems();
        PackageDetails detail = Resources.Load<PackageDetails>("DataAssets/Details");
        for (int i = 0; i < shooters.Count; i++)
        {
            shooters[i].starLevel = PlayerPrefs.GetFloat("StarLevel" + i);
        }
        for (int i = 0; i < titleSprite.Length; i++)
        {
            if (titleSprite[i].name == PlayerPrefs.GetString("PackageLanguage", "package_en"))
            {
                tempSprite = titleSprite[i];
                break;
            }
        }
        for (int i = 1; i <= skills.Count; i++)
        {
            lockLevel.Add(skills[i.ToString()].unlock_level);
        }
        for (int i = 0; i < shooters.Count; i++)
        {
            lockLevel.Add(shooters[i].unlock_level);
        }
        lockLevel.Add(50);
        lockLevel.Sort();
        CutLanguage(PlayerPrefs.GetString("PackageLanguage", "package_en"));
    }

    public void CutLanguage(string path)
    {
        if (path == "") return;
        lang = Resources.Load<LanPackage>("DataAssets/" + path).GetItems();
        if (LanguageEvent != null)
        {
            LanguageEvent();
        }
        for (int i = 0; i < titleSprite.Length; i++)
        {
            if (titleSprite[i].name == path)
            {
                tempSprite = titleSprite[i];
                break;
            }
        }
    }
    public void RemoveEvent()
    {
        if (LanguageEvent != null)
        {
            Delegate[] dels = LanguageEvent.GetInvocationList();
            foreach (Delegate d in dels)
            {
                //得到方法名
                object delObj = d.GetType().GetProperty("Method").GetValue(d, null);
                string funcName = (string)delObj.GetType().GetProperty("Name").GetValue(delObj, null);
                LanguageEvent -= d as SetLanguage;
            }
        }
    }

    public float ConDiamond(int state)
    {
        return Mathf.Round(shooters[state].up_diamond + (shooters[state].starLevel - 1) * shooters[state].up_diamond_growth);
    }
    public string FitHurt(int state)
    {
        return Mathf.Round(shooters[state].atk_attach + (PlayerPrefs.GetFloat("FitLevel" + (RACEIMG)state, CreateModel.Instance.turretGrade)-1) * shooters[state].atk_growth_attach).ToString();
    }
    public string StarHurt(int state)
    {
        return Mathf.Round((shooters[state].atk_type +(shooters[state].starLevel - 1) * shooters[state].atk_growth_type)) + ExcelTool.lang["nextgrade"] + ":" +
               Mathf.Round((shooters[state].atk_type +(shooters[state].starLevel) * shooters[state].atk_growth_type));
    }
    //炮塔升星关卡需求
    public float UpStar(int state)
    {
        return (shooters[state].up_star * (shooters[state].starLevel - 1));
    }
}

