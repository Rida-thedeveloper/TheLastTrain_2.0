
using UnityEngine;

public class Level2Battery : MonoBehaviour
{
    [Header("Battery References")]
    public GameObject batteryModel;
    public Collider batteryTrigger;

    [Header("Fuse Box")]
    public Level2FuseBox fuseBox;

    [Header("Level 2 Objective Panel")]
    public Level2ObjectivePanelManager objectiveManager;

    private bool collected = false;

    public bool IsCollected => collected;

    void Start()
    {
        if (batteryModel == null && transform.childCount > 0)
        {
            batteryModel = transform.GetChild(0).gameObject;
        }

        if (batteryTrigger == null)
            batteryTrigger = GetComponent<Collider>();

        if (batteryTrigger != null)
            batteryTrigger.enabled = false;

        if (batteryModel != null)
            batteryModel.SetActive(false);
    }

    public void ActivateBattery()
    {
        if (collected)
            return;

        if (batteryModel != null)
            batteryModel.SetActive(true);

        if (batteryTrigger != null)
            batteryTrigger.enabled = true;

        Debug.Log("Battery visual and interaction trigger activated!");
    }

    // Called by UniversalInteractable
    public void Interact()
    {
        if (collected)
            return;

        collected = true;

        if (batteryModel != null)
            batteryModel.SetActive(false);

        if (batteryTrigger != null)
            batteryTrigger.enabled = false;

        // Re-enable Fuse Box after collecting battery
        if (fuseBox != null)
        {
            fuseBox.EnableFuseBoxInteraction();
        }
        else
        {
            Debug.LogError("Level2FuseBox reference is missing!", this);
        }

        if (objectiveManager != null)
        {
            objectiveManager.ShowBatteryFoundObjective(
                "BATTERY FOUND!",
                "You found a spare battery now hurry back to the fuse box, replace the dead battery, and restore the power before time runs out.",
                "CONTINUE",
                false
            );
        }
        else
        {
            Debug.LogError(
                "Level2ObjectivePanelManager is not assigned!"
            );
        }
    }
}
