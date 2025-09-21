using UnityEngine;
using UnityEngine.UI;

public class UIsounds : MonoBehaviour
{

    public AudioClip clickSound; 
    private AudioSource audioSource;

    void Awake()
    {
        // pega ou adiciona um AudioSource no mesmo objeto
        audioSource = gameObject.AddComponent<AudioSource>();

       
    }

    public void PlaySound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
