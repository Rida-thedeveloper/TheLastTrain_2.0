
using UnityEngine;
using UnityEngine.Video;

public class Level2SpiderScare : MonoBehaviour
{
    [Header("Flashlight")]
    public Light flashlight;

    [Header("Spider")]
    public Collider spiderCollider;

    [Header("Scare Video")]
    public GameObject videoCanvas;
    public VideoPlayer videoPlayer;

    [Header("Player")]
    public GameObject player;

    [Header("Battery")]
    public Level2Battery battery;

    private bool scarePlayed = false;
    private bool videoPlaying = false;

    void Start()
    {
        if (videoCanvas != null)
            videoCanvas.SetActive(false);

        if (battery != null && battery.batteryModel != null)
            battery.batteryModel.SetActive(false);

        if (flashlight == null)
            Debug.LogError("SpiderScare: Flashlight Light is not assigned!", this);

        if (spiderCollider == null)
            Debug.LogError("SpiderScare: Spider Collider is not assigned!", this);

        if (videoPlayer == null)
            Debug.LogError("SpiderScare: VideoPlayer is not assigned!", this);
    }

    void Update()
    {
        if (scarePlayed || videoPlaying)
            return;

        if (flashlight == null || spiderCollider == null)
            return;

        if (!flashlight.enabled ||
            !flashlight.gameObject.activeInHierarchy)
            return;

        Vector3 origin = flashlight.transform.position;
        Vector3 target = spiderCollider.bounds.center;
        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        if (distance <= 0.01f || distance > flashlight.range)
            return;

        float angle = Vector3.Angle(
            flashlight.transform.forward,
            direction
        );

        if (angle > flashlight.spotAngle * 0.5f)
            return;

        // Check whether the spider is the first object hit.
        if (Physics.Raycast(
            origin,
            direction.normalized,
            out RaycastHit hit,
            distance + 0.2f,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide))
        {
            bool hitSpider =
                hit.collider == spiderCollider ||
                hit.collider.transform.IsChildOf(spiderCollider.transform) ||
                spiderCollider.transform.IsChildOf(hit.collider.transform);

            if (hitSpider)
            {
                Debug.Log("Spider detected by flashlight!");
                PlayScareVideo();
            }
            else
            {
                // Remove this log later if the Console gets too busy.
                Debug.DrawRay(origin, direction.normalized * distance, Color.red);
            }
        }
    }

    void PlayScareVideo()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("Spider scare VideoPlayer is missing!", this);
            return;
        }

        scarePlayed = true;
        videoPlaying = true;

        if (player != null)
        {
            FirstPersonController controller =
                player.GetComponent<FirstPersonController>();

            if (controller != null)
                controller.enabled = false;
        }

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached -= VideoFinished;
        videoPlayer.loopPointReached += VideoFinished;

        videoPlayer.Stop();

        if (videoCanvas != null)
            videoCanvas.SetActive(true);

        videoPlayer.Play();

        Debug.Log("Spider scare video started.");
    }

    void VideoFinished(VideoPlayer vp)
    {
        vp.Stop();
        vp.loopPointReached -= VideoFinished;

        if (videoCanvas != null)
            videoCanvas.SetActive(false);

        if (battery != null && battery.batteryModel != null)
            battery.batteryModel.SetActive(true);

        if (player != null)
        {
            FirstPersonController controller =
                player.GetComponent<FirstPersonController>();

            if (controller != null)
                controller.enabled = true;
        }

        videoPlaying = false;
        gameObject.SetActive(false);

        Debug.Log("Spider scare finished. Battery activated.");
    }
}
