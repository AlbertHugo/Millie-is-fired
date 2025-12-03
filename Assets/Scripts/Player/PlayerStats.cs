using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.VFX;
using UnityEngine.EventSystems;

public class PlayerStats : MonoBehaviour
{
    [Header("Referências")]
    public AudioClip damageTook;
    public VisualEffect damaged;
    //Pega as imagens dos ícones como objetos, quando chama a coroutine
    //deixa a imagem do power up pego ativa ao invés de ter um texto
    public GameObject dmgIcon;
    public GameObject hpIcon;
    public GameObject spdIcon;

    [Header("Atributos do Jogador")]
    public float life = 3f;
    private float baseLife = 3f;
    public static float damage = 3f;

    public static float baseATK = 0f;
    public float speed = 4f;
    private float baseSpeed = 5f;

    [Header("Buffs dos Atributos")]
    public float HPBuff = 0f;
    public float ATKBuff = 1f;
    public float SPDBuff = 1f;

    [Header("Pontuação e distância")]
    public float distance = 0f;
    public float score = 0f;
    private float landMark = 5f;

    [Header("Interação com ult da tesoura")]
    public bool lifeLock = false;
    private bool verifyLock = false;

    //cheat
    private bool isCheating = false;

    private int tapCount = 0;
    private float lastTapTime = 0f;
    private float maxTapInterval = 0.4f;

    private void Start()
    {
        isCheating=false;
        SPDBuff = PlayerPrefs.GetFloat("SPDBuff", 1);
        damaged.gameObject.SetActive(false);
    }

    private void Update()
    {
        DetectCheatTaps();
    }

    private void FixedUpdate()
    {
        //código para medir a distância
        distance = gameObject.gameObject.transform.position.z;
        //a cada certo número de distância percorrida, aumenta a pontuação
        if (distance>=landMark)
        {
            score += 5;
            landMark = distance+5f;
        }
        //código de aceleração
        if (baseSpeed <= 20)
        {
            baseSpeed += gameObject.transform.position.z / 5000;
        }
        //velocidade é multiplicada pelo buff de velocidade atual
        speed = baseSpeed * SPDBuff;
        //vida soma com o buff ao invés de multiplicar, para ficar sempre em valores inteiros
        life = baseLife + HPBuff;
        //dano também é multiplicado
        damage = baseATK * ATKBuff;
        if (lifeLock)
        {
            HPBuff = 0f;
            baseLife = 1;
            verifyLock = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            TakeDamage(1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Dano de objeto básico
        if (other.gameObject.tag == "Obstacle"||other.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
        //Dano de morte imediata
        }else if (other.gameObject.tag == "Insta Kill")
        {
            TakeDamage(life);
        }

        //Colisões com power ups
        if (other.gameObject.tag == "SpeedBuff")
        {
            if (SPDBuff < 3)
            {
                SPDBuff +=0.1f;
                Object.FindFirstObjectByType<UpgradeNotifier>().ShowUpgrade();
                spdIcon.SetActive(true);
            }
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "DamageBuff")
        {
            ATKBuff += 0.1f;
            Destroy(other.gameObject);
            Object.FindFirstObjectByType<UpgradeNotifier>().ShowUpgrade();
            dmgIcon.SetActive(true);
        }
        else if(other.gameObject.tag =="HPBuff")
        {
            HPBuff += 1f;
            Destroy(other.gameObject);
            Object.FindFirstObjectByType<UpgradeNotifier>().ShowUpgrade();
            hpIcon.SetActive(true);
        }
    }
    //código de dano básico
    public void TakeDamage(float damage)
    {
        lifeLock = false;
        baseLife -= damage;
        if (baseLife+HPBuff > 0)
        {
           baseSpeed = baseSpeed / 2;
            RepeatableCode.PlaySound(damageTook, gameObject.transform.position);
            StartCoroutine(CameraShake.instance.Shake(0.6f, 2f));
        }
        else
        {
            SceneManager.LoadScene("Defeat");
        }
        if (verifyLock)
        {
            lifeLock = true;
        }
    }

        private void DetectCheatTaps()
    {
        bool inputDetected = false;

        // PC (mouse)
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                inputDetected = true;
        }

        // Mobile (touch)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                inputDetected = true;
        }

        if (!inputDetected)
            return;

        // Se o intervalo entre toques foi curto, conta como sequência
        if (Time.time - lastTapTime <= maxTapInterval)
        {
            tapCount++;
        }
        else
        {
            tapCount = 1; // reinicia sequência
        }

        lastTapTime = Time.time;

        // Ativa cheat ao atingir 4 toques
        if (tapCount >= 4)
        {
            isCheating = true;
            tapCount = 0; // reset
        }
    }
}
