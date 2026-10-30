using UnityEngine;

public class StartTrailer : MonoBehaviour
{
    public GameObject thumbnail;
    public GameObject playButton;

    public IntroVideoLoader introVideoLoader;

    public void PlayTrailer()
    {
        Debug.Log("PLAY BUTTON CLICKED!");

        if (thumbnail != null)
            thumbnail.SetActive(false);

        if (playButton != null)
            playButton.SetActive(false);

        if (introVideoLoader != null)
            introVideoLoader.PlayIntro();
        else
            Debug.LogError("IntroVideoLoader reference NULL hai!");
    }
}