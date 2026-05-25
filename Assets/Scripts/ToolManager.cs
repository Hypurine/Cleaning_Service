using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public ToolType currentTool =
        ToolType.Hand;

    [Header("Tool Visuals")]
    public GameObject broomObject;

    public GameObject mopObject;
    public GameObject baygoneObject;

    void Start()
    {
        UpdateToolVisuals();
    }

    void Update()
    {
        // Hand
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentTool = ToolType.Hand;

            UpdateToolVisuals();
        }

        // Broom
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentTool = ToolType.Broom;

            UpdateToolVisuals();
        }

        // Mop
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentTool = ToolType.Mop;

            UpdateToolVisuals();
        }

        // Baygone
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentTool = ToolType.Baygone;

            UpdateToolVisuals();
        }
    }

    void UpdateToolVisuals()
    {
        broomObject.SetActive(
            currentTool == ToolType.Broom
        );

        mopObject.SetActive(
            currentTool == ToolType.Mop
        );

        baygoneObject.SetActive(
            currentTool == ToolType.Baygone
        );
    }
}