using UnityEngine;

public class MusicChange : MonoBehaviour
{
    [Header("Efeitos sonoros")]
    private int entranceIndex = 0;
    public AudioClip entrance1;
    public AudioClip entrance2;

    void Start()
    {
        entranceIndex = Random.Range(0, 99);//seleciona qual o música que será tocada na entrada
        //toca o som na entrada, ou não toca nada
        if (entranceIndex <= 74)
        {
            PlaySound(entrance1);
        }
        else if(entranceIndex >= 75)
        {
            PlaySound(entrance2);
        }
    }
     public static void PlaySound(AudioClip clip)
    {
        GameObject tempGO = new GameObject("TempAudio");
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.Play();
        GameObject.Destroy(tempGO, clip.length);
    }
}
