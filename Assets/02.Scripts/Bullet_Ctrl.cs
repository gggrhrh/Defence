using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ctrl : MonoBehaviour
{
    float m_MoveSpeed = 10.0f;  //이동속도
    float m_LifeTime = 1.0f;
    SpriteRenderer m_Renderer = null;
    public Sprite[] sprites = null;

    //--- 적을 찾는 변수
    GameObject Target_Obj = null;   //타겟 참조 변수
    Vector3 m_DesiredDir;           //타겟을 향하는 방향 변수
    bool m_FindEnemy = false;       //타겟이 죽었는지 확인하는 함수
    [HideInInspector] public bool m_isColl = false;          //타겟에 처음 부딪히면
    //--- 적을 찾는 변수

    //--- 총알의 공격력 및 크리티컬ON
    [HideInInspector] public float m_Attack = 0.0f;
    [HideInInspector] public bool m_isCri = false;
    [HideInInspector] public NumType m_NumType = NumType.Beginner;

    //--- 숫자 타입이 이진수라면 숫자총알이 점점 커짐
    float delta = 1.0f;
    float SizeUpSpeed = 5.0f;
    //--- 숫자 타입이 이진수라면 숫자총알이 점점 커짐

    void OnEnable() //Active가 활성화 될 때마다 호출되는 함수
    {
        m_isColl = false;
        m_LifeTime = 1.0f;
            
        // 숫자 타입마다 총알의 Update
        if (m_NumType == NumType.Binary_Num)
            delta = 0.0f;
        else
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        // 숫자 타입마다 총알의 Update

        FindEnemy();
    }

    void Awake()
    {
        m_Renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

        //Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //생존시간
        if (0.0f < m_LifeTime)
        {
            m_LifeTime -= Time.deltaTime;
            if (m_LifeTime <= 0.0f)
            {
                gameObject.SetActive(false);
                m_FindEnemy = false;
                m_isColl = false;
            }
        }
        //생존시간

        if (Target_Obj != null)
            BulletMove();

        else
        {
            transform.Translate(m_DesiredDir * m_MoveSpeed * Time.deltaTime, Space.World);
            gameObject.SetActive(false);
            m_isColl = false;
        }

        //2진수는 범위공격을 하기 때문에 숫자가 점점 커짐
        if (m_NumType == NumType.Binary_Num)
            B_N_Update();
    }

    void FindEnemy()
    {
        GameObject[] a_EnemyList = GameObject.FindGameObjectsWithTag("Monster");

        if (a_EnemyList.Length <= 0) //등장해 있는 몬스터가 하나도 없으면...
            return;     //추적할 대상을 찾지 못한다.

        Target_Obj = a_EnemyList[0];    //제일 먼저 소환된 몬스터부터 잡음

        for (int i = 0; i < a_EnemyList.Length; i++)
        {
            Monster_Ctrl a_MonCtrl = a_EnemyList[i].GetComponent<Monster_Ctrl>();
            if (a_MonCtrl.m_MonType == MonsterType.Boss1 ||
                a_MonCtrl.m_MonType == MonsterType.Boss2 ||
                a_MonCtrl.m_MonType == MonsterType.Boss3)
            {
                Target_Obj = a_EnemyList[i];
            }
        }

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

    void B_N_Update()
    {      
        delta += Time.deltaTime;
        this.transform.localScale = new Vector3(SizeUpSpeed * delta, SizeUpSpeed * delta, 1.0f);
    }

    public void InitBullet(NumType a_NumType, int a_Level, float a_Attack, bool isCri = false)
    {
        m_NumType = a_NumType;
        m_Renderer.sprite = sprites[a_Level];
        m_Attack = a_Attack;
        m_isCri = isCri;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //이진수면 범위공격을 함
        if (m_NumType == NumType.Binary_Num)
            return;

        //몬스터와 충돌하면 다른 몬스터와 겹쳐서 충돌하지 않게 함
        m_isColl = true;
    }
}
