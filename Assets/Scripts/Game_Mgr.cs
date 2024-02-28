using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameRound
{
    ReadyRound,     //�غ����
    MonsterRound,   //�⺻ ���� ����
    BossRound       //��������
}

public class Game_Mgr : MonoBehaviour
{
    [HideInInspector] public GameRound m_GameRound = GameRound.ReadyRound;
    [HideInInspector] public bool isClick = false;

    //--- ���� ��ȯ ����
    [Header("------ Number Spawn ------")]
    public GameObject NumberPrefab;
    public GameObject BNPrefab;
    public GameObject BSPrefab;
    public Button m_SpawnBtn;
    Vector3 m_SpawnPos = Vector3.zero;
    int m_SpawnGold = 100;
    [HideInInspector] public List<int> m_NumPosList = new List<int>();
    //--- ���� ��ȯ ����

    //--- ĳ���� �Ӹ����� ������ ����� ���� ����
    [Header("------ Damage Text ------")]
    public Transform Damage_Canvas = null;
    public GameObject DmgTxtRoot = null;
    GameObject m_DmgClone;  //Damage Text ���纻�� ���� ����
    DmgTxt_Ctrl m_DmgTxt;   //Damage Text ���纻�� �پ� �ִ� DmgTxt_Ctrl ������Ʈ�� ���� ����
    Vector3 m_StCacPos;     //���� ��ġ�� ����� �ֱ� ���� ����
    //--- ĳ���� �Ӹ����� ������ ����� ���� ����

    //--- UI����
    [Header("------ UI ------")]
    public Text RoundText = null;
    public Text TimeText = null;
    public Text NumInfoText = null;
    public Text MonCountText = null;
    public Text GoldText = null;
    int m_MaxMonCount = 50;
    [HideInInspector] public int m_MonCount = 0;
    [HideInInspector] public int m_Gold = 0;
    //--- UI����

    //--- Round & Time ����
    [HideInInspector] public int m_Round = 0;
    int m_MaxRound = 30;
    float m_RoundTime = 30.0f;
    float m_Time = 0;
    //--- Round & Time ����

    //--- Game Over Panel
    [Header("------ Game Over Panel ------")]
    public GameObject GameOverPanel = null;
    public Button Lobby_Btn = null;
    public Button Replay_Btn = null;
    public Text CurGoldText = null;
    public Text CoinText = null;
    //--- Game Over Panel

    //--- Monster Spawn
    [Header("------ Monster Spawn ------")]
    public GameObject MonsterPrefab;

    int m_MonMax = 20;  //�Ѷ���� 20����
    float Dur = 1.0f;
    //--- Monster Spawn

    //--- Help Text Panel
    [Header("------ Help Text Panel ------")]
    public GameObject HelpPanelPrefab;
    public Transform m_Canvas;
    bool IsPanel = false;   //�ߺ� ��ȯ ����
    float PanelTimer = 0.0f;
    //--- Help Text Panel

    //--- Back Panel
    [Header("------ Back Panel ------")]
    public Button m_BackBtn = null;
    public GameObject m_BackPanel = null;
    public Button m_ReplayBtn = null;
    public Button m_GoLobbyBtn = null;
    public Button m_ConfigBtn = null;
    public GameObject m_ConfigPrefab = null;
    public Button m_ExitBtn = null;
    //--- Back Panel

    public static Game_Mgr Inst = null;

    private void Awake()
    {
        Inst = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        //���� �ʱ�ȭ
        Time.timeScale = 1.0f;
        m_GameRound = GameRound.ReadyRound;
        m_RoundTime = 10.0f;
        m_Gold = 5000;
        //���� �ʱ�ȭ

        if (m_SpawnBtn != null)
            m_SpawnBtn.onClick.AddListener(SpawnBtnClick);

        //--- GameOverPanel
        if (Replay_Btn != null)
            Replay_Btn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameScene");
            });

        if (Lobby_Btn != null)
            Lobby_Btn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbyScene");
            });
        //--- GameOverPanel

        //--- BackPanel
        if (m_BackBtn != null)
            m_BackBtn.onClick.AddListener(() =>
            {
                m_BackPanel.SetActive(true);
                Time.timeScale = 0.0f;
            });

        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(ExitBtnClick);

        if (m_ReplayBtn != null)
            m_ReplayBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameScene");
            });

        if (m_GoLobbyBtn != null)
            m_GoLobbyBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbyScene");
            });

        if (m_ConfigBtn != null)
            m_ConfigBtn.onClick.AddListener(ConfigBtnClick);
        //--- BackPanel

        if (GoldText != null)
            GoldText.text = m_Gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        m_RoundTime -= Time.deltaTime;
        if (m_RoundTime <= 0.0f)
        {
            m_Round++;
            RoundUpdate();
            TimerUpdate();
            StartCoroutine(MonsterSpawn());
        }

        RefreshUIUpdate();

        //���Ͱ� 50������ ������ ��������
        if (m_MaxMonCount <= m_MonCount)
            GameDie();

        //PanelTimer �۵�
        if (PanelTimer > 0.0f)
        {
            PanelTimer -= Time.deltaTime;
            if (PanelTimer <= 0.0f)
            {
                PanelTimer = 0.0f;
                IsPanel = false;
            }
        }
        //PanelTimer �۵�

        //sprite Ŭ���� ĳ���� ���� ������� ����
        if (Input.GetMouseButtonDown(0) && IsPointerOverUIObject() == false &&
            isClick == false) //<-- ĳ���� Ŭ���� �ƴҶ�
        {
            NumInfoText.text = "";
        }

    }//void Update()

    public void DamageText(float a_Value, Vector3 a_Pos, Color a_Color)
    {
        if (Damage_Canvas == null || DmgTxtRoot == null)
            return;

        m_DmgClone = (GameObject)Instantiate(DmgTxtRoot);
        m_DmgClone.transform.SetParent(Damage_Canvas);
        m_DmgTxt = m_DmgClone.GetComponent<DmgTxt_Ctrl>();
        if (m_DmgTxt != null)
            m_DmgTxt.InitDamage(a_Value, a_Color);
        m_StCacPos = new Vector3(a_Pos.x, a_Pos.y + 0.5f, 0.0f);
        m_DmgClone.transform.position = m_StCacPos;
    }//public void DamageText(float a_Value, Vector3 a_Pos, Color a_Color)

    void SpawnBtnClick()
    {
        if (m_Gold < m_SpawnGold)   //�����ݾ��� ������ ���
        {
            HelpPanelSpawn("������尡 �����մϴ�.");
            return;
        }

        if (m_NumPosList.Count >= 25)   //ĭ���� ��á�� ��
        {
            HelpPanelSpawn("��� ĭ�� �� �� ��ȯ�� �� �����ϴ�.");
            return;
        }


        int RanIndex = 0;
        while (true)
        {
            RanIndex = Random.Range(0, 25);
            int FIndex = -1;
            FIndex = m_NumPosList.FindIndex(xx => xx == RanIndex);
            if (FIndex < 0)
                break;
            else
                continue;
        }

        int a_Level = 0;
        NumType a_NumType = NumType.Beginner;
        m_NumPosList.Add(RanIndex);
        NumberSpawn(a_NumType, a_Level, RanIndex);

        AddGold(-m_SpawnGold);
    }//void SpawnBtnClick()

    public void NumberSpawn(NumType a_NumType, int a_Level, int a_PosIndex)
    {

        //��ġ ����
        m_SpawnPos.x = -5.0f + (float)(a_PosIndex % 5) * 1.25f;
        m_SpawnPos.y = 2.5f - (a_PosIndex / 5) * 1.25f;
        //��ġ ����

        //Ÿ�Կ� ���� ����ĳ���� ��ȯ
        if (a_NumType == NumType.Number || a_NumType == NumType.Beginner)
        {
            GameObject a_CloneObj = Instantiate(NumberPrefab) as GameObject;
            a_CloneObj.transform.position = m_SpawnPos;
            Number_Ctrl a_MonObj = a_CloneObj.GetComponent<Number_Ctrl>();
            a_MonObj.InitState(a_NumType, a_Level, a_PosIndex);
        }
        else if (a_NumType == NumType.Binary_Num)
        {
            GameObject a_CloneObj = Instantiate(BNPrefab) as GameObject;
            a_CloneObj.transform.position = m_SpawnPos;
            Number_Ctrl a_MonObj = a_CloneObj.GetComponent<Number_Ctrl>();
            a_MonObj.InitState(a_NumType, a_Level, a_PosIndex);
        }
        else if (a_NumType == NumType.Binary_System)
        {
            GameObject a_CloneObj = Instantiate(BSPrefab) as GameObject;
            a_CloneObj.transform.position = m_SpawnPos;
            Number_Ctrl a_MonObj = a_CloneObj.GetComponent<Number_Ctrl>();
            a_MonObj.InitState(a_NumType, a_Level, a_PosIndex);
        }
        //Ÿ�Կ� ���� ����ĳ���� ��ȯ
    }//public void NumberSpawn(NumType a_NumType, int a_Level, int a_PosIndex)

    void RefreshUIUpdate()
    {
        if (RoundText != null)
            RoundText.text = "Round " + m_Round.ToString();
        if (TimeText != null)
            TimeText.text = m_RoundTime.ToString("F2");
        if (MonCountText != null)
            MonCountText.text = m_MonCount.ToString();
    }//void RefreshUIUpdate()

    public void AddGold(int a_Value)
    {
        m_Gold += a_Value;

        if (GoldText != null)
            GoldText.text = m_Gold.ToString();
    }//public void AddGold(int a_Value)

    public void GameDie()
    {
        Time.timeScale = 0.0f;
        GameOverPanel.SetActive(true);
        if (CurGoldText != null)
            CurGoldText.text = "���� ��� : " + m_Gold;
        if (CoinText != null)
            CoinText.text = "���� ���� : " + ((int)m_Gold / 100).ToString();
    }//public void GameDie()

    void TimerUpdate() //���庰 �ð� üũ �� ������Ʈ
    {
        if (m_GameRound == GameRound.ReadyRound)
            m_RoundTime = 10.0f;
        else if (m_GameRound == GameRound.BossRound)
            m_RoundTime = 45.0f;
        else
            m_RoundTime = 30.0f;

    }//void TimerUpdate()

    void RoundUpdate()
    {
        if (m_Round < 0)
            return;

        if (m_Round == 0)
            m_GameRound = GameRound.ReadyRound;
        else if (m_Round == 10 || m_Round == 20 || m_Round == 30)
            m_GameRound = GameRound.BossRound;
        else
            m_GameRound = GameRound.MonsterRound;
    }//void RoundUpdate()

    IEnumerator MonsterSpawn()  //���� ��ȯ �ڷ�ƾ
    {
        if (m_GameRound == GameRound.MonsterRound)
        {
            for (int ii = 0; ii < m_MonMax; ii++)
            {
                Vector3 a_Pos = new Vector3(1.75f, 4.0f, 0.0f);
                GameObject Go = Instantiate(MonsterPrefab) as GameObject;
                Monster_Ctrl MonCtrl = Go.GetComponent<Monster_Ctrl>();
                MonCtrl.InitState(m_Round);
                Go.transform.position = a_Pos;

                m_MonCount++;
                yield return new WaitForSeconds(Dur);
            }
        }

        else if (m_GameRound == GameRound.BossRound)
        {
            Vector3 a_Pos = new Vector3(1.75f, 4.0f, 0.0f);
            GameObject Go = Instantiate(MonsterPrefab) as GameObject;
            Monster_Ctrl MonCtrl = Go.GetComponent<Monster_Ctrl>();
            MonCtrl.InitState(m_Round);
            Go.transform.position = a_Pos;
        }
    }//IEnumerator MonsterSpawn()

    public void HelpPanelSpawn(string errorstr) //�ȳ� �ǳ� ��ȯ
    {
        if (IsPanel == true)
            return;

        IsPanel = true;
        PanelTimer = 3.0f;

        GameObject HelpPanel = Instantiate(HelpPanelPrefab) as GameObject;
        HelpPanel_Ctrl HelpPanel_Ctrl = HelpPanel.GetComponent<HelpPanel_Ctrl>();
        HelpPanel_Ctrl.InitHelpText(errorstr);
        HelpPanel.transform.SetParent(m_Canvas.transform, false);
    }//public void HelpPanelSpawn(int Erroridx)

    void ConfigBtnClick()
    {
        GameObject Go = Instantiate(m_ConfigPrefab) as GameObject;
        Go.transform.position = m_BackPanel.transform.position;
        Go.transform.SetParent(m_BackPanel.transform);
    }

    void ExitBtnClick()
    {
        //ȯ�漳���������� ������ ����
        Config_Ctrl ConObj = GameObject.FindObjectOfType<Config_Ctrl>();
        if (ConObj != null)
            return;

        m_BackPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public static bool IsPointerOverUIObject() //UGUI�� UI���� ���� ��ŷ�Ǵ��� Ȯ���ϴ� �Լ�
    {
        PointerEventData a_EDCurPos = new PointerEventData(EventSystem.current);

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)

			List<RaycastResult> results = new List<RaycastResult>();
			for (int i = 0; i < Input.touchCount; ++i)
			{
				a_EDCurPos.position = Input.GetTouch(i).position;  
				results.Clear();
				EventSystem.current.RaycastAll(a_EDCurPos, results);
                if (0 < results.Count)
                    return true;
			}

			return false;
#else
        a_EDCurPos.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(a_EDCurPos, results);
        return (0 < results.Count);
#endif
    }//public bool IsPointerOverUIObject() 

}//public class Game_Mgr : MonoBehaviour
