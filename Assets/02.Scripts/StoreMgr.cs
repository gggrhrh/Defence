
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreMgr : MonoBehaviour
{
    public Button m_ExitBtn = null;

    [Header("------ ButtonClick ------")]
    public Button m_StatsBtn = null;
    public Button m_SkillBtn = null;
    public Button m_CharBtn = null;
    bool isStats = true;
    bool isSkill = false;
    bool isChar = false;

    [Header("------ GameObject ------")]
    public GameObject m_StatsRoot = null;
    public GameObject m_SkillRoot = null;
    public GameObject m_CharRoot = null;

    [Header("------ GoldText ------")]
    public Text m_GoldText = null;
    [HideInInspector] public int m_UserGold = 0;

    [Header("------ Message ------")]
    public Text MessageText;
    float ShowMsTimer = 0.0f;

    [Header("------ StatsRoot ------")]
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
    //--- 저장용

    [Header("------ Skill Product ------")]
    public GameObject SKill_Content = null;
    public GameObject SkProduct = null;

    SkProduct[] m_SkNodeList;

    //--- 지금 뭘 구입하려고 시도한 건지? 저장해 놓기 위한 변수
    SkillType m_BuySkType;  //어떤 스킬 아이템을 구입하려고 한 건지?
    int m_SvMyGold = 0;    //구입 프로세스에 진입 후 상태 저장용 : 차감된 내 골드가 얼마인지?
    int m_SvMyLevel = 0;    //스킬 레벨 증가 백업해 놓기...
                            //--- 지금 뭘 구입하려고 시도한 건지? 저장해 놓기 위한 변수

    ////--- 서버와의 통신을 위한 변수
    //int m_BackupMyGold = 0;     //Backup 용
    //int[] m_SvSkCount = new int[6];     //Backup 용
    //float m_BuyDelayTime = 0.0f;
    ////--- 서버와의 통신을 위한 변수

    public static StoreMgr Inst;

    void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.LoadGameData();
        m_UserGold = GlobalValue.g_UserGold;

        SkillListUpdate();
        RefreshSkItemList();

        if (m_GoldText != null)
            m_GoldText.text = m_UserGold.ToString();

        //--- 초기 설정
        m_StatsBtn.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        m_SkillBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_CharBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- 초기 설정

        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbyScene");
            });

        //--- 버튼클릭
        if (m_StatsBtn != null)
            m_StatsBtn.onClick.AddListener(StatsBtnClick);

        if (m_SkillBtn != null)
            m_SkillBtn.onClick.AddListener(SkillBtnClick);

        if (m_CharBtn != null)
            m_CharBtn.onClick.AddListener(CharBtnClick);

        //--- 스탯Root
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
        if (0.0f < ShowMsTimer)
        {
            ShowMsTimer -= Time.deltaTime;
            if (ShowMsTimer <= 0.0f)
            {
                MessageOnOff("", false);    //메시지 끄기
            }
        }//if(0.0f < ShowMsTimer)
    }

    void StatsBtnClick()
    {
        if (isStats == true)
            return;

        isStats = !isStats;
        isSkill = false;
        isChar = false;

        //--- 색깔 설정
        m_StatsBtn.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        m_SkillBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_CharBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        //--- 색깔 설정

        //--- 오브젝트 설정
        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- 오브젝트 설정
    }

    void SkillBtnClick()
    {
        if (isSkill == true)
            return;

        isSkill = !isSkill;
        isStats = false;
        isChar = false;

        //--- 스탯 설정 초기화
        ResetBtnClick();
        //--- 스탯 설정 초기화

        //--- 색깔 설정
        m_StatsBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_SkillBtn.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        m_CharBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        //--- 색깔 설정

        //--- 오브젝트 설정
        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- 오브젝트 설정
    }

    void CharBtnClick()
    {
        if (isChar == true)
            return;

        isChar = !isChar;
        isStats = false;
        isSkill = false;

        //--- 스탯 설정 초기화
        ResetBtnClick();
        //--- 스탯 설정 초기화

        //--- 색깔 설정
        m_StatsBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_SkillBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_CharBtn.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        //--- 색깔 설정

        //--- 오브젝트 설정
        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- 오브젝트 설정
    }

    void UpBtnClickMethod(int a_Value, int a_Gold)
    {
        int a_BuyGold = m_BuyGold + a_Gold;
        if (GlobalValue.g_UserGold < a_BuyGold)
        {
            MessageOnOff("보유한 골드가 부족합니다.");
            return;
        }

        if (a_Value == 0)
        {
            if (AttackLevel >= 100)
            {
                MessageOnOff("최대 레벨입니다.");
                return;
            } //만렙 100
            AttackLevel++;
        }
        else if (a_Value == 1)
        {
            if (AttSpeedLevel >= 20)
            {
                MessageOnOff("최대 레벨입니다.");
                return;
            }   //만렙 20
            AttSpeedLevel++;
        }
        else if (a_Value == 2)
        {
            if (CriRateLevel >= 20)
            {
                MessageOnOff("최대 레벨입니다.");
                return;
            }
            CriRateLevel++;
        }
        else if (a_Value == 3)
        {
            if (CriDmgLevel >= 100)
            {
                MessageOnOff("최대 레벨입니다.");
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
            if (AttackLevel <= 0)
            {
                MessageOnOff("최소 레벨입니다.");
                return;
            }

            if (GlobalValue.g_Attack < AttackLevel) m_BuyGold -= a_Gold;
            //기존 레벨에서 올렸다 내리면 골드를 다시 돌려줌
            else
            {
                MessageOnOff("기존 레벨입니다.");
                return;
            }
            //기존 레벨보다 낮아지지 않음

            AttackLevel--;
        }
        else if (a_Value == 1)
        {
            if (AttSpeedLevel <= 0)
            {
                MessageOnOff("최소 레벨입니다.");
                return;
            }

            if (GlobalValue.g_AttSpeed < AttSpeedLevel) m_BuyGold -= a_Gold;
            else
            {
                MessageOnOff("기존 레벨입니다.");
                return;
            }

            AttSpeedLevel--;
        }
        else if (a_Value == 2)
        {
            if (CriRateLevel <= 0)
            {
                MessageOnOff("최소 레벨입니다.");
                return;
            }

            if (GlobalValue.g_CriRate < CriRateLevel) m_BuyGold -= a_Gold;
            else
            {
                MessageOnOff("기존 레벨입니다.");
                return;
            }

            CriRateLevel--;
        }
        else if (a_Value == 3)
        {
            if (CriDmgLevel <= 0)
            {
                MessageOnOff("최소 레벨입니다.");
                return;
            }

            if (GlobalValue.g_CriDmg < CriDmgLevel) m_BuyGold -= a_Gold;
            else
            {
                MessageOnOff("기존 레벨입니다.");
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

        m_GoldText.text = m_UserGold.ToString();
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

        if (m_BuyGold > 0)
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
                a_DlgBox.InitMessage(a_Mess, TryBuyStats);
            else
                a_DlgBox.InitMessage(a_Mess);
        }
    }

    void TryBuyStats()
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

    //--- BuySkill
    void SkillListUpdate()
    {
        GameObject a_SkillObj = null;
        SkProduct a_SkNode = null;
        for (int i = 0; i < GlobalValue.g_SkDataList.Count; i++)
        {
            a_SkillObj = (GameObject)Instantiate(SkProduct);
            a_SkNode = a_SkillObj.GetComponent<SkProduct>();
            a_SkNode.SkProductInit(GlobalValue.g_SkDataList[i].m_SkType);
            a_SkNode.transform.SetParent(SKill_Content.transform, false);
        }
    }

    public void BuySkill(SkillType a_SkType, int a_Price)
    {
        string a_Mess = "";
        bool a_NeedDelegate = false;
        Skill_Info a_SkInfo = GlobalValue.g_SkDataList[(int)a_SkType];

        if (10 <= GlobalValue.g_SkLevelList[(int)a_SkType])
        {
            a_Mess = "스킬의 레벨이 최대입니다.";
        }
        else if (GlobalValue.g_UserGold < a_Price)
        {
            a_Mess = "보유 금액이 부족합니다.";
        }
        else
        {
            a_Mess = "소모 골드 " + a_Price.ToString() + "\n정말 구입하시겠습니까?";
            a_NeedDelegate = true;  //<-- 이 조건일 때 구매
        }

        ////--- Backup 받아 놓기(실패시 돌려 놓기 위한 용도)
        //for (int ii = 0; ii < GlobalValue.g_SkLevelList.Count; ii++)
        //    m_SvSkCount[ii] = GlobalValue.g_SkLevelList[ii];
        //m_BackupMyGold = GlobalValue.g_UserGold;
        ////--- Backup 받아 놓기(실패시 돌려 놓기 위한 용도)

        m_BuySkType = a_SkType;
        m_SvMyGold = GlobalValue.g_UserGold;
        m_SvMyGold -= a_Price;
        m_SvMyLevel = GlobalValue.g_SkLevelList[(int)a_SkType];
        m_SvMyLevel++;  //스킬 보유수 증가 백업해 놓기

        GameObject a_DlgRsc = Resources.Load("DialogBox") as GameObject;
        GameObject a_DlgBoxObj = (GameObject)Instantiate(a_DlgRsc);
        GameObject a_Canvas = GameObject.Find("Canvas");
        a_DlgBoxObj.transform.SetParent(a_Canvas.transform, false);
        DialogBox_Ctrl a_DlgBox = a_DlgBoxObj.GetComponent<DialogBox_Ctrl>();
        if (a_DlgBox != null)
        {
            if (a_NeedDelegate == true)
                a_DlgBox.InitMessage(a_Mess, TryBuySkill);
            else
                a_DlgBox.InitMessage(a_Mess);
        }
    }

    void TryBuySkill()
    {
        if (m_BuySkType < SkillType.Skill_0 || SkillType.SkCount <= m_BuySkType)
            return;

        GlobalValue.g_UserGold = m_SvMyGold;    //골드값 조정
        GlobalValue.g_SkLevelList[(int)m_BuySkType] = m_SvMyLevel; //스킬 보유수 증가 조정

        RefreshSkItemList();

        m_GoldText.text = GlobalValue.g_UserGold.ToString();

        //로컬에 저장
        PlayerPrefs.SetInt("UserGold", GlobalValue.g_UserGold);
        string a_KeyBuff = string.Format("Skill_Item_{0}", (int)m_BuySkType);
        PlayerPrefs.SetInt(a_KeyBuff, GlobalValue.g_SkLevelList[(int)m_BuySkType]);
        //로컬에 저장
    }

    void RefreshSkItemList()
    {
        if (SKill_Content != null)
        {
            if (m_SkNodeList == null || m_SkNodeList.Length <= 0)
                m_SkNodeList = SKill_Content.GetComponentsInChildren<SkProduct>();
        }

        for (int ii = 0; ii < m_SkNodeList.Length; ii++)
        {
            m_SkNodeList[ii].RefreshUI();
        }

    }//void RefreshSkItemList()

    public void MessageOnOff(string Mess = "", bool isOn = true)
    {
        if (isOn == true)
        {
            MessageText.text = Mess;
            MessageText.gameObject.SetActive(true);
            ShowMsTimer = 3.0f;
        }
        else
        {
            MessageText.text = "";
            MessageText.gameObject.SetActive(false);
        }
    }//void MessageOnOff(string Mess = "", bool isOn = true)

}
