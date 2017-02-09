using UnityEngine;
using UnityEngine.Audio;

public class RadioManager : IActivable
{
    public bool Powered
    {
        set
        {
            m_enabled = value;
            UpdatePowered();
        }
    }

    [SerializeField, Range(1f,10f)]
    float m_transitionSpeed;

    [SerializeField]
    MeshRenderer m_waveRenderer;

    [SerializeField, Space]
    AudioSource m_source;
    [SerializeField]
    AudioMixer m_mixer;
    AudioMixerSnapshot m_snapInit;
    AudioMixerSnapshot m_snap1, m_snap2, m_snap3, m_snap4;

    [SerializeField, Range(0,1)]
    public float m_intensity1;
    [SerializeField, Range(0, 1)]
    public float m_intensity2;
    [SerializeField, Range(0, 1)]
    public float m_intensity3;
    [SerializeField, Range(0, 1)]
    public float m_intensity4;

    sealed protected override void Refresh()
    {
        UpdatePowered();
    }

    void Start()
    {
        m_snapInit = m_mixer.FindSnapshot("Init");
        m_snap1 = m_mixer.FindSnapshot("1");
        m_snap2 = m_mixer.FindSnapshot("2");
        m_snap3 = m_mixer.FindSnapshot("3");
        m_snap4 = m_mixer.FindSnapshot("4");
    }

    void Update()
    {
        m_waveRenderer.sharedMaterial.SetFloat("_TintIntensity", Mathf.MoveTowards(m_waveRenderer.sharedMaterial.GetFloat("_TintIntensity"), m_enabled ? 0.5f : 0, Time.deltaTime * 0.5f * m_transitionSpeed));
        m_waveRenderer.sharedMaterial.SetFloat("_BumpAmt", Mathf.MoveTowards(m_waveRenderer.sharedMaterial.GetFloat("_BumpAmt"), m_enabled ? 128 : 0, Time.deltaTime * 128 * m_transitionSpeed));
        m_source.volume = Mathf.MoveTowards(m_source.volume, m_enabled ? 0.5f : 0, Time.deltaTime * 0.5f * m_transitionSpeed);

        m_mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { m_snapInit, m_snap1, m_snap2, m_snap3, m_snap4 },
                                        new float[] { 1 - Mathf.Max(m_intensity1, m_intensity2, m_intensity3, m_intensity4), m_intensity1, m_intensity2 , m_intensity3, m_intensity4 },
                                        0);
    }

    void UpdatePowered()
    {
        if(m_enabled)
            m_waveRenderer.sharedMaterial.SetFloat("_TimeStart", Time.time);
    }
}
