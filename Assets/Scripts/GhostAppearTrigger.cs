
using UnityEngine;

public class GhostAppearTrigger : MonoBehaviour
{
    public GameObject ghost;
    public AudioClip appearSound;
    [Range(0f, 1f)] public float soundVolume = 0.8f;
    public GhostLightFlicker lightFlicker;

    private bool hasAppeared = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasAppeared) return;

        if (other.GetComponentInParent<CharacterController>() == null)
            return;

        hasAppeared = true;

        if (ghost != null)
            ghost.SetActive(true);

        if (lightFlicker != null)
            lightFlicker.StartFlicker();

        if (appearSound != null && ghost != null)
        {
            AudioSource.PlayClipAtPoint(
                appearSound,
                ghost.transform.position,
                soundVolume
            );
        }
    }
}
