using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    public Color highlightColor = Color.yellow;

    private Renderer rend;

    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();

        originalColor = rend.material.color;
    }

    public void Highlight()
    {
        rend.material.color = highlightColor;
    }

    public void RemoveHighlight()
    {
        rend.material.color = originalColor;
    }
}