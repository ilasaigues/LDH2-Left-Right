using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisheyeEffect : MonoBehaviour
{
    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/FisheyeShader"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
