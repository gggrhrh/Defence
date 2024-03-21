using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Skill_0 = 0,        // 본체때리기
    Skill_1,            // 작업관리자
    Skill_2,            // 전원껐다켜기
    //Skill_3,            // "유도탄"
    //Skill_4,            // "더블샷"
    //Skill_5,            // "소환수 공격"
    SkCount
}

public class Skill_Info  //각 Item 정보
{
    public string m_Name = "";                  //캐릭터 이름
    public SkillType m_SkType = SkillType.Skill_0; //캐릭터 타입
    public int m_Price = 100;   //아이템 구매 가격 
    public int m_UpPrice = 50; //업그레이드 가격, 타입에 따라서
    public int m_UserLv = 0;    //스킬을 얻는데 필요한 유저의 레벨
    public float m_Damage = 0.0f;   //스킬의 데미지
    public float m_UpDamage = 0.0f; // 업그레이드 스킬 데미지
    public float m_CoolTime = 0.0f;
    public string m_SkillExp = "";    //스킬 효과 설명
    //public Sprite m_IconImg = null;   //캐릭터 아이템에 사용될 이미지

    public void SetType(SkillType a_SkType)
    {
        m_SkType = a_SkType;

        if (a_SkType == SkillType.Skill_0)
        {
            m_Name = "본체때리기";
            m_Price = 10; //기본가격
            m_UpPrice = 5; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) 가격 필요
            m_UserLv = 0;

            m_Damage = 50.0f;
            m_UpDamage = 10.0f;
            m_CoolTime = 5.0f;
            m_SkillExp = "모든 몬스터에게 데미지를 줍니다.";
        }
        else if (a_SkType == SkillType.Skill_1)
        {
            m_Name = "작업관리자켜기";
            m_Price = 20; //기본가격
            m_UpPrice = 10; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) 가격 필요
            m_UserLv = 0;

            m_Damage = 1000.0f;
            m_UpDamage = 500.0f;
            m_CoolTime = 5.0f;
            m_SkillExp = "하나의 몬스터를 삭제합니다.";
        }
        else if (a_SkType == SkillType.Skill_2)
        {
            m_Name = "전원껐다켜기";
            m_Price = 50;
            m_UpPrice = 25;
            m_UserLv = 10;

            m_Damage = 1000.0f;
            m_UpDamage = 500.0f;
            m_CoolTime = 10.0f;
            m_SkillExp = "모든 몬스터를 삭제합니다.";
        }

    }//public void SetType(SkillType a_SkType)
}
public class GlobalValue
{
    public static List<Skill_Info> g_SkDataList = new List<Skill_Info>(); //스킬 아이템 설정 리스트
    public static List<int> g_SkLevelList = new List<int>();    //스킬의 레벨 리스트

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
        //--- 설정 데이터 로딩
        if (g_SkDataList.Count <= 0)
        {
            Skill_Info a_SkItemNd;
            for (int ii = 0; ii < (int)SkillType.SkCount; ii++)
            {
                a_SkItemNd = new Skill_Info();
                a_SkItemNd.SetType((SkillType)ii);
                g_SkDataList.Add(a_SkItemNd);
            }
        }
        //--- 설정 데이터 로딩
        g_NickName = PlayerPrefs.GetString("NickName", "SBS전사");
        g_UserLevel = PlayerPrefs.GetInt("UserLevel", 0);
        g_UserGold = PlayerPrefs.GetInt("UserGold", 100);
        g_Attack = PlayerPrefs.GetInt("AttackLv", 0);
        g_AttSpeed = PlayerPrefs.GetInt("AttSpeedLv", 0);
        g_CriRate = PlayerPrefs.GetInt("CriRateLv", 0);
        g_CriDmg = PlayerPrefs.GetInt("CriDmgLv", 0);

        //---서버나 로컬에 저장된 보유 상태 로딩
        if (g_SkLevelList.Count <= 0)
        {
            string a_KeyBuff = "";
            int a_SkLevel = 0;
            for (int ii = 0; ii < (int)SkillType.SkCount; ii++)
            {
                a_KeyBuff = string.Format("Skill_Item_{0}", ii); //"Skill_Item_0", "Skill_Item_1"
                a_SkLevel = PlayerPrefs.GetInt(a_KeyBuff, 0);
                // 1이면 보유 0이면 미보유

                g_SkLevelList.Add(a_SkLevel);
            }
        }
        //--- 서버나 로컬에 저장된 보유 상태 로딩
    }
}
