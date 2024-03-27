using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Sound_Mgr : G_Singleton<Sound_Mgr>
{
    [HideInInspector] public AudioSource m_AudioSrc = null;
    Dictionary<string, AudioClip> m_ADClipList = new Dictionary<string, AudioClip>();

    float m_bgmVolume = 0.2f;
    [HideInInspector] public float m_MusicVolume = 1.0f;    //������� ũ��
    [HideInInspector] public float m_SoundVolume = 1.0f;    //ȿ���� ũ��

    //--- ȿ���� ����ȭ�� ���� ���� ����
    int m_EffSdCount = 5;       //������ 5���� ���̾�� �÷���
    int m_SoundCount = 0;       //�ִ� 5������ ����ǰ� ����(������ ����...)
    List<GameObject> m_SndObjList = new List<GameObject>();
    AudioSource[] m_SndSrcList = new AudioSource[10];
    float[] m_EffVolume = new float[10];

    protected override void Init()  //Awake() �Լ� ��� ���
    {
        base.Init();    //�θ��ʿ� �ִ� Init() �Լ� ȣ��

        LoadChildGameObj();
    }

    // Start is called before the first frame update
    void Start()
    {
        //--- ���� ���ҽ� �̸� �ε�
        AudioClip a_GAudioClip = null;
        object[] temp = Resources.LoadAll("Sounds");
        for (int ii = 0; ii < temp.Length; ii++)
        {
            a_GAudioClip = temp[ii] as AudioClip;

            if (m_ADClipList.ContainsKey(a_GAudioClip.name) == true)
                continue;

            m_ADClipList.Add(a_GAudioClip.name, a_GAudioClip);
        }
        //--- ���� ���ҽ� �̸� �ε�

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadChildGameObj()
    {
        m_AudioSrc = this.gameObject.AddComponent<AudioSource>();

        //---���� ȿ���� �÷��̸� ���� 5���� ���̾� ���� �ڵ�
        for (int ii = 0; ii < m_EffSdCount; ii++)
        {
            GameObject newSoundObj = new GameObject();
            newSoundObj.transform.SetParent(this.transform);
            newSoundObj.transform.localPosition = Vector3.zero;
            AudioSource a_AudioSrc = newSoundObj.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            a_AudioSrc.loop = false;
            newSoundObj.name = "SoundEffObj";

            m_SndSrcList[ii] = a_AudioSrc;
            m_SndObjList.Add(newSoundObj);
        }
        //---���� ȿ���� �÷��̸� ���� 5���� ���̾� ���� �ڵ�

        //--- ������ ���۵Ǹ� ����, ���� ���� ���� �ε� �� ����
        float a_MValue = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        MusicVolume(a_MValue);
        float a_SValue = PlayerPrefs.GetFloat("SoumdVolume", 1.0f);
        SoundVolume(a_SValue);
    }

    public void PlayBGM(string a_FileName, float fVolume = 0.2f)
    {
        AudioClip a_GAudioClip = null;

        if (m_ADClipList.ContainsKey(a_FileName) == true)
        {
            a_GAudioClip = m_ADClipList[a_FileName] as AudioClip;
        }
        else
        {
            a_GAudioClip = Resources.Load("Sounds/" + a_FileName) as AudioClip;
            m_ADClipList.Add(a_FileName, a_GAudioClip);
        }

        if (m_AudioSrc == null)
            return;

        if (m_AudioSrc.clip != null && m_AudioSrc.clip.name == a_FileName)
            return;

        m_AudioSrc.clip = a_GAudioClip;
        m_AudioSrc.volume = fVolume * m_MusicVolume;
        m_bgmVolume = fVolume;
        m_AudioSrc.loop = true;
        m_AudioSrc.Play();
    }//public void PlayBGM(string a_FileName, float fVolume = 0.2f)

    public void PlayGUISound(string a_FileName, float fVolume = 0.2f)
    {
        //if (m_SoundOnOff == false)
        //    return;

        AudioClip a_GAudioClip = null;

        if (m_ADClipList.ContainsKey(a_FileName) == true)
        {
            a_GAudioClip = m_ADClipList[a_FileName] as AudioClip;
        }
        else
        {
            a_GAudioClip = Resources.Load("Sounds/" + a_FileName) as AudioClip;
            m_ADClipList.Add(a_FileName, a_GAudioClip);
        }

        if (m_AudioSrc == null)
            return;

        m_AudioSrc.PlayOneShot(a_GAudioClip, fVolume * m_SoundVolume);
    }

    public void PlayEffSound(string a_FileName, float fVolume = 0.2f)
    {
        //if (m_SoundOnOff == false)
        //    return;

        AudioClip a_GAudioClip = null;

        if (m_ADClipList.ContainsKey(a_FileName) == true)
        {
            a_GAudioClip = m_ADClipList[a_FileName] as AudioClip;
        }
        else
        {
            a_GAudioClip = Resources.Load("Sounds/" + a_FileName) as AudioClip;
            m_ADClipList.Add(a_FileName, a_GAudioClip);
        }

        if (a_GAudioClip == null)
            return;

        if (m_SndSrcList[m_SoundCount] != null)
        {
            m_SndSrcList[m_SoundCount].volume = 1.0f;
            m_SndSrcList[m_SoundCount].PlayOneShot(a_GAudioClip, fVolume * m_SoundVolume);
            m_EffVolume[m_SoundCount] = fVolume;

            m_SoundCount++;
            if (m_EffSdCount <= m_SoundCount)
                m_SoundCount = 0;
        }//if (m_SndSrcList[m_SoundCount] != null)

    }//public void PlayEffSound(string a_FileName, float fVolume = 0.2f)

    public void SoundVolume(float fVolume)
    {
        for (int ii = 0; ii < m_EffSdCount; ii++)
        {
            if (m_SndSrcList[ii] != null)
                m_SndSrcList[ii].volume = 1.0f;
        }

        m_SoundVolume = fVolume;
    }

    public void MusicVolume(float fVolume)
    {
        if (m_AudioSrc != null)
            m_AudioSrc.volume = m_bgmVolume * fVolume;
    }
}
