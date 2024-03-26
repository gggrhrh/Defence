using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number_Ctrl : MonoBehaviour
{
    public NumType m_NumType = NumType.Beginner;
    [HideInInspector] public int m_Level = 0;
    [HideInInspector] public int m_NumPos = 0;
    float m_StAttack = 0.0f;    //ó�����ݷ�
    float m_CurAttack = 0.0f;   //���� ���� ���ݷ�
    float m_AttackSpeed = 0.0f; //���� �ӵ�
    float m_CriRate = 0.0f;     //ũ��Ƽ��Ȯ��
    float m_CriDmg = 0.0f;      //ũ��Ƽ�õ�����
    public GameObject m_IconImg = null;
    SpriteRenderer m_SpriteIcon = null;
    string NumInfo = "";    //���� ĳ������ ����
    string NumTypestr = ""; //���� Ÿ���� �ѱ�ȭ

    //--- �Ѿ� �߻�
    [Header("--- Bullet ---")]
    public GameObject m_BulletPrefab = null;
    public GameObject m_ShootPos = null;
    float m_ShootCool = 0.0f;
    //--- �Ѿ� �߻�

    //--- ������AI ���� ����
    public GameObject m_BinaryNum = null;
    float angle = 0.0f;
    float radius = 0.5f;    //ȸ�� �ݰ�
    float speed = 0.0f;  //ȸ�� �ӵ�

    //--- Global Upgrade
    int m_AttUp = 0;
    int m_AttSpeedUp = 0;
    int m_CriRateUp = 0;
    int m_CriDmgUp = 0;

    //�巡�� ���� ����
    Vector3 m_StartPos;
    Vector3 m_Pos;
    float distance;
    int XPos = 0;
    int YPos = 0;
    int CacPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        GlobalUp();

        //--- ��ȯ������ ���׷��̵� �縸ŭ ���ݷ� ����
        if (m_NumType == NumType.Beginner)
            m_CurAttack = m_StAttack;

        UpgradeRefresh(m_NumType);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_NumType == NumType.Beginner || m_NumType == NumType.Number)
            FireUpdate();
        else if (m_NumType == NumType.Binary_Num)
            BinaryNumber_AI();
        else if (m_NumType == NumType.Binary_System)
            BinarySystem_AI();
    }

    void FireUpdate()
    {
        GameObject[] a_Enenmy = GameObject.FindGameObjectsWithTag("Monster");
        if (a_Enenmy.Length <= 0)   //���Ͱ� �������� ������ �Ѿ� �߻����
            return;

        m_ShootCool += Time.deltaTime;

        if (m_AttackSpeed <= m_ShootCool)
        {
            m_ShootCool = 0.0f;
            float a_Cri = Random.Range(0.0f, 1.0f);

            if (a_Cri < m_CriRate)  //ġ��Ÿ ON
            {
                Bullet_Ctrl a_BulletObj = BulletPool_Mgr.Inst.GetBulletPool();
                a_BulletObj.transform.position = m_ShootPos.transform.position;
                a_BulletObj.InitBullet(m_NumType, m_Level, m_CurAttack * m_CriDmg, true);
                a_BulletObj.gameObject.SetActive(true);
            }
            else
            {
                Bullet_Ctrl a_BulletObj = BulletPool_Mgr.Inst.GetBulletPool();
                a_BulletObj.transform.position = m_ShootPos.transform.position;
                a_BulletObj.InitBullet(m_NumType, m_Level, m_CurAttack);
                a_BulletObj.gameObject.SetActive(true);
            }
        }
    }//void FireUpdate()

    void BinarySystem_AI()  //������ ����
    {
        if (m_Level == 0)
            return;

        GameObject[] a_Enenmy = GameObject.FindGameObjectsWithTag("Monster");
        if (a_Enenmy.Length <= 0)//���Ͱ� �������� ������ �Ѿ� �߻����
            return;

        m_ShootCool += Time.deltaTime;
        if (m_AttackSpeed <= m_ShootCool)
        {
            StartCoroutine(BinarySystemFireCorutine());
            m_ShootCool = 0.0f;
        }

    }

    IEnumerator BinarySystemFireCorutine() //������ �ڷ�ƾ
    {
        float a_Cri = Random.Range(0.0f, 1.0f);

        if (a_Cri < m_CriRate)
        {
            for (int ii = 0; ii < m_Level + 1; ii++)
            {
                Bullet_Ctrl a_BulletObj = BulletPool_Mgr.Inst.GetBulletPool();
                a_BulletObj.transform.position = m_ShootPos.transform.position;
                a_BulletObj.InitBullet(m_NumType, m_Level, m_CurAttack * m_CriDmg, true);
                a_BulletObj.gameObject.SetActive(true);

                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            for (int ii = 0; ii < m_Level + 1; ii++)
            {
                Bullet_Ctrl a_BulletObj = BulletPool_Mgr.Inst.GetBulletPool();
                a_BulletObj.transform.position = m_ShootPos.transform.position;
                a_BulletObj.InitBullet(m_NumType, m_Level, m_CurAttack);
                a_BulletObj.gameObject.SetActive(true);

                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    void BinaryNumber_AI()  //������ ����
    {
        speed = 360 / m_AttackSpeed;  //���ݼӵ��� ���� �ѹ��� ���� �Ѿ˹߻�
        angle += Time.deltaTime * speed;
        m_BinaryNum.transform.position = this.transform.position + new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                                        Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0.0f);

        Text a_Text = m_BinaryNum.GetComponentInChildren<Text>();
        a_Text.text = m_Level.ToString();

        GameObject[] a_Enenmy = GameObject.FindGameObjectsWithTag("Monster");
        if (a_Enenmy.Length <= 0)   //���Ͱ� �������� ������ �Ѿ� �߻����
            return;

        if (m_Level == 0)
            return;

        m_ShootCool += Time.deltaTime;

        if (m_AttackSpeed <= m_ShootCool)
        {
            m_ShootCool = 0.0f;
            float a_Cri = Random.Range(0.0f, 1.0f);
            if (a_Cri < m_CriRate)  //ũ��Ƽ�� �ߵ�
            {
                Bullet_Ctrl a_BulletObj = BulletPool_Mgr.Inst.GetBulletPool();
                a_BulletObj.transform.position = m_ShootPos.transform.position;
                a_BulletObj.InitBullet(m_NumType, m_Level, m_CurAttack * m_CriDmg, true);
                a_BulletObj.gameObject.SetActive(true);
            }
            else
            {
                Bullet_Ctrl a_BulletObj = BulletPool_Mgr.Inst.GetBulletPool();
                a_BulletObj.transform.position = m_ShootPos.transform.position;
                a_BulletObj.InitBullet(m_NumType, m_Level, m_CurAttack);
                a_BulletObj.gameObject.SetActive(true);
            }
        }

    }

    public void InitState(NumType a_NumType, int a_Level, int a_NumPos)
    {
        m_NumPos = a_NumPos;    //PosIndex
        m_NumType = a_NumType;  //����Type
        m_Level = a_Level;
        NumberClass a_Num = new NumberClass();
        a_Num.SetType(m_NumType, m_Level);
        m_StAttack = a_Num.m_Attack;    //�ʱ� ���ݷ� ��
        m_AttackSpeed = a_Num.m_AttackSpeed;    //���������� ���ݷ°�
        m_SpriteIcon = m_IconImg.GetComponent<SpriteRenderer>();
        m_SpriteIcon.sprite = a_Num.m_IconImg;
    }

    string NumberInfo(NumType a_NumType)
    {
        if (a_NumType == NumType.Beginner)
            NumTypestr = "���ʼ���";
        else if (a_NumType == NumType.Number)
            NumTypestr = "�⺻����";
        else if (a_NumType == NumType.Binary_Num)
            NumTypestr = "������";
        else if (a_NumType == NumType.Binary_System)
            NumTypestr = "������";
        NumInfo = string.Format("���� Ÿ�� : {0}\n���� : {1}\n���ݷ� : {2}\n���ݼӵ� : {3:0.00}" +
            "\nġ��Ÿ Ȯ�� : {4:0.00}\nġ��Ÿ ������ : {5:0.00}"
            , NumTypestr, m_Level, m_CurAttack, m_AttackSpeed, m_CriRate, m_CriDmg);

        return NumInfo;
    }

    public void UpgradeRefresh(NumType a_NumType) //���׷��̵� ��ư�� ������ �� ���ݷ� ����
    {
        if (m_NumType != a_NumType)
            return;

        else
        {
            if (a_NumType == NumType.Number)
                m_CurAttack = m_StAttack * (1.0f + 0.1f * UpgradeRoot_Ctrl.Num_Lv);
            else if (a_NumType == NumType.Binary_Num)
                m_CurAttack = m_StAttack * (1.0f + 0.1f * UpgradeRoot_Ctrl.B_N_Lv);
            else if (a_NumType == NumType.Binary_System)
                m_CurAttack = m_StAttack * (1.0f + 0.1f * UpgradeRoot_Ctrl.B_S_Lv);
        }
    }//public void UpgradeRefresh(NumType a_NumType) //���׷��̵� ��ư�� ������ �� ���ݷ� ����

    void GlobalUp()
    {
        m_AttUp = GlobalValue.g_Attack;
        m_AttSpeedUp = GlobalValue.g_AttSpeed;
        m_CriRateUp = GlobalValue.g_CriRate;
        m_CriDmgUp = GlobalValue.g_CriDmg;

        m_StAttack = m_StAttack * (1 + 0.01f * m_AttUp);
        m_AttackSpeed = m_AttackSpeed - (0.01f * m_AttSpeedUp);
        m_CriRate = 0.01f * m_CriRateUp;
        m_CriDmg = 2 + (0.01f * m_CriDmgUp);
    }

    //------------------------------------------------------------------------------------------------------------

    //���콺 �巡�� �� ����Ʈ
    void OnMouseDown()  //���콺������
    {
        m_StartPos = transform.position;

        Game_Mgr.Inst.isClick = true;

        if (Game_Mgr.Inst.NumInfoText != null)
            Game_Mgr.Inst.NumInfoText.text = NumberInfo(m_NumType);
    }

    void OnMouseDrag()  //�巡�׽� �̵�
    {
        distance = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = objPos;
    }

    void OnMouseUp()    //���콺 ����
    {
        Game_Mgr.Inst.isClick = false;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = MousePoint(objPos);
    }

    Vector3 MousePoint(Vector3 a_Pos)   //���콺 ��ġ�� ���� 
    {
        //--- x�� ��ǥ
        if (-5.62f < a_Pos.x && a_Pos.x < -4.38f)
        {
            a_Pos.x = -5.0f;
            XPos = 0;
        }
        else if (-4.37f < a_Pos.x && a_Pos.x < -3.13f)
        {
            a_Pos.x = -3.75f;
            XPos = 1;
        }
        else if (-3.12f < a_Pos.x && a_Pos.x < -1.88f)
        {
            a_Pos.x = -2.5f;
            XPos = 2;
        }
        else if (-1.87f < a_Pos.x && a_Pos.x < -0.63f)
        {
            a_Pos.x = -1.25f;
            XPos = 3;
        }
        else if (-0.62f < a_Pos.x && a_Pos.x < 0.62f)
        {
            a_Pos.x = 0.0f;
            XPos = 4;
        }
        else
            a_Pos = m_StartPos;
        //--- x�� ��ǥ

        //--- y�� ��ǥ
        if (-3.12f < a_Pos.y && a_Pos.y < -1.88f)
        {
            a_Pos.y = -2.5f;
            YPos = 4;
        }
        else if (-1.87f < a_Pos.y && a_Pos.y < -0.63f)
        {
            a_Pos.y = -1.25f;
            YPos = 3;
        }
        else if (-0.62f < a_Pos.y && a_Pos.y < 0.62f)
        {
            a_Pos.y = 0.0f;
            YPos = 2;
        }
        else if (0.63f < a_Pos.y && a_Pos.y < 1.87f)
        {
            a_Pos.y = 1.25f;
            YPos = 1;
        }
        else if (1.88f < a_Pos.y && a_Pos.y < 3.12f)
        {
            a_Pos.y = 2.5f;
            YPos = 0;
        }
        else
            a_Pos = m_StartPos;
        //--- y�� ��ǥ

        // ���콺�� �������� ĳ���Ͱ� �ִ��� Ȯ���� �����ϴ��� �����ϴ��� ����
        CacPos = XPos + YPos * 5;   //���콺�� ��ġ�� ������ �����ε���
        int FMIndex = -1;   //�̵��� �ε���
        FMIndex = Game_Mgr.Inst.m_NumPosList.FindIndex(xx => xx == CacPos);    //�ű� ��ġ
        int CIndex = -1;    //���� �ε���
        CIndex = Game_Mgr.Inst.m_NumPosList.FindIndex(xx => xx == m_NumPos); //���� ��ġ

        if (FMIndex < 0)   //�ڸ��� ���������
        {
            m_Pos = a_Pos;

            //����Ʈ �߰� �� ����
            Game_Mgr.Inst.m_NumPosList.Add(CacPos);         //���� ��ġ ����Ʈ �߰�
            Game_Mgr.Inst.m_NumPosList.RemoveAt(CIndex);    //������ġ ����Ʈ ����
            //����Ʈ �߰� �� ����
            m_NumPos = CacPos;

        }
        else //�ű�� ��ġ�� ĳ���Ͱ� �������
        {
            if (a_Pos == m_StartPos)
                return m_StartPos;

            //�ű�� ��ġ�� ĳ���� ���� ��������
            GameObject[] Gos = GameObject.FindGameObjectsWithTag("Player"); //����ĳ���͵��� ���ӿ�����Ʈ�� ������
            Number_Ctrl NumObj;
            int a_FNode = -1;
            for (int ii = 0; ii < Gos.Length; ii++)
            {
                NumObj = Gos[ii].GetComponent<Number_Ctrl>();   //Number_Ctrl ������Ʈ�� ã��
                if (NumObj.m_NumPos == CacPos)
                    a_FNode = ii;
            }
            NumObj = Gos[a_FNode].GetComponent<Number_Ctrl>();  //�ű�� ��ġ ĳ������ ������Ʈ�� ã��
            //�ű�� ��ġ�� ĳ���� ���� ��������

            if (NumObj.m_Level == this.m_Level &&   //�ű�� ��ġ�� ĳ���Ϳ� ���콺 Ŭ������ ĳ������
                NumObj.m_NumType == this.m_NumType) //������ Ÿ���� ���ٸ� ����
            {
                CombiNumber(m_Level, CacPos);

                //������Ʈ�� ����Ʈ ����
                Destroy(this.gameObject);
                Destroy(NumObj.gameObject);
                Game_Mgr.Inst.m_NumPosList.RemoveAt(CIndex);
                //������Ʈ�� ����Ʈ ����
            }
            else   //�����̳� Ÿ���� �ٸ��� ��ġ�� �ʱ���ġ�� �ٽ� ���ư�
                m_Pos = m_StartPos;
        }

        return m_Pos;
    } //Vector3 MousePoint(Vector3 a_Pos)   //���콺 ��ġ�� ���� 

    void CombiNumber(int a_Level, int a_PosIndex)  //���ڸ� �����ϴ� �Լ�
    {
        NumType a_NumType = (NumType)Random.Range(1, 4);
        a_Level++;
        Game_Mgr.Inst.NumberSpawn(a_NumType, a_Level, a_PosIndex);  //���ο� ������ ��ȯ
    }
    //�巡�� ���� ����
}
