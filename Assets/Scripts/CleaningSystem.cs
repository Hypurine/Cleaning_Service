using UnityEngine;

public class CleaningSystem : MonoBehaviour
{
    public Camera cam;

    public float cleanRange = 3f;

    public float cleanSpeed = 1f;

    public ToolManager toolManager;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Clean();
        }
    }

    void Clean()
    {
        Ray ray = cam.ViewportPointToRay(
            new Vector3(0.5f, 0.5f)
        );

        RaycastHit hit;

        float cleanRadius = 0.3f;

        if (Physics.SphereCast(
            ray,
            cleanRadius,
            out hit,
            cleanRange
        ))
        {
            CleanableObject cleanable = hit.collider.GetComponent<CleanableObject>();

            if (cleanable != null)
            {
                cleanable.Clean(
                    cleanSpeed * Time.deltaTime,
                    toolManager.currentTool
                );
            }
        }
    }
}