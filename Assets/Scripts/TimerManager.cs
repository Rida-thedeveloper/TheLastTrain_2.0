
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public float timeRemaining = 129f;

    public TMP_Text timerText;
    public GameObject timerTextObject;

    [Header("Game Over")]
    public GameObject gameOverCanvas;

    [Header("Game Over Audio")]
    public AudioSource gameOverAudioSource;
    public AudioClip gameOverSound;
    public AudioClip buttonClickSound;

    private bool timerRunning = false;
    private bool timeUp = false;

    void Start()
    {
        if (timerTextObject != null)
            timerTextObject.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (gameOverAudioSource != null)
        {
            gameOverAudioSource.playOnAwake = false;
            gameOverAudioSource.loop = false;
            gameOverAudioSource.spatialBlend = 0f;
        }
    }

    void Update()
    {
        if (!timerRunning || timeUp)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerDisplay();

            GameOver();
            return;
        }

        UpdateTimerDisplay();
    }

    public void StartTimer()
    {
        if (timeUp)
            return;

        timerRunning = true;

        if (timerTextObject != null)
            timerTextObject.SetActive(true);

        UpdateTimerDisplay();

        Debug.Log("TIMER STARTED!");
    }

    public void StopTimer()
    {
        timerRunning = false;

        Debug.Log("TIMER STOPPED!");
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        if (timerText != null)
        {
            timerText.text =
                "Time Left: " +
                minutes.ToString("0") +
                ":" +
                seconds.ToString("00");
        }
    }

    void GameOver()
    {
        timeUp = true;
        timerRunning = false;

        if (timerTextObject != null)
            timerTextObject.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Game Over panel sound
        if (gameOverAudioSource != null && gameOverSound != null)
        {
            gameOverAudioSource.PlayOneShot(gameOverSound);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FirstPersonController player =
            FindFirstObjectByType<FirstPersonController>();

        if (player != null)
            player.enabled = false;

        Debug.Log("TIME'S UP → GAME OVER!");
    }

    public void RestartLevel()
    {
        // Play button click sound before restarting
        if (gameOverAudioSource != null && buttonClickSound != null)
        {
            gameOverAudioSource.PlayOneShot(buttonClickSound);
        }

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
