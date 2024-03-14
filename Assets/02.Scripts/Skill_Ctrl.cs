using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ctrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;
    public AnimationClip SkillClip;

    Monster_Ctrl[] m_FindMon = null;
    List<int> FMonIndex = new List<int>();

    float m_Range = 0.0f;       //��ų ����
    float m_Attack = 0.0f;      //��ų ���ݷ�
    float delta = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        m_Attack = 10.0f;

        transform.localScale = new Vector3(0.5f * m_Range, 0.5f * m_Range, 0.0f);

        FindMonster();

        if (m_SkType == SkillType.Skill_0)
            StartCoroutine(FireSkillUpdate());
        else if (m_SkType == SkillType.Skill_1)
            StartCoroutine(WaterSkillUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        //delta += Time.deltaTime;
        //if(delta > SkillClip.length)
        //    this.gameObject.SetActive(false);

    }

    void FindMonster()
    {
        m_FindMon = FindObjectsOfType<Monster_Ctrl>();

        for (int i = 0; i < m_FindMon.Length; i++)
        {
            Vector3 a_Dir = transform.position - m_FindMon[i].transform.position;
            if(Mathf.Abs(a_Dir.magnitude) < m_Range)
            {
                FMonIndex.Add(i);
            }
        }
    }

    IEnumerator FireSkillUpdate()
    {

        for(int i = 0; i < FMonIndex.Count; i++)
        {
            m_FindMon[FMonIndex[i]].TakeDamage(m_Attack);
            //����Ʈ ����
            if (m_FindMon[FMonIndex[i]].m_CurHp <= 0.0f)
                FMonIndex.Remove(FMonIndex[i]);
        }

        for (int i = 0; i < 10; i++)    //10�ʵ��� ���� ������
        {
            for (int ii = 0; ii < FMonIndex.Count; ii++)
            {
                if (m_FindMon[FMonIndex[ii]] != null)
                    m_FindMon[FMonIndex[ii]].TakeDamage(m_Attack / 20.0f); //��ų �������� 0.05��� ��ŭ ���� ������
            }
            yield return new WaitForSeconds(1.0f);
        }

        Destroy(this.gameObject);
    }

    IEnumerator WaterSkillUpdate()
    {
        //������ + �ӵ�����
        for (int i = 0; i < FMonIndex.Count; i++)
        {
            m_FindMon[FMonIndex[i]].m_MoveSpeed = 1.0f;
            m_FindMon[FMonIndex[i]].TakeDamage(m_Attack);
            //����Ʈ ����
            if (m_FindMon[FMonIndex[i]].m_CurHp <= 0.0f)
                FMonIndex.Remove(FMonIndex[i]);
        }

        yield return new WaitForSeconds(5.0f);

        //3���� �ӵ� ����ȭ
        for (int i = 0; i < FMonIndex.Count; i++)
        {
            m_FindMon[FMonIndex[i]].m_MoveSpeed = 2.2f;
        }

        Destroy(this.gameObject);
    }

    public void InitSkState(SkillType a_SkType, float a_Range, float a_Attack)
    {
        m_SkType = a_SkType;
        m_Range = a_Range;
        m_Attack = a_Attack;
    }
}
