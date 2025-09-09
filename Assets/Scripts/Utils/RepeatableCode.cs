using UnityEngine;
using UnityEngine.Audio;

public class RepeatableCode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        //adicionar a linha aSource.outputAudioMixerGroup = sfxMixerGroup; quando tiver dividido os audio mixers
        aSource.Play();
        GameObject.Destroy(tempGO, clip.length);
    }
}
