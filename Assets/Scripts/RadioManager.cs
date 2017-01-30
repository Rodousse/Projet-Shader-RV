using UnityEngine;

public class RadioManager : MonoBehaviour {

    [SerializeField]
    AudioSource m_source;
    [SerializeField]
    AudioLowPassFilter m_lpFilter;
    [SerializeField]
    AudioReverbFilter m_reverbFilter;
    [SerializeField]
    AudioEchoFilter m_echoFilter;
    [SerializeField]
    AudioChorusFilter m_chorusFilter;

    [SerializeField, Range(0,1)]
    float m_intensity1;
    public float Intensity1
    {
        set
        {
            m_intensity1 = value;
            UpdateIntensity1();
        }
    }

    [SerializeField, Range(0, 1)]
    float m_intensity2;
    public float Intensity2
    {
        set
        {
            m_intensity2 = value;
            UpdateIntensity2();
        }
    }

    [SerializeField, Range(0, 1)]
    float m_intensity3;
    public float Intensity3
    {
        set
        {
            m_intensity3 = value;
            UpdateIntensity3();
        }
    }

    void OnValidate()
    {
        UpdateIntensity1();
        UpdateIntensity2();
        UpdateIntensity3();
    }

    void UpdateIntensity1()
    {
        m_source.dopplerLevel = Mathf.Lerp(1,5, m_intensity1);
        m_source.spread = Mathf.Lerp(0, 360, m_intensity1);
        m_source.pitch = Mathf.Lerp(1, -0.6f, m_intensity1);
    }

    void UpdateIntensity2()
    {
        m_lpFilter.cutoffFrequency = Mathf.Lerp(22000, 950, m_intensity2);
        m_reverbFilter.room = Mathf.Lerp(-10000, -420, m_intensity2);
    }

    void UpdateIntensity3()
    {
        m_echoFilter.wetMix = m_intensity3;
        m_echoFilter.dryMix = Mathf.Lerp(1, 0.5f, m_intensity3);
        m_chorusFilter.wetMix1 = m_chorusFilter.wetMix2 = m_chorusFilter.wetMix3 = m_intensity3;
        m_chorusFilter.dryMix = Mathf.Lerp(1,0.5f, m_intensity3);
    }
}
