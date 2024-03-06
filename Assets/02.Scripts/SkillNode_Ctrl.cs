using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode_Ctrl : MonoBehaviour
{
    //--- 스킬 이미지
    [HideInInspector] public SkillType m_SkType;
    //public Sprite[] m_IconImg = null;
    float Skill_Time = 5.0f;
    float Skill_Duration = 5.0f;
    public Image Time_Img = null;
    public Image Icon_Img = null;
    public Button Skill_Btn = null;

    //--- 스킬 사용
    GameObject UseSkill_Prefab = null;
    public GameObject[] m_SkillPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        if (Skill_Btn != null)
            Skill_Btn.onClick.AddListener(BtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        Skill_Time += Time.deltaTime;

        if (Skill_Time >= Skill_Duration)
            Skill_Time = Skill_Duration;

        Time_Img.fillAmount = Skill_Time / Skill_Duration;
    }

    void BtnClick()
    {
        if (Skill_Time < Skill_Duration)
            return;

        //Skill_On = true;
        UseSkill_Prefab = Instantiate(m_SkillPrefab[(int)m_SkType]) as GameObject;
        Vector3 a_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        a_Pos.z = 0.0f;
        UseSkill_Prefab.transform.position = a_Pos;

        Skill_Time = 0.0f;
    }

    //public void InitState(SkillType a_SkType, float a_Time, float a_During)
    //{
    //    m_SkType = a_SkType;
    //    Icon_Img.sprite = m_IconImg[(int)m_SkType];

    //    Skill_Time = 12.0f;
    //    Skill_Duration = 12.0f;
    //}
}
