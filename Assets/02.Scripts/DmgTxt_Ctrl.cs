using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DmgTxt_Ctrl : MonoBehaviour
{
    float m_EffTime = 0.0f;     //연출 시간 계산용 변수
    public Text DamageText = null;  //Text UI 접근용 변수

    //속도 = 거리 / 시간
    float m_EffDur = 1.05f;
    float MvVelocity = 0.5f / 1.0f;    //1초에 0.5m 가는 속도
    float ApVelocity = 1.0f / (1.0f - 0.4f);
    //alpha 0.4초부터 1.0초까지 (0.6초동안) : 0.0 -> 1.0 변화하는 속도

    Vector3 m_CurPos;   //위치 계산용 변수
    Color m_Color;    //색깔 계산용 변수

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        m_EffTime += Time.deltaTime * Game_Mgr.Inst.m_GameSpeed;

        if (m_EffTime < m_EffDur)
        {
            m_CurPos = DamageText.transform.position;
            m_CurPos.y += Time.deltaTime * Game_Mgr.Inst.m_GameSpeed * MvVelocity;
            DamageText.transform.position = m_CurPos;
        }

        if (0.4f < m_EffTime)
        {
            m_Color = DamageText.color;
            m_Color.a -= (Time.deltaTime * Game_Mgr.Inst.m_GameSpeed * ApVelocity);
            if (m_Color.a < 0.0f)
                m_Color.a = 0.0f;
            DamageText.color = m_Color;
        }

        if (m_EffDur < m_EffTime)
        {
            Destroy(gameObject);
        }

    }//void Update()

    public void InitDamage(float a_Damage, Color a_Color, bool isCri = false)
    {
        if (DamageText == null)
            DamageText = this.GetComponentInChildren<Text>();

        if (a_Damage <= 0.0f)
        {
            int a_Dmg = (int)Mathf.Abs(a_Damage);   //절대값 함수
            DamageText.text = "- " + a_Dmg;
        }
        else
        {
            DamageText.text = "+ " + (int)a_Damage;
        }

        a_Color.a = 1.0f;
        DamageText.color = a_Color;

        if (isCri == true)  //크리티컬 ON
        {
            DamageText.fontStyle = FontStyle.BoldAndItalic;
            DamageText.fontSize = 36;
            m_EffDur = 1.5f;
            DamageText.AddComponent<Outline>();
            DamageText.GetComponent<Outline>().effectColor = new Color32(255, 255, 0, 255);
            DamageText.GetComponent<Outline>().effectDistance = new Vector2(2, -2);
        }
    }
}
