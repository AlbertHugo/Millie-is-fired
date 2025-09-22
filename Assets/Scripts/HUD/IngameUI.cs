using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI healthText;
    public GameObject pauseMenu;
    public Transform playerTransform;
    public TextMeshProUGUI scoreText;

    // Update is called once per frame
    void Update()
    {
        // Ele pega como referencia o int da vida do jogador, transforma em string e atualiza o texto na tela na medida que � alterado.
        healthText.text = "Vida: " + playerStats.life.ToString();
        //pega a pontua��o do jogador e exibe
        scoreText.text = "Pontua��o: " + playerStats.score.ToString() + " /3100";
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PlayerMove.pauseMenuActive = false;
    }

    public void LeaveToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToSettings()
    {
               SceneManager.LoadScene("Settings");
    }

    
}
