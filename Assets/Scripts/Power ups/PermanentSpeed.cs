using TMPro;
using UnityEngine;

public class PermanentSpeed : MonoBehaviour
{
    private int pointsUsed = 0;
    private int pointBank = 0;
    private int speedCounter = 0;

    public GameObject player;
    private PlayerStats playerStats;
    public GameObject permanentSpeedUpgradePanel;
    public TextMeshProUGUI upgradeText;
    public GameObject errorPopUp;

    //caso tenha upgrade
    public GameObject refundButton;
    public GameObject originalButton;
    public GameObject upgradeButton;
    private bool isPaused = false;

    void Start()
    {
        AddScoreToBank();
        playerStats = player.GetComponent<PlayerStats>();
        pointBank = PlayerPrefs.GetInt("pointBank", 0);
        pointsUsed = PlayerPrefs.GetInt("pointsUsed", 0);
        speedCounter = PlayerPrefs.GetInt("speedCounter", 0);
        upgradeText.text = "[MELHORIAS: " + speedCounter + "]";
        if (speedCounter >= 1)
        {
            originalButton.SetActive(false);
            refundButton.SetActive(true);
            upgradeButton.SetActive(true);
        }
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
        pointBank += Mathf.RoundToInt(2000);
        PlayerPrefs.SetInt("pointBank", pointBank);
        PlayerPrefs.Save();
    }

    public void BuySpeedUpgrade()
    {
        TryBuyUpgrade(2000);
    }

    public bool TryBuyUpgrade(int cost)
    {
        if (pointBank >= cost&&speedCounter<=4)
        {
            pointBank -= cost;
            pointsUsed += cost;

            playerStats.SPDBuff = PlayerPrefs.GetFloat("SPDBuff", 1);
            playerStats.SPDBuff+=0.3f;
            speedCounter+=1;

            PlayerPrefs.SetInt("pointBank", pointBank);
            PlayerPrefs.SetFloat("SPDBuff", playerStats.SPDBuff);
            PlayerPrefs.SetInt("pointsUsed", pointsUsed);
            PlayerPrefs.SetInt("speedCounter", speedCounter);
            PlayerPrefs.Save();
            upgradeText.text = "[MELHORIAS: " + speedCounter + "]";
            return true;
        }
        else
        {
            errorPopUp.SetActive(true);
            return false;
        }
    }

    public void Refund()
    {

        pointBank += pointsUsed;

        playerStats.SPDBuff = 1;

        pointsUsed = 0;
        speedCounter = 0;

        PlayerPrefs.SetInt("pointBank", pointBank);
        PlayerPrefs.SetFloat("SPDBuff", playerStats.SPDBuff);
        PlayerPrefs.SetInt("pointsUsed", pointsUsed);
        PlayerPrefs.SetInt("speedCounter", speedCounter);
        PlayerPrefs.Save();
        upgradeText.text = "[UPGRADES: " + speedCounter + "]";
    }

    public void HideUpgradePanel()
    {
        if (permanentSpeedUpgradePanel != null)
        {
            permanentSpeedUpgradePanel.SetActive(false);
        }

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
