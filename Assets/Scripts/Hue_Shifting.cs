using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class Hue_Shifting : MonoBehaviour
{
    [SerializeField, Range(0,1)] float m_intensity;
    [SerializeField] float m_speedMuliplier;
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
}
