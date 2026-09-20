using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTransform;
    public float cameraHeight = 1.6f;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float sprintSpeed = 7f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float lookXLimit = 85f;

    [Header("Gravity")]
    public float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Attach camera to player at runtime
        if (cameraTransform != null)
        {
            cameraTransform.SetParent(transform);

            cameraTransform.localPosition = new Vector3(
                0f,
                cameraHeight,
                0f
            );

            cameraTransform.localRotation = Quaternion.identity;
        }

        // First-person cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (cameraTransform == null)
            return;

        // =========================
        // MOUSE LOOK
        // =========================

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Up / Down
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(
            rotationX,
            -lookXLimit,
            lookXLimit
        );

        cameraTransform.localRotation =
            Quaternion.Euler(rotationX, 0f, 0f);

        // Left / Right
        transform.Rotate(
            Vector3.up * mouseX
        );

        // =========================
        // MOVEMENT
        // =========================

        float horizontal =
            Input.GetAxis("Horizontal");

        float vertical =
            Input.GetAxis("Vertical");

        Vector3 move =
            transform.forward * vertical +
            transform.right * horizontal;

        move.Normalize();

        float speed =
            Input.GetKey(KeyCode.LeftShift)
            ? sprintSpeed
            : moveSpeed;

        move *= speed;

        // =========================
        // GRAVITY
        // =========================

        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity +=
                gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        controller.Move(
            move * Time.deltaTime
        );

        // =========================
        // ESCAPE
        // =========================

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState =
                CursorLockMode.None;

            Cursor.visible = true;
        }
    }
}