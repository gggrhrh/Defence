using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ctrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;

    Monster_Ctrl[] m_FindMon = null;
    List<int> FMonIndex = new List<int>();

    float m_Attack = 0.0f;      //��ų ���ݷ�
    float SkAnimTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        FindMonster();

        if (m_SkType == SkillType.Skill_0)
            AttackComUpdate();
        else if (m_SkType == SkillType.Skill_1)
            ComOnOffUpdate();

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
            FMonIndex.Add(i);   //������ ��ȣ�� ����Ʈ�� �߰�
        }
    }

    void CADUpdate()    //�۾������� ��ų
    {

    }

    void  AttackComUpdate() //��ü�����⽺ų
    {

    }

    void ComOnOffUpdate()   //��ǻ�� OnOff �ѱ⽺ų
    {

    }

    public void InitSkState(SkillType a_SkType, float a_Attack)
    {
        m_SkType = a_SkType;

        m_Attack = a_Attack;
    }
}
