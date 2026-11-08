
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Level3MissionUI : MonoBehaviour
{
    [Header("Mission UI")]
    public GameObject missionBriefingPanel;
    public GameObject ghostHorrorMessage;
    public GameObject clueTrackerPanel;

    [Header("Mission Briefing Texts")]
    public TMP_Text missionTitleText;
    public TMP_Text missionDescriptionText;
    public TMP_Text missionClueText1;
    public TMP_Text missionClueText2;
    public TMP_Text missionClueText3;
    public Button missionButton;

    [Header("Mission Briefing Settings")]
    public float titleFadeDuration = 0.8f;
    public float textTypingSpeed = 0.035f;
    public float elementDelay = 0.25f;
    public float buttonFadeDuration = 0.6f;

    [Header("UI Audio")]
    public AudioSource uiAudioSource;
    public AudioSource typingAudioSource;

    public AudioClip titleSound;
    public AudioClip typingSound;
    public AudioClip buttonAppearSound;
    public AudioClip buttonClickSound;

    [Header("Clue Objects")]
    public GameObject oldTrainTicket;
    public GameObject brokenWatch;
    public GameObject ghostPhotograph;

    [Header("Green Tick Images")]
    public GameObject tickClue1;
    public GameObject tickClue2;
    public GameObject tickClue3;

    [Header("Story Message")]
    public GameObject storyMessageObject;
    public TMP_Text storyMessageText;
    public float storyMessageDuration = 4f;

    [Header("Memory Fragments")]
    public MemoryFragments memoryFragments;

    [TextArea]
    public string familiarFaceMessage =
        "A FAMILIAR FACE...\nWhy does she feel so familiar? Have I seen her before?";

    public float familiarMessageDuration = 4f;

    [Header("Tracker Text")]
    public TMP_Text trackerClue1Text;
    public TMP_Text trackerClue2Text;
    public TMP_Text trackerClue3Text;

    [Header("Tracker Colors")]
    public Color activeColor = Color.white;
    public Color mutedColor = new Color(0.55f, 0.55f, 0.55f);

    private bool[] cluesCollected = new bool[3];
    private Coroutine storyRoutine;
    private Coroutine briefingRoutine;
    private int currentClueIndex = 0;

    private CanvasGroup titleGroup;
    private CanvasGroup buttonGroup;

    private void Awake()
    {
        if (missionTitleText != null)
            titleGroup = GetOrAddCanvasGroup(missionTitleText.gameObject);

        if (missionButton != null)
            buttonGroup = GetOrAddCanvasGroup(missionButton.gameObject);
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject target)
    {
        CanvasGroup group = target.GetComponent<CanvasGroup>();

        if (group == null)
            group = target.AddComponent<CanvasGroup>();

        return group;
    }

    public void ShowMissionBriefing()
    {
        if (ghostHorrorMessage != null)
            ghostHorrorMessage.SetActive(false);

        if (missionBriefingPanel != null)
            missionBriefingPanel.SetActive(true);

        if (briefingRoutine != null)
            StopCoroutine(briefingRoutine);

        briefingRoutine = StartCoroutine(PlayMissionBriefingReveal());
    }

    private IEnumerator PlayMissionBriefingReveal()
    {
        // Prepare all briefing elements
        if (missionTitleText != null)
        {
            missionTitleText.gameObject.SetActive(true);
            titleGroup.alpha = 0f;
        }

        if (missionDescriptionText != null)
        {
            missionDescriptionText.gameObject.SetActive(true);
            missionDescriptionText.maxVisibleCharacters = 0;
        }

        TMP_Text[] clueTexts =
        {
            missionClueText1,
            missionClueText2,
            missionClueText3
        };

        foreach (TMP_Text clueText in clueTexts)
        {
            if (clueText != null)
            {
                clueText.gameObject.SetActive(true);
                clueText.maxVisibleCharacters = 0;
            }
        }

        if (missionButton != null)
        {
            missionButton.gameObject.SetActive(false);
            buttonGroup.alpha = 0f;
            missionButton.interactable = false;
        }

        // 1. Title reveal and title sound
        if (missionTitleText != null)
        {
            PlayUISound(titleSound);
            yield return StartCoroutine(FadeCanvasGroup(
                titleGroup, 0f, 1f, titleFadeDuration));
        }

        yield return new WaitForSeconds(elementDelay);

        // 2. Description typewriter
        if (missionDescriptionText != null)
            yield return StartCoroutine(TypeText(missionDescriptionText));

        yield return new WaitForSeconds(elementDelay);

        // 3. Clue texts one by one
        foreach (TMP_Text clueText in clueTexts)
        {
            if (clueText != null)
                yield return StartCoroutine(TypeText(clueText));

            yield return new WaitForSeconds(elementDelay);
        }

        // 4. Button appear
        if (missionButton != null)
        {
            missionButton.gameObject.SetActive(true);
            PlayUISound(buttonAppearSound);

            yield return StartCoroutine(FadeCanvasGroup(
                buttonGroup, 0f, 1f, buttonFadeDuration));

            missionButton.interactable = true;
        }

        briefingRoutine = null;
    }

    private IEnumerator TypeText(TMP_Text target)
    {
        if (target == null)
            yield break;

        target.ForceMeshUpdate();
        target.maxVisibleCharacters = 0;

        bool playTyping = typingAudioSource != null && typingSound != null;

        if (playTyping)
        {
            typingAudioSource.clip = typingSound;
            typingAudioSource.loop = true;
            typingAudioSource.Play();
        }

        int totalCharacters = target.textInfo.characterCount;

        for (int i = 0; i < totalCharacters; i++)
        {
            target.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(textTypingSpeed);
        }

        StopTypingSound();
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        if (group == null)
            yield break;

        group.alpha = startAlpha;

        if (duration <= 0f)
        {
            group.alpha = endAlpha;
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        group.alpha = endAlpha;
    }

    private void PlayUISound(AudioClip clip)
    {
        if (uiAudioSource != null && clip != null)
            uiAudioSource.PlayOneShot(clip);
    }

    private void StopTypingSound()
    {
        if (typingAudioSource != null && typingAudioSource.isPlaying)
            typingAudioSource.Stop();
    }

    public void StartFindingClues()
    {
        PlayUISound(buttonClickSound);
        StopTypingSound();

        if (briefingRoutine != null)
        {
            StopCoroutine(briefingRoutine);
            briefingRoutine = null;
        }

        if (missionBriefingPanel != null)
            missionBriefingPanel.SetActive(false);

        if (clueTrackerPanel != null)
            clueTrackerPanel.SetActive(true);

        if (oldTrainTicket != null)
            oldTrainTicket.SetActive(true);

        if (brokenWatch != null)
            brokenWatch.SetActive(false);

        if (ghostPhotograph != null)
            ghostPhotograph.SetActive(false);

        if (tickClue1 != null) tickClue1.SetActive(false);
        if (tickClue2 != null) tickClue2.SetActive(false);
        if (tickClue3 != null) tickClue3.SetActive(false);

        UpdateTrackerVisuals();
    }

    public void OnClueCollected(int clueIndex)
    {
        if (clueIndex < 0 || clueIndex >= cluesCollected.Length)
            return;

        if (cluesCollected[clueIndex])
            return;

        cluesCollected[clueIndex] = true;

        if (tickClue1 != null && clueIndex == 0)
            tickClue1.SetActive(true);

        if (tickClue2 != null && clueIndex == 1)
            tickClue2.SetActive(true);

        if (tickClue3 != null && clueIndex == 2)
            tickClue3.SetActive(true);

        UpdateTrackerVisuals();

        string story = "";

        if (clueIndex == 0)
            story = "PASSENGER 04...\nWhy does this ticket feel familiar?";
        else if (clueIndex == 1)
            story = "THE BROKEN WATCH...\nTime stopped at the exact moment the passenger vanished.";
        else if (clueIndex == 2)
            story = "THE PHOTOGRAPH...\nSomeone in this picture looks exactly like the figure on the train.";

        if (storyRoutine != null)
            StopCoroutine(storyRoutine);

        storyRoutine = StartCoroutine(
            ShowStoryAndUnlockNext(clueIndex, story)
        );
    }

    private IEnumerator ShowStoryAndUnlockNext(int clueIndex, string story)
    {
        // Show clue story with typewriter
        if (storyMessageObject != null && storyMessageText != null)
        {
            storyMessageObject.SetActive(true);
            storyMessageText.text = story;

            yield return StartCoroutine(TypeText(storyMessageText));

            float remainingTime = Mathf.Max(
                0f,
                storyMessageDuration -
                (storyMessageText.text.Length * textTypingSpeed)
            );

            yield return new WaitForSeconds(remainingTime);

            storyMessageObject.SetActive(false);
        }

        // Unlock the next clue
        currentClueIndex = clueIndex + 1;
        UpdateTrackerVisuals();

        if (clueIndex == 0 && brokenWatch != null)
        {
            brokenWatch.SetActive(true);
        }
        else if (clueIndex == 1 && ghostPhotograph != null)
        {
            ghostPhotograph.SetActive(true);
        }
        else if (clueIndex == 2)
        {
            if (clueTrackerPanel != null)
                clueTrackerPanel.SetActive(false);

            // Familiar face message with typewriter
            if (storyMessageObject != null && storyMessageText != null)
            {
                storyMessageObject.SetActive(true);
                storyMessageText.text = familiarFaceMessage;

                yield return StartCoroutine(TypeText(storyMessageText));

                float remainingTime = Mathf.Max(
                    0f,
                    familiarMessageDuration -
                    (storyMessageText.text.Length * textTypingSpeed)
                );

                yield return new WaitForSeconds(remainingTime);

                storyMessageObject.SetActive(false);
            }

            // Play both memory fragments after the message
            if (memoryFragments != null)
            {
                memoryFragments.PlayFragments();
            }
            else
            {
                Debug.LogWarning("Memory Fragments reference is missing!");
            }
        }

        storyRoutine = null;
    }

    private void UpdateTrackerVisuals()
    {
        SetTrackerText(trackerClue1Text, 0);
        SetTrackerText(trackerClue2Text, 1);
        SetTrackerText(trackerClue3Text, 2);
    }

    private void SetTrackerText(TMP_Text clueText, int clueIndex)
    {
        if (clueText == null)
            return;

        string[] clueNames =
        {
            "◉ Old Train Ticket",
            "◉ Broken Watch",
            "◉ Ghost Photograph"
        };

        clueText.text = clueNames[clueIndex];
        clueText.color = cluesCollected[clueIndex]
            ? mutedColor
            : (clueIndex == currentClueIndex ? activeColor : mutedColor);

        clueText.fontStyle = clueIndex == currentClueIndex
            && !cluesCollected[clueIndex]
            ? FontStyles.Bold
            : FontStyles.Normal;
    }

    public void HideMissionBriefing()
    {
        StopTypingSound();

        if (briefingRoutine != null)
        {
            StopCoroutine(briefingRoutine);
            briefingRoutine = null;
        }

        if (missionBriefingPanel != null)
            missionBriefingPanel.SetActive(false);
    }
}
