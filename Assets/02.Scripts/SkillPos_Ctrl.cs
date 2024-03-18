using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillPos_Ctrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;
    public Image m_Mouse = null;
    public GameObject[] SkillPrefab;    //스킬 프리팹
    
    //스킬의 위치설정 변수
    float m_Damage = 0.0f;
    //작업관리자 스킬
    public GameObject CADSel = null;
    Vector3 skillPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.3f;
        //범위 증가값 = 스킬레벨 * 레벨당 범위증가 
        if (m_SkType == SkillType.Skill_1)   //CAD스킬이면
            CADSel.SetActive(true);
        else
            CADSel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 MousePos = new Vector3(Input.mousePosition.x + 50.0f, Input.mousePosition.y + 50.0f, 0.0f);

        if (MousePos.x < 0.0f + 50.0f)
            MousePos.x = 0.0f + 50.0f;
        if (MousePos.x > Screen.width + 50.0f)
            MousePos.x = Screen.width + 50.0f;
        if (MousePos.y < 0.0f + 50.0f)
            MousePos.y = 0.0f + 50.0f;
        if (MousePos.y > Screen.height + 50.0f)
            MousePos.y = Screen.height + 50.0f;


        m_Mouse.transform.position = MousePos;

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
        SkCtrl.InitSkState(m_SkType, m_Damage);
    }

    public void InitSkill(SkillType a_SkType, float a_Damage)
    {
        m_SkType = a_SkType;
        m_Damage = a_Damage;
    }
}
