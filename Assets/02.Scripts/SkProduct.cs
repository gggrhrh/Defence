using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkProduct : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;

    public Text m_SkLabel = null;
    public Image m_IconImage = null;
    public Text m_SkInfoText = null;
    public Text m_SkUpInfoText = null;
    public Button m_LevelUpBtn = null;

    public GameObject m_LockPanel = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_SkLabel != null)
            m_SkLabel.text = GlobalValue.g_SkDataList[(int)m_SkType].m_Name;

        if (m_LevelUpBtn != null)
            m_LevelUpBtn.onClick.AddListener(LevelUpBtnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RefreshUI()
    {
        int a_Level = GlobalValue.g_SkLevelList[(int)m_SkType];

        if (a_Level == 0)
            m_LockPanel.SetActive(true);
        else
            m_LockPanel.SetActive(false);

        if (m_SkInfoText != null)
            m_SkInfoText.text = "레벨 : " + a_Level.ToString() + "\n공격력 : ";

        if (m_SkUpInfoText != null)
            m_SkUpInfoText.text = "레벨 : " + (a_Level + 1).ToString() + "\n공격력 : ";
    }

    void LevelUpBtnClick()
    {

    }
}
