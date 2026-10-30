
using System.Collections;
using UnityEngine;

public class GhostFootsteps : MonoBehaviour
{
    public AudioClip footstepsClip;
    public float pitch = 0.65f;
    [Range(0f, 1f)] public float volume = 0.8f;
    public float startDelay = 0.5f;
    public Level3MissionUI missionUI;

    private AudioSource audioSource;
    private Coroutine playRoutine;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.loop = false;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
    }

    public void PlayFootsteps()
    {
        if (playRoutine != null)
            StopCoroutine(playRoutine);

        playRoutine = StartCoroutine(PlayDelayed());
    }


    private IEnumerator PlayDelayed()
    {
        yield return new WaitForSeconds(startDelay);

        if (footstepsClip != null)
        {
            audioSource.pitch = pitch;
            audioSource.clip = footstepsClip;
            audioSource.volume = volume;
            audioSource.Play();

            yield return new WaitForSeconds(1.5f);

            float elapsed = 0f;
            float fadeDuration = 5f;
            float startVolume = audioSource.volume;

            while (elapsed < fadeDuration && audioSource.isPlaying)
            {
                elapsed += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(
                    startVolume,
                    0f,
                    elapsed / fadeDuration
                );

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = volume;
        }

        if (missionUI != null)
        {
            missionUI.ShowMissionBriefing();
        }

        playRoutine = null;
    }

}
