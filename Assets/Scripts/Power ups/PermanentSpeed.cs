using UnityEngine;

public class PermanentSpeed : MonoBehaviour
{
    private int pointsUsed = 0;
    private int pointBank = 0;
    private int speedCounter = 0; // conta quantas vezes a velocidade foi aumentada

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();

        pointsUsed = PlayerPrefs.GetInt("pointsUsed", 0);
        pointBank = PlayerPrefs.GetInt("pointBank", 0);
        speedCounter = PlayerPrefs.GetInt("speedCounter", 0); // carrega quantos upgrades já foram feitos
    }

    public void AddScoreToBank()
    {
        if (playerStats == null) return;

        pointBank += Mathf.RoundToInt(playerStats.score);
        playerStats.score = 0;
        PlayerPrefs.SetInt("pointBank", pointBank);
        PlayerPrefs.Save();
    }

    public bool TryBuyUpgrade(int cost)
    {
        if (playerStats == null) return false;

        if (pointBank >= cost)
        {
            pointBank -= cost;
            pointsUsed += cost;

            var baseSpeedField = playerStats.GetType().GetField("baseSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (baseSpeedField != null)
            {
                float currentBaseSpeed = (float)baseSpeedField.GetValue(playerStats);
                float newBaseSpeed = currentBaseSpeed + 1f;
                baseSpeedField.SetValue(playerStats, newBaseSpeed);
                speedCounter++;

                Debug.Log("Velocidade base aumentada! Nova base: " + newBaseSpeed);
            }
            else
            {
                Debug.LogWarning("Não foi possível acessar baseSpeed — verifique se o nome está correto!");
            }

            PlayerPrefs.SetInt("pointBank", pointBank);
            PlayerPrefs.SetInt("pointsUsed", pointsUsed);
            PlayerPrefs.SetInt("speedCounter", speedCounter);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            Debug.Log("Pontos insuficientes!");
            return false;
        }
    }

    public void Refund()
    {
        if (playerStats == null) return;

        
        pointBank += pointsUsed;

        var baseSpeedField = playerStats.GetType().GetField("baseSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (baseSpeedField != null)
        {
            float currentBaseSpeed = (float)baseSpeedField.GetValue(playerStats);
            float newBaseSpeed = currentBaseSpeed - speedCounter;
            baseSpeedField.SetValue(playerStats, newBaseSpeed);

            Debug.Log($"Reembolso aplicado! Velocidade base retornou de {currentBaseSpeed} para {newBaseSpeed}");
        }

        
        pointsUsed = 0;
        speedCounter = 0;

        PlayerPrefs.SetInt("pointBank", pointBank);
        PlayerPrefs.SetInt("pointsUsed", pointsUsed);
        PlayerPrefs.SetInt("speedCounter", speedCounter);
        PlayerPrefs.Save();
    }
}
