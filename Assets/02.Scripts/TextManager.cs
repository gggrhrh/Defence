using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    Dictionary<int, string[]> m_Tutorial; //튜토리얼 종류와 대화배열

    void Awake()
    {
        m_Tutorial = new Dictionary<int, string[]>();
        GenerateDate();
    }

    void GenerateDate()
    {
        m_Tutorial.Add(0, new string[] { "우선 숫자 캐릭터를 소환해 볼까요?",
                        "소환을 하기 위해서는 이 버튼을 눌러야합니다.",
                        "소환비용은 100골드에요. 총 5번 소환해봅시다."});

        m_Tutorial.Add(1, new string[] { "이번에는 숫자 캐릭터들을 조합해볼거에요",
                        "조합은 숫자캐릭터를 드래그하여 다른 숫자캐릭터와 조합하면 됩니다.",
                        "같은 레벨과 같은 타입의 숫자끼리만 조합이 가능하니까 주의하세요~."});

    }

    public string GetText(int Tutorial, int TextIndex)
    {
        if (TextIndex == m_Tutorial[Tutorial].Length)
            return null;

        return m_Tutorial[Tutorial][TextIndex];
    }
}
