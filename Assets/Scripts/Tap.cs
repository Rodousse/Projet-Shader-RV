using UnityEngine;

public class Tap : MonoBehaviour
{
    [SerializeField]
    float m_duration = 2;

    float m_startTime = 0;
    ParticleSystem.EmissionModule m_particleSystemEmission;

    void Start()
    {
        m_particleSystemEmission = GetComponent<ParticleSystem>().emission;
        m_particleSystemEmission.enabled = false;
    }

    void Update()
    {
        if(m_startTime + m_duration > Time.time)
            m_particleSystemEmission.enabled = true;
        else
            m_particleSystemEmission.enabled = false;
    }

    public void Activate()
    {
        m_startTime = Time.time;
    }
}
