
using System.Collections;
using UnityEngine;
using TMPro;

public class Level3MissionUI : MonoBehaviour
{
    [Header("Mission UI")]
    public GameObject missionBriefingPanel;
    public GameObject ghostHorrorMessage;
    public GameObject clueTrackerPanel;

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

    [Header("Tracker Text")]
    public TMP_Text trackerClue1Text;
    public TMP_Text trackerClue2Text;
    public TMP_Text trackerClue3Text;

    [Header("Tracker Colors")]
    public Color activeColor = Color.white;
    public Color mutedColor = new Color(0.55f, 0.55f, 0.55f);

    private bool[] cluesCollected = new bool[3];
    private Coroutine storyRoutine;
    private int currentClueIndex = 0;

    public void ShowMissionBriefing()
    {
        if (ghostHorrorMessage != null)
            ghostHorrorMessage.SetActive(false);

        if (missionBriefingPanel != null)
            missionBriefingPanel.SetActive(true);
    }

    public void StartFindingClues()
    {
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
        if (storyMessageObject != null && storyMessageText != null)
        {
            storyMessageText.text = story;
            storyMessageObject.SetActive(true);

            yield return new WaitForSeconds(storyMessageDuration);

            storyMessageObject.SetActive(false);
        }

        currentClueIndex = clueIndex + 1;
        UpdateTrackerVisuals();

        if (clueIndex == 0 && brokenWatch != null)
            brokenWatch.SetActive(true);
        else if (clueIndex == 1 && ghostPhotograph != null)
            ghostPhotograph.SetActive(true);
        else if (clueIndex == 2)
            Debug.Log("All three clues collected!");

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
        if (missionBriefingPanel != null)
            missionBriefingPanel.SetActive(false);
    }
}
