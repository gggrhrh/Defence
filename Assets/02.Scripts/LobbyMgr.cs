using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMgr : MonoBehaviour
{
    public Button m_PlayBtn = null;
    public Button m_ExitBtn = null;

    [Header("------ CfgBox ------")]
    public Button m_ConfigBtn = null;
    public GameObject m_ConfigBox = null;
    public Transform m_Canvas = null;

    [Header("------ Message ------")]
    public Text MessageText = null;
    float ShowMsTimer = 0.0f;

    [Header("------ UserInfo ------")]
    public Text m_NickNameText = null;
    public Text m_GoldText = null;

    [Header("------ UGUI ------")]
    public Button m_StoreBtn = null;
    public Button m_InvenBtn = null;
    public Button m_CollectionBtn = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_PlayBtn != null)
            m_PlayBtn.onClick.AddListener(PlayBtnClick);

        if (m_ConfigBtn != null)
            m_ConfigBtn.onClick.AddListener(CfgBtnClick);

        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(ExitBtnClick);

        //--- 상점 인벤 도감
        if (m_StoreBtn != null)
            m_StoreBtn.onClick.AddListener(StoreBtnClick);

        if (m_InvenBtn != null)
            m_InvenBtn.onClick.AddListener(InvenBtnClick);

        if (m_CollectionBtn != null)
            m_CollectionBtn.onClick.AddListener(CollectBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (0.0f < ShowMsTimer)
        {
            ShowMsTimer -= Time.unscaledDeltaTime;
            if (ShowMsTimer <= 0.0f)
            {
                MessageOnOff("", false);
            }
        }
    }

    void PlayBtnClick()
    {
        if (CfgBoxCheck() == true)
        {
            MessageOnOff("환경설정 박스를 닫고 다시 눌러주세요.");
            return;
        }

        SceneManager.LoadScene("GameScene");
    }

    void CfgBtnClick()
    {
        GameObject CfgObj = Instantiate(m_ConfigBox) as GameObject;
        CfgObj.transform.SetParent(m_Canvas, false);

        Time.timeScale = 0.0f;
    }

    void ExitBtnClick()
    {
        if(CfgBoxCheck() == true)
        {
            MessageOnOff("환경설정 박스를 닫고 다시 눌러주세요.");
            return;
        }

        Application.Quit();
    }

    void StoreBtnClick()
    {
        if (CfgBoxCheck() == true)
        {
            MessageOnOff("환경설정 박스를 닫고 다시 눌러주세요.");
            return;
        }

        SceneManager.LoadScene("StoreScene");
    }

    void InvenBtnClick()
    {
        if (CfgBoxCheck() == true)
        {
            MessageOnOff("환경설정 박스를 닫고 다시 눌러주세요.");
            return;
        }

        SceneManager.LoadScene("InvenScene");
    }

    void CollectBtnClick()
    {
        if (CfgBoxCheck() == true)
        {
            MessageOnOff("환경설정 박스를 닫고 다시 눌러주세요.");
            return;
        }
    }    

    void MessageOnOff(string Mess = "", bool isOn = true, float a_Time = 3.0f)
    {
        if (isOn == true)
        {
            MessageText.text = Mess;
            MessageText.gameObject.SetActive(true);
            ShowMsTimer = a_Time;
        }
        else
        {
            MessageText.text = "";
            MessageText.gameObject.SetActive(false);
        }
    }

    void RefreshText()
    {
        if(m_NickNameText != null)
        {
            m_NickNameText.text = "";
        }
        if(m_GoldText != null)
        {
            m_GoldText.text = "0";
        }
    }

    bool CfgBoxCheck()
    {
        Config_Ctrl CfgObj = GameObject.FindObjectOfType<Config_Ctrl>();
        if (CfgObj != null)
            return true;
        
        else  return false; 
    }
}
