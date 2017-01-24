using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ChromaticAberration : PostEffectsBase
{
    public float tearingSpeed = 1;
    public float tearingIntensity = 1;
    public float fovVariation = 15;

    private Camera m_Camera;
    private Material chromaticAberrationMaterial = null;
    // Use this for initialization
    public bool isActive = false;
    
    
    public void Start()
    {
        
        chromaticAberrationMaterial = new Material(Shader.Find("Hidden/ChromAber"));
        
    }

    

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isActive)
        {
            chromaticAberrationMaterial.SetTexture("_MainTex", source);
            chromaticAberrationMaterial.SetFloat("_tearingSpeed", tearingSpeed);
            chromaticAberrationMaterial.SetFloat("_tearingIntensity", tearingIntensity);
            Camera.current.fieldOfView = 60 + fovVariation - Mathf.Cos(Time.time * tearingSpeed) * (tearingIntensity * fovVariation);
            Graphics.Blit(source, destination, chromaticAberrationMaterial, 0);
        }
    }
}
