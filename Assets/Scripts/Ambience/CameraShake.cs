using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        cinemachineCam = GetComponent<CinemachineVirtualCamera>();
        if (cinemachineCam != null)
            noise = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (noise == null) yield break;

        float originalAmplitude = noise.m_AmplitudeGain;
        float originalFrequency = noise.m_FrequencyGain;

        // ativa o shake
        noise.m_AmplitudeGain = magnitude;
        noise.m_FrequencyGain = magnitude * 2f;

        Time.timeScale = 0.7f; // efeito de slowmotion

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // restaura valores originais
        noise.m_AmplitudeGain = originalAmplitude;
        noise.m_FrequencyGain = originalFrequency;

        Time.timeScale = 1f;
    }
}
