using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Mouse")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    [Header("Footsteps")]
    public AudioSource footstepAudio;
    public AudioClip grassClip;
    public AudioClip concreteClip;

    [Header("Bobbing")]
    [SerializeField] public HeadBob headBob;

    [Header("Interaction")]
    public float interactRange = 2f;

    private InteractableHighlight currentHighlight;

    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.3f;

    public CharacterController controller;

    private float yVelocity;
    private float xRotation;
    private float stepTimer;
   
    public bool isSprinting {get; private set;}

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Look();
        Footsteps();

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        bool moving = horizontalVelocity.magnitude > 0.1f;

        if (moving == true)
        {
            headBob.isMoving = true;
        }

        headBob.isMoving = moving;
        headBob.isSprinting = isSprinting;
        
        HandleHighlight();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        // Prevent diagonal speed boost
        move = Vector3.ClampMagnitude(move, 1f);

        bool movingBackwards = z < -0.1f;

        isSprinting =
            Input.GetKey(KeyCode.LeftShift) &&
            !movingBackwards &&
            move.magnitude > 0.1f;

        float currentSpeed =
            isSprinting ? sprintSpeed : walkSpeed;

        // Grounded handling
        if (controller.isGrounded)
        {
            if (yVelocity < 0)
                yVelocity = -0.5f;

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = Mathf.Sqrt(
                    jumpHeight * -2f * gravity
                );
            }
        }

        // Better gravity
        if (yVelocity < 0)
        {
            // Falling
            yVelocity +=
                gravity * 1.2f * Time.deltaTime;
        }
        else
        {
            // Rising
            yVelocity +=
                gravity * Time.deltaTime;
        }

        // Separate horizontal and vertical movement
        Vector3 finalMove = move * currentSpeed;

        finalMove.y = yVelocity;

        controller.Move(finalMove * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    void Footsteps()
    {
        if (!controller.isGrounded) return;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;
        if (speed < 0.1f) return;

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            AudioClip clipToPlay = GetSurfaceClip();

            if (clipToPlay != null)
            {
                footstepAudio.pitch = Random.Range(0.9f, 1.1f);
                footstepAudio.PlayOneShot(clipToPlay);
            }

            bool isSprinting = speed > walkSpeed + 1f;
            stepTimer = isSprinting ? sprintStepInterval : walkStepInterval;
        }
    }

    AudioClip GetSurfaceClip()
    {
        RaycastHit hit;

        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        float rayDistance = controller.height / 2f + 0.3f;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Grass"))
                return grassClip;

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Concrete"))
                return concreteClip;
        }

        return concreteClip;
    }

    void HandleHighlight()
    {
        Ray ray = cameraTransform
            .GetComponent<Camera>()
            .ViewportPointToRay(
                new Vector3(0.5f, 0.5f)
            );

        RaycastHit hit;

        float sphereRadius = 0.2f;

        if (Physics.SphereCast(
            ray,
            sphereRadius,
            out hit,
            interactRange
        ))
        {
            InteractableHighlight highlight =
                hit.collider.GetComponent<InteractableHighlight>();

            if (highlight != null)
            {
                if (currentHighlight != highlight)
                {
                    if (currentHighlight != null)
                        currentHighlight.RemoveHighlight();

                    currentHighlight = highlight;

                    currentHighlight.Highlight();
                }

                return;
            }
        }

        if (currentHighlight != null)
        {
            currentHighlight.RemoveHighlight();

            currentHighlight = null;
        }
    }
}