using UnityEngine;
using System.Collections;
using System;

namespace UnityStandardAssets.ImageEffects
{
    [RequireComponent(typeof(Camera))]
    public class Noise : ParametricEffect
    {
        [SerializeField]
        Texture2D noiseTexture;
        Material m_material = null;
        static float TILE_AMOUNT = 64;


        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {

            m_material = new Material(Shader.Find("Hidden/Noise"));

            m_material.SetTexture("_NoiseTex", noiseTexture);
            m_material.SetFloat("_NoiseAmount", intensityMultiplier);

            RenderTexture.active = destination;

            m_material.SetTexture("_ScreenTex", source);

            float stepSizeX = 1.0f / (source.width / TILE_AMOUNT);
            float stepSizeY = stepSizeX * source.width / source.height;

            m_material.SetPass(0);

            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);

            for (float x1 = 0; x1 < 1; x1 += stepSizeX)
                for (float y1 = 0; y1 < 1; y1 += stepSizeY)
                {
                    float XStart = UnityEngine.Random.Range(0.0f, 1.0f);    // On détermine une valeur aléatoire
                    float YStart = UnityEngine.Random.Range(0.0f, 1.0f);    // en X et en Y

                    float texTileSize = (noiseTexture.width / noiseTexture.width) / TILE_AMOUNT;

                    // On dessine les quads et on déplace les coordonnées UV de la texture de noise
                    GL.MultiTexCoord2(0, XStart * TILE_AMOUNT, YStart * TILE_AMOUNT);
                    GL.Vertex3(x1, y1, 0);
                    GL.MultiTexCoord2(0, (XStart + texTileSize) * TILE_AMOUNT, YStart * TILE_AMOUNT);
                    GL.Vertex3(x1 + stepSizeX, y1, 0);
                    GL.MultiTexCoord2(0, (XStart + texTileSize) * TILE_AMOUNT, (YStart + texTileSize) * TILE_AMOUNT);
                    GL.Vertex3(x1 + stepSizeX, y1 + stepSizeY, 0);
                    GL.MultiTexCoord2(0, XStart * TILE_AMOUNT, (YStart + texTileSize) * TILE_AMOUNT);
                    GL.Vertex3(x1, y1 + stepSizeY, 0);
                }

            GL.End();
            GL.PopMatrix();
        }


       // [SerializeField, Range(0, 1)]
        float intensityMultiplier = 0f;
        [SerializeField, Range(0, 1)]
        float maxIntensityMultiplier;

        protected override void Init()
        {

        }

        protected override void UpdateSettings(float t)
        {
            intensityMultiplier = t * maxIntensityMultiplier;
            m_radio.m_intensity2 = t;
        }
    }
}
