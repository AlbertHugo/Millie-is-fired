using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsSettings : MonoBehaviour
{
    public GameObject canvasAudio;
    public GameObject settingsMenu;

    public void ShowCanvasAudio()
    {
        canvasAudio.SetActive(true);
    }

    public void HideCanvasAudio()
    {
        canvasAudio.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void esconderSettings()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

}
