
using UnityEngine;

public class Level2FuseBox : MonoBehaviour
{
    [Header("Level 2 Objective Panel")]
    public Level2ObjectivePanelManager objectiveManager;

    [Header("Spider")]
    public GameObject spider;

    [Header("Battery")]
    public Level2Battery battery;

    [Header("Fuse Box Trigger")]
    public Collider fuseBoxTrigger;

    [Header("Fuse Box Glow")]
    public GameObject fuseBoxGlow;

    [Header("Ending")]
    public Level2EndingSequence endingSequence;

    private bool firstInteractionDone = false;
    private bool endingTriggered = false;

    void Start()
    {
        if (spider != null)
            spider.SetActive(false);

        if (fuseBoxTrigger == null)
            fuseBoxTrigger = GetComponent<Collider>();

        if (fuseBoxTrigger != null)
            fuseBoxTrigger.enabled = true;

        if (fuseBoxGlow != null)
            fuseBoxGlow.SetActive(false);
    }

    // Called after Power Failure panel button click
    public void EnableInitialGlow()
    {
        if (!firstInteractionDone && !endingTriggered)
        {
            if (fuseBoxGlow != null)
                fuseBoxGlow.SetActive(true);

            Debug.Log("Fuse Box initial glow enabled!");
        }
    }

    // Called after Battery Found panel button click
    public void EnableReturnGlow()
    {
        if (firstInteractionDone &&
            !endingTriggered &&
            battery != null &&
            battery.IsCollected)
        {
            if (fuseBoxGlow != null)
                fuseBoxGlow.SetActive(true);

            Debug.Log("Fuse Box return glow enabled!");
        }
    }

    // Called by UniversalInteractable
    public void Interact()
    {
        if (endingTriggered)
            return;

        // FIRST INTERACTION
        if (!firstInteractionDone)
        {
            firstInteractionDone = true;

            if (fuseBoxGlow != null)
                fuseBoxGlow.SetActive(false);

            if (objectiveManager != null)
            {
                objectiveManager.ShowFindSpiderObjective(
                    "BATTERY MISSING",
                    "You opened the fuse box, but the battery is missing. You must find a replacement battery somewhere inside this dark train carriage. Hurry and be careful.",
                    "LET'S FIND",
                    false
                );
            }
            else
            {
                Debug.LogError("Level 2 Objective Manager is missing!");
            }

            if (fuseBoxTrigger != null)
                fuseBoxTrigger.enabled = false;

            return;
        }

        // SECOND INTERACTION
        if (battery != null && battery.IsCollected)
        {
            if (fuseBoxGlow != null)
                fuseBoxGlow.SetActive(false);

            if (endingSequence != null)
            {
                endingTriggered = true;
                endingSequence.PlayEnding();
            }
            else
            {
                Debug.LogError("Level2EndingSequence is not assigned!");
            }
        }
        else
        {
            Debug.Log("You need to find the replacement battery first!");
        }
    }

    // Called by Level2Battery after collection
    public void EnableFuseBoxInteraction()
    {
        if (firstInteractionDone &&
            !endingTriggered &&
            battery != null &&
            battery.IsCollected)
        {
            if (fuseBoxTrigger != null)
            {
                fuseBoxTrigger.enabled = true;
                Debug.Log("Fuse Box interaction enabled again!");
            }
        }
    }

    public void OnLetsFindClicked()
    {
        if (spider != null)
            spider.SetActive(true);
        else
            Debug.LogError("Spider reference is missing!");
    }
}
