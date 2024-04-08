using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMgr : MonoBehaviour
{
    public GameObject TutorialPanel = null;
    public Text m_PanelText = null;
    public TextManager textManager;

    int TextIndex = 0;
    bool IsPanel = false;

    [Header("------ SpawnTutorial ------")]
    public GameObject SpawnPanel = null;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Text(string Tutorial)
    {
        string a_Text = textManager.GetText(Tutorial, TextIndex);

        if (a_Text == null)
        { 
            IsPanel = false;
            return;
        }

        m_PanelText.text = a_Text;
    }

}
