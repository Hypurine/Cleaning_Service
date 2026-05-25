using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragObject : MonoBehaviour
{
    [Header("Pickup")]
    public float pickupRange = 3f;

    [Header("Drag")]
    public float dragSmoothness = 15f;

    [Header("Distance")]
    public float scrollSpeed = 2f;
    public float minDistance = 1f;
    public float maxDistance = 5f;

    [Header("Throw")]
    public float throwForce = 10f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pickupClip;
    public AudioClip dropClip;
    public AudioClip throwClip;

    private Camera cam;

    private Rigidbody rb;

    private bool isDragging;

    private float distance;

    private Vector3 offset;

    void Start()
    {
        cam = Camera.main;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Pickup
        if (Input.GetMouseButtonDown(0))
        {
            TryPickup();
        }

        // Drop
        if (Input.GetMouseButtonUp(0))
        {
            DropObject();
        }

        // Throw
        if (isDragging && Input.GetKeyDown(KeyCode.Q))
        {
            ThrowObject();
        }

        // Drag
        if (isDragging)
        {
            DragObjectMovement();
        }
    }

    void TryPickup()
    {
        Ray ray = cam.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        RaycastHit hit;

        float pickupRadius = 0.2f;

        if (Physics.SphereCast(
            ray,
            pickupRadius,
            out hit,
            pickupRange
        ))
        {
            if (hit.transform == transform)
            {
                isDragging = true;

                distance = Vector3.Distance(
                    cam.transform.position,
                    transform.position
                );

                Vector3 mouseWorldPos =
                    GetMouseWorldPosition();

                offset =
                    transform.position - mouseWorldPos;

                rb.useGravity = false;
                rb.velocity = Vector3.zero;

                if (pickupClip != null)
                {
                    audioSource.PlayOneShot(
                        pickupClip
                    );
                }
            }
        }
    }

    void DragObjectMovement()
    {
        // Scroll wheel distance
        float scroll =
            Input.GetAxis("Mouse ScrollWheel");

        distance += scroll * scrollSpeed;

        distance = Mathf.Clamp(
            distance,
            minDistance,
            maxDistance
        );

        Vector3 targetPos =
            GetMouseWorldPosition() + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * dragSmoothness
        );
    }

    void DropObject()
    {
        if (!isDragging)
            return;

        isDragging = false;

        rb.useGravity = true;

        if (dropClip != null)
        {
            audioSource.PlayOneShot(dropClip);
        }
    }

    void ThrowObject()
    {
        isDragging = false;

        rb.useGravity = true;

        rb.velocity = Vector3.zero;

        rb.AddForce(
            cam.transform.forward * throwForce,
            ForceMode.Impulse
        );

        if (throwClip != null)
        {
            audioSource.PlayOneShot(
                throwClip
            );
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos =
            Input.mousePosition;

        mousePos.z = distance;

        return cam.ScreenToWorldPoint(
            mousePos
        );
    }
}