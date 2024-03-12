using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode_Ctrl : MonoBehaviour
{
    //--- ��ų ����
    [HideInInspector] public SkillType m_SkType;
    public Sprite[] m_IconImg = null;
    [HideInInspector] public float Skill_Time = 0.0f;
    float Skill_Duration = 0.0f;
    float Skill_Range = 0.0f;
    public Image Time_Img = null;
    public Image Icon_Img = null;
    public Button Skill_Btn = null;

    //--- ��ų �ǳ�
    GameObject m_SkillPanel = null;
    GameObject m_Canvas = null;

    // Start is called before the first frame update
    void Start()
    {
        m_SkillPanel = Resources.Load("SkillPosPanel") as GameObject;
        m_Canvas = GameObject.Find("Canvas");

        if (Skill_Btn != null)
            Skill_Btn.onClick.AddListener(() =>
            {
                SkillBtnClick(m_SkType);
            });
    }

    // Update is called once per frame
    void Update()
    {
        Skill_Time += Time.deltaTime;

        if (Skill_Time >= Skill_Duration)
            Skill_Time = Skill_Duration;

        Time_Img.fillAmount = Skill_Time / Skill_Duration;
    }

    public void SkillBtnClick(SkillType a_SkType)  
    {
        SkillPos_Ctrl SkObj = FindObjectOfType<SkillPos_Ctrl>();
        if (SkObj != null)
            return;

        if (Skill_Time < Skill_Duration)
            return;

        //��ư Ŭ���� ���� �ǳ� ����
        GameObject a_SkPan = Instantiate(m_SkillPanel) as GameObject;
        a_SkPan.transform.position = Vector3.zero;
        a_SkPan.transform.SetParent(m_Canvas.transform, false);
        SkillPos_Ctrl a_SkPos = a_SkPan.GetComponent<SkillPos_Ctrl>();
        a_SkPos.InitSkill(m_SkType, Skill_Range);
    }

    public void InitState(SkillType a_SkType, float a_Range, float a_SkTime, float a_SkDur)
    {
        m_SkType = a_SkType;
        Icon_Img.sprite = m_IconImg[(int)m_SkType];
        Skill_Range = a_Range;

        Skill_Time = a_SkTime;
        Skill_Duration = a_SkDur;
    }
}
