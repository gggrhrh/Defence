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

        if(m_GoldText !=null)
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

        if(m_CharBtn != null)
            m_CharBtn.onClick.AddListener(CharBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void MessageOnOff(string a_Mess, bool isOn = true, float a_Time = 3.0f)
    {

    }
}
