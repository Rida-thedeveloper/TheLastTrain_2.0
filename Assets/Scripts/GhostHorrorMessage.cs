
using System.Collections;
using UnityEngine;
using TMPro;

public class GhostHorrorMessage : MonoBehaviour
{
    public GameObject messageObject;
    public TMP_Text messageText;
    public float messageDelay = 2.5f;
    public float showDuration = 3f;
    public float fadeDuration = 1f;
    public GhostFootsteps ghostFootsteps;

    private Coroutine messageRoutine;

    public void ShowMessage(string message)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowAndFade(message));
    }

    private IEnumerator ShowAndFade(string message)
    {
        if (messageObject == null || messageText == null)
            yield break;
        yield return new WaitForSeconds(messageDelay);

        messageObject.SetActive(true);
        messageText.text = message;

        Color color = messageText.color;
        color.a = 1f;
        messageText.color = color;

        yield return new WaitForSeconds(showDuration);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            messageText.color = color;
            yield return null;
        }

        messageObject.SetActive(false);

        if (ghostFootsteps != null)
            ghostFootsteps.PlayFootsteps();

        messageRoutine = null;
    }
}
