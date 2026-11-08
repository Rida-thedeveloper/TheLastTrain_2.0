
using System.Collections;
using UnityEngine;
using TMPro;

public class GrandmotherGuide : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform finalDoor;
    public Camera playerCamera;

    [Header("Ending")]
    public Level3Ending level3Ending;

    [Header("Follow Message")]
    public GameObject followMessage;
    public TMP_Text followMessageText;
    public float messageFadeSpeed = 1.5f;
    public float viewDistance = 25f;
    public float messageDisplayDuration = 4f;

    [Header("Typing Effect")]
    public float typingSpeed = 0.04f;
    public AudioSource typingAudioSource;
    public AudioClip typingSound;

    [Header("Movement")]
    public float followDistance = 3f;
    public float rotationSpeed = 2f;

    private CanvasGroup messageCanvasGroup;

    private bool isGuiding;
    private bool hasStartedMoving;
    private bool hasDisappeared;
    private bool messageHasShown;

    private Coroutine messageRoutine;

    private Vector3 lastPlayerPosition;
    private Vector3 movementDirection;

    private void Awake()
    {
        if (followMessage != null)
        {
            messageCanvasGroup =
                followMessage.GetComponent<CanvasGroup>();

            if (messageCanvasGroup == null)
            {
                messageCanvasGroup =
                    followMessage.AddComponent<CanvasGroup>();
            }

            messageCanvasGroup.alpha = 0f;
            followMessage.SetActive(false);
        }

        if (followMessageText == null && followMessage != null)
        {
            followMessageText =
                followMessage.GetComponentInChildren<TMP_Text>(true);
        }
    }

    public void StartGuiding()
    {
        if (player == null || finalDoor == null ||
            playerCamera == null)
        {
            Debug.LogWarning(
                "GrandmotherGuide: Missing references!"
            );
            return;
        }

        hasStartedMoving = false;
        hasDisappeared = false;
        messageHasShown = false;

        lastPlayerPosition = player.position;

        if (messageRoutine != null)
        {
            StopCoroutine(messageRoutine);
            messageRoutine = null;
        }

        if (typingAudioSource != null)
            typingAudioSource.Stop();

        if (followMessage != null)
        {
            followMessage.SetActive(false);
            messageCanvasGroup.alpha = 0f;
        }

        gameObject.SetActive(true);
        isGuiding = true;

        // Enable ending point when grandmother appears
        if (level3Ending != null)
        {
            level3Ending.EnableEndingPoint();
        }
        else
        {
            Debug.LogWarning(
                "Level3Ending reference is missing!"
            );
        }
    }

    private void Update()
    {
        if (!isGuiding || hasDisappeared)
            return;

        UpdateVisibilityMessage();
        MoveGrandmother();
        CheckEndPoint();
    }

    private void UpdateVisibilityMessage()
    {
        if (messageHasShown ||
            playerCamera == null ||
            messageCanvasGroup == null)
            return;

        Vector3 viewPoint =
            playerCamera.WorldToViewportPoint(transform.position);

        bool isVisible =
            viewPoint.z > 0f &&
            viewPoint.z <= viewDistance &&
            viewPoint.x >= 0f &&
            viewPoint.x <= 1f &&
            viewPoint.y >= 0f &&
            viewPoint.y <= 1f;

        if (isVisible)
        {
            messageHasShown = true;
            messageRoutine =
                StartCoroutine(ShowFollowMessage());
        }
    }

    private IEnumerator ShowFollowMessage()
    {
        followMessage.SetActive(true);
        messageCanvasGroup.alpha = 0f;

        if (followMessageText != null)
        {
            string fullMessage = followMessageText.text;
            followMessageText.maxVisibleCharacters = 0;

            followMessageText.ForceMeshUpdate();

            int totalCharacters =
                followMessageText.textInfo.characterCount;

            // Start typing sound
            if (typingAudioSource != null &&
                typingSound != null)
            {
                typingAudioSource.clip = typingSound;
                typingAudioSource.loop = true;
                typingAudioSource.Play();
            }

            int visibleCharacters = 0;

            while (visibleCharacters < totalCharacters)
            {
                visibleCharacters++;

                followMessageText.maxVisibleCharacters =
                    visibleCharacters;

                messageCanvasGroup.alpha = Mathf.MoveTowards(
                    messageCanvasGroup.alpha,
                    1f,
                    messageFadeSpeed * Time.deltaTime
                );

                yield return new WaitForSeconds(typingSpeed);
            }

            // Stop typing sound after text is complete
            if (typingAudioSource != null)
                typingAudioSource.Stop();

            followMessageText.maxVisibleCharacters =
                totalCharacters;
        }
        else
        {
            Debug.LogWarning(
                "Follow Message TMP Text reference is missing!"
            );

            messageCanvasGroup.alpha = 1f;
        }

        // Finish fading in if text was short
        while (messageCanvasGroup.alpha < 1f)
        {
            messageCanvasGroup.alpha = Mathf.MoveTowards(
                messageCanvasGroup.alpha,
                1f,
                messageFadeSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Wait after the complete message appears
        yield return new WaitForSeconds(
            messageDisplayDuration
        );

        // Fade out
        while (messageCanvasGroup.alpha > 0f)
        {
            messageCanvasGroup.alpha = Mathf.MoveTowards(
                messageCanvasGroup.alpha,
                0f,
                messageFadeSpeed * Time.deltaTime
            );

            yield return null;
        }

        followMessage.SetActive(false);
        messageRoutine = null;
    }

    private void MoveGrandmother()
    {
        Vector3 playerPosition = player.position;
        Vector3 ghostPosition = transform.position;
        Vector3 doorPosition = finalDoor.position;

        playerPosition.y = 0f;
        ghostPosition.y = 0f;
        doorPosition.y = 0f;

        float distance = Vector3.Distance(
            playerPosition,
            ghostPosition
        );

        if (!hasStartedMoving)
        {
            if (distance <= followDistance)
            {
                hasStartedMoving = true;
            }
            else
            {
                return;
            }
        }

        Vector3 directionToDoor =
            doorPosition - playerPosition;

        if (directionToDoor.sqrMagnitude < 0.001f)
            return;

        directionToDoor.Normalize();

        Vector3 targetPosition =
            playerPosition + directionToDoor * followDistance;

        float remainingDistance = Vector3.Distance(
            ghostPosition,
            doorPosition
        );

        if (remainingDistance <= 0.2f)
        {
            CheckEndPoint();
            return;
        }

        Vector3 nextPosition = Vector3.MoveTowards(
            ghostPosition,
            targetPosition,
            Vector3.Distance(ghostPosition, targetPosition)
        );

        if (Vector3.Distance(nextPosition, doorPosition) >
            remainingDistance)
        {
            nextPosition = Vector3.MoveTowards(
                ghostPosition,
                doorPosition,
                Vector3.Distance(ghostPosition, targetPosition)
            );
        }

        transform.position = new Vector3(
            nextPosition.x,
            transform.position.y,
            nextPosition.z
        );

        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void CheckEndPoint()
    {
        Vector3 ghostPosition = transform.position;
        Vector3 endpointPosition = finalDoor.position;

        ghostPosition.y = 0f;
        endpointPosition.y = 0f;

        float distance = Vector3.Distance(
            ghostPosition,
            endpointPosition
        );

        if (distance <= 0.2f)
        {
            hasDisappeared = true;
            isGuiding = false;

            if (messageRoutine != null)
                StopCoroutine(messageRoutine);

            if (typingAudioSource != null)
                typingAudioSource.Stop();

            if (followMessage != null)
                followMessage.SetActive(false);

            gameObject.SetActive(false);
        }
    }
}
