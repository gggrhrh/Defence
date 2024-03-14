using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ctrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;
    public AnimationClip SkillClip;

    Monster_Ctrl[] m_FindMon = null;
    List<int> FMonIndex = new List<int>();

    float m_Range = 0.0f;       //스킬 범위
    float m_Attack = 0.0f;      //스킬 공격력
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
            //리스트 삭제
            if (m_FindMon[FMonIndex[i]].m_CurHp <= 0.0f)
                FMonIndex.Remove(FMonIndex[i]);
        }

        for (int i = 0; i < 10; i++)    //10초동안 지속 데미지
        {
            for (int ii = 0; ii < FMonIndex.Count; ii++)
            {
                if (m_FindMon[FMonIndex[ii]] != null)
                    m_FindMon[FMonIndex[ii]].TakeDamage(m_Attack / 20.0f); //스킬 데미지에 0.05배수 만큼 지속 데미지
            }
            yield return new WaitForSeconds(1.0f);
        }

        Destroy(this.gameObject);
    }

    IEnumerator WaterSkillUpdate()
    {
        //데미지 + 속도감소
        for (int i = 0; i < FMonIndex.Count; i++)
        {
            m_FindMon[FMonIndex[i]].m_MoveSpeed = 1.0f;
            m_FindMon[FMonIndex[i]].TakeDamage(m_Attack);
            //리스트 삭제
            if (m_FindMon[FMonIndex[i]].m_CurHp <= 0.0f)
                FMonIndex.Remove(FMonIndex[i]);
        }

        yield return new WaitForSeconds(5.0f);

        //3초후 속도 정상화
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
