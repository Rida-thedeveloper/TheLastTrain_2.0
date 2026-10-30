
using System.Collections;
using UnityEngine;

public class GhostDisappearTrigger : MonoBehaviour
{
    public GameObject ghost;
    public AudioClip disappearSound;
    [Range(0f, 1f)] public float soundVolume = 0.8f;
    public float disappearDelay = 2f;

    public GhostLightFlicker lightFlicker;
    public GhostHorrorMessage horrorMessage;

    private bool hasDisappeared = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasDisappeared) return;

        if (other.GetComponentInParent<CharacterController>() == null)
            return;

        hasDisappeared = true;
        StartCoroutine(DisappearGhost());
    }

    private IEnumerator DisappearGhost()
    {
        yield return new WaitForSeconds(disappearDelay);

        if (ghost != null && disappearSound != null)
        {
            AudioSource.PlayClipAtPoint(
                disappearSound,
                ghost.transform.position,
                soundVolume
            );
        }

        if (lightFlicker != null)
            lightFlicker.StopFlicker();

        if (ghost != null)
            ghost.SetActive(false);

        if (horrorMessage != null)
            horrorMessage.ShowMessage("DON'T TURN AROUND.");

        gameObject.SetActive(false);
    }
}
