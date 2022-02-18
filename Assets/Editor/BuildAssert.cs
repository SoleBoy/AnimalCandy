using UnityEngine;
using UnityEditor;

/// <summary>
/// 利用ScriptableObject创建资源文件
/// </summary>
public class BuildAsset : Editor {

    [MenuItem("BuildAsset/Build Scriptable Level")]
    public static void ExcuteBuildLevel()
    {
        PackageLevel holder = ScriptableObject.CreateInstance<PackageLevel>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuLevel();

        string path= "Assets/Resources/DataAssets/levelRegular.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();
        Debug.Log("BuildAsset Success!");
    }

    [MenuItem("BuildAsset/Build Scriptable Buff")]
    public static void ExcuteBuildBuff()
    {
        PackageBuff holder = ScriptableObject.CreateInstance<PackageBuff>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuBuff();

        string path = "Assets/Resources/DataAssets/Buff.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }

    [MenuItem("BuildAsset/Build Scriptable Turret")]
    public static void ExcuteBuildTurret()
    {
        PackageShooter holder = ScriptableObject.CreateInstance<PackageShooter>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuTurret();

        string path = "Assets/Resources/DataAssets/Turret.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }

    [MenuItem("BuildAsset/Build Scriptable Enemy")]
    public static void ExcuteBuildEnemy()
    {
        PackageEnemy holder = ScriptableObject.CreateInstance<PackageEnemy>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuEnemy();

        string path = "Assets/Resources/DataAssets/Enemy.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }

    [MenuItem("BuildAsset/Build Scriptable Task")]
    public static void ExcuteBuildTask()
    {
        PackageTask holder = ScriptableObject.CreateInstance<PackageTask>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuTask();

        string path = "Assets/Resources/DataAssets/Task.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Scriptable Source")]
    public static void ExcuteBuildSource()
    {
        PackageSource holder = ScriptableObject.CreateInstance<PackageSource>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuSource();

        string path = "Assets/Resources/DataAssets/Source.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Scriptable Skill")]
    public static void ExcuteBuildSkill()
    {
        PackageSkill holder = ScriptableObject.CreateInstance<PackageSkill>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuSkill();

        string path = "Assets/Resources/DataAssets/Skill.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Scriptable Details")]
    public static void ExcuteBuildDetails()
    {
        PackageDetails holder = ScriptableObject.CreateInstance<PackageDetails>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuDetails();

        string path = "Assets/Resources/DataAssets/Details.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Scriptable Line")]
    public static void ExcuteBuildLine()
    {
        LinePackage holder = ScriptableObject.CreateInstance<LinePackage>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectLineItems();
        string path = "Assets/Resources/DataAssets/line.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }

    [MenuItem("BuildAsset/Build Scriptable Langcn")]
    public static void ExcuteBuildLangcn()
    {
        LanPackage holder = ScriptableObject.CreateInstance<LanPackage>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuLang(0);

        string path = "Assets/Resources/DataAssets/package_cn.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Scriptable Langen")]
    public static void ExcuteBuildLangen()
    {
        LanPackage holder = ScriptableObject.CreateInstance<LanPackage>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuLang(1);

        string path = "Assets/Resources/DataAssets/package_en.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Scriptable Langjp")]
    public static void ExcuteBuildLangjp()
    {
        LanPackage holder = ScriptableObject.CreateInstance<LanPackage>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuLang(2);
        string path = "Assets/Resources/DataAssets/package_jp.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Scriptable Langbig")]
    public static void ExcuteBuildLangbig()
    {
        LanPackage holder = ScriptableObject.CreateInstance<LanPackage>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuLang(3);
        string path = "Assets/Resources/DataAssets/package_big.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Scriptable Langkor")]
    public static void ExcuteBuildLangkor()
    {
        LanPackage holder = ScriptableObject.CreateInstance<LanPackage>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuLang(4);
        string path = "Assets/Resources/DataAssets/package_kor.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();

        Debug.Log("BuildAsset Success!");
    }
    
}
