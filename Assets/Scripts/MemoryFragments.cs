
using System.Collections;
using UnityEngine;

public class MemoryFragments : MonoBehaviour
{
    [Header("Memory 1")]
    public CanvasGroup memory1Blurred;
    public CanvasGroup memory1Clear;

    [Header("Memory 2")]
    public CanvasGroup memory2Blurred;
    public CanvasGroup memory2Clear;

    [Header("Grandmother")]
    public GrandmotherGuide grandmotherGuide;

    [Header("Timing")]
    public float transitionDuration = 4f;
    public float displayDuration = 3f;
    public float fadeOutDuration = 2.5f;

    private bool hasPlayed = false;

    public void PlayFragments()
    {
        if (hasPlayed)
            return;

        hasPlayed = true;
        StartCoroutine(PlayMemorySequence());
    }

    private IEnumerator PlayMemorySequence()
    {
        yield return StartCoroutine(ShowMemory(
            memory1Blurred,
            memory1Clear
        ));

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(ShowMemory(
            memory2Blurred,
            memory2Clear
        ));

        Debug.Log("Memory fragments completed!");

        // Grandmother appears after both memories finish.
        // StartGuiding will enable the ending point.
        if (grandmotherGuide != null)
        {
            grandmotherGuide.StartGuiding();
        }
        else
        {
            Debug.LogWarning(
                "Grandmother Guide reference is missing!"
            );
        }
    }

    private IEnumerator ShowMemory(
        CanvasGroup blurred,
        CanvasGroup clear)
    {
        if (blurred == null || clear == null)
            yield break;

        blurred.gameObject.SetActive(true);
        clear.gameObject.SetActive(true);

        blurred.alpha = 1f;
        clear.alpha = 0f;

        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(
                timer / transitionDuration
            );

            blurred.alpha = 1f - progress;
            clear.alpha = progress;

            yield return null;
        }

        blurred.alpha = 0f;
        clear.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        timer = 0f;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(
                timer / fadeOutDuration
            );

            blurred.alpha = progress;
            clear.alpha = 1f - progress;

            yield return null;
        }

        blurred.alpha = 0f;
        clear.alpha = 0f;

        blurred.gameObject.SetActive(false);
        clear.gameObject.SetActive(false);
    }
}
