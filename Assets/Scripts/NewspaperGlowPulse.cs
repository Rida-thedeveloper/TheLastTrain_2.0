
using UnityEngine;

public class NewspaperGlowPulse : MonoBehaviour
{
    public float minScale = 0.75f;
    public float maxScale = 1.3f;
    public float pulseSpeed = 2f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        float scale = Mathf.Lerp(minScale, maxScale, pulse);

        transform.localScale = originalScale * scale;
    }
}
