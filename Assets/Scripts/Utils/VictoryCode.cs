using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryCode : MonoBehaviour
{

    public GameObject upgradeScreen;
    public GameObject creditsScreen;
    public Button backButton;
    private float popUpTimer;
    private bool popUpSpawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popUpSpawned=false;
        upgradeScreen.SetActive(false);
        popUpTimer = Time.time + 0.5f;
        backButton.onClick.AddListener(BackToMenu);
        creditsScreen.SetActive(true);
    }

    void FixedUpdate()
    {
        if (Time.time >= popUpTimer&&popUpSpawned==false)
        {
            upgradeScreen.SetActive(true);
            popUpSpawned=true;
        }
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void HideCredits()
    {
        creditsScreen.SetActive(false);
    }
}
