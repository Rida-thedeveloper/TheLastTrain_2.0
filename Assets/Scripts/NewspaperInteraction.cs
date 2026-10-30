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
        if (collected || player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= pickupDistance && Input.GetKeyDown(KeyCode.E))
        {
            CollectNewspaper();
        }
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
                "The newspaper revealed a mysterious train accident that happened years ago. The date seems strangely familiar, and this train may be connected to what happened that night. There must be a reason you found this newspaper here. Find the key and see if it can lead you closer to the truth.",
                "LET'S FIND",
                false
            );
        }

        Debug.Log("NEWSPAPER STORY FINISHED → KEY ACTIVATED → FIND THE KEY OBJECTIVE");
    }
}