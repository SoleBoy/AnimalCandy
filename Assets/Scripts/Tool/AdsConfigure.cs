using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsConfigure : MonoBehaviour
{
    public static string APPID = "1542609300";
    //adMob广告 
    //
    public static string ADMOB_APPID = "ca-app-pub-3940256099942544~3347511713";   //adMob广告ID

    public static string ADMOB_ATKREWARD = "ca-app-pub-3940256099942544/5224354917"; //攻击
    public static string ADMOB_EARREWARD = "ca-app-pub-3940256099942544/5224354917"; //收益
    public static string ADMOB_AUTOREWARD = "ca-app-pub-3940256099942544/5224354917";//自动合并
    public static string ADMOB_SWEETSREWARD = "ca-app-pub-3940256099942544/5224354917"; //大箱子
    public static string ADMOB_DIAMREWARD = "ca-app-pub-3940256099942544/5224354917";//钻石
    public static string ADMOB_Skill = "ca-app-pub-3940256099942544/5224354917";//钻石
    public static string ADMOB_RESTORE = "ca-app-pub-3940256099942544/5224354917";//复活
    public static string ADMOB_RESET = "ca-app-pub-3940256099942544/5224354917";//重置
    public static string ADMOB_Banners = "ca-app-pub-3940256099942544/6300978111";//横幅
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
