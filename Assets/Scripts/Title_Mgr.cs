using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_Mgr : MonoBehaviour
{
    public Button m_StartBtn;

    // Start is called before the first frame update
    void Start()
    {
        if(m_StartBtn != null)    
            m_StartBtn.onClick.AddListener(() =>   
            {     
                SceneManager.LoadScene("LobbyScene");
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
