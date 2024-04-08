using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("------ SpawnTutorial ------")]
    public GameObject SpawnPanel = null;

    // Start is called before the first frame update
    void Start()
    {
        TextNext(TutorialIndex);

        if (textNextBtn != null)
            textNextBtn.onClick.AddListener(() =>
            {
                TextNext(TutorialIndex);
            });

        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        TutorialPanel.SetActive(IsPanel);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            TextNext(TutorialIndex);
        }
    }

    void TextNext(int TutorialIndex)
    {
        if (TutorialIndex > 1)
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
            return;
        }

        m_PanelText.text = a_Text;
        TextIndex++;
    }

}
