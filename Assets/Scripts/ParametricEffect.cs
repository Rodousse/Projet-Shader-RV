using UnityEngine;
using UnityStandardAssets.ImageEffects;

public abstract class ParametricEffect : PostEffectsBase
{
    [SerializeField]
    AnimationCurve m_curve;
    [SerializeField, Range(1, 25)]
    float m_TimeEffect = 10f;
    [SerializeField]
    protected RadioManager m_radio;

    protected float m_timeStart;

    protected float CurveValue
    {
        get
        {
            return m_curve.Evaluate(1 - (Time.time - m_timeStart) / (m_TimeEffect));
        }
    }

    void Update()
    {
        if (m_timeStart + m_TimeEffect > Time.time)
            UpdateSettings(CurveValue);
    }

    public void Activate()
    {
        m_timeStart = Time.time;
    }

    protected new void Start () {
        base.Start();
        Init();
        m_timeStart = float.MinValue;
    }

    protected abstract void Init();
    protected abstract void UpdateSettings(float t);
}
