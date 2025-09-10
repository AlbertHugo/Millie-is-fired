using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{


public void StartGame()
{     
        // Coisa símples, só manda pro jogo
    SceneManager.LoadScene("Stage1");
}

public void LeaveGame()
    {
        // literalmente fecha o jogo
        Application.Quit();
    }



}


