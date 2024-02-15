using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ctrl : MonoBehaviour
{
    float m_MoveSpeed = 10.0f;  //�̵��ӵ�
    float m_LifeTime = 1.0f;
    
    //--- ���� ã�� ����
    GameObject Target_Obj = null;   //Ÿ�� ���� ����
    Vector3 m_DesiredDir;           //Ÿ���� ���ϴ� ���� ����
    bool m_FindEnemy = false;       //Ÿ���� �׾����� Ȯ���ϴ� �Լ�
    //--- ���� ã�� ����

    //--- �Ѿ��� ���ݷ�
    [HideInInspector] public float m_Attack = 0.0f;

    void OnEnable() //Active�� Ȱ��ȭ �� ������ ȣ��Ǵ� �Լ�
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

        if (a_EnemyList.Length <= 0) //������ �ִ� ���Ͱ� �ϳ��� ������...
            return;     //������ ����� ã�� ���Ѵ�.

        Target_Obj = a_EnemyList[0];

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

    public void InitBullet(float a_Attack)
    {
        m_Attack = a_Attack;
    }
}
