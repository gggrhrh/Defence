using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillPos_Ctrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;
    public Image m_SkPointer = null;
    public GameObject[] SkillPrefab;    //스킬 프리팹

    //스킬의 범위및 위치설정 변수
    float m_SkillRange = 0.0f;
    float m_UpRange = 0.0f;
    Vector3 skillPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.3f;
        //범위 증가값 = 스킬레벨 * 레벨당 범위증가 
        m_UpRange = GlobalValue.g_SkLevelList[(int)m_SkType] *
            GlobalValue.g_SkDataList[(int)m_SkType].m_UpRange;

        m_SkillRange += m_UpRange;

        if (m_SkPointer != null)
        {
            m_SkPointer.rectTransform.localScale = new Vector3(m_SkillRange, m_SkillRange, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);

        if (MousePos.x < 0.0f)
            MousePos.x = 0.0f;
        if (MousePos.x > Screen.width)
            MousePos.x = Screen.width;
        if (MousePos.y < 0.0f)
            MousePos.y = 0.0f;
        if (MousePos.y > Screen.height)
            MousePos.y = Screen.height;

        m_SkPointer.transform.position = MousePos;

        if (Input.GetMouseButtonDown(0) == true)
        {
            skillPos = MousePos;
            UseSkill();
            Time.timeScale = 1.0f;
            Destroy(gameObject);
        }
        else if (Input.GetMouseButtonDown(1) == true)
        {
            Time.timeScale = 1.0f;
            Destroy(gameObject);
        }
    }

    void UseSkill()
    {
        //---쿨타임 갱신
        SkillNode_Ctrl[] a_SkNode = FindObjectsOfType<SkillNode_Ctrl>(true);
        for (int i = 0; i < a_SkNode.Length; i++)
        {
            if (a_SkNode[i].m_SkType == m_SkType)
            {
                a_SkNode[i].Skill_Time = 0.0f;
                break;
            }
        }//for(int i = 0; i < a_SkNode.Length; i++)
        //---쿨타임 갱신

        //--스킬 프리팹 스폰 위치 밑 범위
        Vector3 pos = Camera.main.ScreenToWorldPoint(skillPos);
        pos.z = 0.0f;
        GameObject SkObj = Instantiate(SkillPrefab[(int)m_SkType]) as GameObject;
        SkObj.transform.position = pos;
        Skill_Ctrl SkCtrl = SkObj.GetComponent<Skill_Ctrl>();
        SkCtrl.InitSkState(m_SkType, m_SkillRange);
    }

    public void InitSkill(SkillType a_SkType, float a_Range)
    {
        m_SkType = a_SkType;
        m_SkillRange = a_Range;
    }
}
