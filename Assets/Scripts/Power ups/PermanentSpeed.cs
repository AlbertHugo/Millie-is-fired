using UnityEngine;

public class PermanentSpeed : MonoBehaviour
{
    private int pointsUsed = 0;
    private int pointBank = 0;
    private int speedCounter = 0;

    private PlayerStats playerStats;

    public GameObject permanentSpeedUpgradePanel;
    private bool isPaused = false;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();

        pointsUsed = PlayerPrefs.GetInt("pointsUsed", 0);
        pointBank = PlayerPrefs.GetInt("pointBank", 0);
        speedCounter = PlayerPrefs.GetInt("speedCounter", 0);

        if (permanentSpeedUpgradePanel != null)
        {
            permanentSpeedUpgradePanel.SetActive(true);
            PauseGame();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            HideUpgradePanel();
        }
    }

    public void AddScoreToBank()
    {
        if (playerStats == null) return;

        pointBank += Mathf.RoundToInt(playerStats.score);
        playerStats.score = 0;
        PlayerPrefs.SetInt("pointBank", pointBank);
        PlayerPrefs.Save();
    }

    public void BuySpeedUpgrade()
    {
        if (TryBuyUpgrade(2000))
        {
            HideUpgradePanel();
        }
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
        }

        pointsUsed = 0;
        speedCounter = 0;

        PlayerPrefs.SetInt("pointBank", pointBank);
        PlayerPrefs.SetInt("pointsUsed", pointsUsed);
        PlayerPrefs.SetInt("speedCounter", speedCounter);
        PlayerPrefs.Save();
    }

    public void HideUpgradePanel()
    {
        if (permanentSpeedUpgradePanel != null)
            permanentSpeedUpgradePanel.SetActive(false);

        ResumeGame();
    }

    public void ShowUpgradePanel()
    {
        if (permanentSpeedUpgradePanel != null)
            permanentSpeedUpgradePanel.SetActive(true);

        PauseGame();
    }

    private void PauseGame()
    {
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
}
