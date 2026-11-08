
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerFootsteps : MonoBehaviour
{
    private CharacterController controller;
    private AudioSource footstepAudio;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        footstepAudio = GetComponent<AudioSource>();

        footstepAudio.loop = true;
        footstepAudio.playOnAwake = false;
    }

    void Update()
    {
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        bool isMoving =
            horizontalVelocity.magnitude > 0.1f;

        bool isGrounded = controller.isGrounded;

        if (isMoving && isGrounded)
        {
            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }
}
