using UnityEngine;
using UnityEngine.Audio;

public class PopUpSound : MonoBehaviour
{
    public AudioClip popUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        RepeatableCode.PlaySound(popUp, gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
