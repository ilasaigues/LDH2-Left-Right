using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurEffect: MonoBehaviour
{
    [Range(0, 5)]
    public float distance;

    [Range(0, 1)]
    public float intensity;

    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/BlurShader"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity == 0)
        {
            Graphics.Blit(source, destination, material);
            return;
        }
        material.SetFloat("_Distance", distance);
        material.SetFloat("_Intensity", intensity);
        material.SetFloat("_Width", Camera.main.pixelWidth);
        material.SetFloat("_Height", Camera.main.pixelHeight);
        Graphics.Blit(source, destination, material);
    }
}
