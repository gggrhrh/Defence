using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Skill_0 = 0,        // Boom
    Skill_1,            // Iceage
    //Skill_2,            // "��ȣ��"
    //Skill_3,            // "����ź"
    //Skill_4,            // "����"
    //Skill_5,            // "��ȯ�� ����"
    SkCount
}

public class Skill_Info  //�� Item ����
{
    public string m_Name = "";                  //ĳ���� �̸�
    public SkillType m_SkType = SkillType.Skill_0; //ĳ���� Ÿ��
    public int m_Price = 100;   //������ �⺻ ���� 
    public int m_UpPrice = 50; //���׷��̵� ����, Ÿ�Կ� ����
    public int m_Level = 0;     //��ų�� ����
    public int m_UserLv = 0;    //��ų�� ��µ� �ʿ��� ������ ����
    public float m_Damage = 0.0f;
    //public string m_SkillExp = "";    //��ų ȿ�� ����
    //public Sprite m_IconImg = null;   //ĳ���� �����ۿ� ���� �̹���

    public void SetType(SkillType a_SkType)
    {
        m_SkType = a_SkType;

        if (a_SkType == SkillType.Skill_0)
        {
            m_Name = "ȭ������";
            m_Price = 10; //�⺻����
            m_UpPrice = 5; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) ���� �ʿ�
            m_UserLv = 0;

        }
        else if (a_SkType == SkillType.Skill_1)
        {
            m_Name = "���̽�������";

            m_Price = 20; //�⺻����
            m_UpPrice = 10; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) ���� �ʿ�

            m_UserLv = 3;
        }
        //else if (a_SkType == SkillType.Skill_2)
        //{
        //    m_Name = "����ȣ";
 
        //    m_Price = 400; //�⺻����
        //    m_UpPrice = 200; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) ���� �ʿ�

        //    m_SkillExp = "��ȣ��";
        //    m_IconImg = Resources.Load("IconImg/m0054", typeof(Sprite)) as Sprite;
        //}
        //else if (a_SkType == SkillType.Skill_3)
        //{
        //    m_Name = "�߿���";
        //    m_IconSize.x = 0.93f;     //���ο� ���� ���� ����
        //    m_IconSize.y = 1.0f;     //���θ� �������� ���� ���̱� ������ �׳� 1.0f

        //    m_Price = 800; //�⺻����
        //    m_UpPrice = 400; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) ���� �ʿ�

        //    m_SkillExp = "����ź";
        //    m_IconImg = Resources.Load("IconImg/m0423", typeof(Sprite)) as Sprite;
        //}
        //else if (a_SkType == SkillType.Skill_4)
        //{
        //    m_Name = "�巡��";
        //    m_IconSize.x = 0.93f;     //���ο� ���� ���� ����
        //    m_IconSize.y = 1.0f;     //���θ� �������� ���� ���̱� ������ �׳� 1.0f

        //    m_Price = 1600; //�⺻����
        //    m_UpPrice = 800; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) ���� �ʿ�

        //    m_SkillExp = "����";
        //    m_IconImg = Resources.Load("IconImg/m0244", typeof(Sprite)) as Sprite;
        //}
        //else if (a_SkType == SkillType.Skill_5)
        //{
        //    m_Name = "��Ŀ��";
        //    m_IconSize.x = 0.93f;    //���ο� ���� ���� ����
        //    m_IconSize.y = 1.0f;     //���θ� �������� ���� ���̱� ������ �׳� 1.0f

        //    m_Price = 3000;   //�⺻����
        //    m_UpPrice = 1600; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) ���� �ʿ�

        //    m_SkillExp = "��ȯ�� ����";
        //    m_IconImg = Resources.Load("IconImg/m0172", typeof(Sprite)) as Sprite;
        //}

    }//public void SetType(SkillType a_SkType)
}
public class GlobalValue
{
    public static List<Skill_Info> g_SkDataList = new List<Skill_Info>(); //��ų ������ ���� ����Ʈ
    public static List<int> g_SkLevelList = new List<int>();    //��ų�� ���� ����Ʈ

    public static string g_UniqID = "";     //���� ���̵�
    public static string g_NickName = "";   //���� �г���
    public static int g_UserGold = 0;       //������ ���
    public static int g_UserLevel = 0;      //������ ����
    public static int g_Exp = 0;            //���� ����ġ
    public static int g_Attack = 0;         //�������� ���׷��̵� �ϴ� ���ݷ� ���� (100���� ����)
    public static int g_AttSpeed = 0;       //���ݼӵ� ����(20���� ����)
    public static int g_CriRate = 0;        //ũ��Ƽ�� Ȯ��
    public static int g_CriDmg = 0;         //ũ��Ƽ�� ������

    public static void LoadGameData()
    {
        //--- ���� ������ �ε�
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
        //--- ���� ������ �ε�

        //PlayerPrefs.DeleteAll();
        g_NickName = PlayerPrefs.GetString("NickName", "SBS����");
        g_UserLevel = PlayerPrefs.GetInt("UserLevel", 0);
        g_UserGold = PlayerPrefs.GetInt("UserGold", 100);
        g_Attack = PlayerPrefs.GetInt("AttackLv", 0);
        g_AttSpeed = PlayerPrefs.GetInt("AttSpeedLv", 0);
        g_CriRate = PlayerPrefs.GetInt("CriRateLv", 0);
        g_CriDmg = PlayerPrefs.GetInt("CriDmgLv", 0);

        //---������ ���ÿ� ����� ���� ���� �ε�
        if (g_SkLevelList.Count <= 0)
        {
            string a_KeyBuff = "";
            int a_SkLevel = 0;
            for (int ii = 0; ii < (int)SkillType.SkCount; ii++)
            {
                a_KeyBuff = string.Format("Skill_Item_{0}", ii); //"Skill_Item_0", "Skill_Item_1"
                a_SkLevel = PlayerPrefs.GetInt(a_KeyBuff, 0);
                // 1�̸� ���� 0�̸� �̺���

                g_SkLevelList.Add(a_SkLevel);
            }
        }
        //--- ������ ���ÿ� ����� ���� ���� �ε�
    }
}
