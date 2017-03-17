using UnityEngine;

public class Tap : IActivable
{
    [SerializeField]
    float m_duration = 5.5f;

    float m_startTime = 0;
    ParticleSystem.EmissionModule m_particleSystemEmission;

    AudioSource m_audioSource;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();

        m_particleSystemEmission = GetComponent<ParticleSystem>().emission;
        m_particleSystemEmission.enabled = false;
    }

    void Update()
    {
        if(m_startTime + m_duration > Time.time && m_enabled)
            m_particleSystemEmission.enabled = true;
        else
        {
            m_particleSystemEmission.enabled = false;
            m_enabled = false;
        }
    }

    sealed protected override void Refresh()
    {
        if(m_audioSource)
            m_audioSource.Play();
        m_startTime = Time.time;
    }
}
