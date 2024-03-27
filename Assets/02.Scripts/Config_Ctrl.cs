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


        //슬라이드 상태가 변경 되었을 때 호출되는 함수를 대기하는 코드
        if (m_Sound_Slider != null)
            m_Sound_Slider.onValueChanged.AddListener(SoundChanged);

        if(m_Music_Slider != null)
            m_Music_Slider.onValueChanged.AddListener(MusicChanged);

        if (m_Sound_Slider != null)
            m_Sound_Slider.value = PlayerPrefs.GetFloat("SoundVolume", 1.0f);

        if (m_Music_Slider != null)
            m_Music_Slider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SoundChanged(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        Sound_Mgr.Instance.SoundVolume(value);
    }

    private void MusicChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        Sound_Mgr.Instance.MusicVolume(value);
    }
}
