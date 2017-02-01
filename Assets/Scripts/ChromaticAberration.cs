using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using System;

public class ChromaticAberration : ParametricEffect
{
    float tearingSpeed = 0;
    float tearingIntensity = 0;
    float fovVariation = 0;

    private Camera m_Camera;
    private Material chromaticAberrationMaterial = null;


    protected override void Init()
    {
        chromaticAberrationMaterial = new Material(Shader.Find("Hidden/ChromAber"));
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

    protected override void UpdateSettings(float t)
    {
        tearingSpeed = t * maxTearingSpeed;
        tearingIntensity = t * maxTearingIntensity;
        fovVariation = t * maxFovVariation;
        m_radio.Intensity1 = t;
    }
}
