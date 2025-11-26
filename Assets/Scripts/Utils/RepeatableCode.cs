using UnityEngine;
using UnityEngine.Audio;

public class RepeatableCode : MonoBehaviour
{
    public static AudioMixerGroup sfxMixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.outputAudioMixerGroup = sfxMixer;
        aSource.clip = clip;
        if (Time.timeScale > 0.2f)
        {
            aSource.PlayOneShot(clip);
        }
        else
        {
            Time.timeScale = 0.1f;
            aSource.PlayOneShot(clip);
            Time.timeScale = 0f;
        }
        GameObject.Destroy(tempGO, clip.length);
    }
}
