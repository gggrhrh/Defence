using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointer_Ctrl : MonoBehaviour
{
    public Image m_SkPointer = null;
    [HideInInspector] public SkillType m_SkType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);

        if (MousePos.x < 0.0f)
            MousePos.x = 0.0f;
        if (MousePos.x > Screen.width)
            MousePos.x = Screen.width;
        if (MousePos.y < 0.0f)
            MousePos.y = 0.0f;
        if (MousePos.y > Screen.height)
            MousePos.y = Screen.height;

        m_SkPointer.transform.position = MousePos;
    }
}
