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
    public GameObject settingsMenu;

    // Update is called once per frame
    void Update()
    {
        // Ele pega como referencia o int da vida do jogador, transforma em string e atualiza o texto na tela na medida que e alterado.
        healthText.text = playerStats.life.ToString();
        //pega a pontuacao do jogador e exibe
        scoreText.text = playerStats.score.ToString() + " /2000";
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
       settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    
}
