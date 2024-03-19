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

    //--- ���� �� �����Ϸ��� �õ��� ����? ������ ���� ���� ����
    SkillType m_BuySkType;  //� ��ų �������� �����Ϸ��� �� ����?
    int m_SvMyGold = 0;    //���� ���μ����� ���� �� ���� ����� : ������ �� ��尡 ������?
    int m_SvMyLevel = 0;    //��ų ���� ���� ����� ����...
                            //--- ���� �� �����Ϸ��� �õ��� ����? ������ ���� ���� ����

    //--- �������� ����� ���� ����
    int m_BackupMyGold = 0;     //Backup ��
    int[] m_SvSkCount = new int[6];     //Backup ��
    float m_BuyDelayTime = 0.0f;
    bool isNetworkLock = false;
    //--- �������� ����� ���� ����

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

        //--- �ʱ� ����
        m_StatsBtn.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        m_SkillBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_CharBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- �ʱ� ����

        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbyScene");
            });

        //--- ��ưŬ��
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
                MessageOnOff("", false);    //�޽��� ����
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

        //--- ���� ����
        m_StatsBtn.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        m_SkillBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_CharBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        //--- ���� ����

        //--- ������Ʈ ����
        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- ������Ʈ ����
    }

    void SkillBtnClick()
    {
        if (isSkill == true)
            return;

        isSkill = !isSkill;
        isStats = false;
        isChar = false;

        //--- ���� ����
        m_StatsBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_SkillBtn.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        m_CharBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        //--- ���� ����

        //--- ������Ʈ ����
        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- ������Ʈ ����
    }

    void CharBtnClick()
    {
        if (isChar == true)
            return;

        isChar = !isChar;
        isStats = false;
        isSkill = false;

        //--- ���� ����
        m_StatsBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_SkillBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        m_CharBtn.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        //--- ���� ����

        //--- ������Ʈ ����
        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- ������Ʈ ����
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
            a_Mess = "��ų�� ������ �ִ��Դϴ�.";
        }
        else if (GlobalValue.g_UserGold < a_Price)
        {
            a_Mess = "���� �ݾ��� �����մϴ�.";
        }
        else
        {
            a_Mess = "�Ҹ� ��� " + a_Price.ToString() + "\n���� �����Ͻðڽ��ϱ�?";
            a_NeedDelegate = true;  //<-- �� ������ �� ����
        }

        //--- Backup �޾� ����(���н� ���� ���� ���� �뵵)
        for (int ii = 0; ii < GlobalValue.g_SkLevelList.Count; ii++)
            m_SvSkCount[ii] = GlobalValue.g_SkLevelList[ii];
        m_BackupMyGold = GlobalValue.g_UserGold;
        //--- Backup �޾� ����(���н� ���� ���� ���� �뵵)

        m_BuySkType = a_SkType;
        m_SvMyGold = GlobalValue.g_UserGold;
        m_SvMyGold -= a_Price;
        m_SvMyLevel = GlobalValue.g_SkLevelList[(int)a_SkType];
        m_SvMyLevel++;  //��ų ������ ���� ����� ����

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
