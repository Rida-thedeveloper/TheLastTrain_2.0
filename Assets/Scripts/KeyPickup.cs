
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float pickupDistance = 1f;

    public GameObject player;

    [Header("Objective")]
    public ObjectivePanelManager objectivePanelManager;

    [Header("Door Trigger")]
    public Collider doorTrigger;

    [Header("Door Glow")]
    public GameObject doorGlowEffects;

    private bool keyCollected = false;
    private bool canInteract = false;

    void Start()
    {
        // Door interaction disabled initially
        if (doorTrigger != null)
            doorTrigger.enabled = false;

        // Door glow disabled initially
        if (doorGlowEffects != null)
            doorGlowEffects.SetActive(false);
    }

    void Update()
    {
        if (keyCollected || player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= pickupDistance && canInteract)
        {
            PickupKey();
            canInteract = false;
        }
    }

    public void Interact()
    {
        if (!keyCollected)
        {
            canInteract = true;
        }
    }

    void PickupKey()
    {
        keyCollected = true;

        // Enable Door interaction after collecting key
        if (doorTrigger != null)
        {
            doorTrigger.enabled = true;
            Debug.Log("DOOR TRIGGER ENABLED!");
        }
        else
        {
            Debug.LogError("Door Trigger reference is missing!", this);
        }

        // Enable Door Glow after collecting key
        if (doorGlowEffects != null)
        {
            doorGlowEffects.SetActive(true);
            Debug.Log("DOOR GLOW ENABLED!");
        }

        // Key disappear
        gameObject.SetActive(false);

        // Show next objective
        if (objectivePanelManager != null)
        {
            objectivePanelManager.ShowObjective(
                "FOUND THE KEY",
                "You found the key. Find out what is waiting ahead. Use the key and open the door.",
                "OPEN DOOR",
                true
            );
        }

        Debug.Log("KEY PICKED UP → OPEN THE DOOR OBJECTIVE");
    }
}
