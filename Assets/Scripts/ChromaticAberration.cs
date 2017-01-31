using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ChromaticAberration : PostEffectsBase
{
    [SerializeField]
    AnimationCurve m_curve;

    float m_timeStart;

    float tearingSpeed = 0;
    float tearingIntensity = 0;
    float fovVariation = 0;

    private Camera m_Camera;
    private Material chromaticAberrationMaterial = null;
    
    
    public void Start()
    {    
        chromaticAberrationMaterial = new Material(Shader.Find("Hidden/ChromAber"));
        m_timeStart = float.MinValue;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        chromaticAberrationMaterial.SetTexture("_MainTex", source);
        chromaticAberrationMaterial.SetFloat("_tearingSpeed", tearingSpeed);
        chromaticAberrationMaterial.SetFloat("_tearingIntensity", tearingIntensity);
        Camera.current.fieldOfView = 60 + fovVariation - Mathf.Cos(Time.time * tearingSpeed) * (tearingIntensity * fovVariation);
        Graphics.Blit(source, destination, chromaticAberrationMaterial, 0);
    }

    [SerializeField]
    float maxTearingSpeed = 3.0f;

    [SerializeField]
    float maxTearingIntensity = 3.0f;

    [SerializeField]
    float maxFovVariation = 25f;

    [SerializeField, Range(1, 25)]
    float drugTimeEffect = 10f;

    [SerializeField, Range(0, 0.01f)]
    float StepValue = 0.02f;

    [SerializeField, Range(0, 3)]
    float TimeStepValue = 0.02f;

    public void Activate(System.Action callback)
    {
        //StartCoroutine(animate(callback));
        m_timeStart = Time.time;
    }

    public void Update()
    {
        float t = m_curve.Evaluate(1 - (Time.time - m_timeStart) / (drugTimeEffect));

        if(m_timeStart + drugTimeEffect > Time.time)
        {
            tearingSpeed = t * maxTearingSpeed;
            tearingIntensity = t * maxTearingIntensity;
            fovVariation = t * maxFovVariation;
        }
    }

    IEnumerator animate(System.Action callback)
    {
        float _dTimeEffect = drugTimeEffect;

        while(_dTimeEffect>=0)
        {
            if(tearingSpeed  <maxTearingSpeed)
            {
                tearingSpeed += StepValue;
            }
            if(tearingIntensity < maxTearingIntensity)
            {
                tearingIntensity += StepValue;
            }
            if(fovVariation < maxFovVariation)
            {
                fovVariation += StepValue;
            }
            _dTimeEffect -= TimeStepValue;
            yield return new WaitForSeconds(TimeStepValue);
        }
        yield return new WaitForSeconds(3f);
        while(_dTimeEffect<=drugTimeEffect)
        {
            if (tearingSpeed > 0)
            {
                tearingSpeed -= StepValue;
            }
            if (tearingIntensity > 0)
            {
                tearingIntensity -= StepValue;
            }
            if (fovVariation >0)
            {
                fovVariation -= StepValue;
            }
            _dTimeEffect += TimeStepValue;
            yield return new WaitForSeconds(TimeStepValue);
        }
        tearingIntensity = 0;
        fovVariation = 0;
        tearingSpeed = 0;
        callback();
        yield return null;
    }
}
