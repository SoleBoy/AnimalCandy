using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsConfigure : MonoBehaviour
{
    public static string APPID = "1490059764";
    //adMob广告  
    public static string ADMOB_APPID = "ca-app-pub-3694982375313195~4004351704";   //adMob广告ID

    public static string ADMOB_ATKREWARD = "ca-app-pub-3694982375313195/4108277577"; //攻击
    public static string ADMOB_EARREWARD = "ca-app-pub-3694982375313195/8406631147"; //收益
    public static string ADMOB_AUTOREWARD = "ca-app-pub-3694982375313195/1321497989";//自动合并
    public static string ADMOB_SWEETSREWARD = "ca-app-pub-3694982375313195/9859297701"; //大箱子
    public static string ADMOB_DIAMREWARD = "ca-app-pub-3694982375313195/5327018147";//钻石
    public static string ADMOB_Skill = "ca-app-pub-3694982375313195/5042101207";//技能
    public static string ADMOB_RESTORE = "ca-app-pub-3694982375313195/4449050744";//复活
    public static string ADMOB_RESET = "ca-app-pub-3694982375313195/6336847480";//重置
    public static string ADMOB_Banners = "ca-app-pub-3694982375313195/8897569821";//横幅

    //public static string ADMOB_APPID = "ca-app-pub-3940256099942544~1458002511";   //adMob广告ID
    //public static string ADMOB_ATKREWARD = "ca-app-pub-3940256099942544/1712485313"; //攻击
    //public static string ADMOB_EARREWARD = "ca-app-pub-3940256099942544/1712485313"; //收益
    //public static string ADMOB_AUTOREWARD = "ca-app-pub-3940256099942544/1712485313";//自动合并
    //public static string ADMOB_SWEETSREWARD = "ca-app-pub-3940256099942544/1712485313"; //大箱子
    //public static string ADMOB_DIAMREWARD = "ca-app-pub-3940256099942544/1712485313";//钻石
    //public static string ADMOB_Skill = "ca-app-pub-3940256099942544/1712485313";//钻石
    //public static string ADMOB_RESTORE = "ca-app-pub-3940256099942544/1712485313";//复活
    //public static string ADMOB_RESET = "ca-app-pub-3940256099942544/1712485313";//重置
    //public static string ADMOB_Banners = "ca-app-pub-3940256099942544/2934735716";//横幅
    //商店购买  产品ID
    public static string ProductID_Candy = "ac_50000candies_1_app";
    public static string ProductID_Diamond1 = "ac_100diamonds_1_app";
    public static string ProductID_Diamond2 = "ac_600diamonds_30_app";
    public static string ProductID_Diamond3 = "ac_1500diamonds_68_app";
    public static string ProductID_Auto = "ac_automaticfit_6_app";
    public static string ProductID_Attack = "ac_attackadd5_12_app";
    public static string ProductID_Income = "ac_3timescandy_12_app";
    public static string ProductID_Bank = "ac_Candybank_18_app";
    public static string ProductID_VIP = "ac_vipgift_30_app";
    public static string ProductID_task = "ac_diamondmission_12_app";
}

//广告类型
public enum AdsType
{
    attack,
    earnings,
    auto,
    sweets,
    skill,
    diamond,
    restore,
    reset,
}
