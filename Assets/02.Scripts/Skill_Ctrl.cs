using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ctrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;

    Monster_Ctrl[] m_FindMon = null;    //보스포함 몬스터
    List<Monster_Ctrl> m_Monster = new List<Monster_Ctrl>();    //보스제외 몬스터 

    float m_Attack = 0.0f;      //스킬 공격력

    //--- 스킬 이펙트
    private GameObject OnOffEffPrefab;

    private void Awake()
    {
        OnOffEffPrefab = Resources.Load<GameObject>("OnOffEffect");
    }

    // Start is called before the first frame update
    void Start()
    {
        FindMonster();

        if (m_FindMon == null)
            Destroy(gameObject);

        if (m_SkType == SkillType.Skill_0)
            AttackComUpdate();
        else if (m_SkType == SkillType.Skill_1)
            CADUpdate();
        else if (m_SkType == SkillType.Skill_2)
            ComOnOffUpdate();

        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FindMonster()
    {
        m_FindMon = FindObjectsOfType<Monster_Ctrl>();

        for (int i = 0; i < m_FindMon.Length; i++)
        {
            if (m_FindMon[i].m_MonType == MonsterType.Round1 ||
                m_FindMon[i].m_MonType == MonsterType.Round2 ||
                 m_FindMon[i].m_MonType == MonsterType.Round3)
            {
                m_Monster.Add(m_FindMon[i]);
            }
        }
    }

    void AttackComUpdate() //본체때리기스킬
    {
        for (int i = 0; i < m_FindMon.Length; i++)
        {
            m_FindMon[i].TakeDamage(m_Attack);
        }
    }

    void CADUpdate()    //작업관리자 스킬
    {
        for(int i = 0; i < m_Monster.Count; i++)
        {
            Vector3 a_Dist = m_Monster[i].transform.position - transform.position;
            if (a_Dist.magnitude < 1.0f)
            {
                m_Monster[i].TakeDamage(m_Attack);
                break;
            }
        }
    }

    void ComOnOffUpdate()   //컴퓨터 OnOff 켜기스킬
    {
        GameObject EffPrefab = Instantiate(OnOffEffPrefab) as GameObject;
        Destroy(EffPrefab, 1.0f);

        for (int i = 0; i < m_Monster.Count; i++)
        {
            m_Monster[i].TakeDamage(m_Attack);
        }
    }

    public void InitSkState(SkillType a_SkType, float a_Attack)
    {
        m_SkType = a_SkType;

        m_Attack = a_Attack;
    }
}
