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

    [Header("------ Skill Product ------")]
    public GameObject SKill_Content = null;
    public GameObject SkProduct = null;

    //--- 지금 뭘 구입하려고 시도한 건지? 저장해 놓기 위한 변수
    SkillType m_BuySkType;  //어떤 스킬 아이템을 구입하려고 한 건지?
    int m_SvMyGold = 0;    //구입 프로세스에 진입 후 상태 저장용 : 차감된 내 골드가 얼마인지?
    int m_SvMyLevel = 0;    //스킬 레벨 증가 백업해 놓기...
                            //--- 지금 뭘 구입하려고 시도한 건지? 저장해 놓기 위한 변수

    //--- 서버와의 통신을 위한 변수
    int m_BackupMyGold = 0;     //Backup 용
    int[] m_SvSkCount = new int[6];     //Backup 용
    float m_BuyDelayTime = 0.0f;
    bool isNetworkLock = false;
    //--- 서버와의 통신을 위한 변수

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

        //--- Backup 받아 놓기(실패시 돌려 놓기 위한 용도)
        for (int ii = 0; ii < GlobalValue.g_SkLevelList.Count; ii++)
            m_SvSkCount[ii] = GlobalValue.g_SkLevelList[ii];
        m_BackupMyGold = GlobalValue.g_UserGold;
        //--- Backup 받아 놓기(실패시 돌려 놓기 위한 용도)

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

    }

    public void MessageOnOff(string Mess = "", bool isOn = true)
    {
        if (isOn == true)
        {
            MessageText.text = Mess;
            MessageText.gameObject.SetActive(true);
            ShowMsTimer = 7.0f;
        }
        else
        {
            MessageText.text = "";
            MessageText.gameObject.SetActive(false);
        }
    }//void MessageOnOff(string Mess = "", bool isOn = true)

}
