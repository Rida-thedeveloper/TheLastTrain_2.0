using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
    public GameObject levelCompleteCanvas;

    public void ShowLevelComplete()
    {
        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void NextLevel()
    {
        Debug.Log("NEXT LEVEL");
        SceneManager.LoadScene("Level2");
    }
}