using UnityEngine;
using TMPro;

public class ObjectivePanelManager : MonoBehaviour
{
    public GameObject objectivePanel;

    [Header("UI Text")]
    public TMP_Text objectiveTitle;
    public TMP_Text objectiveDescription;
    public TMP_Text buttonText;

    [Header("Green Tick")]
    public GameObject greenTick;

    [Header("Timer")]
    public TimerManager timerManager;

    private bool startTimerOnNextClick = false;

    public void ClosePanel()
    {
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
        if (objectiveTitle != null)
            objectiveTitle.text = title;

        if (objectiveDescription != null)
            objectiveDescription.text = description;

        if (buttonText != null)
            buttonText.text = buttonLabel;

        if (greenTick != null)
            greenTick.SetActive(showGreenTick);

        if (objectivePanel != null)
            objectivePanel.SetActive(true);
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
}