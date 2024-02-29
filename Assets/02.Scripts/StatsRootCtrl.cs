using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsRootCtrl : MonoBehaviour
{
    public Button m_ResetBtn;
    public Button m_SaveBtn;

    [Header("------ UpButton ------")]
    public Button AttackUpBtn;
    public Button AttSpeedUpBtn;
    public Button CriRateUpBtn;
    public Button CriDmgUpBtn;

    [Header("------ DownButton ------")]
    public Button AttackDownBtn;
    public Button AttSpeedDownBtn;
    public Button CriRateDownBtn;
    public Button CriDmgDownBtn;

    [Header("------ Text ------")]
    public Text AttackText;
    public Text AttSpeedText;
    public Text CriRateText;
    public Text CriDmgText;

    //--- 저장용 
    int AttackLevel = 0;
    int AttSpeedLevel = 0;
    int CriRateLevel = 0;
    int CriDmgLevel = 0;
    int m_BuyGold = 0;
    int m_UserGold = 0;
    //--- 저장용

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.LoadGameData();
        ResetBtnClick();

        if (m_ResetBtn != null)
            m_ResetBtn.onClick.AddListener(ResetBtnClick);

        if (m_SaveBtn != null)
            m_SaveBtn.onClick.AddListener(SaveBtnClick);

        //------ UpBtn Click
        if (AttackUpBtn != null)
            AttackUpBtn.onClick.AddListener(() =>
            {
                UpBtnClickMethod(0, 1);
            });

        if (AttSpeedUpBtn != null)
            AttSpeedUpBtn.onClick.AddListener(() =>
            {
                UpBtnClickMethod(1, 5);
            });

        if (CriRateUpBtn != null)
            CriRateUpBtn.onClick.AddListener(() =>
            {
                UpBtnClickMethod(2, 5);
            });

        if (CriDmgUpBtn != null)
            CriDmgUpBtn.onClick.AddListener(() =>
            {
                UpBtnClickMethod(3, 1);
            });

        //------ DownBtn Click
        if (AttackDownBtn != null)
            AttackDownBtn.onClick.AddListener(() =>
            {
                DownBtnClickMethod(0, 1);
            });

        if (AttSpeedDownBtn != null)
            AttSpeedDownBtn.onClick.AddListener(() =>
            {
                DownBtnClickMethod(1, 5);
            });

        if (CriRateDownBtn != null)
            CriRateDownBtn.onClick.AddListener(() =>
            {
                DownBtnClickMethod(2, 5);
            });

        if (CriDmgDownBtn != null)
            CriDmgDownBtn.onClick.AddListener(() =>
            {
                DownBtnClickMethod(3, 1);
            });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpBtnClickMethod(int a_Value, int a_Gold)
    {
        m_BuyGold += a_Gold;
        if (GlobalValue.g_UserGold < m_BuyGold)
        {
            return;
        }

        if (a_Value == 0)
        {
            if (AttackLevel >= 100) return;
            AttackLevel++;
        }
        else if (a_Value == 1)
        {
            if (AttSpeedLevel >= 20) return;
            AttSpeedLevel++;
        }
        else if (a_Value == 2)
        {
            if (CriRateLevel >= 20) return;
            CriRateLevel++;
        }
        else if (a_Value == 3)
        {
            if (CriDmgLevel >= 100) return;
            CriDmgLevel++;
        }

        m_UserGold = GlobalValue.g_UserGold - m_BuyGold;
        StoreMgr.Inst.m_GoldText.text = m_UserGold.ToString();

        RefreshText();
    }

    void DownBtnClickMethod(int a_Value, int a_Gold)
    {
        if (m_BuyGold < 0)
            return;

        if (a_Value == 0)
        {
            if (AttackLevel <= 0) return;
            if (GlobalValue.g_Attack < AttackLevel) m_BuyGold -= a_Gold;
            AttackLevel--;
        }
        else if (a_Value == 1)
        {
            if (AttSpeedLevel <= 0) return;
            if (GlobalValue.g_AttSpeed < AttSpeedLevel) m_BuyGold -= a_Gold;
            AttSpeedLevel--;
        }
        else if (a_Value == 2)
        {
            if (CriRateLevel <= 0) return;
            if (GlobalValue.g_CriRate < CriRateLevel) m_BuyGold -= a_Gold;
            CriRateLevel--;
        }
        else if (a_Value == 3)
        {
            if (CriDmgLevel <= 0) return;
            if (GlobalValue.g_CriDmg < CriDmgLevel) m_BuyGold -= a_Gold;
            CriDmgLevel--;
        }

        m_UserGold = GlobalValue.g_UserGold - m_BuyGold;
        StoreMgr.Inst.m_GoldText.text = m_UserGold.ToString();

        RefreshText();
    }

    void RefreshText()
    {
        if (AttackText != null)
            AttackText.text = AttackLevel.ToString();

        if (AttSpeedText != null)
            AttSpeedText.text = AttSpeedLevel.ToString();

        if (CriRateText != null)
            CriRateText.text = CriRateLevel.ToString();

        if (CriDmgText != null)
            CriDmgText.text = CriDmgLevel.ToString();
    }

    void ResetBtnClick()
    {
        AttackLevel = GlobalValue.g_Attack;
        AttSpeedLevel = GlobalValue.g_AttSpeed;
        CriRateLevel = GlobalValue.g_CriRate;
        CriDmgLevel = GlobalValue.g_CriDmg;

        RefreshText();
    }

    void SaveBtnClick()
    {
        string a_Mess = "";
        bool a_NeedDelegate = false;

        if (m_BuyGold <= 0)
            a_Mess = "올리고 싶은 스탯을 (+)버튼을 통해 올리세요.";
        if (m_UserGold < 0)
            a_Mess = "보유 금액이 부족합니다.";
        if(m_BuyGold > 0)
        {
            a_NeedDelegate = true;
            a_Mess = m_BuyGold.ToString() + "골드를 사용하여 스탯을 올리시겠습니까?";
        }

        GameObject a_DlgRsc = Resources.Load("DialogBox") as GameObject;
        GameObject a_DlgBoxObj = (GameObject)Instantiate(a_DlgRsc);
        GameObject a_Canvas = GameObject.Find("Canvas");
        a_DlgBoxObj.transform.SetParent(a_Canvas.transform, false);
        DialogBox_Ctrl a_DlgBox = a_DlgBoxObj.GetComponent<DialogBox_Ctrl>();
        if (a_DlgBox != null)
        {
            if (a_NeedDelegate == true)
                a_DlgBox.InitMessage(a_Mess, TryBuyItem);
            else
                a_DlgBox.InitMessage(a_Mess);
        }
    }

    void TryBuyItem()
    {
        Debug.Log("aaa");
        m_BuyGold = 0;

        //서버에 저장
        PlayerPrefs.SetInt("AttackLv", AttackLevel);
        PlayerPrefs.SetInt("AttSpeedLv", AttSpeedLevel);
        PlayerPrefs.SetInt("CriRateLv", CriRateLevel);
        PlayerPrefs.SetInt("CriDmgLv", CriDmgLevel);
        PlayerPrefs.SetInt("UserGold", m_UserGold);
        //서버에 저장

        //저장 데이터 불러오기
        GlobalValue.LoadGameData();

        //변수 저장및 텍스트 초기화
        ResetBtnClick();
        StoreMgr.Inst.m_GoldText.text = m_UserGold.ToString();
    }

}
