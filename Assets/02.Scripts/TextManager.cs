using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    Dictionary<string, string[]> m_Tutorial; //Ʃ�丮�� ������ ��ȭ�迭

    void Awake()
    {
        m_Tutorial = new Dictionary<string, string[]>();
        GenerateDate();
    }

    void GenerateDate()
    {
        m_Tutorial.Add("��ȯ�ϱ�", new string[] { "�켱 ���� ĳ���͸� ��ȯ�� �����?",
                        "��ȯ�� �ϱ� ���ؼ��� �� ��ư�� �������մϴ�.",
                        "��ȯ����� 100��忡��. �� 5�� ��ȯ�غ��ô�."});

        m_Tutorial.Add("�����ϱ�", new string[] { "�̹����� ���� ĳ���͵��� �����غ��ſ���",
                        "������ ����ĳ���͸� �巡���Ͽ� �ٸ� ����ĳ���Ϳ� �����ϸ� �˴ϴ�.",
                        "���� ������ ���� Ÿ���� ���ڳ����� ������ �����ϴϱ� �����ϼ���~."});

    }

    public string GetText(string Tutorial, int TextIndex)
    {
        if (TextIndex == m_Tutorial[Tutorial].Length)
            return null;

        return m_Tutorial[Tutorial][TextIndex];
    }
}
