using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    Dictionary<int, string[]> m_Tutorial; //Ʃ�丮�� ������ ��ȭ�迭

    void Awake()
    {
        m_Tutorial = new Dictionary<int, string[]>();
        GenerateDate();
    }

    void GenerateDate()
    {
        m_Tutorial.Add(0, new string[] { "�켱 ���� ĳ���͸� ��ȯ�� �����?",
                        "��ȯ�� �ϱ� ���ؼ��� ���� �Ʒ� �� ��ư�� �������մϴ�.",
                        "��ȯ����� 100��忡��. �� 5�� ��ȯ�غ��ô�."});

        m_Tutorial.Add(1, new string[] { "�� �ϼ̾��~.", "�̹����� ���� ĳ���͵��� �����غ��ſ���.",
                        "������ ����ĳ���͸� �巡���Ͽ� �ٸ� ����ĳ���Ϳ� �����ϸ� �˴ϴ�.",
                        "�ѹ� �غ����?"});

        m_Tutorial.Add(2, new string[] { "������ �������� �⺻����, ������, ������ �̷��� 3������ �ֽ��ϴ�. ���� ������ �������� �����ϴ°� ����������!!",
                        "������ ��ư���� ������ �˷��ٰԿ�. ȭ���� Ŭ���ϸ� �۾��� ������� �� �а� ȭ���� Ŭ�����ּ���."});

        m_Tutorial.Add(3, new string[] { "������ �⺻���� ������ �������. ���� ������ �����غ����?" });

    }

    public string GetText(int Tutorial, int TextIndex)
    {
        if (TextIndex == m_Tutorial[Tutorial].Length)
            return null;

        return m_Tutorial[Tutorial][TextIndex];
    }
}
