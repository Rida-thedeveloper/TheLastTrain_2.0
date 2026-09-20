using UnityEngine;
using UnityEngine.Video;

public class IntroVideoLoader : MonoBehaviour
{
    public GameObject train;
    public GameObject player;
    public GameObject intro;

    private VideoPlayer videoPlayer;
    private bool videoFinished = false;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component nahi mila!");
            return;
        }

        // Video sirf aik baar chalegi
        videoPlayer.isLooping = false;

        // Video khatam hone par VideoFinished function chalega
        videoPlayer.loopPointReached += VideoFinished;

        // Start mein train hide
        if (train != null)
            train.SetActive(false);

        // Start mein player hide
        if (player != null)
            player.SetActive(false);

        // Intro video show
        if (intro != null)
            intro.SetActive(true);

        // Video play
        videoPlayer.Play();
    }

    void VideoFinished(VideoPlayer vp)
    {
        if (videoFinished)
            return;

        videoFinished = true;

        Debug.Log("INTRO VIDEO FINISHED!");

        // Intro video hide
        if (intro != null)
            intro.SetActive(false);

        // Train show
        if (train != null)
            train.SetActive(true);

        // Player show
        if (player != null)
            player.SetActive(true);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= VideoFinished;
        }
    }
}