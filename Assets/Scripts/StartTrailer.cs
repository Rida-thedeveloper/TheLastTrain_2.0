using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class StartTrailer : MonoBehaviour
{
    public GameObject thumbnail;
    public GameObject playButton;
    public GameObject videoScreen;

    public VideoPlayer videoPlayer;

    public string nextSceneName = "Untitled 2";

    void Start()
    {
        videoPlayer.loopPointReached += VideoFinished;
    }

    public void PlayTrailer()
    {
        Debug.Log("PlayTrailer CALLED");   // button click aa raha hai ya nahi

        if (thumbnail == null) Debug.LogError("thumbnail reference NULL hai!");
        else thumbnail.SetActive(false);

        if (playButton == null) Debug.LogError("playButton reference NULL hai!");
        else playButton.SetActive(false);

        if (videoScreen == null) Debug.LogError("videoScreen reference NULL hai!");
        else
        {
            videoScreen.SetActive(true);
            Debug.Log("VideoScreen ACTIVE ho gaya: " + videoScreen.activeInHierarchy);
        }

        if (videoPlayer == null) Debug.LogError("videoPlayer reference NULL hai!");
        else videoPlayer.Play();
    }

    void VideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}