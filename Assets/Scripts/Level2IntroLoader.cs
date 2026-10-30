
using UnityEngine;
using UnityEngine.Video;

public class Level2IntroLoader : MonoBehaviour
{
    [Header("Intro Video")]
    public VideoPlayer videoPlayer;

    [Header("Gameplay")]
    public GameObject train;
    public GameObject player;
    public Level2ObjectiveController objectiveController;
    private bool videoFinished = false;

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            Debug.LogError("Level 2 VideoPlayer missing!");
            return;
        }

        // Hide gameplay while the intro plays
        if (train != null)
            train.SetActive(false);

        if (player != null)
            player.SetActive(false);

        videoPlayer.isLooping = false;
        videoPlayer.playOnAwake = false;

        videoPlayer.loopPointReached -= VideoFinished;
        videoPlayer.loopPointReached += VideoFinished;

        videoPlayer.Play();

        Debug.Log("LEVEL 2 INTRO STARTED!");
    }

    void VideoFinished(VideoPlayer vp)
    {
        if (videoFinished)
            return;

        videoFinished = true;

        videoPlayer.Stop();

        if (train != null)
            train.SetActive(true);

        if (player != null)
            player.SetActive(true);

        if (objectiveController != null)
            objectiveController.ShowPowerFailureObjective();

        Debug.Log("LEVEL 2 INTRO FINISHED!");
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= VideoFinished;
    }
}
