using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode_Ctrl : MonoBehaviour
{
    //--- 스킬 이미지
    [HideInInspector] public SkillType m_SkType;
    public Sprite[] m_IconImg = null;
    float Skill_Time = 0.0f;
    float Skill_Duration = 0.0f;
    public Image Time_Img = null;
    public Image Icon_Img = null;
    public Button Skill_Btn = null;

    //--- 스킬 판넬
    GameObject m_SkillPanel = null;
    GameObject m_Canvas = null;

    // Start is called before the first frame update
    void Start()
    {
        m_SkillPanel = Resources.Load("SkillPanel") as GameObject;
        m_Canvas = GameObject.Find("Canvas");

        if (Skill_Btn != null)
            Skill_Btn.onClick.AddListener(() =>
            {
                SkillPanel(m_SkType);
            });
    }

    // Update is called once per frame
    void Update()
    {
        Skill_Time += Time.deltaTime;

        if (Skill_Time >= Skill_Duration)
            Skill_Time = Skill_Duration;

        Time_Img.fillAmount = Skill_Time / Skill_Duration;

        PointerCheck();
    }

    public void SkillPanel(SkillType a_SkType)
    {
        SkillPointer_Ctrl SkObj = FindObjectOfType<SkillPointer_Ctrl>();
        if (SkObj != null)
            return;

        if (Skill_Time < Skill_Duration)
            return;

        GameObject a_SkPan = Instantiate(m_SkillPanel) as GameObject;
        a_SkPan.transform.position = Vector3.zero;
        a_SkPan.transform.SetParent(m_Canvas.transform, false);
        a_SkPan.GetComponent<SkillPointer_Ctrl>().m_SkType = a_SkType;

        Time.timeScale = 0.3f;
    }

    void PointerCheck()
    {
        SkillPointer_Ctrl SkObj = FindObjectOfType<SkillPointer_Ctrl>();
        if (SkObj == null)
            return;

        if (Skill_Time < Skill_Duration)
            return;

        if (m_SkType != SkObj.m_SkType)
            return;

        if (Input.GetMouseButtonDown(0) == true)
        {
            Skill_Time = 0.0f;
            Destroy(SkObj.gameObject);
            UseSkill(m_SkType);
            Time.timeScale = 1.0f;
        }
        else if (Input.GetMouseButtonDown(1) == true)
        {
            Destroy(SkObj.gameObject);
            Time.timeScale = 1.0f;
        }
    }

    void UseSkill(SkillType a_SkType)
    {

    }

    public void InitState(SkillType a_SkType, float a_SkTime, float a_SkDur)
    {
        m_SkType = a_SkType;
        Icon_Img.sprite = m_IconImg[(int)m_SkType];

        Skill_Time = a_SkTime;
        Skill_Duration = a_SkDur;
    }
}
