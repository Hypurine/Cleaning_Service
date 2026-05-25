using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public Light[] lights;

    private bool isOn;

    void Start()
    {
        // Read first light state
        if (lights.Length > 0)
        {
            isOn = lights[0].enabled;
        }
    }

    void OnMouseDown()
    {
        ToggleLights();
        Debug.Log("Clicked switch");
    }

    void ToggleLights()
    {
        // Flip state ONCE
        isOn = !isOn;

        // Apply same state to all lights
        foreach (Light l in lights)
        {
            l.enabled = isOn;
        }
    }
}