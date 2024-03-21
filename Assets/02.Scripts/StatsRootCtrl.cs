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

    //--- 저장용 변수
    int AttackLevel = 0;
    int AttSpeedLevel = 0;
    int CriRateLevel = 0;
    int CriDmgLevel = 0;
    int m_BuyGold = 0;  //총구매에 필요한 금액
    int m_UserGold = 0;
    //--- 저장용

    // Start is called before the first frame update
    void Start()
    {
        LoadStats();
        RefreshText();

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
        int a_BuyGold = m_BuyGold + a_Gold;
        if (GlobalValue.g_UserGold < a_BuyGold)
        {
            StoreMgr.Inst.MessageOnOff("보유한 골드가 부족합니다.");
            return;
        }

        if (a_Value == 0)
        {
            if (AttackLevel >= 100)
            {
                StoreMgr.Inst.MessageOnOff("최대 레벨입니다.");
                return;
            } //만렙 100
            AttackLevel++;
        }
        else if (a_Value == 1)
        {
            if (AttSpeedLevel >= 20)
            {
                StoreMgr.Inst.MessageOnOff("최대 레벨입니다.");
                return;
            }   //만렙 20
            AttSpeedLevel++;
        }
        else if (a_Value == 2)
        {
            if (CriRateLevel >= 20)
            {
                StoreMgr.Inst.MessageOnOff("최대 레벨입니다.");
                return;
            }
            CriRateLevel++;
        }
        else if (a_Value == 3)
        {
            if (CriDmgLevel >= 100)
            {
                StoreMgr.Inst.MessageOnOff("최대 레벨입니다.");
                return;
            }
            CriDmgLevel++;
        }

        m_BuyGold = a_BuyGold;

        m_UserGold = GlobalValue.g_UserGold - m_BuyGold;

        RefreshText();
    }

    void DownBtnClickMethod(int a_Value, int a_Gold)
    {
        if (m_BuyGold < 0)
            return;

        if (a_Value == 0)
        {
            if(AttackLevel <= 0)
            {
                StoreMgr.Inst.MessageOnOff("최소 레벨입니다.");
                return;
            }

            if (GlobalValue.g_Attack < AttackLevel) m_BuyGold -= a_Gold;
            //기존 레벨에서 올렸다 내리면 골드를 다시 돌려줌
            else
            {
                StoreMgr.Inst.MessageOnOff("기존 레벨입니다.");
                return;
            }
            //기존 레벨보다 낮아지지 않음

            AttackLevel--;
        }
        else if (a_Value == 1)
        {
            if (AttSpeedLevel <= 0)
            {
                StoreMgr.Inst.MessageOnOff("최소 레벨입니다.");
                return;
            }

            if (GlobalValue.g_AttSpeed < AttSpeedLevel) m_BuyGold -= a_Gold;
            else
            {
                StoreMgr.Inst.MessageOnOff("기존 레벨입니다.");
                return;
            }
            
            AttSpeedLevel--;
        }
        else if (a_Value == 2)
        {
            if (CriRateLevel <= 0)
            {
                StoreMgr.Inst.MessageOnOff("최소 레벨입니다.");
                return;
            }

            if (GlobalValue.g_CriRate < CriRateLevel) m_BuyGold -= a_Gold;
            else
            {
                StoreMgr.Inst.MessageOnOff("기존 레벨입니다.");
                return;
            }
            
            CriRateLevel--;
        }
        else if (a_Value == 3)
        {
            if (CriDmgLevel <= 0)
            {
                StoreMgr.Inst.MessageOnOff("최소 레벨입니다.");
                return;
            }

            if (GlobalValue.g_CriDmg < CriDmgLevel) m_BuyGold -= a_Gold;
            else
            {
                StoreMgr.Inst.MessageOnOff("기존 레벨입니다.");
                return;
            }
            
            CriDmgLevel--;
        }

        m_UserGold = GlobalValue.g_UserGold - m_BuyGold;

        RefreshText();
    }

    void LoadStats()
    {
        AttackLevel = GlobalValue.g_Attack;
        AttSpeedLevel = GlobalValue.g_AttSpeed;
        CriRateLevel = GlobalValue.g_CriRate;
        CriDmgLevel = GlobalValue.g_CriDmg;

        m_UserGold = GlobalValue.g_UserGold;
    }//void SaveStats()

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

        StoreMgr.Inst.m_GoldText.text = m_UserGold.ToString();
    }

    void ResetBtnClick()
    {
        LoadStats();

        RefreshText();

        //구매골드 0으로 초기화
        m_BuyGold = 0;
    }

    void SaveBtnClick()
    {
        string a_Mess = "";
        bool a_NeedDelegate = false;

        if (m_BuyGold <= 0)
            a_Mess = "스탯을 조정해주세요.";

        if (m_UserGold < 0)
            a_Mess = "보유 금액이 부족합니다.";

        if(m_BuyGold > 0)
        {
            a_NeedDelegate = true;
            a_Mess = "소모 금액 " + m_BuyGold.ToString() + "\n정말 구입하시겠습니까?";
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
        m_BuyGold = 0;

        GlobalValue.g_Attack = AttackLevel;
        GlobalValue.g_AttSpeed = AttSpeedLevel;
        GlobalValue.g_CriRate = CriRateLevel;
        GlobalValue.g_CriDmg = CriDmgLevel;
        GlobalValue.g_UserGold = m_UserGold;

        //서버에 저장
        PlayerPrefs.SetInt("AttackLv", GlobalValue.g_Attack);
        PlayerPrefs.SetInt("AttSpeedLv", GlobalValue.g_AttSpeed);
        PlayerPrefs.SetInt("CriRateLv", GlobalValue.g_CriRate);
        PlayerPrefs.SetInt("CriDmgLv", GlobalValue.g_CriDmg);
        PlayerPrefs.SetInt("UserGold", GlobalValue.g_UserGold);
        //서버에 저장

        //변수 저장및 텍스트 초기화
        m_BuyGold = 0;
        
    }

}
