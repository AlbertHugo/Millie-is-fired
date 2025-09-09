using UnityEngine;
using TMPro; 

public class IngameUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI healthText;

    // Update is called once per frame
    void Update()
    {
        // Ele pega como referencia o int da vida do jogador, transforma em string e atualiza o texto na tela na medida que é alterado.
        healthText.text = "Vida: " + playerStats.life.ToString();
    }
}
