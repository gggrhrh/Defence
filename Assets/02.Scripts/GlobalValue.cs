using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue
{
    public static string g_UniqID = "";     //유저 아이디
    public static string g_NickName = "";   //유저 닉네임
    public static int g_UserGold = 0;       //유저의 골드
    public static int g_UserLevel = 0;      //유저의 레벨
    public static int g_Exp = 0;            //유저 경험치
    public static int g_Attack = 0;         //상점에서 업그레이드 하는 공격력 레벨 (100레벨 까지)
    public static int g_AttSpeed = 0;       //공격속도 레벨(20레벨 까지)
    public static int g_CriRate = 0;        //크리티컬 확률
    public static int g_CriDmg = 0;         //크리티컬 데미지

    public static void LoadGameData()
    {
        //PlayerPrefs.DeleteAll();
        g_NickName = PlayerPrefs.GetString("NickName", "SBS전사");
        g_UserLevel = PlayerPrefs.GetInt("UserLevel", 0);
        g_UserGold = PlayerPrefs.GetInt("UserGold", 100);
        g_Attack = PlayerPrefs.GetInt("AttackLv", 0);
        g_AttSpeed = PlayerPrefs.GetInt("AttSpeedLv", 0);
        g_CriRate = PlayerPrefs.GetInt("CriRateLv", 0);
        g_CriDmg = PlayerPrefs.GetInt("CriDmgLv", 0);
    }
}
