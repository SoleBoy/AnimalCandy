using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;

public class ExcelAccess
{
    public static string Excel = "config";

    //查询关卡表
    public static List<LevelItem> SelectMenuLevel()
    {
        string excelName = Excel + ".xlsx";
        //string sheetName = "package_cn";
        //int sheetId = 0;
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName,2);

        List<LevelItem> menuArray = new List<LevelItem>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            LevelItem level = new LevelItem
            {
                id= collect[i][0].ToString(),
                interval = collect[i][1].ToString(),
                max = float.Parse(collect[i][2].ToString()),
                suol_max = float.Parse(collect[i][3].ToString()),
                soldier_id = collect[i][4].ToString(),
                soldier_lv = collect[i][5].ToString(),
                soldier_1 = collect[i][6].ToString(),
                soldier_2 = collect[i][7].ToString(),
                soldier_3 = collect[i][8].ToString(),
                soldier_4 = collect[i][9].ToString(),
                soldier_5 = collect[i][10].ToString(),
                soldier_6 = collect[i][11].ToString(),
                soldier_7 = collect[i][12].ToString(),
                boos = collect[i][13].ToString(),
                mapId = collect[i][14].ToString(),
                weatherID = collect[i][15].ToString(),
                minssion_diamond = float.Parse(collect[i][16].ToString()),
                timeAngle = collect[i][17].ToString(),
                lightcolor = collect[i][18].ToString(),
                skycolor = collect[i][19].ToString(),
                soldier_lv_timemode = collect[i][20].ToString(),
                suol_max_timemode = float.Parse(collect[i][21].ToString())
            };
            menuArray.Add(level);
        }
        return menuArray;
    }
    //查询敌人表
    public static List<EnemyItem> SelectMenuEnemy()
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection enemyData = ExcelAccess.ReadExcel(excelName, 0);

        List<EnemyItem> menuArray = new List<EnemyItem>();
        for (int i = 1; i < enemyData.Count; i++)
        {
            if (enemyData[i][1].ToString() == "") continue;

            EnemyItem buff = new EnemyItem
            {
                id = enemyData[i][0].ToString(),
                def_spe = float.Parse(enemyData[i][1].ToString()),
                def_spe_type = float.Parse(enemyData[i][2].ToString()),
                speed = enemyData[i][3].ToString(),
                realName = enemyData[i][4].ToString(),
                def = float.Parse(enemyData[i][5].ToString()),
                hp = float.Parse(enemyData[i][6].ToString()),
                def_spe_growth = float.Parse(enemyData[i][7].ToString()),
                def_growth = float.Parse(enemyData[i][8].ToString()),
                hp_growth = float.Parse(enemyData[i][9].ToString()),
                soul = float.Parse(enemyData[i][10].ToString()),
                dropG = float.Parse(enemyData[i][11].ToString()),
                jump_power=float.Parse(enemyData[i][12].ToString()),
                jump_interval=enemyData[i][13].ToString(),
                jump_speed=enemyData[i][14].ToString(),
                speed_add_max=float.Parse(enemyData[i][15].ToString()),
                shoot_if = float.Parse(enemyData[i][16].ToString()),
                shoot_interval = enemyData[i][17].ToString(),
                shoot_speed = enemyData[i][18].ToString(),
                shoot_type = float.Parse(enemyData[i][19].ToString()),
                shoot_number = float.Parse(enemyData[i][20].ToString()),
                shoot_distance = enemyData[i][21].ToString(),
                body_type = float.Parse(enemyData[i][22].ToString()),
                shoot_distance_line = enemyData[i][23].ToString(),
            };
                menuArray.Add(buff);
        }
        return menuArray;
    }
    //查询炮塔表
    public static List<ShooterItem> SelectMenuTurret()
    {
        string excelName = Excel +".xlsx";
        DataRowCollection shooterData = ExcelAccess.ReadExcel(excelName, 1);

        List<ShooterItem> menuArray = new List<ShooterItem>();
        for (int i = 1; i < shooterData.Count; i++)
        {
            if (shooterData[i][1].ToString() == "") continue;
            ShooterItem item=new ShooterItem
            {
                id=shooterData[i][0].ToString(),
                type = float.Parse(shooterData[i][1].ToString()),
                realname = shooterData[i][2].ToString(),
                atk_distance = shooterData[i][3].ToString(),
                atk_type = float.Parse(shooterData[i][4].ToString()),
                atk_growth_type = float.Parse(shooterData[i][5].ToString()),
                atk_attach = float.Parse(shooterData[i][6].ToString()),
                atk_growth_attach = float.Parse(shooterData[i][7].ToString()),
                att_interval = float.Parse(shooterData[i][8].ToString()),
                unlock_diamond = float.Parse(shooterData[i][9].ToString()),
                up_star = float.Parse(shooterData[i][10].ToString()),
                up_diamond = float.Parse(shooterData[i][11].ToString()),
                unlock_level = float.Parse(shooterData[i][12].ToString()),
                up_diamond_growth = float.Parse(shooterData[i][13].ToString()),
                drop_shooter = float.Parse(shooterData[i][14].ToString()),
                bullet_type = float.Parse(shooterData[i][15].ToString()),
                num = float.Parse(shooterData[i][16].ToString()),
                param = shooterData[i][17].ToString(),
                speed = shooterData[i][18].ToString(),
                range = shooterData[i][19].ToString(),
                dam_max = float.Parse(shooterData[i][20].ToString()),
                buff = shooterData[i][21].ToString(),
                explain= shooterData[i][22].ToString(),
                ip_name= shooterData[i][23].ToString(),
                param_line= shooterData[i][24].ToString(),
                attackrange= shooterData[i][25].ToString(),
            };
            menuArray.Add(item);
        }
        return menuArray;
    }
    //查询BUff表
    public static List<BuffItem> SelectMenuBuff()
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName, 3);

        List<BuffItem> menuArray = new List<BuffItem>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            BuffItem buff = new BuffItem
            {
                id = collect[i][0].ToString(),
                type = float.Parse(collect[i][1].ToString()),
                des = collect[i][2].ToString(),
                time = float.Parse(collect[i][3].ToString()),
                probability = float.Parse(collect[i][4].ToString()),
                boosOdds= float.Parse(collect[i][5].ToString()),
                number= collect[i][6].ToString(),
            };
            menuArray.Add(buff);
        }
        return menuArray;
    }
    //查询任务表
    public static List<MissionItem> SelectMenuTask()
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName, 5);

        List<MissionItem> menuArray = new List<MissionItem>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            MissionItem task = new MissionItem
            {
                id = collect[i][0].ToString(),
                type_mission = float.Parse(collect[i][1].ToString()),
                type_reward = float.Parse(collect[i][2].ToString()),
                need = float.Parse(collect[i][3].ToString()),
                need_growth = float.Parse(collect[i][4].ToString()),
                reward = float.Parse(collect[i][5].ToString()),
                reward_growth = float.Parse(collect[i][6].ToString()),
                icon_name = collect[i][7].ToString(),
                description = collect[i][8].ToString(),
                resistance = collect[i][9].ToString(),
            };
            menuArray.Add(task);
        }
        return menuArray;
    }

    //查询音效
    public static List<SourceItem> SelectMenuSource()
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName, 4);

        List<SourceItem> menuArray = new List<SourceItem>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            SourceItem source = new SourceItem
            {
                id = i.ToString(),
                source = collect[i][0].ToString(),
            };
            menuArray.Add(source);
        }
        return menuArray;
    }
    //查询技能表
    public static List<SkillItem> SelectMenuSkill()
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName, 6);

        List<SkillItem> menuArray = new List<SkillItem>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;
            SkillItem skill = new SkillItem
            {
                id = collect[i][0].ToString(),
                realname = collect[i][1].ToString(),
                unlock_level = float.Parse(collect[i][2].ToString()),
                up_star = float.Parse(collect[i][3].ToString()),
                up__star_growth = float.Parse(collect[i][4].ToString()),
                unlock_diamond = float.Parse(collect[i][5].ToString()),
                up_diamond = float.Parse(collect[i][6].ToString()),
                up_diamond_growth = float.Parse(collect[i][7].ToString()),
                forcibly_unlock_diamond = float.Parse(collect[i][8].ToString()),
                dam_max = collect[i][9].ToString(),
                num = float.Parse(collect[i][10].ToString()),
                bullet_type = float.Parse(collect[i][11].ToString()),
                range = collect[i][12].ToString(),
                atk_percentage = float.Parse(collect[i][13].ToString()),
                atk_num = float.Parse(collect[i][14].ToString()),
                atk_growth = float.Parse(collect[i][15].ToString()),
                param = collect[i][16].ToString(),
                buff_id = collect[i][17].ToString(),
                att_interval = collect[i][18].ToString(),
                cost = collect[i][19].ToString(),
                des = collect[i][20].ToString(),
                drop_skill = float.Parse(collect[i][21].ToString()),
                boss_atk_percentage = float.Parse(collect[i][22].ToString()),
                type= float.Parse(collect[i][23].ToString()),
                ip_name = collect[i][24].ToString(),
                param_line = collect[i][25].ToString(),
            };
            menuArray.Add(skill);
        }
        return menuArray;
    }
    //查询信息
    public static List<DetailsItem> SelectMenuDetails()
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName, 7);

        List<DetailsItem> menuArray = new List<DetailsItem>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            DetailsItem source = new DetailsItem
            {
                id = collect[i][0].ToString(),
                realname = collect[i][1].ToString(),
                des = collect[i][2].ToString(),
                ip_name = collect[i][3].ToString(),
            };
            menuArray.Add(source);
        }
        return menuArray;
    } 
    //路线关卡
    public static List<LineItem> SelectLineItems()
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName, 8);

        List<LineItem> menuArray = new List<LineItem>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            LineItem source = new LineItem
            {
                id = collect[i][0].ToString(),
                line_id = collect[i][1].ToString(),
                start = collect[i][2].ToString(),
                end = collect[i][3].ToString(),
                shooter_position = collect[i][4].ToString(),
                start_gold = collect[i][5].ToString(),
                line_1 = collect[i][6].ToString(),
                line_2 = collect[i][7].ToString(),
                route_1 = collect[i][8].ToString(),
                route_2 = collect[i][9].ToString(),
                route_3 = collect[i][10].ToString(),
                route_4 = collect[i][11].ToString(),
                line_enemy_wave = collect[i][12].ToString(),
                line_order = collect[i][13].ToString(),
                line_enemy_type = collect[i][14].ToString(),
                line_enemy_level = collect[i][15].ToString(),
                line_enemy_num = collect[i][16].ToString(),
                wave_interval = collect[i][17].ToString(),
                enemy_interval = collect[i][18].ToString(),
                line_shooter = collect[i][19].ToString(),
                line_skill = collect[i][20].ToString(),
                battlefield_mission_type = collect[i][21].ToString(),
                battlefield_mission_id_num = collect[i][22].ToString(),
                time = collect[i][23].ToString(),
                lightcolor = collect[i][24].ToString(),
                skycolor = collect[i][25].ToString(),
                mapID = collect[i][26].ToString(),
                weatherID = collect[i][27].ToString(),
                Line_Bg = int.Parse(collect[i][28].ToString()),
                add_suger = float.Parse(collect[i][29].ToString()),
            };
            menuArray.Add(source);
        }
        return menuArray;
    }
    //查询语言表
    public static List<LanguageItem> SelectMenuLang(int index)
    {
        string excelName = "Lan.xlsx";
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName, index);

        List<LanguageItem> menuArray = new List<LanguageItem>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            LanguageItem menu = new LanguageItem
            {
                key = collect[i][0].ToString(),
                value = collect[i][1].ToString(),
            };
            menuArray.Add(menu);
        }
        return menuArray;
    }


    /// <summary>
    /// 读取 Excel ; 需要添加 Excel.dll; System.Data.dll;
    /// </summary>
    /// <param name="excelName">excel文件名</param>
    /// <param name="sheetName">sheet名称</param>
    /// <returns>DataRow的集合</returns>
    static DataRowCollection ReadExcel(string excelName,int sheetName)
    {
        string path= Application.dataPath + "/Excel/" + excelName;
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        //int columns = result.Tables[0].Columns.Count;
        //int rows = result.Tables[0].Rows.Count;

        //tables可以按照sheet名获取，也可以按照sheet索引获取
       // return result.Tables[3].Rows;
        return result.Tables[sheetName].Rows;
    }

    /// <summary>
    /// 写入 Excel ; 需要添加 OfficeOpenXml.dll;
    /// </summary>
    /// <param name="excelName">excel文件名</param>
    /// <param name="sheetName">sheet名称</param>
    public static void WriteExcel(string excelName, string sheetName)
    {
        //通过面板设置excel路径
        //string outputDir = EditorUtility.SaveFilePanel("Save Excel", "", "New Resource", "xlsx");

        //自定义excel的路径
        string path = Application.dataPath + "/" + excelName;
        FileInfo newFile = new FileInfo(path);
        if (newFile.Exists)
        {
            //创建一个新的excel文件
            newFile.Delete();
            newFile = new FileInfo(path);
        }

        //通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            //在excel空文件添加新sheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
            //添加列名
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Product";
            worksheet.Cells[1, 3].Value = "Quantity";
            worksheet.Cells[1, 4].Value = "Price";
            worksheet.Cells[1, 5].Value = "Value";

            //添加一行数据
            worksheet.Cells["A2"].Value = 12001;
            worksheet.Cells["B2"].Value = "Nails";
            worksheet.Cells["C2"].Value = 37;
            worksheet.Cells["D2"].Value = 3.99;
            //添加一行数据
            worksheet.Cells["A3"].Value = 12002;
            worksheet.Cells["B3"].Value = "Hammer";
            worksheet.Cells["C3"].Value = 5;
            worksheet.Cells["D3"].Value = 12.10;
            //添加一行数据
            worksheet.Cells["A4"].Value = 12003;
            worksheet.Cells["B4"].Value = "Saw";
            worksheet.Cells["C4"].Value = 12;
            worksheet.Cells["D4"].Value = 15.37;

            //保存excel
            package.Save();
        }
    }

}

