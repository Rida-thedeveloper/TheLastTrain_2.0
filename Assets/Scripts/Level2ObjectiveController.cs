
using UnityEngine;

public class Level2ObjectiveController : MonoBehaviour
{
    [Header("Level 2 References")]
    public Level2ObjectivePanelManager objectiveManager;
    public GameObject flashlight;

    [Header("Objective")]
    public string objectiveTitle = "POWER FAILURE";

    [TextArea(3, 5)]
    public string objectiveDescription =
        "The train has plunged into darkness, and all power systems have failed. Find the fuse box hidden somewhere inside the carriage and restore the power before time runs out. Be careful… something may be lurking in the dark.";

    private bool objectiveShown = false;

    void Start()
    {
        if (flashlight != null)
            flashlight.SetActive(false);
    }

    public void ShowPowerFailureObjective()
    {
        if (objectiveShown)
            return;

        objectiveShown = true;

        if (objectiveManager != null)
        {
            objectiveManager.ShowFirstObjective(
                objectiveTitle,
                objectiveDescription,
                "CONTINUE"
            );
        }
        else
        {
            Debug.LogError(
                "Level2ObjectivePanelManager is not assigned!"
            );
        }
    }

    public void ContinueGameplay()
    {
        if (flashlight != null)
            flashlight.SetActive(true);
        else
            Debug.LogError("Flashlight reference is missing!");
    }
}
