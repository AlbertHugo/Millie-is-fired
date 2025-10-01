using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.VFX;

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
    private float baseSpeed = 15f;

    [Header("Buffs dos Atributos")]
    public float HPBuff = 0f;
    public float ATKBuff = 1f;
    public float SPDBuff = 1f;

    // Ainda preciso programar melhor a aplicação deles
    [Header("Pontuação e distância")]
    public float distance = 0f;
    public float score = 0f;
    private float landMark = 5f;

    private void Start()
    {
        damaged.gameObject.SetActive(false);
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
        if (speed <= 20)
        {
            baseSpeed += gameObject.transform.position.z / 5000;
        }
        //velocidade é multiplicada pelo buff de velocidade atual
        speed = baseSpeed * SPDBuff;
        //vida soma com o buff ao invés de multiplicar, para ficar sempre em valores inteiros
        life = baseLife + HPBuff;
        //dano também é multiplicado
        damage = baseATK * ATKBuff;
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
    }
}
