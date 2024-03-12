using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ctrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;
    public AnimationClip[] SkillClip;
    
    float m_Range = 0.0f;

    float delta = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.5f * m_Range, 0.5f * m_Range, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if(delta > SkillClip[(int)m_SkType].length)
            Destroy(gameObject);

        if (m_SkType == SkillType.Skill_0)
            FireSkillUpdate();
        else if (m_SkType == SkillType.Skill_1)
            WaterSkillUpdate();
    }

    void FireSkillUpdate()
    {

    }

    void WaterSkillUpdate()
    {

    }

    public void InitSkState(SkillType a_SkType, float a_Range)
    {
        m_SkType = a_SkType;
        m_Range = a_Range;
    }
}
