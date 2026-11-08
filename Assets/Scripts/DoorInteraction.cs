
using UnityEngine;
using UnityEngine.Video;

public class DoorInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject door;
    public GameObject key;
    public GameObject newspaper;

    [Header("Door Cutscene")]
    public VideoPlayer doorCutscene;
    public GameObject doorCutsceneObject;

    [Header("Gameplay Objects")]
    public GameObject train;
    public GameObject objectiveCanvas;

    private bool cutscenePlaying = false;

    void Start()
    {
        if (doorCutscene != null)
        {
            doorCutscene.loopPointReached += CutsceneFinished;
        }
    }

    public void Interact()
    {
        if (cutscenePlaying)
            return;

        // Newspaper aur Key dono inactive hone chahiye
        if (newspaper != null && !newspaper.activeSelf &&
            key != null && !key.activeSelf)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("You need to find the newspaper and key first!");
        }
    }

    void OpenDoor()
    {
        Debug.Log("NEWSPAPER + KEY FOUND → DOOR CUTSCENE START");

        // Stop and disable timer
        TimerManager timer = FindFirstObjectByType<TimerManager>();

        if (timer != null)
        {
            timer.StopTimer();

            if (timer.timerTextObject != null)
                timer.timerTextObject.SetActive(false);

            timer.enabled = false;

            Debug.Log("Timer stopped and disabled!");
        }

        cutscenePlaying = true;

        // Stop player movement, but KEEP player/camera active
        if (player != null)
        {
            FirstPersonController controller =
                player.GetComponent<FirstPersonController>();

            if (controller != null)
                controller.enabled = false;
        }

        // Hide gameplay
        if (train != null)
            train.SetActive(false);

        if (objectiveCanvas != null)
            objectiveCanvas.SetActive(false);

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Show and play video
        if (doorCutsceneObject != null)
            doorCutsceneObject.SetActive(true);

        if (doorCutscene != null)
            doorCutscene.Play();
        else
            Debug.LogError("Door Cutscene VideoPlayer missing!");
    }

    void CutsceneFinished(VideoPlayer vp)
    {
        Debug.Log("DOOR CUTSCENE FINISHED!");

        if (doorCutsceneObject != null)
            doorCutsceneObject.SetActive(false);

        Debug.Log("LEVEL 1 COMPLETED!");

        LevelCompleteManager manager =
            FindFirstObjectByType<LevelCompleteManager>();

        if (manager != null)
            manager.ShowLevelComplete();
    }

    void OnDestroy()
    {
        if (doorCutscene != null)
        {
            doorCutscene.loopPointReached -= CutsceneFinished;
        }
    }
}
