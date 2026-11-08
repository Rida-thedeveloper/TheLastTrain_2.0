
using UnityEngine;

public class DoorEdgeGlowPulse : MonoBehaviour
{
    public float minAlpha = 0.2f;
    public float maxAlpha = 0.9f;
    public float pulseSpeed = 2f;

    private Renderer glowRenderer;
    private MaterialPropertyBlock propertyBlock;
    private Color originalColor;

    private static readonly int BaseColorID =
        Shader.PropertyToID("_BaseColor");

    void Start()
    {
        glowRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        if (glowRenderer != null &&
            glowRenderer.sharedMaterial != null)
        {
            originalColor =
                glowRenderer.sharedMaterial.GetColor(BaseColorID);
        }
    }

    void Update()
    {
        if (glowRenderer == null)
            return;

        float pulse =
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        Color glowColor = originalColor;
        glowColor.a = Mathf.Lerp(minAlpha, maxAlpha, pulse);

        glowRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColorID, glowColor);
        glowRenderer.SetPropertyBlock(propertyBlock);
    }
}
