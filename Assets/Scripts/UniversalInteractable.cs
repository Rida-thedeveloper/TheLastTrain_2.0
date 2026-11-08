
using UnityEngine;
using UnityEngine.Events;

public class UniversalInteractable : MonoBehaviour
{
    [Header("Prompt")]
    [TextArea]
    public string promptMessage = "Press E to interact";

    [Header("Interaction")]
    public UnityEvent onInteract;

    private UniversalInteraction interactionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionManager == null)
            {
                interactionManager = FindObjectOfType<UniversalInteraction>();
            }

            if (interactionManager != null)
            {
                interactionManager.ShowPrompt(this);
            }
            else
            {
                Debug.LogError("UniversalInteraction not found in the scene!", this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionManager != null)
            {
                interactionManager.HidePrompt();
            }
        }
    }

    public void Interact()
    {
        onInteract.Invoke();
    }
}
