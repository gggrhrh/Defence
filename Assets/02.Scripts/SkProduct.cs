using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkProduct : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;

    public Text m_SkLabel = null;
    public Image m_IconImage = null;
    public Text m_SkExpText = null;
    public Text m_SkInfoText = null;
    public Button m_LevelUpBtn = null;
    public Sprite[] IconSprites;

    public GameObject m_LockPanel = null;
    int m_Price = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (m_LevelUpBtn != null)
            m_LevelUpBtn.onClick.AddListener(() =>
            {
                StoreMgr.Inst.BuySkill(m_SkType, m_Price);
            });
    }

    // Update is called once per frame
    void Update()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        int a_Level = GlobalValue.g_SkLevelList[(int)m_SkType];

        if (a_Level == 0)
            m_LockPanel.SetActive(true);
        else
            m_LockPanel.SetActive(false);

        //스킬의 레벨에 따른 변화값
        m_Price = GlobalValue.g_SkDataList[(int)m_SkType].m_Price +
        GlobalValue.g_SkDataList[(int)m_SkType].m_UpPrice * (GlobalValue.g_SkLevelList[(int)m_SkType] - 1);

        float a_Damage = 0.0f;
        a_Damage = GlobalValue.g_SkDataList[(int)m_SkType].m_Damage +
        GlobalValue.g_SkDataList[(int)m_SkType].m_UpDamage * (GlobalValue.g_SkLevelList[(int)m_SkType] - 1);

        float a_CoolTime = 0.0f;
        a_CoolTime = GlobalValue.g_SkDataList[(int)m_SkType].m_CoolTime;
        //스킬의 레벨에 따른 변화값

        m_LevelUpBtn.GetComponentInChildren<Text>().text = m_Price.ToString();

        if (a_Damage > 1000.0f)
        {
            m_SkInfoText.text = "Level : " + a_Level + "\nCool Time : " + a_CoolTime.ToString("F1") + "s\nDamage : 보스제외 즉사스킬";
        }
        else
        {
            m_SkInfoText.text = "Level : " + a_Level + "\nCool Time : " + a_CoolTime.ToString("F1") + "s\nDamage : "+ a_Damage.ToString();
        }

    }

    public void SkProductInit(SkillType a_SkType)
    {
        m_SkType = a_SkType;

        m_SkLabel.text = GlobalValue.g_SkDataList[(int)a_SkType].m_Name;
        m_IconImage.sprite = IconSprites[(int)a_SkType];

        m_SkExpText.text = GlobalValue.g_SkDataList[(int)a_SkType].m_SkillExp;
    }

}
