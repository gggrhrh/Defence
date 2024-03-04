using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue
{
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
        //PlayerPrefs.DeleteAll();
        g_NickName = PlayerPrefs.GetString("NickName", "SBS����");
        g_UserLevel = PlayerPrefs.GetInt("UserLevel", 0);
        g_UserGold = PlayerPrefs.GetInt("UserGold", 100);
        g_Attack = PlayerPrefs.GetInt("AttackLv", 0);
        g_AttSpeed = PlayerPrefs.GetInt("AttSpeedLv", 0);
        g_CriRate = PlayerPrefs.GetInt("CriRateLv", 0);
        g_CriDmg = PlayerPrefs.GetInt("CriDmgLv", 0);
    }
}
