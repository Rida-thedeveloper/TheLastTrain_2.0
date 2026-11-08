
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

    [Header("Temporary Protection")]
    public static float protectedUntil = 0f;

    private bool videoPlaying = false;

    public static void BlockScareForSeconds(float seconds)
    {
        protectedUntil = Mathf.Max(
            protectedUntil,
            Time.time + seconds
        );
    }

    void Update()
    {
        if (!allowVideoInteraction || videoPlaying || player == null)
            return;

        // Temporary protection after collecting/interacting
        if (Time.time < protectedUntil)
            return;

        // Original protected-area detection
        if (IsPlayerInsideProtectedArea())
            return;

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

    bool IsPlayerInsideProtectedArea()
    {
        Collider playerCollider = player.GetComponent<Collider>();

        Collider[] nearbyColliders;

        if (playerCollider != null)
        {
            Bounds bounds = playerCollider.bounds;

            nearbyColliders = Physics.OverlapBox(
                bounds.center,
                bounds.extents,
                Quaternion.identity,
                ~0,
                QueryTriggerInteraction.Collide
            );
        }
        else
        {
            nearbyColliders = Physics.OverlapSphere(
                player.transform.position,
                0.25f,
                ~0,
                QueryTriggerInteraction.Collide
            );
        }

        foreach (Collider col in nearbyColliders)
        {
            if (col.CompareTag("NoBugScare"))
                return true;
        }

        return false;
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
