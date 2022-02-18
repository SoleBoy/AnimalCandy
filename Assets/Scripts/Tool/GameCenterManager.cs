using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterManager : MonoBehaviour
{
    public void Init()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Social.localUser.Authenticate(success => {
                if (success)
                {
                    Debug.Log("登录成功");
                }
                else
                {
                }
            });
        }
    }
    void Initialized()
    {
        ReportScore((long)(PlayerPrefs.GetFloat("RouteMaxLevel") + PlayerPrefs.GetFloat("MaxLevel")), "ac.Stars");
        ReportScore((long)PlayerPrefs.GetFloat("maxDiamond"), "ac.Diamonds");
        ReportScore((long)HighestFit(), "ac.Total_fit_level");
        ReportScore((long)HighestStar(), "ac.Total_star_rating");
        ReportScore((long)PlayerPrefs.GetFloat("PassLevel"), "ac.perfectstage");
        ReportScore((long)PlayerPrefs.GetFloat("MaxScore"), "ac.building");
        ReportScore((long)PlayerPrefs.GetInt("TurnUpTreasure"), "ac.treasure");
        ReportScore((long)PlayerPrefs.GetInt("PatternTime"), "ac.time"); 
        ReportScore((long)PlayerPrefs.GetFloat("RouteMaxLevel"), "ac.LineStage");
    }
    //糖果合体等级之和
    public float HighestFit()
    {
        float num = 0;
        for (int i = 0; i < ExcelTool.Instance.shooters.Count; i++)
        {
            num += PlayerPrefs.GetFloat("FitLevel" + (RACEIMG)i);
        }
        return num;
    }
    //糖果 and 技能 升星等级之和
    public float HighestStar()
    {
        float num = 0;
        for (int i = 0; i < ExcelTool.Instance.shooters.Count; i++)
        {
            num += ExcelTool.Instance.shooters[i].starLevel;
        }
        for (int i = 1; i <= 10; i++)
        {
            num += PlayerPrefs.GetInt("SkillGrade" + i);
        }
        return num;
    }
    public void ReportScore(long score, string leaderboardID)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(score, leaderboardID, success => {
                });
            }
        }
    }
    public void ShowLeaderboard()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Social.localUser.authenticated)
            {
                Initialized();
                Social.ShowLeaderboardUI();
            }
        }
    }
}
