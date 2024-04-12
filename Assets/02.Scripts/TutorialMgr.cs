using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialMgr : MonoBehaviour
{
    public GameObject TutorialPanel = null;
    public Text m_PanelText = null;
    public TextManager textManager;
    public Button textNextBtn;

    int TextIndex = 0;
    int TutorialIndex = 0;
    bool IsPanel = false;

    Number_Ctrl[] m_NumCount = null;

    [Header("------ SpawnTutorial ------")]
    public GameObject SpawnPanel = null;

    public GameObject BtnExpPanel = null;

    // Start is called before the first frame update
    void Start()
    {
        TextNext(TutorialIndex);

        if (textNextBtn != null)
            textNextBtn.onClick.AddListener(() =>
            {
                TextNext(TutorialIndex);
            });
    }

    // Update is called once per frame
    void Update()
    {
        TutorialPanel.SetActive(IsPanel);

        if (Input.GetKeyDown(KeyCode.Return) && TutorialPanel.activeSelf == true)
        {
            TextNext(TutorialIndex);
        }

        m_NumCount = FindObjectsOfType<Number_Ctrl>();
        if (m_NumCount.Length == 5 && TextIndex == 0 && TutorialIndex == 1)   //5마리 총 소환 시
        {
            TextNext(TutorialIndex);
        }
        for(int i = 0; i < m_NumCount.Length; i++)
        {
            if (m_NumCount[i].m_Level == 1 && TextIndex == 0 && TutorialIndex == 2) //조합하나 성공 시
                TextNext(TutorialIndex);
        }

        if(BtnExpPanel.activeSelf == true && Input.GetMouseButtonDown(0))
        {
            BtnExpPanel.SetActive(false);
            TextNext(TutorialIndex);
        }

    }

    void TextNext(int TutorialIndex)
    {
        if (TutorialIndex > 4)
            return;

        IsPanel = true;

        if (TutorialIndex == 0 && TextIndex == 1)
            SpawnPanel.SetActive(true);
        else
            SpawnPanel.SetActive(false);

        Text(TutorialIndex);
    }

    void Text(int Tutorial)
    {   
        string a_Text = textManager.GetText(Tutorial, TextIndex);

        if (a_Text == null)
        { 
            IsPanel = false;
            TextIndex = 0;
            TutorialIndex++;
            if (Tutorial == 2)
                BtnExpPanel.SetActive(true);
            if (Tutorial == 3)
            {
                GlobalValue.g_Tutorial = 1;
                PlayerPrefs.SetInt("Tutorial", GlobalValue.g_Tutorial);
                SceneManager.LoadScene("GameScene");
            }
            return;
        }

        m_PanelText.text = a_Text;
        TextIndex++;
    }

}
