using UnityEngine;

[ExecuteInEditMode]
public class PostProcess : MonoBehaviour
{
    public Material material;
    [Range(1, 10)] public int scale = 1;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width / scale;
        int height = source.height / scale;
        RenderTextureFormat format = source.format;
        RenderTexture r = RenderTexture.GetTemporary(width, height, 0, format);
        r.filterMode = FilterMode.Point;

        Graphics.Blit(source, r);
        material.SetFloat("_Scale", 1f / scale);
        Graphics.Blit(r, destination, material);
        RenderTexture.ReleaseTemporary(r);
    }
}
