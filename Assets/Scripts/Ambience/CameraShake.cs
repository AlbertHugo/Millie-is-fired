using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (noise == null) yield break;

        float originalAmplitude = noise.AmplitudeGain;
        float originalFrequency = noise.FrequencyGain;

        // ativa o shake
        noise.AmplitudeGain = magnitude;
        noise.FrequencyGain = magnitude * 2f;

        Time.timeScale = 0.9f; // efeito de slowmotion

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // restaura valores originais
        noise.AmplitudeGain = originalAmplitude;
        noise.FrequencyGain = originalFrequency;

        Time.timeScale = 1f;
    }
}
