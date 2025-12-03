using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerUltimate : MonoBehaviour
{
    public static bool haveUlt;
    public static int ultIndex;

    [Header("Caneta")]
    public VisualEffect inkExplosion;
    public AudioClip inkSound;

    [Header("Tesoura")]
    public AudioClip scissorSound;
    public Volume globalVolume;
    private bool scissorActive = false;
    private float vignetteTimer = 0f;
    private Vignette vignette;
    private bool isDecrasingVignnete = false;
    public Color vignetteColor;

    //referências
    PlayerStats playerStats;

    [Header("UI Elements")]

    public Button ultButton;
    public Image ultIconGUI;        // Moldura (Image no Canvas)

    public GameObject ultIconPen;   // Caneta (objeto da imagem interna)
    public GameObject ultIconScissor;// Tesoura
    public Sprite guiEmptySprite;   // Moldura descarregada

    public Sprite firstCharge; //imagem da primeira carga

    public Sprite secondCharge; //imagem da segunda carga
    public Sprite guiFullSprite;    // Moldura carregada

    private float cooldown = 0f;
    private float cooldownTime = 9f;
    private bool isOnCooldown = false;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        scissorActive = false;
        if(ultIndex==0)
        {
            haveUlt=false;
        }

        // Tudo começa escondido
        ultIconGUI.gameObject.SetActive(false);
        inkExplosion.gameObject.SetActive(false);
        ultIconPen.SetActive(false);
        ultIconScissor.SetActive(false);
        ultButton.onClick.AddListener(PenUltimate);
    }

    void Update()
    {
        // Mostra ícones apenas quando o jogador tiver a ult
        if (haveUlt)
        {
            ultIconGUI.gameObject.SetActive(true);
            if (ultIndex == 1)
            {
                ultButton.onClick.AddListener(PenUltimate);
                ultIconPen.gameObject.SetActive(true);
            }else if (ultIndex == 2)
            {
                ultButton.onClick.AddListener(ScissorUltimate);
                ultIconScissor.gameObject.SetActive(true);
            }
        }

        // Ativa a ult se estiver pronta
        if (haveUlt && !isOnCooldown && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (ultIndex == 1)
            {
                PenUltimate();
            }
            else if (ultIndex == 2)
            {
                ScissorUltimate();
            }
        }

        // Atualiza o estado visual durante o cooldown
        if (isOnCooldown && Time.time >= cooldown)
        {
            EndCooldown();
        }else if (isOnCooldown && cooldown - Time.time <= cooldownTime/3)
        {
            ultIconGUI.sprite = secondCharge;
        }else if(isOnCooldown && cooldown - Time.time <= ((cooldownTime/3)*2))
        {
            ultIconGUI.sprite = firstCharge;
        }else if(isOnCooldown&&cooldown - Time.time <= 7)
        {
            //para a animação
            inkExplosion.Stop();
        }

        //efeito visual da tesoura
        if (scissorActive)
        {
            if(globalVolume.profile.TryGet<Vignette>(out vignette))
            {
                vignette.color.value = vignetteColor;
                if (Time.time >= vignetteTimer && vignette.intensity.value <= 0.5f && isDecrasingVignnete == false)
                {
                    vignette.intensity.value += 0.01f;
                    vignetteTimer = Time.time + 0.05f;
                }
                else if (Time.time >= vignetteTimer && vignette.intensity.value > 0.38f)
                {
                    Debug.Log(isDecrasingVignnete);
                    isDecrasingVignnete = true;
                    vignette.intensity.value -= 0.01f;
                    vignetteTimer = Time.time + 0.05f;
                }
                else if (vignette.intensity.value <= 0.38f)
                {
                    isDecrasingVignnete = false;
                }
            }
        }
    }

    void PenUltimate()
    {
        // Destruir inimigos, tocar o efeito visual
        RepeatableCode.PlaySound(inkSound, gameObject.transform.position);
        inkExplosion.gameObject.SetActive(true);
        inkExplosion.Play();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObj in enemies)
        {
            LifeController enemy = enemyObj.GetComponent<LifeController>();
            if (enemy!=null)
            {
                enemy.TakeDamage(100, 1);
            }
            
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
        cooldownTime = 9f;
        StartCooldown();
    }

    void ScissorUltimate()
    {
        RepeatableCode.PlaySound(scissorSound, gameObject.transform.position);
        playerStats.ATKBuff += 2;
        playerStats.lifeLock = true;
        cooldownTime = 9999f;
        scissorActive = true;
        vignetteTimer = Time.time + 0.5f;
        StartCooldown();
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
