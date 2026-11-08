
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ObjectivePanelManager : MonoBehaviour
{
    public GameObject objectivePanel;

    [Header("UI Text")]
    public TMP_Text objectiveTitle;
    public TMP_Text objectiveDescription;
    public TMP_Text buttonText;

    [Header("Green Tick")]
    public GameObject greenTick;

    [Header("Button")]
    public GameObject objectiveButton;

    [Header("Animation Timing")]
    public float titleDelay = 0.3f;
    public float descriptionDelay = 1.2f;
    public float typingSpeed = 0.035f;
    public float buttonDelay = 1f;
    public float fadeSpeed = 2f;

    [Header("Audio Sources")]
    public AudioSource uiAudioSource;
    public AudioSource typingAudioSource;

    [Header("Audio Clips")]
    public AudioClip titleSound;
    public AudioClip typingSound;
    public AudioClip buttonAppearSound;
    public AudioClip buttonClickSound;

    [Header("Timer")]
    public TimerManager timerManager;

    private bool startTimerOnNextClick = false;

    private CanvasGroup titleGroup;
    private CanvasGroup descriptionGroup;
    private CanvasGroup buttonGroup;
    private CanvasGroup tickGroup;

    private Button uiButton;
    private Coroutine revealCoroutine;

    void Awake()
    {
        if (objectiveTitle != null)
            titleGroup = GetGroup(objectiveTitle.gameObject);

        if (objectiveDescription != null)
            descriptionGroup = GetGroup(objectiveDescription.gameObject);

        if (objectiveButton != null)
        {
            buttonGroup = GetGroup(objectiveButton);
            uiButton = objectiveButton.GetComponent<Button>();
        }
        else if (buttonText != null && buttonText.transform.parent != null)
        {
            buttonGroup = GetGroup(buttonText.transform.parent.gameObject);
            uiButton = buttonText.transform.parent.GetComponent<Button>();
        }

        if (greenTick != null)
            tickGroup = GetGroup(greenTick);
    }

    CanvasGroup GetGroup(GameObject obj)
    {
        CanvasGroup group = obj.GetComponent<CanvasGroup>();

        if (group == null)
            group = obj.AddComponent<CanvasGroup>();

        return group;
    }

    public void ClosePanel()
    {
        // Button CLICK sound
        PlayUISound(buttonClickSound);

        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
            revealCoroutine = null;
        }

        if (typingAudioSource != null)
            typingAudioSource.Stop();

        if (startTimerOnNextClick)
        {
            if (timerManager != null)
                timerManager.StartTimer();

            startTimerOnNextClick = false;
        }

        if (objectivePanel != null)
            objectivePanel.SetActive(false);
    }

    public void ShowObjective(
        string title,
        string description,
        string buttonLabel,
        bool showGreenTick
    )
    {
        if (objectivePanel != null)
            objectivePanel.SetActive(true);

        if (objectiveTitle != null)
            objectiveTitle.text = title;

        if (objectiveDescription != null)
            objectiveDescription.text = description;

        if (buttonText != null)
            buttonText.text = buttonLabel;

        if (revealCoroutine != null)
            StopCoroutine(revealCoroutine);

        if (typingAudioSource != null)
            typingAudioSource.Stop();

        revealCoroutine = StartCoroutine(
            RevealObjective(showGreenTick)
        );
    }

    public void ShowFirstObjective(
        string title,
        string description,
        string buttonLabel
    )
    {
        startTimerOnNextClick = true;

        ShowObjective(
            title,
            description,
            buttonLabel,
            false
        );
    }

    IEnumerator RevealObjective(bool showGreenTick)
    {
        // Hide all elements initially
        if (titleGroup != null)
            titleGroup.alpha = 0;

        if (descriptionGroup != null)
            descriptionGroup.alpha = 0;

        if (buttonGroup != null)
        {
            buttonGroup.alpha = 0;
            buttonGroup.interactable = false;
            buttonGroup.blocksRaycasts = false;
        }

        if (uiButton != null)
            uiButton.interactable = false;

        if (greenTick != null)
        {
            greenTick.SetActive(showGreenTick);

            if (tickGroup != null)
                tickGroup.alpha = 0;
        }

        if (objectiveDescription != null)
            objectiveDescription.maxVisibleCharacters = 0;

        // TITLE
        yield return new WaitForSeconds(titleDelay);

        PlayUISound(titleSound);

        // Title and green tick appear together
        if (showGreenTick && greenTick != null && tickGroup != null)
            tickGroup.alpha = 1;

        yield return FadeIn(titleGroup);

        // DESCRIPTION
        yield return new WaitForSeconds(descriptionDelay);

        if (descriptionGroup != null)
            descriptionGroup.alpha = 1;

        if (objectiveDescription != null)
        {
            objectiveDescription.ForceMeshUpdate();

            int totalCharacters =
                objectiveDescription.textInfo.characterCount;

            if (typingAudioSource != null && typingSound != null)
            {
                typingAudioSource.clip = typingSound;
                typingAudioSource.loop = true;
                typingAudioSource.Play();
            }

            for (int i = 0; i <= totalCharacters; i++)
            {
                objectiveDescription.maxVisibleCharacters = i;

                yield return new WaitForSeconds(typingSpeed);
            }

            if (typingAudioSource != null)
                typingAudioSource.Stop();
        }

        // BUTTON APPEAR
        yield return new WaitForSeconds(buttonDelay);

        PlayUISound(buttonAppearSound);

        yield return FadeIn(buttonGroup);

        if (buttonGroup != null)
        {
            buttonGroup.interactable = true;
            buttonGroup.blocksRaycasts = true;
        }

        if (uiButton != null)
            uiButton.interactable = true;

        revealCoroutine = null;
    }

    IEnumerator FadeIn(CanvasGroup group)
    {
        if (group == null)
            yield break;

        group.alpha = 0;

        while (group.alpha < 1f)
        {
            group.alpha = Mathf.MoveTowards(
                group.alpha,
                1f,
                fadeSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    void PlayUISound(AudioClip clip)
    {
        if (uiAudioSource != null && clip != null)
            uiAudioSource.PlayOneShot(clip);
    }
}
