using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI healthText;
    public GameObject pauseMenu;
    public Transform playerTransform;
    public TextMeshProUGUI scoreText;
    public GameObject settingsMenu;
    public Animator animator;
    public InGameMusic inGameMusic;

    // Update is called once per frame
    void Update()
    {
        // Ele pega como referencia o int da vida do jogador, transforma em string e atualiza o texto na tela na medida que e alterado.
        healthText.text = playerStats.life.ToString();
        //pega a pontuacao do jogador e exibe
        scoreText.text = playerStats.score.ToString() + " /2000";

        //pausa o jogo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PlayerMove.pauseMenuActive = false;
        inGameMusic.GameSwitch();
    }

    public void LeaveToMainMenu()
    {
        PlayerMove.pauseMenuActive = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToSettings()
    {
       settingsMenu.SetActive(!settingsMenu.activeSelf);
        animator.Play("ConfigPausePopUp");
    }

    public void pauseGame()
    {
            Time.timeScale = 0f;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PlayerMove.pauseMenuActive = true;
            inGameMusic.PauseSwitch();
    }

    
}
