using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameRound
{
    ReadyRound,     //준비라운드
    MonsterRound,   //기본 몬스터 라운드
    BossRound       //보스라운드
}

public class Game_Mgr : MonoBehaviour
{
    [HideInInspector] public GameRound m_GameRound = GameRound.ReadyRound;

    //--- 숫자 소환 변수
    [Header("------ Number Spawn ------")]
    public GameObject NumberPrefab;
    public GameObject BNPrefab;
    public GameObject BSPrefab;
    public Button m_SpawnBtn;
    Vector3 m_SpawnPos = Vector3.zero;
    int m_SpawnGold = 100;
    [HideInInspector] public List<int> m_NumPosList = new List<int>();
    //--- 숫자 소환 변수

    //--- 캐릭터 머리위에 데미지 띄우기용 변수 선언
    [Header("------ Damage Text ------")]
    public Transform Damage_Canvas = null;
    public GameObject DmgTxtRoot = null;
    GameObject m_DmgClone;  //Damage Text 복사본을 받을 변수
    DmgTxt_Ctrl m_DmgTxt;   //Damage Text 복사본에 붙어 있는 DmgTxt_Ctrl 컴포넌트를 받을 변수
    Vector3 m_StCacPos;     //시작 위치를 계산해 주기 위한 변수
    //--- 캐릭터 머리위에 데미지 띄우기용 변수 선언

    //--- UI변수
    [Header("------ UI ------")]
    public Text RoundText = null;
    public Text TimeText = null;
    public Text NumInfoText = null;
    public Text MonCountText = null;
    public Text GoldText = null;
    int m_MaxMonCount = 50;
    [HideInInspector] public int m_MonCount = 0;
    [HideInInspector] public int m_Gold = 0;
    //--- UI변수

    //--- Round & Time 변수
    [HideInInspector] public int m_Round = 0;
    int m_MaxRound = 30;
    float m_RoundTime = 30.0f;
    float m_Time = 0;
    //--- Round & Time 변수

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

    int m_MonMax = 20;  //한라운드당 20마리
    float Dur = 1.0f;
    //--- Monster Spawn

    //--- Help Text Panel
    [Header("------ Help Text Panel ------")]
    public GameObject HelpPanelPrefab;
    public Transform m_Canvas;
    bool IsPanel = false;   //중복 소환 방지
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
        //게임 초기화
        Time.timeScale = 1.0f;
        m_GameRound = GameRound.ReadyRound;
        m_RoundTime = 10.0f;
        m_Gold = 5000;
        //게임 초기화

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
                SceneManager.LoadScene("TitleScene");
            });
        //--- GameOverPanel

        //--- BackPanel
        if (m_BackBtn != null)
            m_BackBtn.onClick.AddListener(()=>
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
                SceneManager.LoadScene("TitleScene");
            });

        if (m_ConfigBtn != null)
            m_ConfigBtn.onClick.AddListener(ConfigBtnClick);
        //--- BackPanel
    }

    // Update is called once per frame
    void Update()
    {
        m_RoundTime -= Time.deltaTime;
        if(m_RoundTime <= 0.0f)
        {
            m_Round++;
            RoundUpdate();
            TimerUpdate();
            StartCoroutine(MonsterSpawn());
        }

        RefreshUIUpdate();

        //몬스터가 50마리를 넘으면 게임종료
        if (m_MaxMonCount <= m_MonCount)
            GameDie();

        //PanelTimer 작동
        if(PanelTimer > 0.0f)
        {
            PanelTimer -= Time.deltaTime;
            if (PanelTimer <= 0.0f)
            {
                PanelTimer = 0.0f;
                IsPanel = false;
            }
        }
        //PanelTimer 작동

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
        if (m_Gold < m_SpawnGold)   //보유금액이 부족한 경우
        {
            HelpPanelSpawn(1);
            return;
        }

        if (m_NumPosList.Count >= 25)
        {
            HelpPanelSpawn(2);
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

        //위치 설정
        m_SpawnPos.x = -5.0f + (float)(a_PosIndex % 5) * 1.25f;
        m_SpawnPos.y = 2.5f - (a_PosIndex / 5) * 1.25f;
        //위치 설정

        //타입에 따른 숫자캐릭터 소환
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
        //타입에 따른 숫자캐릭터 소환
    }//public void NumberSpawn(NumType a_NumType, int a_Level, int a_PosIndex)

    void RefreshUIUpdate()
    {
        if (RoundText != null)
            RoundText.text = "Round " + m_Round.ToString();
        if (TimeText != null)
            TimeText.text = m_RoundTime.ToString("F2");
        if (MonCountText != null)
            MonCountText.text = m_MonCount.ToString();
        if (GoldText != null)
            GoldText.text = m_Gold.ToString();
    }//void RefreshUIUpdate()

    public void AddGold(int a_Value)
    {
        m_Gold += a_Value;
    }//public void AddGold(int a_Value)

    public void GameDie()
    {
        Time.timeScale = 0.0f;
        GameOverPanel.SetActive(true);
        if (CurGoldText != null)
            CurGoldText.text = "남은 골드 : " + m_Gold;
        if (CoinText != null)
            CoinText.text = "얻은 코인 : " + ((int)m_Gold / 100).ToString();
    }//public void GameDie()

    void TimerUpdate() //라운드별 시간 체크 및 업데이트
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

    IEnumerator MonsterSpawn()
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

        else if(m_GameRound == GameRound.BossRound)
        {
            Vector3 a_Pos = new Vector3(1.75f, 4.0f, 0.0f);
            GameObject Go = Instantiate(MonsterPrefab) as GameObject;
            Monster_Ctrl MonCtrl = Go.GetComponent<Monster_Ctrl>();
            MonCtrl.InitState(m_Round);
            Go.transform.position = a_Pos;
        }
    }//IEnumerator MonsterSpawn()

    public void HelpPanelSpawn(int Erroridx)
    {
        if (IsPanel == true)
            return;

        IsPanel = true;
        PanelTimer = 3.0f;

        //Erroridx = 1:골드 부족  2:칸수 꽉참 
        GameObject HelpPanel = Instantiate(HelpPanelPrefab) as GameObject;
        HelpPanel_Ctrl HelpPanel_Ctrl = HelpPanel.GetComponent<HelpPanel_Ctrl>();
        HelpPanel_Ctrl.InitHelpText(Erroridx);
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
        Config_Ctrl ConObj = GameObject.FindObjectOfType<Config_Ctrl>();
        if (ConObj != null)
            return;

        m_BackPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

}//public class Game_Mgr : MonoBehaviour
