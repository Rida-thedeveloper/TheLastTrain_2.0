
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Level3Ending : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform finalDoor;
    public VideoPlayer endingVideo;

    [Header("UI")]
    public GameObject levelCompleteCanvas;

    [Header("Settings")]
    public float endingDistance = 1.2f;
    public string nextLevelSceneName = "Level4";
    public string currentLevelSceneName = "Level3";

    private bool endingStarted = false;
    private bool endingPointEnabled = false;

    private void Start()
    {
        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(false);

        if (endingVideo != null)
        {
            endingVideo.playOnAwake = false;
            endingVideo.isLooping = false;
            endingVideo.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("Ending Video reference is missing!");
        }
    }

    // Called when grandmother appears after memory fragments
    public void EnableEndingPoint()
    {
        if (endingStarted)
            return;

        endingPointEnabled = true;

        Debug.Log("Ending point is now enabled!");
    }

    private void Update()
    {
        // Do not check distance until grandmother appears
        if (!endingPointEnabled || endingStarted)
            return;

        if (player == null || finalDoor == null || endingVideo == null)
            return;

        Vector3 playerPosition = player.position;
        Vector3 doorPosition = finalDoor.position;

        playerPosition.y = 0f;
        doorPosition.y = 0f;

        float distance = Vector3.Distance(
            playerPosition,
            doorPosition
        );

        if (distance <= endingDistance)
        {
            StartEnding();
        }
    }

    private void StartEnding()
    {
        if (endingStarted)
            return;

        endingStarted = true;

        endingVideo.Play();

        Debug.Log("Level 3 Ending Video Started!");
    }

    private void OnVideoFinished(VideoPlayer video)
    {
        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelSceneName);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(currentLevelSceneName);
    }

    private void OnDestroy()
    {
        if (endingVideo != null)
            endingVideo.loopPointReached -= OnVideoFinished;
    }
}
