
using UnityEngine;

public class CluePickup : MonoBehaviour
{
    public string clueName = "Old Train Ticket";
    public GameObject pickupPrompt;
    public GameObject clueVisual;
    public Level3MissionUI missionUI;
    public int clueIndex = 0;

    private bool playerNearby = false;
    private bool collected = false;

    private void Start()
    {
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
    }

    private void Update()
    {
        if (!playerNearby || collected)
            return;

        if (Input.GetKeyDown(KeyCode.E))
            CollectClue();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (other.GetComponentInParent<CharacterController>() == null)
            return;

        playerNearby = true;

        if (pickupPrompt != null)
            pickupPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<CharacterController>() == null)
            return;

        playerNearby = false;

        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
    }

    private void CollectClue()
    {
        collected = true;
        playerNearby = false;

        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);

        if (clueVisual != null)
            clueVisual.SetActive(false);

        if (missionUI != null)
            missionUI.OnClueCollected(clueIndex);

        Debug.Log(clueName + " collected!");
    }
}
