using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Camera))]
public class Hue_Shifting : ParametricEffect
{
    float m_HueIncrement;
    Material m_material;
    
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

    protected override void Init()
    {
        m_material = new Material(Shader.Find("Hidden/Hue_Shifting"));
    }

    protected override void UpdateSettings(float t)
    {
        m_speedMuliplier = t * maxSpeedMultiplier;
        m_intensity = t * maxIntensity;
        m_HueIncrement = Mathf.Sin(Time.time * m_speedMuliplier) * m_intensity / 2;
        
    }
}
