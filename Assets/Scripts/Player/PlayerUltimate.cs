using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUltimate : MonoBehaviour
{
    public static bool haveUlt;
    public static int ultIndex;

    [Header("UI Elements")]

    public Button ultButton;
    public Image ultIconGUI;        // Moldura (Image no Canvas)
    public GameObject ultIconPen;   // Caneta (objeto da imagem interna)
    public Sprite guiEmptySprite;   // Moldura descarregada

    public Sprite firstCharge; //imagem da primeira carga

    public Sprite secondCharge; //imagem da segunda carga
    public Sprite guiFullSprite;    // Moldura carregada

    private float cooldown = 0f;
    private float cooldownTime = 9f;
    private bool isOnCooldown = false;

    void Start()
    {
        haveUlt = false;
        ultIndex = 0;

        // Tudo começa escondido
        ultIconGUI.gameObject.SetActive(false);
        ultIconPen.SetActive(false);
        ultButton.onClick.AddListener(PenUltimate);
    }

    void Update()
    {
        // Mostra ícones apenas quando o jogador tiver a ult
        if (haveUlt)
        {
            ultIconGUI.gameObject.SetActive(true);
            ultIconPen.SetActive(true);
        }

        // Ativa a ult se estiver pronta
        if (haveUlt && !isOnCooldown && Input.GetKeyDown(KeyCode.X))
        {
            PenUltimate();
            StartCooldown();
        }

        // Atualiza o estado visual durante o cooldown
        if (isOnCooldown && Time.time >= cooldown)
        {
            EndCooldown();
        }else if (isOnCooldown && cooldown - Time.time <= 3)
        {
            ultIconGUI.sprite = secondCharge;
        }else if(isOnCooldown && cooldown - Time.time <= 3)
        {
            ultIconGUI.sprite = firstCharge;
        }
    }

    void PenUltimate()
    {
        // Destruir inimigos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObj in enemies)
        {
            BasicEnemy enemy = enemyObj.GetComponent<BasicEnemy>();
            if (enemy != null)
                enemy.TakeDamage(100, 1);
        }

        // Destruir obstáculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacleObj in obstacles)
        {
            Obstacle obstacle = obstacleObj.GetComponent<Obstacle>();
            if (obstacle != null)
                obstacle.Destruction();
        }

        // Destruir objetos "Insta Kill"
        GameObject[] instaKillObjects = GameObject.FindGameObjectsWithTag("Insta Kill");
        foreach (GameObject instaKillObj in instaKillObjects)
        {
            Obstacle instaKill = instaKillObj.GetComponent<Obstacle>();
            if (instaKill != null)
                instaKill.Destruction();
        }
    }

    void StartCooldown()
    {
        ultButton.interactable = false;
        isOnCooldown = true;
        cooldown = Time.time + cooldownTime;

        // Muda a GUI (fundo) para descarregada
        ultIconGUI.sprite = guiEmptySprite;
    }

    void EndCooldown()
    {
        ultButton.interactable = true;
        isOnCooldown = false;

        // Muda a GUI (fundo) para carregada
        ultIconGUI.sprite = guiFullSprite;
    }

    // 🔹 Chame esta função quando o jogador pegar a caneta
    public void UnlockUltimate()
    {
        haveUlt = true;
        ultIconGUI.gameObject.SetActive(true);
        ultIconPen.SetActive(true);

        // Começa com o fundo descarregado
        ultIconGUI.sprite = guiEmptySprite;

        // Inicia o carregamento
        StartCooldown();
    }
}
