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
                        "소환을 하기 위해서는 우측 아래 이 버튼을 눌러야합니다.",
                        "소환비용은 100골드에요. 총 5번 소환해봅시다."});

        m_Tutorial.Add(1, new string[] { "잘 하셨어요~.", "이번에는 숫자 캐릭터들을 조합해볼거에요.",
                        "조합은 숫자캐릭터를 드래그하여 다른 숫자캐릭터와 조합하면 됩니다.",
                        "한번 해볼까요?"});

        m_Tutorial.Add(2, new string[] { "숫자의 종류에는 기본숫자, 이진법, 이진수 이렇게 3종류가 있습니다. 같은 종류와 레벨끼리 조합하는거 잊지마세요!!",
                        "나머지 버튼들의 사용법을 알려줄게요. 화면을 클릭하면 글씨가 사라지니 다 읽고 화면을 클릭해주세요."});

        m_Tutorial.Add(3, new string[] { "게임의 기본적인 설명은 끝났어요. 이제 게임을 시작해볼까요?" });

    }

    public string GetText(int Tutorial, int TextIndex)
    {
        if (TextIndex == m_Tutorial[Tutorial].Length)
            return null;

        return m_Tutorial[Tutorial][TextIndex];
    }
}
