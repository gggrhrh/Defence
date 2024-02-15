using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterType
{ 
    Round1,
    Round2,
    Round3,
    Boss1,
    Boss2,
    Boss3
}

public class Monster_Ctrl : MonoBehaviour
{
    public MonsterType m_MonType = MonsterType.Round1;
    public Image m_HpBar;

    int m_Round = 0;
    int m_Gold = 0; //몬스터 타입마다 주는 골드 바뀜
    float m_CurHp = 0.0f;
    float m_MaxHp = 0.0f;

    float m_MoveSpeed = 2.0f;
    Vector3 m_Pos;
    Vector3 m_Dir = Vector3.down;

    // Start is called before the first frame update
    void Start()
    {
        MonsterState();
    }

    // Update is called once per frame
    void Update()
    {
        m_Pos = transform.position;

        if (1.75f < m_Pos.x && m_Pos.y <= 4.0f)
        {
            m_Pos.x = 1.75f;
            transform.position = m_Pos;
            m_Dir = Vector3.down;
        }
        else if (m_Pos.y < -4.0f && m_Pos.x <= 1.75f)
        {
            m_Pos.y = -4.0f;
            transform.position = m_Pos;
            m_Dir = Vector3.left;
        }
        else if (m_Pos.x < -6.75f && -4.0f <= m_Pos.y)
        {
            m_Pos.x = -6.75f;
            transform.position = m_Pos;
            m_Dir = Vector3.up;
        }
        else if (4.0f < m_Pos.y && -6.75f <= m_Pos.x)
        {
            m_Pos.y = 4.0f;
            transform.position = m_Pos;
            m_Dir = Vector3.right;
        }

        transform.position += m_Dir * m_MoveSpeed * Time.deltaTime;

    }//void Update()

    void MonsterState()
    {
        if(m_MonType == MonsterType.Round1)
        {
            m_MaxHp = 100.0f + 10.0f * (m_Round-1);
            m_CurHp = m_MaxHp;
            m_Gold = 10;
        }
        else if(m_MonType == MonsterType.Round2)
        {
            m_MaxHp = 200.0f + 10.0f * (m_Round - 11);
            m_CurHp = m_MaxHp;
            m_Gold = 15;
        }
        else if(m_MonType == MonsterType.Round3)
        {
            m_MaxHp = 300.0f + 10.0f * (m_Round - 21);
            m_CurHp = m_MaxHp;
            m_Gold = 20;
        }
        else if(m_MonType == MonsterType.Boss1)
        {
            m_MaxHp = 1000.0f;
            m_CurHp = m_MaxHp;
            m_Gold = 300;
        }
        else if(m_MonType == MonsterType.Boss2)
        {
            m_MaxHp = 2000.0f;
            m_CurHp = m_MaxHp;
            m_Gold = 500;
        }
        else if(m_MonType == MonsterType.Boss3)
        {
            m_MaxHp = 3000.0f;
            m_CurHp = m_MaxHp;
            m_Gold = 700;
        }    
    }//void MonsterState()

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Bullet")
        {
            Bullet_Ctrl a_BCtrl = coll.GetComponent<Bullet_Ctrl>();
            float a_Attack = a_BCtrl.m_Attack;
            TakeDamage(a_Attack);
            coll.gameObject.SetActive(false);
        }
    }

    void TakeDamage(float a_Value)
    {
        if (m_CurHp <= 0.0f)    //이 몬스터가 이미 죽어 있으면
            return;             

        float a_CacDmg = a_Value;
        if (m_CurHp < a_Value)  //남은 체력보다 데미지가 더 높으면
            a_CacDmg = m_CurHp; // 남은 체력만큼 데미지를 줌

        //피격데미지
        Game_Mgr.Inst.DamageText(-a_CacDmg, transform.position, Color.red);

        m_CurHp -= a_Value;
        if (m_CurHp < 0.0f)
            m_CurHp = 0.0f;

        if (m_HpBar != null)
            m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        if (m_CurHp <= 0.0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (m_CurHp < 0)
            return;
        //보상주기
        Game_Mgr.Inst.AddGold(m_Gold);
        Game_Mgr.Inst.m_MonCount--;
        Destroy(gameObject);
    }

    public void InitState(int a_Round)
    {
        if (a_Round <= 0)
            return;

        m_Round = a_Round;

        if (m_Round < 10)
            m_MonType = MonsterType.Round1;
        else if (m_Round == 10)
            m_MonType = MonsterType.Boss1;
        else if (10 < m_Round && m_Round < 20)
            m_MonType = MonsterType.Round2;
        else if (m_Round == 20)
            m_MonType = MonsterType.Boss2;
        else if (20 < m_Round && m_Round < 30)
            m_MonType = MonsterType.Round3;
        else if (m_Round == 30)
            m_MonType = MonsterType.Boss3;
    }

}//public class Monster_Ctrl : MonoBehaviour
