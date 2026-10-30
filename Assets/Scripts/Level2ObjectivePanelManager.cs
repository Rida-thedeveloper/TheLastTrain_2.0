
using UnityEngine;
using TMPro;

public class Level2ObjectivePanelManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject objectivePanel;
    public TMP_Text objectiveTitle;
    public TMP_Text objectiveDescription;
    public TMP_Text buttonText;
    public GameObject greenTick;

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

    public void ShowFirstObjective(
        string title,
        string description,
        string buttonLabel)
    {
        startTimerOnNextClick = true;

        ShowObjective(title, description, buttonLabel, false);

        currentButtonAction = ButtonAction.InitialContinue;
    }

    public void ShowFindSpiderObjective(
        string title,
        string description,
        string buttonLabel,
        bool showGreenTick)
    {
        ShowObjective(title, description, buttonLabel, showGreenTick);

        currentButtonAction = ButtonAction.FindSpider;
    }

    public void ShowBatteryFoundObjective(
        string title,
        string description,
        string buttonLabel,
        bool showGreenTick)
    {
        ShowObjective(title, description, buttonLabel, showGreenTick);

        currentButtonAction = ButtonAction.CloseOnly;
    }

    public void ShowObjective(
        string title,
        string description,
        string buttonLabel,
        bool showGreenTick)
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
                break;

            default:

                ClosePanel();
                break;
        }
    }

    public void ClosePanel()
    {
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
}
