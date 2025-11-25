using UnityEngine;
using UnityEngine.Audio;

public class InGameMusic : MonoBehaviour
{
    public AudioSource gameMusic;
    public AudioSource pauseMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameMusic.loop = true;
        pauseMusic.loop = true;
        gameMusic.Play();
    }

    // Update is called once per frame
    public void PauseSwitch()
    {
        gameMusic.Pause();
        Time.timeScale = 0.1f;
        pauseMusic.Play();
        Time.timeScale = 0f;
    }

    public void GameSwitch()
    {
        gameMusic.UnPause();
        pauseMusic.Stop();
    }
}
