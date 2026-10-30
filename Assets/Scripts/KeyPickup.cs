using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float pickupDistance = 1f;

    public GameObject player;

    [Header("Objective")]
    public ObjectivePanelManager objectivePanelManager;

    private bool keyCollected = false;

    void Update()
    {
        if (keyCollected || player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= pickupDistance && Input.GetKeyDown(KeyCode.E))
        {
            PickupKey();
        }
    }

    void PickupKey()
    {
        keyCollected = true;

        // Key disappear
        gameObject.SetActive(false);

        // Show next objective
        if (objectivePanelManager != null)
        {
            objectivePanelManager.ShowObjective(
                "FOUND THE KEY",
                "You found the key. Whatever happened to this train may be connected to the accident described in the newspaper. There is only one way to find out what is waiting ahead. Use the key and open the door.",
                "OPEN DOOR",
                true
            );
        }

        Debug.Log("KEY PICKED UP → OPEN THE DOOR OBJECTIVE");
    }
}