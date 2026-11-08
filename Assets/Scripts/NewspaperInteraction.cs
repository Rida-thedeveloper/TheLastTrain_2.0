
using UnityEngine;

public class NewspaperInteraction : MonoBehaviour
{
    public float pickupDistance = 2f;

    public GameObject player;
    public GameObject newspaperCanvas;
    public GameObject rustKey;

    [Header("Next Objective")]
    public ObjectivePanelManager objectivePanelManager;

    private bool collected = false;

    void Update()
    {
        // No distance check needed.
        // UniversalInteractable handles the E interaction.
    }

    public void Interact()
    {
        if (collected)
            return;

        CollectNewspaper();
    }

    void CollectNewspaper()
    {
        collected = true;

        // Newspaper hide
        gameObject.SetActive(false);

        // Newspaper story show
        if (newspaperCanvas != null)
            newspaperCanvas.SetActive(true);

        Debug.Log("NEWSPAPER FOUND!");
    }

    public void NextStory()
    {
        // Newspaper story hide
        if (newspaperCanvas != null)
            newspaperCanvas.SetActive(false);

        // Key activate
        if (rustKey != null)
            rustKey.SetActive(true);

        // Show FIND THE KEY objective
        if (objectivePanelManager != null)
        {
            objectivePanelManager.ShowObjective(
                "FIND THE KEY",
                "The newspaper revealed a mysterious train accident that happened years ago and this train may be connected to what happened that night. Find the key and see if it can lead you closer to the truth.",
                "LET'S FIND",
                false
            );
        }

        Debug.Log("NEWSPAPER STORY FINISHED → KEY ACTIVATED → FIND THE KEY OBJECTIVE");
    }
}
