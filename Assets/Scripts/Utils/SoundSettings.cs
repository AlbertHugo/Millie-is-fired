using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer masterMixer;
    public AudioMixerGroup sfx;

    private void Start()
    {
        //atribui o canal de SFX aos audio sources temporários
        RepeatableCode.sfxMixer = sfx;
        // Carrega valores salvos (ou 100 se não houver)
        float masterVol = PlayerPrefs.GetFloat("SavedMasterVolume", 100);
        float musicVol = PlayerPrefs.GetFloat("SavedMusicVolume", 100);
        float sfxVol = PlayerPrefs.GetFloat("SavedSFXVolume", 100);

        // Atualiza cada volume
        SetVolume("Master", masterVol);
        SetVolume("Music", musicVol);
        SetVolume("SFX", sfxVol);
    }

  
    public void SetVolume(string target, float value)
    {
        if (value < 1)
            value = 0.001f; 

        if(masterSlider != null)
        {
            RefreshSlider(target, value);
        }

        PlayerPrefs.SetFloat($"Saved{target}Volume", value);

        masterMixer.SetFloat($"{target}Volume", Mathf.Log10(value / 100f) * 20f);
    }

    public void SetVolumeFromSlider(string target)
    {
        switch (target)
        {
            case "Master": SetVolume("Master", masterSlider.value); break;
            case "Music": SetVolume("Music", musicSlider.value); break;
            case "SFX": SetVolume("SFX", sfxSlider.value); break;
        }
    }

    private void RefreshSlider(string target, float value)
    {
        switch (target)
        {
            case "Master": masterSlider.value = value; break;
            case "Music": musicSlider.value = value; break;
            case "SFX": sfxSlider.value = value; break;
        }
    }
}
