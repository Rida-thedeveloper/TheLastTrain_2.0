using UnityEngine;
using UnityEngine.Video;

public class LocketPickup : MonoBehaviour
{
    public GameObject pickupPrompt;
    public GameObject locketObject;
    public VideoPlayer endingVideoPlayer;

    private bool playerNearby = false;
    private bool videoStarted = false;

    void Start()
    {
        pickupPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !videoStarted)
        {
            videoStarted = true;

            pickupPrompt.SetActive(false);
            locketObject.SetActive(false);

            endingVideoPlayer.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            pickupPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            pickupPrompt.SetActive(false);
        }
    }
}