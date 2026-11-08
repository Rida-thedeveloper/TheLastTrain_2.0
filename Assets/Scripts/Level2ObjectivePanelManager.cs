
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Level2ObjectivePanelManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject objectivePanel;
    public TMP_Text objectiveTitle;
    public TMP_Text objectiveDescription;
    public TMP_Text buttonText;
    public GameObject greenTick;
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

    [Header("Level 2 References")]
    public Level2ObjectiveController level2ObjectiveController;
    public Level2FuseBox level2FuseBox;

    private bool startTimerOnNextClick = false;

    private enum ButtonAction
    {
        None,
        InitialContinue,
        FindSpider,
        CloseOnly
    }

    private ButtonAction currentButtonAction = ButtonAction.None;

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
        else if (buttonText != null &&
                 buttonText.transform.parent != null)
        {
            GameObject buttonObject =
                buttonText.transform.parent.gameObject;

            buttonGroup = GetGroup(buttonObject);
            uiButton = buttonObject.GetComponent<Button>();
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

    // INITIAL POWER FAILURE OBJECTIVE
    public void ShowFirstObjective(
        string title,
        string description,
        string buttonLabel)
    {
        startTimerOnNextClick = true;
        currentButtonAction = ButtonAction.InitialContinue;

        ShowObjective(title, description, buttonLabel, false);
    }

    // BATTERY MISSING OBJECTIVE
    public void ShowFindSpiderObjective(
        string title,
        string description,
        string buttonLabel,
        bool showGreenTick)
    {
        currentButtonAction = ButtonAction.FindSpider;

        ShowObjective(title, description, buttonLabel, showGreenTick);
    }

    // BATTERY FOUND OBJECTIVE
    public void ShowBatteryFoundObjective(
        string title,
        string description,
        string buttonLabel,
        bool showGreenTick)
    {
        currentButtonAction = ButtonAction.CloseOnly;

        ShowObjective(title, description, buttonLabel, showGreenTick);
    }

    // SHOW OBJECTIVE PANEL
    public void ShowObjective(
        string title,
        string description,
        string buttonLabel,
        bool showGreenTick)
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

    // CINEMATIC REVEAL
    IEnumerator RevealObjective(bool showGreenTick)
    {
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

        // GREEN TICK WITH TITLE
        if (showGreenTick && tickGroup != null)
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

    // BUTTON ACTIONS
    public void HandlePrimaryButtonClick()
    {
        ButtonAction action = currentButtonAction;
        currentButtonAction = ButtonAction.None;

        switch (action)
        {
            case ButtonAction.InitialContinue:

                if (level2ObjectiveController != null)
                    level2ObjectiveController.ContinueGameplay();

                ClosePanel();

                // Enable initial Fuse Box glow
                if (level2FuseBox != null)
                    level2FuseBox.EnableInitialGlow();

                break;

            case ButtonAction.FindSpider:

                ClosePanel();

                if (level2FuseBox != null)
                    level2FuseBox.OnLetsFindClicked();
                else
                    Debug.LogError(
                        "Level2FuseBox reference missing!"
                    );

                break;

            case ButtonAction.CloseOnly:

                ClosePanel();

                // Enable Fuse Box glow again after Battery Found
                if (level2FuseBox != null)
                    level2FuseBox.EnableReturnGlow();

                break;

            default:

                ClosePanel();
                break;
        }
    }

    // CLOSE PANEL
    public void ClosePanel()
    {
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
            else
                Debug.LogError("TimerManager reference missing!");

            startTimerOnNextClick = false;
        }

        if (objectivePanel != null)
            objectivePanel.SetActive(false);
    }

    // FADE ANIMATION
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

    // UI AUDIO
    void PlayUISound(AudioClip clip)
    {
        if (uiAudioSource != null && clip != null)
            uiAudioSource.PlayOneShot(clip);
    }
}
