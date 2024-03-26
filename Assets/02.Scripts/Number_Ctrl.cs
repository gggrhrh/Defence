using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number_Ctrl : MonoBehaviour
{
    public NumType m_NumType = NumType.Beginner;
    [HideInInspector] public int m_Level = 0;
    [HideInInspector] public int m_NumPos = 0;
    float m_StAttack = 0.0f;    //처음공격력
    float m_CurAttack = 0.0f;   //업글 적용 공격력
    float m_AttackSpeed = 0.0f; //공격 속도
    float m_CriRate = 0.0f;     //크리티컬확률
    float m_CriDmg = 0.0f;      //크리티컬데미지
    public GameObject m_IconImg = null;
    SpriteRenderer m_SpriteIcon = null;
    string NumInfo = "";    //숫자 캐릭터의 정보
    string NumTypestr = ""; //숫자 타입의 한글화

    //--- 총알 발사
    [Header("--- Bullet ---")]
    public GameObject m_BulletPrefab = null;
    public GameObject m_ShootPos = null;
    float m_ShootCool = 0.0f;
    //--- 총알 발사

    //--- 이진수AI 관련 변수
    public GameObject m_BinaryNum = null;
    float angle = 0.0f;
    float radius = 0.5f;    //회전 반경
    float speed = 0.0f;  //회전 속도

    //--- Global Upgrade
    int m_AttUp = 0;
    int m_AttSpeedUp = 0;
    int m_CriRateUp = 0;
    int m_CriDmgUp = 0;

    //드래그 관련 변수
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

        //--- 소환했을때 업그레이드 양만큼 공격력 증가
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
        if (a_Enenmy.Length <= 0)   //몬스터가 존재하지 않으면 총알 발사안함
            return;

        m_ShootCool += Time.deltaTime;

        if (m_AttackSpeed <= m_ShootCool)
        {
            m_ShootCool = 0.0f;
            float a_Cri = Random.Range(0.0f, 1.0f);

            if (a_Cri < m_CriRate)  //치명타 ON
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

    void BinarySystem_AI()  //이진법 공격
    {
        if (m_Level == 0)
            return;

        GameObject[] a_Enenmy = GameObject.FindGameObjectsWithTag("Monster");
        if (a_Enenmy.Length <= 0)//몬스터가 존재하지 않으면 총알 발사안함
            return;

        m_ShootCool += Time.deltaTime;
        if (m_AttackSpeed <= m_ShootCool)
        {
            StartCoroutine(BinarySystemFireCorutine());
            m_ShootCool = 0.0f;
        }

    }

    IEnumerator BinarySystemFireCorutine() //이진법 코루틴
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

    void BinaryNumber_AI()  //이진수 공격
    {
        speed = 360 / m_AttackSpeed;  //공격속도에 맞춰 한바퀴 돌고 총알발사
        angle += Time.deltaTime * speed;
        m_BinaryNum.transform.position = this.transform.position + new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                                        Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0.0f);

        Text a_Text = m_BinaryNum.GetComponentInChildren<Text>();
        a_Text.text = m_Level.ToString();

        GameObject[] a_Enenmy = GameObject.FindGameObjectsWithTag("Monster");
        if (a_Enenmy.Length <= 0)   //몬스터가 존재하지 않으면 총알 발사안함
            return;

        if (m_Level == 0)
            return;

        m_ShootCool += Time.deltaTime;

        if (m_AttackSpeed <= m_ShootCool)
        {
            m_ShootCool = 0.0f;
            float a_Cri = Random.Range(0.0f, 1.0f);
            if (a_Cri < m_CriRate)  //크리티컬 발동
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
        m_NumType = a_NumType;  //숫자Type
        m_Level = a_Level;
        NumberClass a_Num = new NumberClass();
        a_Num.SetType(m_NumType, m_Level);
        m_StAttack = a_Num.m_Attack;    //초기 공격력 값
        m_AttackSpeed = a_Num.m_AttackSpeed;    //업글정렬후 공격력값
        m_SpriteIcon = m_IconImg.GetComponent<SpriteRenderer>();
        m_SpriteIcon.sprite = a_Num.m_IconImg;
    }

    string NumberInfo(NumType a_NumType)
    {
        if (a_NumType == NumType.Beginner)
            NumTypestr = "기초숫자";
        else if (a_NumType == NumType.Number)
            NumTypestr = "기본숫자";
        else if (a_NumType == NumType.Binary_Num)
            NumTypestr = "이진수";
        else if (a_NumType == NumType.Binary_System)
            NumTypestr = "이진법";
        NumInfo = string.Format("숫자 타입 : {0}\n레벨 : {1}\n공격력 : {2}\n공격속도 : {3:0.00}" +
            "\n치명타 확률 : {4:0.00}\n치명타 데미지 : {5:0.00}"
            , NumTypestr, m_Level, m_CurAttack, m_AttackSpeed, m_CriRate, m_CriDmg);

        return NumInfo;
    }

    public void UpgradeRefresh(NumType a_NumType) //업그레이드 버튼을 눌렀을 때 공격력 업글
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
    }//public void UpgradeRefresh(NumType a_NumType) //업그레이드 버튼을 눌렀을 때 공격력 업글

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

    //마우스 드래그 및 포인트
    void OnMouseDown()  //마우스누를때
    {
        m_StartPos = transform.position;

        Game_Mgr.Inst.isClick = true;

        if (Game_Mgr.Inst.NumInfoText != null)
            Game_Mgr.Inst.NumInfoText.text = NumberInfo(m_NumType);
    }

    void OnMouseDrag()  //드래그시 이동
    {
        distance = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = objPos;
    }

    void OnMouseUp()    //마우스 뗄때
    {
        Game_Mgr.Inst.isClick = false;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = MousePoint(objPos);
    }

    Vector3 MousePoint(Vector3 a_Pos)   //마우스 위치에 따른 
    {
        //--- x축 좌표
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
        //--- x축 좌표

        //--- y축 좌표
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
        //--- y축 좌표

        // 마우스를 놓은곳에 캐릭터가 있는지 확인후 조합하는지 리턴하는지 정함
        CacPos = XPos + YPos * 5;   //마우스가 위치한 숫자의 포스인덱스
        int FMIndex = -1;   //이동할 인덱스
        FMIndex = Game_Mgr.Inst.m_NumPosList.FindIndex(xx => xx == CacPos);    //옮길 위치
        int CIndex = -1;    //현재 인덱스
        CIndex = Game_Mgr.Inst.m_NumPosList.FindIndex(xx => xx == m_NumPos); //현재 위치

        if (FMIndex < 0)   //자리가 비어있을때
        {
            m_Pos = a_Pos;

            //리스트 추가 및 삭제
            Game_Mgr.Inst.m_NumPosList.Add(CacPos);         //현재 위치 리스트 추가
            Game_Mgr.Inst.m_NumPosList.RemoveAt(CIndex);    //이전위치 리스트 삭제
            //리스트 추가 및 삭제
            m_NumPos = CacPos;

        }
        else //옮기는 위치에 캐릭터가 있을경우
        {
            if (a_Pos == m_StartPos)
                return m_StartPos;

            //옮기는 위치의 캐릭터 정보 가져오기
            GameObject[] Gos = GameObject.FindGameObjectsWithTag("Player"); //숫자캐릭터들의 게임오브젝트를 가져옴
            Number_Ctrl NumObj;
            int a_FNode = -1;
            for (int ii = 0; ii < Gos.Length; ii++)
            {
                NumObj = Gos[ii].GetComponent<Number_Ctrl>();   //Number_Ctrl 컴포넌트를 찾음
                if (NumObj.m_NumPos == CacPos)
                    a_FNode = ii;
            }
            NumObj = Gos[a_FNode].GetComponent<Number_Ctrl>();  //옮기는 위치 캐릭터의 오브젝트를 찾음
            //옮기는 위치의 캐릭터 정보 가져오기

            if (NumObj.m_Level == this.m_Level &&   //옮기는 위치의 캐릭터와 마우스 클릭중인 캐릭터의
                NumObj.m_NumType == this.m_NumType) //레벨과 타입이 같다면 조합
            {
                CombiNumber(m_Level, CacPos);

                //오브젝트와 리스트 삭제
                Destroy(this.gameObject);
                Destroy(NumObj.gameObject);
                Game_Mgr.Inst.m_NumPosList.RemoveAt(CIndex);
                //오브젝트와 리스트 삭제
            }
            else   //레벨이나 타입이 다르면 위치는 초기위치로 다시 돌아감
                m_Pos = m_StartPos;
        }

        return m_Pos;
    } //Vector3 MousePoint(Vector3 a_Pos)   //마우스 위치에 따른 

    void CombiNumber(int a_Level, int a_PosIndex)  //숫자를 조합하는 함수
    {
        NumType a_NumType = (NumType)Random.Range(1, 4);
        a_Level++;
        Game_Mgr.Inst.NumberSpawn(a_NumType, a_Level, a_PosIndex);  //새로운 프리팹 소환
    }
    //드래그 관련 변수
}
