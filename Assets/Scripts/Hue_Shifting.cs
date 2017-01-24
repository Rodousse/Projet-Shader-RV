using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class Hue_Shifting : MonoBehaviour
{
   

    float m_HueIncrement;
    Material m_material;
    
    void Start()
    {
        m_material = new Material(Shader.Find("Hidden/Hue_Shifting"));
    }

    void Update()
    {
        m_HueIncrement = Mathf.Sin(Time.time * m_speedMuliplier) * m_intensity /2;
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_HueIncrement == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        m_material.SetFloat("_HueIncrement", m_HueIncrement);
        Graphics.Blit(source, destination, m_material);
    }

    //[SerializeField, Range(0, 1)]
    float m_intensity=0f;
    //[SerializeField]
    float m_speedMuliplier=0f;

    [SerializeField, Range(0, 1)]
    float maxIntensity;
    [SerializeField]
    float maxSpeedMultiplier;


    [SerializeField, Range(1, 25)]
    float drugTimeEffect = 10f;

    [SerializeField, Range(0, 0.01f)]
    float StepValue = 0.02f;

    [SerializeField, Range(0, 3)]
    float TimeStepValue = 0.02f;

    public void Activate(System.Action callback)
    {
        StartCoroutine(animate(callback));
    }

    IEnumerator animate(System.Action callback)
    {
        float _dTimeEffect = drugTimeEffect;

        while (_dTimeEffect >= 0)
        {
            if (m_speedMuliplier < maxSpeedMultiplier)
                m_speedMuliplier += StepValue;
            if (m_intensity < maxIntensity)
                m_intensity += StepValue;
            _dTimeEffect -= TimeStepValue;
            yield return new WaitForSeconds(TimeStepValue);
        }
        yield return new WaitForSeconds(2f);
        while (_dTimeEffect <= drugTimeEffect)
        {
            if (m_speedMuliplier > 0)
                m_speedMuliplier -= StepValue;
            if (m_intensity > 0)
                m_intensity -= StepValue;
            _dTimeEffect += TimeStepValue;
            yield return new WaitForSeconds(TimeStepValue);
        }
        m_intensity = 0;
        m_speedMuliplier = 0;
        callback();
        yield return null;
    }
}
