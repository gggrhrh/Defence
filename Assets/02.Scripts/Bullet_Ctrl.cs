using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ctrl : MonoBehaviour
{
    float m_MoveSpeed = 10.0f;  //�̵��ӵ�
    float m_LifeTime = 1.0f;
    SpriteRenderer m_Renderer = null;
    public Sprite[] sprites = null;

    //--- ���� ã�� ����
    GameObject Target_Obj = null;   //Ÿ�� ���� ����
    Vector3 m_DesiredDir;           //Ÿ���� ���ϴ� ���� ����
    bool m_FindEnemy = false;       //Ÿ���� �׾����� Ȯ���ϴ� �Լ�
    [HideInInspector] public bool m_isColl = false;          //Ÿ�ٿ� ó�� �ε�����
    //--- ���� ã�� ����

    //--- �Ѿ��� ���ݷ� �� ũ��Ƽ��ON
    [HideInInspector] public float m_Attack = 0.0f;
    [HideInInspector] public bool m_isCri = false;
    [HideInInspector] public NumType m_NumType = NumType.Beginner;

    //--- ���� Ÿ���� ��������� �����Ѿ��� ���� Ŀ��
    float delta = 1.0f;
    float SizeUpSpeed = 5.0f;
    //--- ���� Ÿ���� ��������� �����Ѿ��� ���� Ŀ��

    void OnEnable() //Active�� Ȱ��ȭ �� ������ ȣ��Ǵ� �Լ�
    {
        m_isColl = false;
        m_LifeTime = 1.0f;
            
        // ���� Ÿ�Ը��� �Ѿ��� Update
        if (m_NumType == NumType.Binary_Num)
            delta = 0.0f;
        else
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        // ���� Ÿ�Ը��� �Ѿ��� Update

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
        //�����ð�
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
        //�����ð�

        if (Target_Obj != null)
            BulletMove();

        else
        {
            transform.Translate(m_DesiredDir * m_MoveSpeed * Time.deltaTime, Space.World);
            gameObject.SetActive(false);
            m_isColl = false;
        }

        //2������ ���������� �ϱ� ������ ���ڰ� ���� Ŀ��
        if (m_NumType == NumType.Binary_Num)
            B_N_Update();
    }

    void FindEnemy()
    {
        GameObject[] a_EnemyList = GameObject.FindGameObjectsWithTag("Monster");

        if (a_EnemyList.Length <= 0) //������ �ִ� ���Ͱ� �ϳ��� ������...
            return;     //������ ����� ã�� ���Ѵ�.

        Target_Obj = a_EnemyList[0];    //���� ���� ��ȯ�� ���ͺ��� ����

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

        if (Target_Obj != null) //������ ����� ã��
            m_FindEnemy = true;
    }

    void BulletMove()
    {
        m_DesiredDir = Target_Obj.transform.position - transform.position;
        m_DesiredDir.z = 0.0f;
        m_DesiredDir.Normalize();

        //����ź�� �ٶ󺸴� ���������� �����̰� �ϱ�...
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
        //�������� ���������� ��
        if (m_NumType == NumType.Binary_Num)
            return;

        //���Ϳ� �浹�ϸ� �ٸ� ���Ϳ� ���ļ� �浹���� �ʰ� ��
        m_isColl = true;
    }
}
