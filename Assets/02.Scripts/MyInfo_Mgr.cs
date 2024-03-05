using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyInfo_Mgr : MonoBehaviour
{
    public Button m_ExitBtn = null;

    [Header("------ ButtonClick ------")]
    public Button m_StatsBtn = null;
    public Button m_SkillBtn = null;
    public Button m_CharBtn = null;
    bool isStats = true;
    bool isSkill = false;
    bool isChar = false;
    Color32 OnColor = new Color32(240, 80, 255, 255);
    Color32 Offcolor = new Color32(255, 255, 255, 255);

    [Header("------ GameObject ------")]
    public GameObject m_StatsRoot = null;
    public GameObject m_SkillRoot = null;
    public GameObject m_CharRoot = null;

    [Header("------ StatsRoot ------")]
    public Text AttackText = null;
    public Text AttSpeedText = null;
    public Text CriRateText = null;
    public Text CriDmgText = null;
    int AttackLv = 0;
    int AttSpeedLv = 0;
    int CriRateLv = 0;
    int CriDmgLv = 0;

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.LoadGameData();
        StatsTextRefresh();

        //--- 초기 설정
        m_StatsBtn.GetComponent<Image>().color = OnColor;
        m_SkillBtn.GetComponent<Image>().color = Offcolor;
        m_CharBtn.GetComponent<Image>().color = Offcolor;

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
        
    }

    void StatsBtnClick()
    {
        if (isStats == true)
            return;

        isStats = !isStats;
        isSkill = false;
        isChar = false;

        //--- 색깔 설정
        m_StatsBtn.GetComponent<Image>().color = OnColor;
        m_SkillBtn.GetComponent<Image>().color = Offcolor;
        m_CharBtn.GetComponent<Image>().color = Offcolor;
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
        m_StatsBtn.GetComponent<Image>().color = Offcolor;
        m_SkillBtn.GetComponent<Image>().color = OnColor;
        m_CharBtn.GetComponent<Image>().color = Offcolor;
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
        m_StatsBtn.GetComponent<Image>().color = Offcolor;
        m_SkillBtn.GetComponent<Image>().color = Offcolor;
        m_CharBtn.GetComponent<Image>().color = OnColor;
        //--- 색깔 설정

        //--- 오브젝트 설정
        m_StatsRoot.SetActive(isStats);
        m_SkillRoot.SetActive(isSkill);
        m_CharRoot.SetActive(isChar);
        //--- 오브젝트 설정
    }

    void StatsTextRefresh()
    {
        AttackLv = GlobalValue.g_Attack;
        AttSpeedLv = GlobalValue.g_AttSpeed;
        CriRateLv = GlobalValue.g_CriRate;
        CriDmgLv = GlobalValue.g_CriDmg;

        if (AttackText != null)
            AttackText.text = "공 격 력  : " + AttackLv + "Lv ( X " + (1.0f + AttackLv * 0.01f).ToString("F2") + " )";
        if (AttSpeedText != null)
            AttSpeedText.text = "공격속도 : " + AttSpeedLv + "Lv ( - " + (AttSpeedLv * 0.01f).ToString("F2") + "s )";
        if (CriRateText != null)
            CriRateText.text = "크리티컬 확률 : " + CriRateLv + "Lv ( + " + CriRateLv.ToString() + "% )";
        if (CriDmgText != null)
            CriDmgText.text = "크리티컬 데미지 : " + CriDmgLv + "Lv ( X " + (2.0f + CriDmgLv * 0.01f).ToString("F2") + " )";
    }
}
