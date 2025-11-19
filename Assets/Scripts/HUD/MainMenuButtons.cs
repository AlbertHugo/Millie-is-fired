using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject buildPanel;
    void Start()
{
    Time.timeScale=1f;
    PlayerStats.baseATK = 0f;
    PlayerAttack.projectile = null;
    PlayerAttack.projectileIndex = 0;
}
public void StartGame()
{     
        // Coisa s�mples, s� manda pro jogo
    SceneManager.LoadScene("Stage1");
}


    public void GoToSettings()
    {
        // Manda para a cena de settings uau
        SceneManager.LoadScene("Settings");
    }

    public void LeaveGame()
    {
        // literalmente fecha o jogo
        Application.Quit();
    }

    public void ShowBuildPanel()
    {
        buildPanel.SetActive(true);
    }
}


