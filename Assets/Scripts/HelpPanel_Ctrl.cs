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
       
    }

    public void InitHelpText(string errorstr)
    {
        if (m_HelpText != null)
            m_HelpText.text = errorstr;

        //if (Erroridx == 1)
        //    helpText = "������尡 �����մϴ�.";
        //else if (Erroridx == 2)
        //    helpText = "��� ĭ�� �� �� ��ȯ�� �� �����ϴ�.";
    }
}
