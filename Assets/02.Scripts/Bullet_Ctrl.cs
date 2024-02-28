using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ctrl : MonoBehaviour
{
    float m_MoveSpeed = 10.0f;  //이동속도
    float m_LifeTime = 1.0f;
    
    //--- 적을 찾는 변수
    GameObject Target_Obj = null;   //타겟 참조 변수
    Vector3 m_DesiredDir;           //타겟을 향하는 방향 변수
    bool m_FindEnemy = false;       //타겟이 죽었는지 확인하는 함수
    //--- 적을 찾는 변수

    //--- 총알의 공격력
    [HideInInspector] public float m_Attack = 0.0f;

    void OnEnable() //Active가 활성화 될 때마다 호출되는 함수
    {
        m_LifeTime = 1.0f;

        FindEnemy();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(0.0f < m_LifeTime)
        {
            m_LifeTime -= Time.deltaTime;
            if (m_LifeTime <= 0.0f)
            {
                gameObject.SetActive(false);
                m_FindEnemy = false;
            }
        }

        if (Target_Obj != null)
            BulletMove();

        else
        {
            transform.Translate(m_DesiredDir * m_MoveSpeed * Time.deltaTime, Space.World);
            gameObject.SetActive(false);
        }
    }

    void FindEnemy()
    {
        GameObject[] a_EnemyList = GameObject.FindGameObjectsWithTag("Monster");

        if (a_EnemyList.Length <= 0) //등장해 있는 몬스터가 하나도 없으면...
            return;     //추적할 대상을 찾지 못한다.

        Target_Obj = a_EnemyList[0];

        if (Target_Obj != null) //추적한 대상을 찾음
            m_FindEnemy = true;
    }

    void BulletMove()
    {
        m_DesiredDir = Target_Obj.transform.position - transform.position;
        m_DesiredDir.z = 0.0f;
        m_DesiredDir.Normalize();

        //유도탄이 바라보는 방향쪽으로 움직이게 하기...
        transform.Translate(m_DesiredDir * m_MoveSpeed * Time.deltaTime, Space.World);
    }

    public void InitBullet(float a_Attack)
    {
        m_Attack = a_Attack;
    }
}
