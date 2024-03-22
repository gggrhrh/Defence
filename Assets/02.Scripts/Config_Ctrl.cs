using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Config_Ctrl : MonoBehaviour
{
    public Button m_Exit_Btn = null;
    public Slider m_Sound_Slider = null;
    public Slider m_Music_Slider = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Exit_Btn != null)
            m_Exit_Btn.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
