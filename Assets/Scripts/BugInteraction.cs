
using UnityEngine;
using UnityEngine.Video;

public class BugInteraction : MonoBehaviour
{
    public float pickupDistance = 1.5f;
    public bool allowVideoInteraction = true;

    [Header("References")]
    public GameObject player;

    [Header("Optional Bug Video References")]
    public GameObject bugVideoCanvas;
    public VideoPlayer bugVideoPlayer;

    private bool videoPlaying = false;

    void Update()
    {
        // Videos disabled: do nothing, no video references needed.
        if (!allowVideoInteraction || videoPlaying || player == null)
            return;

        // Avoid trying to play if required references are missing.
        if (bugVideoPlayer == null || bugVideoCanvas == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= pickupDistance &&
            Input.GetKeyDown(KeyCode.E))
        {
            PlayBugVideo();
        }
    }

    void PlayBugVideo()
    {
        if (!allowVideoInteraction ||
            bugVideoPlayer == null ||
            bugVideoCanvas == null)
            return;

        videoPlaying = true;

        Debug.Log("BUG VIDEO START");

        FirstPersonController controller =
            player.GetComponent<FirstPersonController>();

        if (controller != null)
            controller.enabled = false;

        bugVideoCanvas.SetActive(true);
        bugVideoPlayer.gameObject.SetActive(true);

        bugVideoPlayer.loopPointReached -= VideoFinished;
        bugVideoPlayer.loopPointReached += VideoFinished;

        bugVideoPlayer.Stop();
        bugVideoPlayer.Play();
    }

    void VideoFinished(VideoPlayer vp)
    {
        Debug.Log("BUG VIDEO FINISHED");

        vp.Stop();
        vp.loopPointReached -= VideoFinished;

        if (bugVideoCanvas != null)
            bugVideoCanvas.SetActive(false);

        if (bugVideoPlayer != null)
            bugVideoPlayer.gameObject.SetActive(false);

        if (player != null)
        {
            FirstPersonController controller =
                player.GetComponent<FirstPersonController>();

            if (controller != null)
                controller.enabled = true;
        }

        videoPlaying = false;

        Debug.Log("GAMEPLAY RESUMED");
    }
}
