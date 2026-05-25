using UnityEngine;
using TMPro;

public class DoorInteract : MonoBehaviour
{
    public Camera cam;

    public float interactRange = 3f;

    public Animator animator;

    public TextMeshProUGUI interactText;

    private bool isOpen;

    void Update()
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        Ray ray = cam.ViewportPointToRay(
            new Vector3(0.5f, 0.5f)
        );

        RaycastHit hit;

        bool lookingAtDoor = false;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.transform == transform)
            {
                lookingAtDoor = true;

                interactText.gameObject.SetActive(true);

                interactText.text =
                    "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ToggleDoor();
                }
            }
        }

        if (!lookingAtDoor)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;

        animator.SetBool(
            "isOpen",
            isOpen
        );
    }
}