using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel_Ctrl : MonoBehaviour
{
    public Text m_HelpText = null;
    string helpText = "";

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_HelpText != null)
            m_HelpText.text = helpText;
    }

    public string InitHelpText(int Erroridx)
    {
        if (Erroridx == 1)
            helpText = "보유골드가 부족합니다.";
        else if (Erroridx == 2)
            helpText = "모든 칸이 꽉 차 소환할 수 없습니다.";

        return helpText;
    }
}
