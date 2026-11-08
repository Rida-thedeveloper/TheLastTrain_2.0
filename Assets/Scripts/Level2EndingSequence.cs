
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Level2EndingSequence : MonoBehaviour
{
    [Header("Ending Video")]
    public VideoPlayer endingVideoPlayer;

    [Header("Level Complete Screen")]
    public GameObject levelCompleteCanvas;

    [Header("Player")]
    public GameObject player;

    [Header("Fuse Box")]
    public GameObject fuseBox;

    [Header("Timer")]
    public TimerManager timerManager;

    private bool endingStarted = false;

    void Start()
    {
        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(false);

        if (endingVideoPlayer != null)
        {
            endingVideoPlayer.playOnAwake = false;
            endingVideoPlayer.isLooping = false;
        }
    }

    public void PlayEnding()
    {
        if (endingStarted)
            return;

        if (endingVideoPlayer == null)
        {
            Debug.LogError("Ending VideoPlayer is not assigned!");
            return;
        }

        if (endingVideoPlayer.clip == null)
        {
            Debug.LogError("Ending video clip is not assigned!");
            return;
        }

        endingStarted = true;

        // Stop and disable the Level 2 timer
        if (timerManager != null)
        {
            timerManager.StopTimer();

            if (timerManager.timerTextObject != null)
                timerManager.timerTextObject.SetActive(false);

            timerManager.enabled = false;

            Debug.Log("Level 2 Timer stopped and disabled!");
        }
        else
        {
            Debug.LogError("TimerManager reference is missing!");
        }

        if (fuseBox != null)
        {
            fuseBox.SetActive(false);
        }

        if (player != null)
        {
            FirstPersonController controller =
                player.GetComponent<FirstPersonController>();

            if (controller != null)
                controller.enabled = false;
        }

        endingVideoPlayer.loopPointReached -= EndingFinished;
        endingVideoPlayer.loopPointReached += EndingFinished;

        endingVideoPlayer.Stop();
        endingVideoPlayer.Play();

        Debug.Log("Level 2 ending video started!");
    }

    void EndingFinished(VideoPlayer vp)
    {
        vp.Stop();
        vp.loopPointReached -= EndingFinished;

        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("LEVEL 2 COMPLETE!");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void NextLevel()
    {
        Debug.Log("NEXT LEVEL!");
        SceneManager.LoadScene("Level3");
    }
}
