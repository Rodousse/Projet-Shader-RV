using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
    [RequireComponent(typeof(Camera))]
    public class Noise : PostEffectsBase
    {
        [SerializeField, Range(0, 1)]
        float intensityMultiplier = 0.25f;

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
                    float XStart = Random.Range(0.0f, 1.0f);
                    float YStart = Random.Range(0.0f, 1.0f);

                    float texTileSize = (noiseTexture.width / noiseTexture.width) / TILE_AMOUNT;

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
    }
}
