using UnityEngine;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    private PlayerStats playerStats;
    private TMP_Text statText;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        statText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (playerStats == null || statText == null) return;

        
        var baseSpeedField = playerStats.GetType().GetField("baseSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        float baseSpeed = 0f;

        if (baseSpeedField != null)
            baseSpeed = (float)baseSpeedField.GetValue(playerStats);

       
        statText.text =
            $"Damage: {(int)PlayerStats.damage}\n" +
            $"Speed: {(int)baseSpeed}";
    }
}
