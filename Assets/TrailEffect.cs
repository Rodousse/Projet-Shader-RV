using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class TrailEffect : MonoBehaviour
{
    public Shader shader = null;
    private Material material = null;

    private Matrix4x4 viewProj = Matrix4x4.identity;

    private Camera camera;
    public float SamplesBlurr;
    private Vector3 pos;
    public void Awake()
    {
        shader = Shader.Find("TrailImageEffectShader");
        if (!shader.isSupported)
        {
            enabled = false;

            return;
        }

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;

        camera = GetComponent<Camera>();

        camera.depthTextureMode = DepthTextureMode.Depth;
        viewProj = camera.worldToCameraMatrix.inverse * camera.projectionMatrix;
        pos = camera.transform.position;

    }
    [ImageEffectOpaque]
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Matrix4x4 VP = camera.worldToCameraMatrix.inverse * camera.projectionMatrix;

        material.SetMatrix("_UNITY_VP_INV", VP.inverse);
        material.SetMatrix("_UNITY_VP_prev", viewProj);
        material.SetFloat("samples", SamplesBlurr);
        viewProj = VP;
        Debug.Log("vp : " + camera.cameraToWorldMatrix);
        camera.ResetWorldToCameraMatrix();
        Debug.Log("vpRESET : " + camera.cameraToWorldMatrix);

        Graphics.Blit(source, destination, material);
    }
}