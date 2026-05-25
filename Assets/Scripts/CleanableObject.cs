using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CleanableObject : MonoBehaviour
{
    [Header("Cleaning")]
    public float cleanTime = 3f;

    public ToolType requiredTool =
        ToolType.Broom;

    [Header("Audio")]
    public AudioClip cleaningLoop;

    public AudioClip cleanComplete;

    private float cleanProgress;

    private Renderer rend;

    private Color originalColor;

    private AudioSource audioSource;

    private bool isBeingCleaned;

    void Start()
    {
        rend = GetComponent<Renderer>();

        originalColor =
            rend.material.color;

        audioSource =
            GetComponent<AudioSource>();
    }

    void Update()
    {
        // Stop loop if not cleaning this frame
        if (isBeingCleaned == false)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        isBeingCleaned = false;
    }

    public void Clean(
        float amount,
        ToolType toolUsed
    )
    {
        // Wrong tool
        if (toolUsed != requiredTool)
            return;

        isBeingCleaned = true;

        // Play cleaning loop
        if (!audioSource.isPlaying &&
            cleaningLoop != null)
        {
            audioSource.clip = cleaningLoop;

            audioSource.loop = true;

            audioSource.Play();
        }

        cleanProgress += amount;

        float percent =
            Mathf.Clamp01(
                cleanProgress / cleanTime
            );

        // Fade transparency
        Color c = rend.material.color;

        c.a = 1f - percent;

        rend.material.color = c;

        // Fully cleaned
        if (percent >= 1f)
        {
            // Stop loop
            audioSource.Stop();

            // Completion sound
            if (cleanComplete != null)
            {
                AudioSource.PlayClipAtPoint(
                    cleanComplete,
                    transform.position
                );
            }

            Destroy(gameObject);
        }
    }
}