using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
    [RequireComponent(typeof(Camera))]
    public class Flip : PostEffectsBase
    {
        [SerializeField, Range(-1, 1)]
        int m_direction;
        Material m_material;

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            m_material = new Material(Shader.Find("Hidden/Flip"));
            m_material.SetFloat("_Flip", m_direction);
            Graphics.Blit(source, destination, m_material);
        }
    }
}