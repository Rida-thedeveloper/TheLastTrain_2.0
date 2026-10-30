
using UnityEngine;

public class Level2Battery : MonoBehaviour
{
    [Header("Battery References")]
    public GameObject batteryModel;

    [Header("Level 2 Objective Panel")]
    public Level2ObjectivePanelManager objectiveManager;

    private bool playerNearby = false;
    private bool collected = false;

    public bool IsCollected => collected;

    void Start()
    {
        if (batteryModel == null && transform.childCount > 0)
        {
            batteryModel = transform.GetChild(0).gameObject;
        }
    }

    void Update()
    {
        if (playerNearby &&
            !collected &&
            Input.GetKeyDown(KeyCode.E))
        {
            collected = true;

            if (batteryModel != null)
                batteryModel.SetActive(false);

            if (objectiveManager != null)
            {
                objectiveManager.ShowBatteryFoundObjective(
                    "BATTERY FOUND!",
                    "You found a spare battery hidden inside the dark train carriage. This might be your only chance to bring the power back. The emergency lights are still dead, and something may be lurking in the darkness. Hurry back to the fuse box, replace the dead battery, and restore the power before time runs out. You’re one step closer to escaping this nightmare… but don’t let your guard down.",
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
