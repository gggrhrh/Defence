using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText
{
    public Dictionary<string, string> m_Tutorial = new Dictionary<string, string>();
    public void SetText()
    {
        m_Tutorial.Add("Spawn", "우선 숫자 캐릭터를 소환해 볼까요?");
        m_Tutorial.Add("Spawn", "소환은 이 버튼으로 소환이 가능합니다.");
        m_Tutorial.Add("Spawn", "숫자 캐릭터를 소환할때 100골드씩 소모됩니다.");
    }
}
