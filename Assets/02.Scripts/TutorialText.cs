using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText
{
    public Dictionary<string, string> m_Tutorial = new Dictionary<string, string>();
    public void SetText()
    {
        m_Tutorial.Add("Spawn", "�켱 ���� ĳ���͸� ��ȯ�� �����?");
        m_Tutorial.Add("Spawn", "��ȯ�� �� ��ư���� ��ȯ�� �����մϴ�.");
        m_Tutorial.Add("Spawn", "���� ĳ���͸� ��ȯ�Ҷ� 100��徿 �Ҹ�˴ϴ�.");
    }
}
