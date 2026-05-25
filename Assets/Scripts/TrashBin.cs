using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip TrashedClip;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            Destroy(other.gameObject);
            audioSource.PlayOneShot(TrashedClip);

            Debug.Log("Trash cleaned!");
        }
    }
} 