
using UnityEngine;
using TMPro;

public class UniversalInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;
    public TMP_Text promptText;

    [Header("Bug Protection")]
    public float bugProtectionDuration = 3f;

    private UniversalInteractable currentObject;

    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (currentObject != null && Input.GetKeyDown(KeyCode.E))
        {
            // Activate temporary protection only for tagged objects
            if (IsProtectedObject(currentObject))
            {
                BugInteraction.BlockScareForSeconds(
                    bugProtectionDuration
                );
            }

            currentObject.Interact();
            HidePrompt();
        }
    }

    bool IsProtectedObject(UniversalInteractable interactable)
    {
        Transform target = interactable.transform;

        // Check the interactable and its parent objects
        Transform current = target;

        while (current != null)
        {
            if (current.CompareTag("NoBugScare"))
                return true;

            current = current.parent;
        }

        // Check colliders on the interactable and its children
        Collider[] colliders =
            target.GetComponentsInChildren<Collider>(true);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("NoBugScare"))
                return true;
        }

        return false;
    }

    public void ShowPrompt(UniversalInteractable interactable)
    {
        currentObject = interactable;
        promptText.text = interactable.promptMessage;
        interactionPrompt.SetActive(true);
    }

    public void HidePrompt()
    {
        currentObject = null;
        interactionPrompt.SetActive(false);
    }
}
