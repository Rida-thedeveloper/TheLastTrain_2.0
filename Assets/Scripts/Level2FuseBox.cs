
using UnityEngine;

public class Level2FuseBox : MonoBehaviour
{
    [Header("Level 2 Objective Panel")]
    public Level2ObjectivePanelManager objectiveManager;

    [Header("Spider")]
    public GameObject spider;

    [Header("Battery")]
    public Level2Battery battery;

    [Header("Ending")]
    public Level2EndingSequence endingSequence;

    private bool playerNearby = false;
    private bool firstInteractionDone = false;
    private bool endingTriggered = false;

    void Start()
    {
        if (spider != null)
            spider.SetActive(false);
    }

    void Update()
    {
        if (!playerNearby || endingTriggered ||
            !Input.GetKeyDown(KeyCode.E))
            return;

        // First interaction: show the missing battery objective.
        if (!firstInteractionDone)
        {
            firstInteractionDone = true;

            if (objectiveManager != null)
            {
                objectiveManager.ShowFindSpiderObjective(
                    "BATTERY MISSING",
                    "You opened the fuse box, but something is wrong. The power system has completely failed, and the backup battery is missing. Without a working battery, there is no way to restore the lights. You must find a replacement battery somewhere inside this dark train carriage. Hurry, because you don't have enough time and be careful of the things hiding in darkness.",
                    "LET'S FIND",
                    false
                );
            }
            else
            {
                Debug.LogError("Level 2 Objective Manager is missing!");
            }

            return;
        }

        // Second interaction: only proceed if the battery was collected.
        if (battery != null && battery.IsCollected)
        {
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

    public void OnLetsFindClicked()
    {
        if (spider != null)
            spider.SetActive(true);
        else
            Debug.LogError("Spider reference is missing!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
