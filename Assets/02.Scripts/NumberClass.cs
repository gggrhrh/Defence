using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NumType
{
    Beginner,       //처음 숫자
    Number,         //기본 숫자
    Binary_Num,     //이진수
    Binary_System   //이진법
}

public class NumberClass
{
    public NumType m_NumType = NumType.Beginner;
    public int m_Level = 0;
    public int m_maxLevel = 5;
    public Sprite m_IconImg = null;
    public float m_Attack = 0.0f;
    public float m_AttackSpeed = 0.0f;
    public float m_CriRate = 0.0f;

    public void SetType(NumType a_NumType, int a_Level)
    {
        m_NumType = a_NumType;
        m_Level = a_Level;
        if (a_NumType == NumType.Beginner)
        {
            m_Attack = 10.0f;
            m_AttackSpeed = 1.2f;
            string Spritestr = string.Format("Number/Beginner/{0}", a_Level);
            m_IconImg = Resources.Load(Spritestr, typeof(Sprite)) as Sprite;
        }
        else if (a_NumType == NumType.Number)
        {
            m_Attack = 15.0f + (15.0f * a_Level - 1) * (1 - 0.065f * a_Level);
            m_AttackSpeed = 1.2f - 0.1f * a_Level;
            string Spritestr = string.Format("Number/Number/{0}", a_Level);
            m_IconImg = Resources.Load(Spritestr, typeof(Sprite)) as Sprite;
        }
        else if (a_NumType == NumType.Binary_Num)
        {
            m_Attack = 25.0f + (15.0f * a_Level - 1) + (5.0f * a_Level - 2);
            m_AttackSpeed = 1.5f;
            m_IconImg = Resources.Load("Number/Binary_Num/1", typeof(Sprite)) as Sprite;
        }
        else if (a_NumType == NumType.Binary_System)
        {
            m_Attack = 5.0f + (5.0f * a_Level - 1) * (1 - 0.05f * a_Level);
            m_AttackSpeed = 1.0f - 0.1f * a_Level;
            string Spritestr = string.Format("Number/Binary_System/{0}", a_Level);
            m_IconImg = Resources.Load(Spritestr, typeof(Sprite)) as Sprite;
        }

    }
}
