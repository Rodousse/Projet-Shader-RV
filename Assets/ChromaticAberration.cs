using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class ChromaticAberration : PostEffectsBase
{
    
    public Shader chromaticAberrationShader = null;
    public float tearingSpeed = 1;
    public float tearingIntensity = 1;
    public float fovVariation = 15;

    private Camera m_Camera;
    private Material chromaticAberrationMaterial = null;
    // Use this for initialization

    
    
    public override bool CheckResources()
    {
        m_Camera = gameObject.GetComponent<Camera>();
        chromaticAberrationMaterial = CheckShaderAndCreateMaterial(chromaticAberrationShader, chromaticAberrationMaterial);
        if (!isSupported)
            ReportAutoDisable();
        return isSupported;
    }

    

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        chromaticAberrationMaterial.SetTexture("_MainTex", source);
        chromaticAberrationMaterial.SetFloat("_tearingSpeed", tearingSpeed);
        chromaticAberrationMaterial.SetFloat("_tearingIntensity", tearingIntensity);
        Camera.current.fieldOfView=  60 + fovVariation - Mathf.Cos(Time.time * tearingSpeed)*(tearingIntensity*fovVariation);
        Graphics.Blit(source, destination, chromaticAberrationMaterial, 0);
    }
}
