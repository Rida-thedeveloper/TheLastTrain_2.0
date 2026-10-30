using UnityEngine;
using UnityEngine.Video;

public class IntroVideoLoader : MonoBehaviour
{
    public GameObject train;
    public GameObject player;
    public GameObject intro;

    public GameObject ObjectiveCanvas;
    public GameObject ObjectivePanel;
    public ObjectivePanelManager objectivePanelManager;

    [Header("Development")]

    public bool developmentMode = false;

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

        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += VideoFinished;

        // DEVELOPMENT MODE
        if (developmentMode)
        {
            if (train != null)
                train.SetActive(true);

            if (player != null)
                player.SetActive(true);

            if (intro != null)
                intro.SetActive(false);

            if (ObjectiveCanvas != null)
                ObjectiveCanvas.SetActive(true);

            if (ObjectivePanel != null)
                ObjectivePanel.SetActive(true);

            return;
        }

        // FINAL GAME MODE
        // Start mein train aur player hide
        if (train != null)
            train.SetActive(false);

        if (player != null)
            player.SetActive(false);

        // Intro visible rahe
        if (intro != null)
            intro.SetActive(true);

        // Objective start mein hide
        if (ObjectiveCanvas != null)
            ObjectiveCanvas.SetActive(false);

        // Video automatically play nahi hogi
    }

    public void PlayIntro()
    {
        Debug.Log("PLAY BUTTON → INTRO START");

        // Video start hote hi objective hide
        if (ObjectiveCanvas != null)
            ObjectiveCanvas.SetActive(false);

        if (intro != null)
            intro.SetActive(true);

        videoPlayer.Play();
    }

    void VideoFinished(VideoPlayer vp)
    {
        if (videoFinished)
            return;

        videoFinished = true;

        Debug.Log("INTRO VIDEO FINISHED!");

        if (intro != null)
            intro.SetActive(false);

        if (train != null)
            train.SetActive(true);

        if (player != null)
            player.SetActive(true);

        // Video khatam → first objective show
        if (ObjectiveCanvas != null)
            ObjectiveCanvas.SetActive(true);

        if (objectivePanelManager != null)
        {
            objectivePanelManager.ShowFirstObjective(
                "SOMETHING IS FISHY",
                "The train was normal just moments ago. There were people everywhere, but now the entire carriage is strangely empty. Something has clearly changed. Search the train car and find out what happened.",
                "LET'S BEGIN"
            );
        }


    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= VideoFinished;
        }
    }
}
