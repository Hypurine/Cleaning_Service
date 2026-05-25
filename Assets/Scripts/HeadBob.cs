using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float walkBobSpeed = 10f;
    public float sprintMultiplier = 1.5f;
    public float bobAmount = 0.05f;
    public float smoothSpeed = 8f;

    private float defaultYPos;
    private float timer;

    private float currentBobAmount;
    private float targetBobAmount;

    public bool isMoving;
    public bool isSprinting;

    void Start()
    {
        defaultYPos = transform.localPosition.y;
    }

    void Update()
    {
        if (isMoving == true)
            targetBobAmount = bobAmount;
        else
            targetBobAmount = 0f;

        currentBobAmount = Mathf.Lerp(
            currentBobAmount,
            targetBobAmount,
            Time.deltaTime * smoothSpeed
        );

        float speed = isSprinting ? walkBobSpeed * sprintMultiplier : walkBobSpeed;

        if (currentBobAmount > 0.001f)
            timer += Time.deltaTime * speed;
        else
            timer = 0f;

        float newY = defaultYPos + Mathf.Sin(timer) * currentBobAmount;

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            newY,
            transform.localPosition.z
        );
    }
}